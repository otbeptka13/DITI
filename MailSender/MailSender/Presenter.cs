using System;

namespace MailSender
{ 
    class Presenter
    {
        
        private Sender MailSender;
        private MailSenderForm UI;
        bool Sending = false;

        public Presenter(MailSenderForm mailSenderform)
        {
            UI = mailSenderform;
            MailSender = new Sender();
           
            UI.buttonSend.Click += new EventHandler(OnPressSend);
            UI.buttonEdit.Click += new EventHandler(OnPressEdit);
            UI.buttonLogs.Click += new EventHandler(OnPressLogs);
            UI.buttonSettings.Click += new EventHandler(OnPressSettings);
            UI.buttonApplySettings.Click += new EventHandler(OnPressApplySettings);
            UI.Shown+= new EventHandler(OnFormCreate);
            MailSender.SendingComplitEvent = new EventHandler<EventArgs>(OnSendedComplit);
            MailSender.MailSendedEvent = new EventHandler<EventArgsSendedMail>(OnProgressChanged);          
        }

        private void OnSendedComplit(object sender, EventArgs e)
        {         
            UI.ChangeButtonTextToSend();
            Sending = false;
        }

        private void OnProgressChanged(object sender, EventArgsSendedMail e)
        {
            int count = e.CountFiles;
            int suc = e.SucMails;
            int fail = e.FailMails;
             UI.Progress(count, suc, fail,false);
        }

        private void  OnPressSend(object sender, EventArgs e)
        {

            if (Sending)
            {
                EventHandler<EventArgs> eh;
                lock (this) { eh = MailSender.BreakSendingEvent; }
                if (eh != null) eh(this, EventArgs.Empty);
                Sending = false;

            }
            else
            {
                if (MailSender.GetFolderPath() != "")
                {
                    Sending = true;
                    MailSender.Sending(this,EventArgs.Empty);
                    UI.ChangeButtonTextToStop();
                    
                }
                else
                    if (MailSender.SetFolderPath())
                {
                    UI.SetFolderPath(MailSender.GetFolderPath());
                    UI.ChangeButtonTextToStop();
                    Sending = true;
                    MailSender.Sending(this, EventArgs.Empty);

                }

            }
                           
        }

        private void OnPressEdit(object sender, EventArgs e)
        {
          if(!Sending)
            if (MailSender.SetFolderPath())
            {
                MailSender.GetFolderPath();
                UI.SetFolderPath(MailSender.GetFolderPath());
                }
        }

        private void OnFormCreate(object sender, EventArgs e)
        {
            SettingsData Settings = MailSender.Settings;
                if (MailSender.Settings.Email != null)
                    UI.User = MailSender.Settings.Email;
                else UI.OpenSettings();
        }

        private void OnPressLogs(object sender, EventArgs e)
        {
            try
            {
                if (!Logging.OpenFolderLogs())
                    UI.ShowMessage("No logs have been created yet!");
            }

            catch (Exception ex)
            {
                UI.ShowMessage("Error! Problem with opening folder: " + ex.Message);
            }
        }

        private void OnPressSettings(object sender, EventArgs e)
        {
            if (!Sending)
                try
            {
            SettingsData settings = MailSender.Settings;
                if (settings != null)
                {
                    UI.Login = settings.Login;
                    UI.Password = settings.Password;
                    UI.Smtp = settings.Smtp.Host;
                    UI.Port = Convert.ToString(settings.Smtp.Port);
                    UI.Ssl = settings.Smtp.EnableSsl;
                    UI.Email = settings.Email;
                }
            UI.OpenSettings();
            }

            catch (Exception ex)
            {
                UI.ShowMessage("Error! Problem with  showing settings : " + ex.Message);
            }
        }

        private void OnPressApplySettings(object sender, EventArgs e)
        {
            if((UI.Smtp == "") || (UI.Port == "") || (UI.Login == "") || (UI.Password == "") || (UI.Email == ""))
                UI.ShowMessage("Fill in all the fields!");
            else
            try
            {
                SettingsControl settingsControl = new SettingsControl();
                SettingsData settings = settingsControl.Settings;
                settings.Smtp = new System.Net.Mail.SmtpClient(UI.Smtp, Convert.ToInt16(UI.Port));
                settings.Smtp.EnableSsl = UI.Ssl;
                settings.Login = UI.Login;
                settings.Password = UI.Password;
                settings.Smtp.Credentials = new System.Net.NetworkCredential(settings.Login, settings.Password);    // логин и пароль
                settings.Email = UI.Email;
                settings.From = new System.Net.Mail.MailAddress(settings.Email);
                MailSender.Settings=settings;
                settingsControl.WriteConfigFile();
                UI.User = settings.Email;
                UI.CloseSettings();
                   
            }
            catch (Exception ex)
            {
                UI.ShowMessage("Incorrect connection's settings. Error: " + ex.Message);
            }
        }

    }
}
