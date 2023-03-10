using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaginationSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Curator> curators;

        public List<Curator> Curators
        {
            get => curators;
            set
            {
                curators = value;
                Signal();
            }
        }

        public List<int> ViewRowsVariants { get; set; }
        public int ViewRowsCount { 
            get => paginator.CountRows;
            set
            {
                paginator.CountRows = value;
                buttonToStart(this, null);
            }
        }

        public string SearchText { get; set; }

        Paginator<Curator> paginator;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            int rowsOnPage = 2;
            paginator = new Paginator<Curator>(
                "SELECT * FROM curator",
                rowsOnPage,
                s => new Curator { 
                    ID = s.GetInt32("id"),
                    FirstName = s.GetString("firstName"), 
                    LastName = s.GetString("lastName") },
                "curator"
                );

            ViewRowsVariants = new List<int>() { 5, 10, 15 };

            buttonToStart(this, null);

            DB.GetInstance().GetCrossPrepodDisciplinesInfo("where idPrepod = 4");
        }

        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler? PropertyChanged;

        private void buttonToStart(object sender, RoutedEventArgs e)
        {
            paginator.PageIndex = 0;
            Curators = paginator.GetPageValues();
        }

        private void buttonBack(object sender, RoutedEventArgs e)
        {
            paginator.PageIndex--;
            Curators = paginator.GetPageValues();
        }

        private void buttonForward(object sender, RoutedEventArgs e)
        {
            paginator.PageIndex++;
            Curators = paginator.GetPageValues();
        }

        private void buttonToEnd(object sender, RoutedEventArgs e)
        {
            paginator.PageIndex = int.MaxValue;
            Curators = paginator.GetPageValues();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            paginator.Query = $"SELECT* FROM curator WHERE lastName LIKE '%{SearchText}%' or firstName LIKE '%{SearchText}%'";
            buttonToStart(this, new RoutedEventArgs());
        }
    }
}
