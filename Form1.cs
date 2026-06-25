using System;

using System.Windows.Forms;

namespace SecurityAgent

{

    public partial class Form1 : Form

    {

        SecurityAgent agent = new SecurityAgent();

        public Form1()

        {

            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e) { }

        private void Form1_Load(object sender, EventArgs e)

        {

            try

            {

                // Updated Title for the 6th Commit Polish

                this.Text = "CyberDefend Terminal";

                agent.FormatOutput = AppendTimeSignature;

                // Fixed the hardcoded audio path

                System.Media.SoundPlayer player = new System.Media.SoundPlayer("greeting.wav");

                player.Play();

                rtbChatArea.AppendText("**************************************************\n");

                rtbChatArea.AppendText("    Initializing CyberDefend Support System...    \n");

                rtbChatArea.AppendText("**************************************************\n\n");

                rtbChatArea.AppendText("System online. Please authenticate with your preferred name below and click Send.\n");

            }

            catch (Exception ex)

            {

                rtbChatArea.AppendText("System Error: Audio file missing. " + ex.Message + "\n");

            }

        }

        private void btnSend_Click_1(object sender, EventArgs e)

        {

            string userInput = txtUserInput.Text;

            if (string.IsNullOrWhiteSpace(userInput)) return;

            if (string.IsNullOrEmpty(agent.ClientName))

            {

                agent.ClientName = userInput;

                rtbChatArea.AppendText($"\nSystem: Authentication successful. User {agent.ClientName} registered.\n");

                rtbChatArea.AppendText("[System]: How can I assist your network security today?\n");

                txtUserInput.Clear();

                return;

            }

            rtbChatArea.AppendText($"\n{agent.ClientName}: {userInput}\n");

            string botResponse = agent.GetBotResponse(userInput);

            rtbChatArea.AppendText($"{botResponse}\n");

            txtUserInput.Clear();

        }

        private string AppendTimeSignature(string message)

        {

            string time = DateTime.Now.ToString("HH:mm");

            return $"[{time}] {message}";

        }

    }

}
