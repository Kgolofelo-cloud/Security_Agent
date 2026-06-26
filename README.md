# CyberDefend Terminal
 
## Project Overview

CyberDefend Terminal is an interactive, GUI-based C# desktop application. It acts as a simulated System Administrator to train users on network security, phishing identification, and data protection.
 
## Core Modules

* **Dynamic Intent Routing (NLP):** Processes user text to detect objectives like scheduling tasks or starting trivia.

* **Objective Tracker (MySQL):** Connected to a local `CyberDefendDB` database to log, retrieve, and close user security objectives.

* **Trivia Challenge:** A built-in, 11-question interactive assessment with automated scoring and feedback.

* **System Action History:** A background logger that tracks the bot's state changes and query responses.

* **Adaptive Dialogue:** Adjusts terminal responses based on user sentiment (e.g., stress/frustration).
 
## Installation & Setup

1. Clone this repository to your local machine.

2. Open MySQL Workbench and run the SQL script to generate the `CyberDefendDB` database and `UserTasks` table.

3. Update the `dbConnection` variable in `SecurityAgent.cs` with your local MySQL password.

4. Build and run the solution via Visual Studio. Ensure `greeting.wav` is set to "Copy if newer".
