using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PaginationSample
{
    internal class DB
    {
        private DB() { }
        static DB instance;
        public static DB GetInstance()
        {
            if (instance == null)
                instance = new();
            return instance;
        }
        public MySqlConnection Connection { get => connection; }

        MySqlConnection connection;
        public void ConfigureConnection()
        {
            MySqlConnectionStringBuilder sb =
                new MySqlConnectionStringBuilder();
            sb.Server = "192.168.200.13";
            sb.UserID = "student";
            sb.Password = "student";
            sb.Database = "1125_students";
            sb.CharacterSet = "utf8";
            connection = new MySqlConnection(
                sb.ToString());
        }

        public bool OpenConnection()
        {
            if (connection == null)
                ConfigureConnection();
            try
            {
                connection.Open();
            }
            catch (Exception e) {
                System.Windows.MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }

        public void CloseConnection()
        {
            try 
            {
                connection.Close();
            }
            catch 
            { }
        }

        public void TestConnection()
        {
            if (OpenConnection())
            {
                connection.Close();
                System.Windows.MessageBox.Show("Успешно");
            }
        }


    }
}
