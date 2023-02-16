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

        internal List<Prepod>? GetPrepods(string prepodFilter)
        {
            List<Prepod> result = new List<Prepod>();
            if (OpenConnection())
            {
                string sql = $"select * from tbl_prepods where firstName like '%{prepodFilter}%' or lastName like '%{prepodFilter}%'";
                using (var mc = new MySqlCommand(
                    sql, Connection))
                using (var dr = mc.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        result.Add(new Prepod {
                         ID = dr.GetInt32("id"),
                         LastName = dr.GetString("lastName"),
                         FirstName = dr.GetString("firstName")
                        });
                    }
                }
               CloseConnection();
            }
            return result;
        }

        internal List<CrossPrepodDiscipline> GetCrossPrepodDisciplinesInfo(string whereSql)
        {
            string sql = "SELECT c.id, c.idPrepod, c.idDiscipline, c.dayOfWeek, td.title, tp.firstName, tp.lastName FROM crossPrepodDiscipline c JOIN tbl_prepods tp ON c.idPrepod = tp.id JOIN tbl_discipline td ON c.idDiscipline = td.id " + whereSql;
            List<CrossPrepodDiscipline> result = new List<CrossPrepodDiscipline>();
            if (OpenConnection())
            {
                using (var mc = new MySqlCommand(sql, connection))
                using (var dr = mc.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        result.Add(new CrossPrepodDiscipline { 
                          Id = dr.GetInt32("id"),
                          IdPrepod = dr.GetInt32("idPrepod"),
                          IdDiscipline = dr.GetInt32("idDiscipline"),
                          DayOfWeek = (DayOfWeek)dr.GetInt32("dayOfWeek"),
                          Discipline = new Discipline {
                           ID = dr.GetInt32("idDiscipline"),
                           Title = dr.GetString("title"),
                          },
                          Prepod = new Prepod {
                              ID = dr.GetInt32("idPrepod"),
                              FirstName = dr.GetString("firstName"),
                              LastName = dr.GetString("lastName"),
                          }
                        });
                    }
                }
                CloseConnection();
            }
            return result;
        }

        internal bool CreatePrepodDisciplineCross(Prepod selectedPrepod, Discipline selectedDiscipline, DayOfWeek selectedDay)
        {
            int rows = 0;
            if (OpenConnection())
            {
                string sql = "insert into crossPrepodDiscipline " +
                    $"(idPrepod, idDiscipline, dayOfWeek) values ({selectedPrepod.ID}, {selectedDiscipline.ID}, {(int)selectedDay})";
                rows = MySqlHelper.ExecuteNonQuery(connection, sql);
                CloseConnection();
            }
            return rows > 0;
        }

        internal List<Discipline>? GetDisciplines(string disciplineFilter)
        {
            List<Discipline> result = new List<Discipline>();
            if (OpenConnection())
            {
                string sql = $"select * from tbl_discipline where title like '%{disciplineFilter}%'";
                using (var mc = new MySqlCommand(
                    sql, Connection))
                using (var dr = mc.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        result.Add(new Discipline
                        {
                            ID = dr.GetInt32("id"),
                            Title = dr.GetString("title"),
                        });
                    }
                }
                CloseConnection();
            }
            return result;
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
