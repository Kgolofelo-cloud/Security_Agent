namespace SecurityAgent
{
    partial class Form1
    {
      
       
      
        private System.ComponentModel.IContainer components = null;

                      
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            rtbChatArea = new RichTextBox();
            txtUserInput = new TextBox();
            btnSend = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // rtbChatArea
            // 
            rtbChatArea.BackColor = Color.Black;
            rtbChatArea.ForeColor = SystemColors.Window;
            rtbChatArea.Location = new Point(67, 211);
            rtbChatArea.Name = "rtbChatArea";
            rtbChatArea.ReadOnly = true;
            rtbChatArea.Size = new Size(1001, 337);
            rtbChatArea.TabIndex = 0;
            rtbChatArea.Text = "";
            rtbChatArea.TextChanged += rtbChatArea_TextChanged;
            // 
            // txtUserInput
            // 
            txtUserInput.Location = new Point(67, 554);
            txtUserInput.Name = "txtUserInput";
            txtUserInput.Size = new Size(1001, 31);
            txtUserInput.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(67, 591);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(112, 34);
            btnSend.TabIndex = 2;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(0, 192, 0);
            label1.Location = new Point(753, 8);
            label1.Name = "label1";
            label1.Size = new Size(315, 200);
            label1.TabIndex = 3;
            label1.Text = resources.GetString("label1.Text");
            label1.Click += label1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1113, 637);
            Controls.Add(label1);
            Controls.Add(btnSend);
            Controls.Add(txtUserInput);
            Controls.Add(rtbChatArea);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtbChatArea;
        private TextBox txtUserInput;
        private Button btnSend;
        private Label label1;
    }
}
