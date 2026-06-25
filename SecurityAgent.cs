using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient; // Required for MySQL

namespace SecurityAgent
{
    public delegate string ResponseFormatter(string message);

    // Renamed helper class for the Mini-Game
    internal class TriviaItem
    {
        public string Inquiry { get; set; }
        public string ValidAnswer { get; set; }
        public string Reason { get; set; }
    }

    internal class SecurityAgent
    {
        public string ClientName { get; set; }
        public ResponseFormatter FormatOutput;
        public string FavoriteTopic { get; set; } = "";
        public string LastDiscussedTopic { get; set; } = "";

        private Dictionary<string, string> keywordResponses;
        private List<string> phishingTips;
        private Random randomGen;
        // Distinct Activity Log Name
        private List<string> ActionHistory;

        // Distinct Quiz Variables
        private bool QuizModeEnabled = false;
        private int ActiveQuestion = 0;
        private int TotalPoints = 0;
        private List<TriviaItem> TriviaBank;

        // DB String - STUDENT MUST ENTER THEIR PASSWORD HERE
        private string dbConnection = "Server=127.0.0.1;Database=CyberDefendDB;Uid=root;Pwd=YOUR_PASSWORD_HERE;";

        public SecurityAgent()
        {
            randomGen = new Random();
            FormatOutput = (msg) => msg;
            ActionHistory = new List<string>();

            phishingTips = new List<string>
            {
                "Always be wary of urgent requests for money or data. Scammers use panic to force mistakes.",
                "Verify links by hovering over them before clicking to reveal the true destination URL.",
                "Banks will never ask for your PIN or full password via email or text."
            };

            keywordResponses = new Dictionary<string, string>
            {
                { "password", "Use a passphrase instead of a single word. 'BlueHorseBatteryStaple' is harder to crack than 'P@ssw0rd1'." },
                { "phishing", "Phishing is a deception technique used to harvest credentials. Always verify the source." },
                { "safe browsing", "Ensure you see the HTTPS lock icon, but remember it only means the connection is encrypted, not that the site is honest." },
                { "scam", "Beware of unsolicited offers. Verify through official channels before engaging." },
                { "privacy", "Regularly audit app permissions on your phone to protect your privacy." }
            };

            // Distinct set of 11 questions
            TriviaBank = new List<TriviaItem>
            {
                new TriviaItem { Inquiry = "Q1: What is malware?\nA) Bad hardware\nB) Malicious software\nC) A network error", ValidAnswer = "b", Reason = "Malware includes viruses, ransomware, and spyware designed to harm your device." },
                new TriviaItem { Inquiry = "Q2: True or False: You should use public Wi-Fi to check your bank account.", ValidAnswer = "false", Reason = "Public networks can be easily intercepted by cybercriminals." },
                new TriviaItem { Inquiry = "Q3: What is the best defense against ransomware?\nA) Paying the ransom\nB) Offline backups\nC) Changing your password", ValidAnswer = "b", Reason = "If you have a secure offline backup, you can restore your files without paying criminals." },
                new TriviaItem { Inquiry = "Q4: True or False: Multi-Factor Authentication (MFA) blocks most automated account hacks.", ValidAnswer = "true", Reason = "MFA requires an extra step, making stolen passwords useless on their own." },
                new TriviaItem { Inquiry = "Q5: Smishing is a cyber attack conducted via:\nA) Email\nB) Phone calls\nC) SMS/Text Messages", ValidAnswer = "c", Reason = "Smishing stands for SMS Phishing." },
                new TriviaItem { Inquiry = "Q6: True or False: Incognito mode hides your browsing from your internet service provider.", ValidAnswer = "false", Reason = "Incognito only stops local history saving. Your ISP and employer can still see your traffic." },
                new TriviaItem { Inquiry = "Q7: What does a VPN do?\nA) Makes internet faster\nB) Encrypts your internet traffic\nC) Blocks all ads", ValidAnswer = "b", Reason = "A Virtual Private Network creates a secure tunnel for your data." },
                new TriviaItem { Inquiry = "Q8: True or False: Phishing emails always have bad spelling and grammar.", ValidAnswer = "false", Reason = "Modern phishing emails, often powered by AI, can be grammatically perfect." },
                new TriviaItem { Inquiry = "Q9: If you find a random USB drive on the ground, you should:\nA) Plug it in to find the owner\nB) Hand it to IT or throw it away\nC) Format it to use it", ValidAnswer = "b", Reason = "Malicious USB drops are a real tactic used to infect computers automatically." },
                new TriviaItem { Inquiry = "Q10: True or False: Antivirus software catches 100% of all new viruses.", ValidAnswer = "false", Reason = "No software is perfect; zero-day exploits can bypass antivirus until they are updated." },
                new TriviaItem { Inquiry = "Q11: The practice of looking over someone's shoulder to steal data is called:\nA) Eavesdropping\nB) Shoulder Surfing\nC) Vishing", ValidAnswer = "b", Reason = "Shoulder surfing is a physical security threat, especially in public spaces." }
            };
        }

