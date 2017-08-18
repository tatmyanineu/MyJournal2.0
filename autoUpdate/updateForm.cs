using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace autoUpdate
{

    public partial class updateForm : Form
    {
        public updateForm()
        {
            InitializeComponent();
        }
        string progName = null;
        int i = 1;
        private void updateForm_Load(object sender, EventArgs e)
        {
            string remoteUri = "https://vsbt174.ru/ff32f4g345h56nt44/";
            string versionfile = "version.xml", listFiles = "files.xml", myStringWebResource = null;
            WebClient myWebClient = new WebClient();


            string version1 = "";
            string version2 = "";
            if (File.Exists("version.xml"))
            {
                
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("version.xml");
                version1 = xmlDoc.GetElementsByTagName("data").Item(0).InnerText;
                //как то надо закрыть документ для чтения
                xmlDoc = new XmlDocument();
                myWebClient.DownloadFile("" + remoteUri + "" + versionfile + "", versionfile);
                xmlDoc.Load("version.xml");
                version2 = xmlDoc.GetElementsByTagName("data").Item(0).InnerText;

                if (version1 != version2)
                {
                //        myWebClient.DownloadFile("" + remoteUri + "" + listFiles + "", listFiles);
                //        int i = 1;
                //        string progName = null;
                //        XmlTextReader reader = new XmlTextReader(listFiles);
                //        while (reader.Read())
                //        {
                //            switch (reader.NodeType)
                //            {
                //                case XmlNodeType.Text: // Вывести текст в каждом элементе.
                //                    //MessageBox.Show(reader.Value);
                //                    if (i == 1)
                //                    {
                //                        progName = reader.Value;
                //                    }
                //                    myWebClient.DownloadFile("" + remoteUri + "" + reader.Value + "", reader.Value);
                //                    i++;
                //                    break;
                //            }
                //        }
                //        reader.Close();
                //        File.Delete(listFiles);
                //        Process.Start(progName);
                //        this.Close();
                }
                else
                {


                        Process.Start("MyJournal2.0.exe");
                        this.Close();
                 }
            }
            else
            {
                myWebClient.DownloadFile("" + remoteUri + "" + versionfile + "", versionfile);
                myWebClient.DownloadFile("" + remoteUri + "" + listFiles + "", listFiles);


                
               
                XmlTextReader reader = new XmlTextReader(listFiles);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Text: // Вывести текст в каждом элементе.
                                               //MessageBox.Show(reader.Value);
                            if (i == 3)
                            {
                                progName = reader.Value;
                            }
                            //dict.Add(new Uri(remoteUri + reader.Value), reader.Value);
                            //myWebClient.DownloadFile("" + remoteUri + "" + reader.Value + "", reader.Value);
                            //myWebClient.DownloadProgressChanged += (s, e) => progressBar1.Value = e.ProgressPercentage;
                            //myWebClient.DownloadFileAsync(new Uri(remoteUri + "" + reader.Value), reader.Value);
                            dwn(remoteUri, reader.Value);

                            i++;
                            break;
                    }
                }

                reader.Close();
                File.Delete(listFiles);
                Thread.Sleep(1000);
                Process.Start(progName);
                this.Close();

            }

        }


        private void button1_Click(object sender, EventArgs e)
        {

        }
        public void dwn(string url, string fname)
        {
            WebClient web = new WebClient();
            web.DownloadProgressChanged += Web_DownloadProgressChanged;
            web.DownloadFileCompleted += Web_DownloadFileCompleted;
            web.DownloadFileAsync(new Uri(url + fname), fname);
        }

        private void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

            
        }

        private void Web_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Minimum = 0;
            double receive = double.Parse(e.BytesReceived.ToString());
            double total = double.Parse(e.TotalBytesToReceive.ToString());
            double percent = receive / total * 100;
            progressBar1.Value = int.Parse(Math.Truncate(percent).ToString());
        }
    }
}
