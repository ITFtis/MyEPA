using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPASchedule
{
    internal class TotalUnitMsg
    {
        public List<string> Types { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Msg { get; set; }
        public int CitySort { get; set; }
    }
}
