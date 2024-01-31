namespace NEA_Client_App
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Passtxtbox = new System.Windows.Forms.MaskedTextBox();
            this.Usertxtbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CredCheck = new System.Windows.Forms.Button();
            this.NewUser = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Passtxtbox
            // 
            this.Passtxtbox.Location = new System.Drawing.Point(316, 290);
            this.Passtxtbox.Name = "Passtxtbox";
            this.Passtxtbox.PasswordChar = '*';
            this.Passtxtbox.Size = new System.Drawing.Size(169, 20);
            this.Passtxtbox.TabIndex = 0;
            // 
            // Usertxtbox
            // 
            this.Usertxtbox.Location = new System.Drawing.Point(316, 245);
            this.Usertxtbox.Name = "Usertxtbox";
            this.Usertxtbox.Size = new System.Drawing.Size(169, 20);
            this.Usertxtbox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(313, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(336, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 31);
            this.label3.TabIndex = 4;
            this.label3.Text = "FilmApp";
            // 
            // CredCheck
            // 
            this.CredCheck.Location = new System.Drawing.Point(367, 345);
            this.CredCheck.Name = "CredCheck";
            this.CredCheck.Size = new System.Drawing.Size(69, 19);
            this.CredCheck.TabIndex = 5;
            this.CredCheck.Text = "Login";
            this.CredCheck.UseVisualStyleBackColor = true;
            this.CredCheck.Click += new System.EventHandler(this.CredCheck_Click);
            // 
            // NewUser
            // 
            this.NewUser.Location = new System.Drawing.Point(521, 345);
            this.NewUser.Name = "NewUser";
            this.NewUser.Size = new System.Drawing.Size(66, 19);
            this.NewUser.TabIndex = 6;
            this.NewUser.Text = "Register";
            this.NewUser.UseVisualStyleBackColor = true;
            this.NewUser.Click += new System.EventHandler(this.NewUser_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(468, 346);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "or";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NewUser);
            this.Controls.Add(this.CredCheck);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Usertxtbox);
            this.Controls.Add(this.Passtxtbox);
            this.Name = "LoginForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox Passtxtbox;
        private System.Windows.Forms.TextBox Usertxtbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CredCheck;
        private System.Windows.Forms.Button NewUser;
        private System.Windows.Forms.Label label4;
    }
}