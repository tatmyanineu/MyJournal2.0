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

        private void updateForm_Load(object sender, EventArgs e)
        {
            string remoteUri = "https://vsbt174.ru/ff32f4g345h56nt44/";
            string versionfile = "version.xml", listFiles = "files.xml", myStringWebResource = null;
            WebClient myWebClient;

            string version1="";
            string version2="";
            if (File.Exists("version.xml"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("version.xml");
                version1 = xmlDoc.GetElementsByTagName("data").Item(0).InnerText;
                myWebClient = new WebClient();
                myWebClient.DownloadFile("" + remoteUri + "" + versionfile + "", versionfile);
                xmlDoc.Load("version.xml");
                version2 = xmlDoc.GetElementsByTagName("data").Item(0).InnerText;

                if (version1 != version2)
                {
                    myWebClient.DownloadFile("" + remoteUri + "" + listFiles + "", listFiles);
                    int i = 1;
                    string progName = null;
                    XmlTextReader reader = new XmlTextReader(listFiles);
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Text: // Вывести текст в каждом элементе.
                                //MessageBox.Show(reader.Value);
                                if (i == 1)
                                {
                                    progName = reader.Value;
                                }
                                myWebClient.DownloadFile("" + remoteUri + "" + reader.Value + "", reader.Value);
                                i++;
                                break;
                        }
                    }
                    reader.Close();
                    File.Delete(listFiles);
                    Process.Start(progName);
                    this.Close();
                }
                else
                {
                    myWebClient.DownloadFile("" + remoteUri + "" + listFiles + "", listFiles);

                }
            }
            else
            {
                myWebClient = new WebClient();
            
                //myWebClient.DownloadFile("" + remoteUri + "" + versionfile + "", versionfile);
                //myWebClient.DownloadFile("" + remoteUri + "" + listFiles + "", listFiles);

                Dictionary<Uri, string> dict = new Dictionary<Uri, string>();
                dict.Add(new Uri(remoteUri + "" + versionfile), versionfile);
                dict.Add(new Uri(remoteUri + "" + listFiles), listFiles); 

                //XmlDocument xmlFiles = new XmlDocument();
                //xmlFiles.Load(listFiles);
                //for(int i=1; )





   
                //int i = 1;
                //string progName = null;
                //XmlTextReader reader = new XmlTextReader(listFiles);
                //while (reader.Read())
                //{
                //    switch (reader.NodeType)
                //    {
                //        case XmlNodeType.Text: // Вывести текст в каждом элементе.
                //            //MessageBox.Show(reader.Value);
                //            if (i == 1)
                //            {
                //                progName = reader.Value;
                //            }
                //            //dict.Add(new Uri(remoteUri + reader.Value), reader.Value);
                //            myWebClient.DownloadFile("" + remoteUri + "" + reader.Value + "", reader.Value);
                //            myWebClient.DownloadProgressChanged += (s, e) => progressBar1.Value = e.ProgressPercentage;
                //            //myWebClient.DownloadFileAsync(new Uri(remoteUri + "" + reader.Value), reader.Value);
                //            i++;
                //            break;
                //    }
                //}

                //reader.Close();
                //File.Delete(listFiles);
                //Process.Start(progName);
                //this.Close();
            }
        }

      
        public void upd(Dictionary<Uri, string> files)
        {
            foreach (KeyValuePair<Uri, string> pair in files)
            {

            }
        }

        public async Task DownloadManyFiles(Dictionary<Uri, string> files)
        {
            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += (s, e) => progressBar1.Value = e.ProgressPercentage;
            foreach (KeyValuePair<Uri, string> pair in files) { await wc.DownloadFileTaskAsync(pair.Key, pair.Value); }
            wc.Dispose();



        }


        public void ipdateFiles()
        {
            string remoteUri = "https://vsbt174.ru/ff32f4g345h56nt44/";
            string versionfile = "version.xml", listFiles = "files.xml", myStringWebResource = null;

            Dictionary<Uri, string> dict = new Dictionary<Uri, string>();
            dict.Add(new Uri(remoteUri + versionfile), versionfile);
            dict.Add(new Uri(remoteUri + listFiles), listFiles);
            DownloadManyFiles(dict);
        }
       
    }
}
