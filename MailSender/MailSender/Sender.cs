using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using System.Windows.Forms;


namespace MailSender
{
    
    public class Decoder
    {
        public string Coding(string password)     //процедура "Шифрование". используем шифр Виженера.
        {
            string key = "vtyfdhgjhm";
            string all = @"`1234567890-=~!@#$%^&*()_+qwertyuiop[]QWERTYUIOP{}asdfghjkl;'\ASDFGHJKL:""|ZXCVBNM<>?zxcvbnm,./№ёЁйцукенгшщзхъЙЦУКЕНГШЩЗХЪфывапролджэФЫВАПРОЛДЖЭячсмитьбюЯЧСМИТЬБЮ";//все символы, которые могут быть использовани при вводе пароляя
            string st; int center;                           //объявление новых переменных.
            string leftSlice, rightSlice, cPass = "";

            if (key.Length > password.Length)               //если длина строки пароля (ключа для входа в программу и для шифрования)>длины строки пароля (какого-либо сайта и т.д.),
            {
                key = key.Substring(0, password.Length);    //то переменная key обрежется и станет равной длинне пароля 
            }
            else                                            // Иначе повторять ключ (ключключключклю), пока не станет равным длинне пароля
                for (int i = 0; key.Length < password.Length; i++)
                {
                    key = key + key.Substring(i, 1);
                }
            //основной цикл шифрования
            for (int i = 0; i < password.Length; i++)
            {//находим центр строки all (центр - это будущий первый символ строки со сдвигом)
                center = all.IndexOf(key.Substring(i, 1));
                leftSlice = all.Substring(center); //берем левую часть будущей строки со сдвигом
                rightSlice = all.Substring(0, center);// затем правую
                st = leftSlice + rightSlice;// формируем строку со сдвигом
                center = all.IndexOf(password.Substring(i, 1));// теперь в переменную center запишем индекс очередного символа шифруемой строки
                cPass += st.Substring(center, 1);    //поскольку индексы символа из строки со сдвигом и из обычной строки совпадают, то нужный нам символ берется по такому же индексу
            }

            return cPass;
        }

        public string Uncoding(string password)
        {
            string key = "vtyfdhgjhm";
            // строка all содержит все символы, которые можно вводить с русской и англ раскладки клавиатуры
            string all = @"`1234567890-=~!@#$%^&*()_+qwertyuiop[]QWERTYUIOP{}asdfghjkl;'\ASDFGHJKL:""|ZXCVBNM<>?zxcvbnm,./№ёЁйцукенгшщзхъЙЦУКЕНГШЩЗХЪфывапролджэФЫВАПРОЛДЖЭячсмитьбюЯЧСМИТЬБЮ";
            //строка st со сдвигом по ключу (в качестве ключа используем наш пароль для входа)
            string st; int center; // центр указывает на индекс символа, до которого идет сдвиг по ключу.
            string leftSlice, rightSlice, cPass = ""; //leftSlice, rightSlice - правый срез, левый срез. из них составляется строка со сдвигом st. 

            //если пароль короче ключа - обрезаем ключ
            if (key.Length > password.Length)
            {
                key = key.Substring(0, password.Length);
            }
            //Иначе повторяем ключ, пока он не примет длинну пароля.
            else
                for (int i = 0; key.Length < password.Length; i++)
                {
                    key = key + key.Substring(i, 1);
                }
            // основной цикл расшифрования.
            for (int i = 0; i < password.Length; i++)
            {
                //находим центр строки all (центр - это будущий первый символ строки со сдвигом)
                center = all.IndexOf(key.Substring(i, 1));
                leftSlice = all.Substring(center); //берем левую часть будущей строки со сдвигом
                rightSlice = all.Substring(0, center);// затем правую
                st = leftSlice + rightSlice; // формируем строку со сдвигом
                center = st.IndexOf(password.Substring(i, 1)); // теперь в переменную center запишем индекс очередного символа расшифроввываемой строки
                cPass += all.Substring(center, 1); //поскольку индексы символа из строки со сдвигом и из обычной строки совпадают, то нужный нам символ берется по такому же индексу
            }
            return cPass; //возвращаем расшифрованный пароль.
        }
    }
    public class SettingsData
    {     
        private SmtpClient m_Smtp;
        private string m_Login;
        private string m_Password;
        private string m_Email;
        private string m_FolderPath = "";
        private MailAddress m_From;
        public string FolderPath
        {
            get { return m_FolderPath; }
            set { m_FolderPath = value; }
        }
        public string Login
        {
            get { return m_Login; }
            set { m_Login = value; }
        }
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }
        public SmtpClient Smtp
        {
            get { return m_Smtp; }
            set { m_Smtp = value; }
        }

