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
            sb.Database = "Rectoran1125";
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

        internal List<Order> GetOrders()
        {
            List<Order> result = new List<Order>();
            string sql = "SELECT o.id, o.idperson, o.idtable, " +
            "td.id AS 'drink_id', td.title AS 'drink_title', td.type, td.cost AS 'drink_cost', td.volume, " +
            "tt.number, tp.FIO, tt.title AS 'table_title', " +
            "tm.id AS 'meal_id', tm.title AS 'meal_title', tm.cost AS 'meal_cost', tm.kalories, tm.gramm " +
            "FROM tbl_order o " +
            "LEFT JOIN tbl_order_drinks d ON o.id = d.idorder " +
            "LEFT JOIN tbl_order_meal m ON o.id = m.idorder " +
            "LEFT JOIN tbl_drinks td ON d.iddrinks = td.id " +
            "LEFT JOIN tbl_meal tm ON m.idmeal = tm.id " +
            "LEFT JOIN tbl_person tp ON o.idperson = tp.id " +
            "LEFT JOIN tbl_table tt ON o.idtable = tt.id ";
            if (OpenConnection())
            {
                Dictionary<int, Drink> drinks = new Dictionary<int, Drink>();
                Dictionary<int, Meal> meals = new Dictionary<int, Meal>();
                Dictionary<int, Order> orders = new Dictionary<int, Order>();

                using (var mc = new MySqlCommand(sql, Connection))
                using (var dr  = mc.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var id = dr.GetInt32("id");
                        Order order;
                        if (orders.ContainsKey(id))
                            order = orders[id];
                        else
                        {
                            order = new Order
                            {
                                ID = id,
                                IdPerson = dr.GetInt32("idperson"),
                                IdTable = dr.GetInt32("idtable"),
                            };

                            order.Table = new Table
                            {
                                ID = dr.GetInt32("idtable"),
                                Number = dr.GetInt32("number"),
                                Title = dr.GetString("table_title")
                            };

                            order.Person = new Person
                            {
                                ID = dr.GetInt32("idperson"),
                                FIO = dr.GetString("FIO")
                            };
                            orders.Add(id, order);
                        }

                        if (!dr.IsDBNull(3))
                        {
                            var drink_id = dr.GetInt32("drink_id");

                            if (!drinks.ContainsKey(drink_id))
                            {
                                drinks.Add(drink_id, new Drink
                                {
                                    ID = drink_id,
                                    Title = dr.GetString("drink_title"),
                                    Type = dr.GetString("type"),
                                    Cost = dr.GetInt32("drink_cost"),
                                    Volume = dr.GetInt32("volume")
                                });
                                order.Drinks.Add(drinks[drink_id]);
                            }
                        }

                        if (!dr.IsDBNull(11))
                        {
                            var meal_id = dr.GetInt32("meal_id");

                            if (!meals.ContainsKey(meal_id))
                            {
                                meals.Add(meal_id, new Meal
                                {
                                    ID = meal_id,
                                    Title = dr.GetString("meal_title"),
                                    Kalories = dr.GetInt32("kalories"),
                                    Cost = dr.GetInt32("meal_cost"),
                                    Gramm = dr.GetInt32("gramm")
                                });
                                order.Meals.Add(meals[meal_id]);
                            }
                        }
                    }
                }

                result = new List<Order>(orders.Values);
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
