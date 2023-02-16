using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaginationSample
{
    public class CrossPrepodDiscipline
    {
        public int Id { get; set; }
        public int IdPrepod { get; set; }
        public int IdDiscipline { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public Prepod Prepod { get; set; }
        public Discipline Discipline { get; set; }
    }

}
