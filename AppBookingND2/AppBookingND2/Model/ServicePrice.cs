using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Model
{
    public class ServicePrice
    {

        public int Id { get; set; }
        public decimal? RegularPrice { get; set; }        // GiaTH - Giá thường
        public decimal? InsurancePrice { get; set; }      // GiaBH - Giá bảo hiểm
        public decimal? CompanyPrice { get; set; }        // GiaCS - Giá công ty, cơ sở
        public decimal? ServicePrices { get; set; }        // GiaDV - Giá dịch vụ
        public decimal? ForeignerPrice { get; set; }      // GiaNN - Giá nước ngoài
        public decimal? CheckupPrice { get; set; }        // GiaKSK - Giá khám sức khỏe
        public decimal? VipPrice { get; set; }            // GiaVIP - Giá VIP


        public string Name { get; set; }

        public string Description { get; set; }

        public string ServicePrice_IdVP_Orcale { get; set; }
        public int? ServicePrice_IdPK_Orcale { get; set; }
        public int? ServicePrice_Type_Postgresql { get; set; }
        public int Hide_Orcale { get; set; }
        public bool Enable { get; set; }
    }
}
