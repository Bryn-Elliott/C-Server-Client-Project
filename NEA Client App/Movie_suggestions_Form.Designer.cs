namespace NEA_Client_App
{
    partial class Movie_suggestions_Form
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
            this.ExitButton = new System.Windows.Forms.Button();
            this.SuggestionRequest = new System.Windows.Forms.Button();
            this.Review = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(363, 364);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 0;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // SuggestionRequest
            // 
            this.SuggestionRequest.Location = new System.Drawing.Point(73, 128);
            this.SuggestionRequest.Name = "SuggestionRequest";
            this.SuggestionRequest.Size = new System.Drawing.Size(202, 102);
            this.SuggestionRequest.TabIndex = 1;
            this.SuggestionRequest.Text = "Generate Suggestion";
            this.SuggestionRequest.UseVisualStyleBackColor = true;
            this.SuggestionRequest.Click += new System.EventHandler(this.SuggestionRequest_Click);
            // 
            // Review
            // 
            this.Review.Location = new System.Drawing.Point(525, 129);
            this.Review.Name = "Review";
            this.Review.Size = new System.Drawing.Size(202, 101);
            this.Review.TabIndex = 2;
            this.Review.Text = "Review a movie";
            this.Review.UseVisualStyleBackColor = true;
            this.Review.Click += new System.EventHandler(this.Review_Click);
            // 
            // Movie_suggestions_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Review);
            this.Controls.Add(this.SuggestionRequest);
            this.Controls.Add(this.ExitButton);
            this.Name = "Movie_suggestions_Form";
            this.Text = "Movie_suggestions_Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button SuggestionRequest;
        private System.Windows.Forms.Button Review;
    }
}