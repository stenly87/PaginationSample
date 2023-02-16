using System.Collections.Generic;

namespace PaginationSample
{
    public class Order
    {
        public int ID { get; internal set; }
        public int IdPerson { get; internal set; }
        public int IdTable { get; internal set; }

        public List<Drink> Drinks { get; set; } = new List<Drink>();
        public List<Meal> Meals { get; set; } = new List<Meal>();
        public Table Table { get;  set; }
        public Person Person { get; internal set; }
    }
}