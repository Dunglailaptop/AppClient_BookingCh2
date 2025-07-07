using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Service
{
    public class Zone
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name; // hiển thị trên ComboBoxEdit
        }
    }
}
