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
        [DisplayName("Tổng slot có thể nhận")]
        public int Total { get; set; }
        [DisplayName("giữ số cho tiếp đón")]
        public int HoldSlot { get; set; }

        [DisplayName("Chuyên khoa")]
        public int Specialtyid { get; set; } = 0;
        [DisplayName("Loại Khám")]
        public FormExam FormExam { get; set; }
        [DisplayName("Khu khám")]
        public int? ExamTypeId { get; set; }

        [DisplayName("Phòng khám")]
        public int RoomId { get; set; } = 0;
        [DisplayName("Ca khám")]
        public int ExaminationId { get; set; } = 0;
        [DisplayName("Bác sĩ")]
        public int DoctorId { get; set; } = 0;
        [DisplayName("Khoa phòng")]
        public int DepartmentHospitalId { get; set; } = 0;
        [DisplayName("Thời gian bắt đầu")]
        public TimeSpan startSlot { get; set; }
        [DisplayName("Thời gian kết thúc")]
        public TimeSpan endSlot { get; set; }

        [DisplayName("Trạng thái")]
        public bool Status { get; set; }

       
        public int SpaceMinutes { get; set; } = 10;

    
    }
    public class CinicscheduleCreate
    {
        public DateTime DateInWeek { get; set; }
        public int Total { get; set; }
        public int SpaceMinutes { get; set; }
        public int SpecialtyId { get; set; }
        public int RoomId { get; set; }
        public int ExaminationId { get; set; }
        public int DoctorId { get; set; }
        public int DepartmentHospitalId { get; set; }
        public int ExamTypeId { get; set; }
        public string StartSlot { get; set; }
        public string EndSlot { get; set; }
        public int HoldSlot { get; set; }

    }
    public enum FormExam
    {

        [Description("Khám chuyên khoa")]
        ChuyenKhoa = 1,
        [Description("Khám tổng quát")]
        TongQuat = 2,
        [Description("Khám sàn lọc")]
        SanLoc = 3,

    }
}
