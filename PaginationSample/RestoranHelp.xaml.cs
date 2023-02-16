using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaginationSample
{
    /// <summary>
    /// Логика взаимодействия для RestoranHelp.xaml
    /// </summary>
    public partial class RestoranHelp : Window
    {
        public List<Order> Data { get; set; }

        public RestoranHelp()
        {
            InitializeComponent();

            Data = DB.GetInstance().GetOrders();
            DataContext = this;
        }
    }
}
