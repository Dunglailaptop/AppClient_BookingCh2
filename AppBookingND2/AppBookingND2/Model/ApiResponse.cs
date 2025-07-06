using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Model
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } // 1: Thành công, 0: Lỗi
        public string Message { get; set; }

    }
}