        public void RecordActivity(string eventDesc)
        {
            string time = DateTime.Now.ToString("MM/dd HH:mm");
            ActionHistory.Add($"[{time}] {eventDesc}");
        }

        public string GetBotResponse(string input)
        {
            input = input.ToLower().Trim();

            // ---------------------------------------------------------
            // QUIZ MODULE
            // ---------------------------------------------------------
            if (QuizModeEnabled)
            {
                if (input == "stop trivia" || input == "exit")
                {
                    QuizModeEnabled = false;
                    RecordActivity($"Trivia challenge aborted at Q{ActiveQuestion + 1}.");
                    return FormatOutput("[System]: Trivia mode deactivated. Resuming standard operations.");
                }

                TriviaItem currentQ = TriviaBank[ActiveQuestion];
                string feedback = "";

                if (input == currentQ.ValidAnswer || input.StartsWith(currentQ.ValidAnswer))
                {
                    TotalPoints++;
                    feedback = $"[System]: Correct! {currentQ.Reason}\n\n";
                }
                else
                {
                    feedback = $"[System]: Incorrect. The right answer is {currentQ.ValidAnswer.ToUpper()}. {currentQ.Reason}\n\n";
                }

                ActiveQuestion++;

                if (ActiveQuestion >= TriviaBank.Count)
                {
                    QuizModeEnabled = false;
                    RecordActivity($"Trivia finished. Score: {TotalPoints}/{TriviaBank.Count}.");
                    string grade = TotalPoints >= 9 ? "Outstanding work! Your cyber defenses are strong." :
                                   TotalPoints >= 6 ? "Fair performance. Review basic protocols." :
                                   "Security risk detected. Please review cybersecurity basics.";

                    return FormatOutput($"{feedback}--- TRIVIA COMPLETE ---\nFinal Score: {TotalPoints}/{TriviaBank.Count}\n{grade}\n\nStandard operations resumed.");
                }
                return FormatOutput($"{feedback}{TriviaBank[ActiveQuestion].Inquiry}\n(Provide your answer, or type 'stop trivia')");
            }

            // ---------------------------------------------------------
            // NLP & INTENT DETECTION
            // ---------------------------------------------------------
            if (input.Contains("trivia") || input.Contains("test me") || input.Contains("start quiz"))
            {
                QuizModeEnabled = true;
                ActiveQuestion = 0;
                TotalPoints = 0;
                RecordActivity("Initiated Cybersecurity Trivia.");
                return FormatOutput($"[System]: Trivia Challenge engaged! {TriviaBank.Count} questions loaded.\n\n{TriviaBank[0].Inquiry}\n(Type your answer)");
            }

            // Task Intent Detection
            if ((input.Contains("schedule") || input.Contains("add") || input.Contains("remind")) &&
                (input.Contains("task") || input.Contains("objective") || input.Contains("to do")))
            {
                string newTask = input.Replace("schedule a task to", "").Replace("add a task to", "").Replace("remind me to", "").Trim();
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(dbConnection))
                    {
                        conn.Open();
                        string query = "INSERT INTO UserTasks (Objective) VALUES (@obj)";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@obj", newTask);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    RecordActivity($"DB: Objective recorded - '{newTask}'");
                    return FormatOutput($"[System]: Objective successfully registered: '{newTask}'.");
                }
                catch (Exception ex) { return FormatOutput($"[System]: DB Fault. ({ex.Message})"); }
            }

