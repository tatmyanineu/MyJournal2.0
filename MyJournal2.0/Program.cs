using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace MyJournal2._0
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }

       
    }
    static class Data
    {
        public static string Value { get; set; }
        public static int User_id { get; set; }
        public static int Dist_id { get; set; }
        public static string User_info { get; set; }
        public static int User_priveleg { get; set; }
        public static NpgsqlConnection Connect { get; set; }

    }
}
