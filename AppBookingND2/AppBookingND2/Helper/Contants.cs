using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Helper
{
    public class DateInfo
    {
        public int Year { get; set; }
        public int WeekOfYear { get; set; }
        public int DayOfWeekNumber { get; set; } // Số ngày (1 = Thứ Hai, 7 = Chủ Nhật)
        public string DayOfWeekName { get; set; } // Tên ngày trong tuần
        public DateTime InputDate { get; set; }
    }
    public static class DateHelper
    {
        public static DateInfo GetDateDetails(DateTime date)
        {
            DateInfo result = new DateInfo
            {
                Year = date.Year,
                InputDate = date,
                // Tính tuần thứ mấy của năm (theo chuẩn ISO 8601)
                WeekOfYear = GetIso8601WeekOfYear(date),
                // Tính số ngày trong tuần (1 = Thứ Hai, 7 = Chủ Nhật)
                DayOfWeekNumber = ((int)date.DayOfWeek + 6) % 7 + 1,
                // Gán tên ngày trong tuần
                DayOfWeekName = GetDayOfWeekName(((int)date.DayOfWeek + 6) % 7 + 1)
            };

            return result;
        }

        // Hàm tính tuần theo chuẩn ISO 8601
        private static int GetIso8601WeekOfYear(DateTime date)
        {
            DayOfWeek day = date.DayOfWeek;
            DateTime startOfYear = new DateTime(date.Year, 1, 1).AddDays(day == DayOfWeek.Sunday ? -6 : (1 - (int)day));
            return (int)Math.Floor((date - startOfYear).TotalDays / 7.0) + 1;
        }

        // Hàm ánh xạ số ngày thành tên ngày trong tuần
        private static string GetDayOfWeekName(int dayNumber)
        {
            switch (dayNumber)
            {
                case 1: return "Thứ Hai";
                case 2: return "Thứ Ba";
                case 3: return "Thứ Tư";
                case 4: return "Thứ Năm";
                case 5: return "Thứ Sáu";
                case 6: return "Thứ Bảy";
                case 7: return "Chủ Nhật";
                default: return "Không xác định";
            }
        }
    }
    public static class Contants
    {

    }
}
