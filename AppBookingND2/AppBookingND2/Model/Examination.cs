using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Model
{
    public class Examination
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TimeSpan startTime { get; set; }

        public TimeSpan endTime { get; set; }

    }
}