        public MailAddress From
        {
            get { return m_From; }
            set {m_From = value; }
        }
        public void SetSettings(string smtpHost, int port, bool ssl, string login, string password, string email)
        {
            try
            {
                Smtp = new SmtpClient(smtpHost, port);
                Smtp.EnableSsl = ssl;
                Login = login;
                Email = email;
                Password = password;
            }
            catch (Exception e)
            {
               MessageBox.Show( "Problem with creation SMTP connetion: " + e.Message);
            }
        }
    }
    public class SettingsControl
    {
        private SettingsData m_Settings;
        private Decoder m_Decoder;
        public SettingsControl()
        {
            m_Settings = new SettingsData();
            m_Decoder = new Decoder();
            ReadConfigFile();
        }
        public SettingsData Settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }
        public bool ReadConfigFile()
        {
            try
            {
                if (File.Exists("config.ini"))
                {
                    StreamReader streamreader = new StreamReader("config.ini", Encoding.UTF8);
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = streamreader.ReadLine();
                    smtp.Port = Convert.ToInt16(streamreader.ReadLine());
                    if ("True" == streamreader.ReadLine()) smtp.EnableSsl = true;
                    else smtp.EnableSsl = false;
                    Settings.Login = streamreader.ReadLine();
                    Settings.Password = m_Decoder.Uncoding(streamreader.ReadLine());
                    Settings.Email = streamreader.ReadLine();
                    Settings.From = new MailAddress(Settings.Email);
                    smtp.Credentials = new NetworkCredential(Settings.Login, Settings.Password);
                    Settings.Smtp = smtp;
                    streamreader.Close();
                    return true;
                }
                else return false;

            }
            catch (Exception e)
            {
                MessageBox.Show("Read config.ini failed : " + e.Message);
                return false;
            }
        }
        public void WriteConfigFile()
        {
            try
            {

                StreamWriter streamwriter = new StreamWriter("config.ini", false, Encoding.UTF8);
                streamwriter.WriteLine(Settings.Smtp.Host);
                streamwriter.WriteLine(Settings.Smtp.Port);
                streamwriter.WriteLine(Settings.Smtp.EnableSsl);
                streamwriter.WriteLine(Settings.Login);
                streamwriter.WriteLine(m_Decoder.Coding(Settings.Password));
                streamwriter.WriteLine(Settings.Email);
                streamwriter.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show("Create config.ini failed : " + e.Message);
            }
        }

    }
    public class Logging
    {
        private string LogName = "";
        private bool LogCreate = false;

        public bool isCreate
        {
            get { return LogCreate; }
            set { LogCreate = value; }
        }
        public string Name
        {
            get { return LogName; }
            set { LogName = value; }
        }
        public void Create()
        {
            DateTime localDate = DateTime.Now;
            LogName = "log " + localDate.Year + "." + localDate.Month + "." + localDate.Day + " " + localDate.Hour + "-" + localDate.Minute + "-" + localDate.Second;
            var culture = new CultureInfo("ru-RU");
            Directory.CreateDirectory("Logs");
        }
        public bool Write(string text)
        {
            try
            {               
                StreamWriter streamwriter = new StreamWriter(@"Logs\" + LogName + ".txt", true, Encoding.UTF8);
                var culture = new CultureInfo("ru-RU");
                streamwriter.WriteLine(DateTime.Now.ToString(culture) +"  "+ text);
                streamwriter.Close();
                LogCreate = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Write log failed: " + e.Message);
                return false;
            }
        }


        public static bool OpenFolderLogs()
        {
            if (Directory.Exists("Logs"))
            {
                System.Diagnostics.Process.Start("explorer", "Logs");
                return true;
            }
            else return false;
        }
    }
    public class EventArgsSendedMail : EventArgs
    {
        private int СountFile;
        private int SucMail;
        private int FailMail;
        public EventArgsSendedMail()
        { }
        public EventArgsSendedMail(int countFiles, int sucMail, int failMail)
        {
            СountFile = countFiles;
            SucMail = sucMail;
            FailMail = failMail;
        }

        public int CountFiles
        {
            get { return СountFile; }
            set { СountFile = value; }
        }
        public int FailMails
        {
            get { return FailMail; }
            set { FailMail = value; }
        }
        public int SucMails
        {
            get { return SucMail; }
            set { SucMail = value; }
        }
    }
    public class Sender
    {
        public EventHandler<EventArgsSendedMail> MailSendedEvent;
        public EventHandler<EventArgs> SendingComplitEvent;
        public EventHandler<EventArgs> BreakSendingEvent;

        private SettingsControl Configuration;
        private Logging Log;
        bool StopButtonPressed;
        private MailMessage Mail;

        public Sender()
        {
            Log = new Logging();
            Configuration = new SettingsControl();
            BreakSendingEvent = new EventHandler<EventArgs>(CheckButtonPressed);

        }

        public bool StopButtonPress
        {
            get { return StopButtonPressed; }
            set { StopButtonPressed = value; }
        }

        private void CheckButtonPressed(object sender, EventArgs e)
        {
            StopButtonPress = true;
        }

        private void ReplaceSended(string path)
        {
            
            try {
             string folderPath = Settings.FolderPath;
            if (!Directory.Exists(folderPath + @"\" + "Sended"))
                Directory.CreateDirectory(folderPath + @"/" + "Sended");
                if (File.Exists(folderPath + @"\Sended\" + Path.GetFileName(path)))
                    File.Delete(folderPath + @"\Sended\" + Path.GetFileName(path));
             File.Move(path, folderPath  + @"\Sended\"+ Path.GetFileName(path));
             Log.Write( "file " + Path.GetFileName(path) + " move in Sended");         
        }
            catch (Exception e)
            {
                Log.Write("file " + Path.GetFileName(path) + " has NOT move! Error: "+e.Message);
            }

}    

        public string GetFolderPath()
        {
            return Settings.FolderPath;
        }

        public bool SetFolderPath()
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();
            fBD.ShowNewFolderButton = false;
            fBD.Description = "Choose folder with *.csv files";
            if (fBD.ShowDialog() == DialogResult.OK)
            {
                Settings.FolderPath = fBD.SelectedPath;
                Log.isCreate = false;

                return true;
            }
            else return false;
        }
  
        public SettingsData Settings 
            { get{ return Configuration.Settings;}
              set { Configuration.Settings = value; }
            }     

        public async void Sending(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() => FindCSV(),
                                             TaskCreationOptions.LongRunning);          
        }

        public  void FindCSV()
        {
            try
            {
                if (Settings.FolderPath != "")
                {
                    int FailMail = 0;
                    int SucMail = 0;
                    int СountFiles = 0;
                    Log.Create();
                    var files = Directory.EnumerateFiles(Settings.FolderPath, "*.csv");
                    СountFiles = files.Count();
                    
                    if (files.Count() == 0)
                        Log.Write(" *.csv files NOT FOUND in " + Settings.FolderPath);
                    else
                    {
                        EventArgsSendedMail eventArgs = new EventArgsSendedMail(СountFiles, 0, 0);
                        foreach (var file in files)
                        {
                            Task.Delay(500).Wait();
                            if (StopButtonPressed) 
                       
                            {
                                Log.Write("User cancelled process!");
                                StopButtonPressed = false;
                                break;
                            }
                            Log.Write("read  " + Path.GetFileName(file));
                            if (ReadMailAdress(file))
                                if (SendMessage(file))
                                    SucMail++;
                                else FailMail++;
                            else FailMail++;
                            eventArgs.FailMails = FailMail;
                            eventArgs.SucMails = SucMail;

                            EventHandler<EventArgsSendedMail> eh2;
                            lock (this) { eh2 = MailSendedEvent; }
                            if (eh2 != null) eh2(this, eventArgs);

                        }
                        

                    }
                    Log.Write(SucMail + " letters have been sent successful");
                    Log.Write(FailMail + " letters have NOT been sent");
                    
                    
                    EventHandler<EventArgs> eh3;
                    lock (this) { eh3 = SendingComplitEvent; }
                    if (eh3 != null) eh3(this, EventArgs.Empty);
                    if (Log.isCreate) MessageBox.Show("Finish! Log with name \"" + Log.Name + ".txt\" has been created");

                }
            }
            catch (Exception ex)
            {
                EventHandler<EventArgs> eh3;
                lock (this) { eh3 = SendingComplitEvent; }
                if (eh3 != null) eh3(this, new EventArgsSendedMail());

  
                Log.Write("The process failed. " + ex.Message);
            }

        }

        private bool ReadMailAdress(string currentCSV)

        {
            string mailAdress = "";
            try
            {
                string MailMessage = "";
                string Attachment = "";
                StreamReader streamreader = new StreamReader(currentCSV, Encoding.UTF8);
                string str = streamreader.ReadLine();
                string[] arrayOfStrings = str.Split(';');
                mailAdress = arrayOfStrings[0];
                if (arrayOfStrings.Length > 1)
                    MailMessage = arrayOfStrings[1];
                streamreader.Close();
                string fileName = Path.GetFileNameWithoutExtension(currentCSV);
                if (File.Exists(Settings.FolderPath + @"\" + fileName + ".pdf"))
                    Attachment = Settings.FolderPath + @"\" + fileName + ".pdf";
                else Attachment = "";
                MailAddress To = new MailAddress(mailAdress);
                Mail = new MailMessage(Settings.From, To);
                Mail.Body = MailMessage;
                Mail.IsBodyHtml = false;
                if (Attachment != "") Mail.Attachments.Add(new Attachment(Attachment));
                return true;
            }
            catch (SmtpFailedRecipientsException e)
            {
                Log.Write("ERROR sending mail to " + mailAdress + "! Error: " + e.Message);
                return false;
            }
            catch (Exception e)
            {
                Log.Write("ERROR reading " + Path.GetFileName(currentCSV) + "! Error: " + e.Message);
                return false;
            }
        }

        private bool SendMessage(string currentCSV)
        {
            try
            {    
               
                Settings.Smtp.Send(Mail);
                string log = "SUCCESSFUL sending mail to " + Mail.To.ToString()+ " with message \"" + Mail.Body + "\"";          
                if (Mail.Attachments.Count>0)
                    if (Mail.Attachments[0]!= null)
                    {
                        log += ". With attachment: "+ Mail.Attachments[0].Name;
                        string attachment = Settings.FolderPath + @"\" + Mail.Attachments[0].Name;
                        Mail.Attachments.Dispose();
                        ReplaceSended(attachment);
                    }
                else log += ". Without attachments!";             
                Log.Write(log);              
                ReplaceSended(currentCSV);         
                return true;

            }
            catch (Exception e)
            { 
                Log.Write("ERROR sending mail to " + Mail.To.ToString() + "! Error: " + e.Message);
                return false;
            }
        }

    }
}