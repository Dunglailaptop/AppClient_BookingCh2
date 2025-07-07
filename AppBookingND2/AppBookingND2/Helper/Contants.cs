using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Helper
{
    public class WeekHelper
    {
        /// <summary>
        /// Lấy danh sách ngày trong tuần
        /// </summary>
        /// <param name="year">Năm</param>
        /// <param name="week">Số tuần trong năm (1-53)</param>
        /// <returns>List DateTime của 7 ngày trong tuần (từ thứ 2 đến chủ nhật)</returns>
        public static List<DateTime> GetWeekDays(int year, int week)
        {
            // Tính ngày đầu tuần (thứ 2)
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)DayOfWeek.Monday - (int)jan1.DayOfWeek;
            if (daysOffset > 0) daysOffset -= 7;

            DateTime firstMonday = jan1.AddDays(daysOffset);
            DateTime startOfWeek = firstMonday.AddDays(7 * (week - 1));

            // Tạo danh sách 7 ngày
            List<DateTime> weekDays = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                weekDays.Add(startOfWeek.AddDays(i));
            }

            return weekDays;
        }

        /// <summary>
        /// Lấy danh sách ngày trong tuần với format text
        /// </summary>
        /// <param name="year">Năm</param>
        /// <param name="week">Số tuần trong năm (1-53)</param>
        /// <param name="format">Format hiển thị (mặc định: "dd/MM/yyyy - dddd")</param>
        /// <returns>List string đã format</returns>
        public static List<string> GetWeekDaysFormatted(int year, int week, string format = "dd/MM/yyyy - dddd")
        {
            List<DateTime> weekDays = GetWeekDays(year, week);
            List<string> formattedDays = new List<string>();

            foreach (DateTime day in weekDays)
            {
                formattedDays.Add(day.ToString(format, new CultureInfo("vi-VN")));
            }

            return formattedDays;
        }

        /// <summary>
        /// Lấy danh sách ngày trong tuần cho ComboBox
        /// </summary>
        /// <param name="year">Năm</param>
        /// <param name="week">Số tuần trong năm (1-53)</param>
        /// <returns>List KeyValuePair với Key là display text, Value là DateTime</returns>
        public static List<KeyValuePair<string, DateTime>> GetWeekDaysForComboBox(int year, int week)
        {
            List<DateTime> weekDays = GetWeekDays(year, week);
            List<KeyValuePair<string, DateTime>> comboBoxItems = new List<KeyValuePair<string, DateTime>>();

            foreach (DateTime day in weekDays)
            {
                string displayText = day.ToString("dd/MM/yyyy - dddd", new CultureInfo("vi-VN"));
                comboBoxItems.Add(new KeyValuePair<string, DateTime>(displayText, day));
            }

            return comboBoxItems;
        }
    }
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
