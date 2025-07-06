using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Service
{
    public class TimeSlot
    {
        [Browsable(false)]
        public int Id { get; set; }
        public int DepartmentalAppointmentSchedulingId { get; set; }
        public int? OnlineRegistrationId { get; set; }
        public TimeSpan Slot { get; set; }

        public int STT { get; set; }

        public bool Enable { get; set; }
    }
}
