namespace NEA_Client_App
{
    partial class MenuForm
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
            this.SuggestionRequest = new System.Windows.Forms.Button();
            this.ReviewRequest = new System.Windows.Forms.Button();
            this.Menu_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SuggestionRequest
            // 
            this.SuggestionRequest.Location = new System.Drawing.Point(90, 233);
            this.SuggestionRequest.Name = "SuggestionRequest";
            this.SuggestionRequest.Size = new System.Drawing.Size(235, 90);
            this.SuggestionRequest.TabIndex = 0;
            this.SuggestionRequest.Text = "Get Movie Suggestions";
            this.SuggestionRequest.UseVisualStyleBackColor = true;
            this.SuggestionRequest.Click += new System.EventHandler(this.SuggestionRequest_Click);
            // 
            // ReviewRequest
            // 
            this.ReviewRequest.Location = new System.Drawing.Point(475, 225);
            this.ReviewRequest.Name = "ReviewRequest";
            this.ReviewRequest.Size = new System.Drawing.Size(235, 90);
            this.ReviewRequest.TabIndex = 1;
            this.ReviewRequest.Text = "Review a Movie";
            this.ReviewRequest.UseVisualStyleBackColor = true;
            this.ReviewRequest.Click += new System.EventHandler(this.ReviewRequest_Click);
            // 
            // Menu_Label
            // 
            this.Menu_Label.AutoSize = true;
            this.Menu_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Menu_Label.Location = new System.Drawing.Point(335, 96);
            this.Menu_Label.Name = "Menu_Label";
            this.Menu_Label.Size = new System.Drawing.Size(107, 39);
            this.Menu_Label.TabIndex = 2;
            this.Menu_Label.Text = "Menu";
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Menu_Label);
            this.Controls.Add(this.ReviewRequest);
            this.Controls.Add(this.SuggestionRequest);
            this.Name = "MenuForm";
            this.Text = "MenuForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SuggestionRequest;
        private System.Windows.Forms.Button ReviewRequest;
        private System.Windows.Forms.Label Menu_Label;
    }
}