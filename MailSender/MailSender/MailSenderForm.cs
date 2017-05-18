using System;
using System.Windows.Forms;

namespace MailSender
{
   
    public partial class MailSenderForm : Form

    {
        private Presenter Presenter1;

        public MailSenderForm()
        {
            InitializeComponent();
            Presenter1= new Presenter(this);
            label9.Text = "";
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = true;
            smtp.Visible = false;
            port.Visible = false;
            password.Visible = false;
            login.Visible = false;
            ssl.Visible = false;
            buttonApplySettings.Visible = false;
            this.Height = 220;
        }

        public void SetFolderPath(string path)
        {
            textBox1.Text = path;
            textBox1.Update();
        }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    
        public void OpenSettings()
        {
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = false;
            smtp.Visible = true;
            port.Visible = true;
            password.Visible = true;
            login.Visible = true;
            ssl.Visible = true;
            buttonSettings.Visible = false;
            buttonSend.Visible = false;
            buttonLogs.Visible = false;
            buttonApplySettings.Visible = true;
            this.Height = 340;
 
    }
        public void CloseSettings()
        {
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = true;
            label9.Text = "";
            smtp.Visible = false;
            port.Visible = false;
            password.Visible = false;
            login.Visible = false;
            ssl.Visible = false;
            buttonApplySettings.Visible = false;
            buttonLogs.Visible = true;
            buttonSend.Visible = true;
            buttonSettings.Visible = true;
            this.Height = 220;
        }

        public void ChangeButtonTextToStop()
        {
            buttonSend.Text = "STOP";
            buttonSend.Update();

        }
        public void ChangeButtonTextToSend()
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    buttonSend.Text = "SEND";
                    buttonSend.Update();
                }));
            }
       }

        public string Smtp
        {
            get { return smtp.Text; }
            set { smtp.Text = value; }
        }
        public string Port
        {
            get { return port.Text; }
            set { port.Text = value; }
        }
        public string Login
        {
            get { return login.Text; }
            set { login.Text = value; }
        }
        public string Password
        {
            get { return password.Text; }
            set { password.Text = value; }
        }
        public string Email
        {
            get { return email.Text; }
            set { email.Text = value; }
        }
        public bool Ssl
        {
            get { return ssl.Checked; }
            set { ssl.Checked = value; }
        }
        public string User
        {
            get { return user.Text; }
            set { user.Text = value; }
        }
        public void Progress(int count, int suc, int fail,bool breaking)
        {
            if (this.InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    string process = "";
                    if (breaking) process = "Sending mails has been stoped! ";
                    else process = "Sending emails... Processed ";
                    string s = process + (suc + fail) + " / " + count
                      + " (" + fail + " failed, " + suc + " successful)";
                    if (count == 0)
                        toolStripProgressBar1.Value = 0;
                    else
                    {
                        label9.Text = s;
                        label9.Update();
                        int progress = (suc + fail) * 100 / count;
                        if (progress > 100) progress = 100;
                        if (!breaking)
                            toolStripProgressBar1.Value = progress;
                        else toolStripProgressBar1.Value = 100;
                    }

                }));
            }
        }
    }
}


    