using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA_Client_App
{
    public partial class Movie_suggestions_Form : Form
    {
        public Movie_suggestions_Form()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void SuggestionRequest_Click(object sender, EventArgs e)
        {
            new SuggestForm().Show();
            this.Hide();
        }

        private void Review_Click(object sender, EventArgs e)
        {
            (new ReviewForm1()).Show();
            this.Hide();
        }
    }
}
