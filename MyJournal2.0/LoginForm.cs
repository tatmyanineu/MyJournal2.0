using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MyJournal2._0
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

            Data.Connect = new NpgsqlConnection("Server=193.138.131.70;Port=5432;User Id=postgres;Password=postgres;Database=Kabinet;");

            Data.Connect.Open();
            NpgsqlCommand sql_dist = new NpgsqlCommand("SELECT id, dist_name FROM district_cnt", Data.Connect);
            NpgsqlDataReader _reader_dist = sql_dist.ExecuteReader();

            while (_reader_dist.Read())
            {
                BoxDistrict.Items.Add(_reader_dist[1].ToString());
            }
            BoxDistrict.SelectedIndex = 0;
            Data.Connect.Close();


            Login.MaxLength = 15;
            Passwd.MaxLength = 15;
            Passwd.PasswordChar = '*';
            if (!File.Exists("settings.xml"))
            {
                XmlTextWriter textWritter = new XmlTextWriter("settings.xml", null);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("settings");
                textWritter.WriteEndElement();
                textWritter.Close();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("settings.xml");
                XmlElement user = xmlDoc.CreateElement("user");
                user.InnerText = "";
                xmlDoc.DocumentElement.AppendChild(user);

                XmlElement passwd = xmlDoc.CreateElement("passwd");
                passwd.InnerText = "";
                xmlDoc.DocumentElement.AppendChild(passwd);

                XmlElement distr = xmlDoc.CreateElement("district");
                distr.InnerText = "";
                xmlDoc.DocumentElement.AppendChild(distr);

                xmlDoc.Save("settings.xml");
            }
            else
            {
                int index_dist = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("settings.xml");
                Login.Text = xmlDoc.GetElementsByTagName("user").Item(0).InnerText;
                Passwd.Text = xmlDoc.GetElementsByTagName("passwd").Item(0).InnerText;
                index_dist = Convert.ToInt32(xmlDoc.GetElementsByTagName("district").Item(0).InnerText);
                BoxDistrict.SelectedIndex = index_dist;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Login.Text != "" && Passwd.Text != "")
            {

                if (checkBox1.Checked == true)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load("settings.xml");

                    xmlDoc.GetElementsByTagName("user").Item(0).InnerText = Login.Text;
                    xmlDoc.GetElementsByTagName("passwd").Item(0).InnerText = Passwd.Text;
                    xmlDoc.GetElementsByTagName("district").Item(0).InnerText = BoxDistrict.SelectedIndex.ToString();

                    xmlDoc.Save("settings.xml");
                }

                string sql_text = @"SELECT id, login_user, passwd_user, fio, position, e_mail, privelege
                                    FROM user_cnt
                                    WHERE login_user=@user AND
                                          passwd_user=@passwd";

                NpgsqlCommand logins = new NpgsqlCommand(sql_text, Data.Connect);
                logins.Parameters.Add("user", NpgsqlTypes.NpgsqlDbType.Text).Value = Login.Text;
                logins.Parameters.Add("passwd", NpgsqlTypes.NpgsqlDbType.Text).Value = Passwd.Text;
                Data.Connect.Open(); //Открываем соединение.
                NpgsqlDataReader _reader = logins.ExecuteReader();
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {

                        Data.Dist_id = BoxDistrict.SelectedIndex;
                        Data.User_id = Convert.ToInt32(_reader[0].ToString());
                        Data.User_info = _reader[3].ToString();
                        Data.User_priveleg = Convert.ToInt32(_reader[6].ToString());
                        JournalForm fj = new JournalForm();
                        this.Hide();
                        fj.ShowDialog();
                        this.Close();

                    }
                }
                else
                {
                    MessageBox.Show("Неверные имя пользователя и пароль");
                }
                Data.Connect.Close(); //Закрываем соединение.
            }
        }
    }
}
