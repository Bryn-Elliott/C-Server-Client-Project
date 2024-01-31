using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA_Client_App
{
    public partial class LoginForm : Form
    {
        public bool Done = false;
        public bool Verify = false;
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void CredCheck_Click(object sender, EventArgs e)
        {
            Verify = false;
            string User = Usertxtbox.Text;
            string Pass = Passtxtbox.Text;
            Verify = await LogicThread.UserPassCheck(User, Pass);
            if (Verify)
            {
                Login();
            }
            else
            {
                Usertxtbox.Text = "";
                Passtxtbox.Text = "";
                MessageBox.Show("Incorrect Username or Password. Try Again");
            }
        }
        private void Login()
        {
            MessageBox.Show("Login successful");
            ThreadStart childref = new ThreadStart(LoggedIn);
            Thread childThread = new Thread(childref);
            childThread.Start();
            this.Hide();
        }

        private void NewUser_Click(object sender, EventArgs e)
        {
            ThreadStart childref = new ThreadStart(Register);
            Thread childThread = new Thread(childref);
            childThread.Start();
            this.Hide();
        }

        private void Register()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RegisterForm());
        }

        private void LoggedIn()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Movie_suggestions_Form());
        }
    }
}