            // View Tasks
            if (input.Contains("view objectives") || input.Contains("show tasks") || input.Contains("my list"))
            {
                try
                {
                    string listData = "[System]: Retrieving current objectives:\n";
                    using (MySqlConnection conn = new MySqlConnection(dbConnection))
                    {
                        conn.Open();
                        string query = "SELECT RecordID, Objective, IsFinished FROM UserTasks";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows) return FormatOutput("[System]: No pending objectives found.");
                            while (reader.Read())
                            {
                                string status = Convert.ToBoolean(reader["IsFinished"]) ? "[CLOSED]" : "[OPEN]";
                                listData += $"Record #{reader["RecordID"]}: {reader["Objective"]} {status}\n";
                            }
                        }
                    }
                    RecordActivity("DB: Objectives retrieved by user.");
                    return FormatOutput(listData);
                }
                catch (Exception ex) { return FormatOutput($"[System]: Connection failure. ({ex.Message})"); }
            }

            // Complete Task
            if (input.Contains("close task") || input.Contains("complete task") || input.Contains("finish task"))
            {
                string[] segments = input.Split(' ');
                if (segments.Length > 2 && int.TryParse(segments[2], out int recId))
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(dbConnection))
                        {
                            conn.Open();
                            string query = "UPDATE UserTasks SET IsFinished = 1 WHERE RecordID = @id";
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", recId);
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    RecordActivity($"DB: Objective #{recId} closed.");
                                    return FormatOutput($"[System]: Objective #{recId} has been securely marked as closed.");
                                }
                                else return FormatOutput($"[System]: Record #{recId} not found in database.");
                            }
                        }
                    }
                    catch (Exception ex) { return FormatOutput($"[System]: DB Fault. ({ex.Message})"); }
                }
                return FormatOutput("[System]: Syntax error. Use 'close task [Number]'.");
            }

            // ---------------------------------------------------------
            // ACTIVITY LOG
            // ---------------------------------------------------------
            if (input.Contains("history") || input.Contains("activity log") || input.Contains("recent actions"))
            {
                if (ActionHistory.Count == 0) return FormatOutput("[System]: System log is currently empty.");
                string logs = "[System]: Fetching recent system events:\n";
                int start = Math.Max(0, ActionHistory.Count - 6);
                for (int i = start; i < ActionHistory.Count; i++) logs += $"{ActionHistory[i]}\n";
                return FormatOutput(logs);
            }

            // ---------------------------------------------------------
            // SENTIMENT, MEMORY, & KEYWORDS
            // ---------------------------------------------------------
            string supportText = "";
            if (input.Contains("worried") || input.Contains("stressed")) supportText = "Security can be stressful, but you are taking the right steps. Let's look at the protocols. ";
            else if (input.Contains("frustrated") || input.Contains("stuck")) supportText = "Technical issues are frustrating. Let's break this down logically. ";

            if (input.Contains("interested in "))
            {
                string[] tokens = input.Split(new string[] { "interested in " }, StringSplitOptions.None);
                if (tokens.Length > 1)
                {
                    FavoriteTopic = tokens[1].Trim(new char[] { '.', '!', '?', ' ' });
                    RecordActivity($"Memory: Logged user interest: '{FavoriteTopic}'");
                    return FormatOutput($"[System]: Acknowledged. I have prioritized {FavoriteTopic} in your learning profile.");
                }
            }

            if (input == "expand on that" || input == "next tip" || input == "more details")
            {
                if (!string.IsNullOrEmpty(LastDiscussedTopic)) input = LastDiscussedTopic;
                else return FormatOutput("[System]: Topic undefined. Please specify a subject.");
            }

            if (input.Contains("phishing tip"))
            {
                LastDiscussedTopic = "phishing tip";
                int idx = randomGen.Next(phishingTips.Count);
                RecordActivity("Dispensed random security protocol.");
                return FormatOutput($"[System]: {supportText}{phishingTips[idx]}");
            }

            foreach (var key in keywordResponses.Keys)
            {
                if (input.Contains(key))
                {
                    LastDiscussedTopic = key;
                    string memoryHighlight = (FavoriteTopic == key) ? $"Note: This aligns with your interest in {FavoriteTopic}. " : "";
                    RecordActivity($"Processed keyword: '{key}'.");
                    return FormatOutput($"[System]: {supportText}{memoryHighlight}{keywordResponses[key]}");
                }
            }

            return FormatOutput("[System]: Command unrecognized. Please query about passwords, scams, or schedule a task.");
        }
    }
}