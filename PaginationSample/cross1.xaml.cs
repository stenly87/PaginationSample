using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для cross1.xaml
    /// </summary>
    public partial class cross1 : Window, INotifyPropertyChanged
    {
        public List<Prepod> Prepods { get; set; }
        public Prepod SelectedPrepod { get; set; }

        public List<Discipline> Disciplines { get; set; }
        public Discipline SelectedDiscipline { get; set; }
        
        public List<DayOfWeek> WeekDays { get; set; }
        public DayOfWeek SelectedDay { get; set; }

        public string PrepodFilter { get; set; }
        public string DisciplineFilter { get; set; }

        public cross1()
        {
            InitializeComponent();            
            WeekDays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();

            DataContext = this;
        }
        void Signal(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public event PropertyChangedEventHandler? PropertyChanged;

        private void Save(object sender, RoutedEventArgs e)
        {
            if (SelectedDay != null && SelectedDiscipline != null &&
                SelectedPrepod != null)
            {
                if (DB.GetInstance().CreatePrepodDisciplineCross(SelectedPrepod, 
                    SelectedDiscipline, SelectedDay))
                    MessageBox.Show("ok");
            }
        }

        private void FilterPrepods(object sender, RoutedEventArgs e)
        {
            Prepods = DB.GetInstance().GetPrepods(PrepodFilter);
            Signal(nameof(Prepods));
        }

        private void FilterDiscipline(object sender, RoutedEventArgs e)
        {
            Disciplines = DB.GetInstance().GetDisciplines(DisciplineFilter);
            Signal(nameof(Disciplines));
        }
    }
}
