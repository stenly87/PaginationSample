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
using System.Windows.Shapes;

namespace PaginationSample
{
    /// <summary>
    /// Логика взаимодействия для ListCross.xaml
    /// </summary>
    public partial class ListCross : Window, INotifyPropertyChanged
    {
        private List<CrossPrepodDiscipline> data;
        private Prepod selectedPrepod;
        private Discipline selectedDiscipline;
        private DayOfWeek selectedDayOfWeek;

        public List<Prepod> Prepods { get; set; }
        public List<Discipline> Disciplines { get; set; }
        public List<DayOfWeek> DayOfWeeks { get; set; }

        public Prepod SelectedPrepod
        {
            get => selectedPrepod;
            set
            {
                selectedPrepod = value;
                FilterData("where idPrepod = " + value?.ID);
            }
        }

        private void FilterData(string sql)
        {
            Data = DB.GetInstance().GetCrossPrepodDisciplinesInfo(sql);
        }

        public Discipline SelectedDiscipline
        {
            get => selectedDiscipline;
            set
            {
                selectedDiscipline = value;
                FilterData("where idDiscipline = " + value?.ID);
            }
        }
        public DayOfWeek SelectedDayOfWeek
        {
            get => selectedDayOfWeek;
            set
            {
                selectedDayOfWeek = value;
                FilterData("where dayOfWeek = " + (int)value);
            }
        }

        public List<CrossPrepodDiscipline> Data
        {
            get => data;
            set
            {
                data = value;
                Signal();
            }
        }
        public ListCross()
        {
            InitializeComponent();


            Prepods = DB.GetInstance().GetPrepods("");
            Disciplines = DB.GetInstance().GetDisciplines("");
            DayOfWeeks = Enum.GetValues<DayOfWeek>().ToList();
            DataContext = this;
        }

        private void searchAll(object sender, RoutedEventArgs e)
        {
            FilterData($"where idPrepod = {selectedPrepod?.ID} and idDiscipline = {selectedDiscipline?.ID} and dayOfWeek = {(int)SelectedDayOfWeek}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

       
    }
}
