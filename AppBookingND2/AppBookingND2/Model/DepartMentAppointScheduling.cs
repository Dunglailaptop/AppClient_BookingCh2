using DevExpress.Utils.Serializing.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Model
{
    public class DepartMentAppointScheduling
    {
        [Browsable(false)]
        public int Id { get; set; }
        [DisplayName("Năm")]
        public int Year { get; set; }
        [DisplayName("Tuần")]
        public int Week { get; set; }
        [DisplayName("Thứ")]
        public string DayInWeek { get; set; }
        [DisplayName("Ngày")]
        public DateTime DateInWeek { get; set; }
        [DisplayName("Chuyên khoa")]
        public int Specialtyid { get; set; } = 0;
        [DisplayName("Phòng khám")]
        public int RoomId { get; set; } = 0;
        [DisplayName("Ca khám")]
        public int ExaminationId { get; set; } = 0;
        [DisplayName("Bác sĩ")]
        public int DoctorId { get; set; } = 0;
        [DisplayName("Khoa phòng")]
        public int DepartmentHospitalId { get; set; } = 0;
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
        [Browsable(false)]
        public int Total { get; set; } = 20;
        [Browsable(false)]
        public int SpaceMinutes { get; set; } = 10;
    }

}
