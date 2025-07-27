using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingND2.Model
{
    public class ServicePrice
    {
     
        public int id { get; set; }
        public decimal? regularPrice { get; set; }        // GiaTH - Giá thường
        public decimal? insurancePrice { get; set; }      // GiaBH - Giá bảo hiểm
        public decimal? companyPrice { get; set; }        // GiaCS - Giá công ty, cơ sở
        public decimal? servicePrices { get; set; }        // GiaDV - Giá dịch vụ
        public decimal? foreignerPrice { get; set; }      // GiaNN - Giá nước ngoài
        public decimal? checkupPrice { get; set; }        // GiaKSK - Giá khám sức khỏe
        public decimal? vipPrice { get; set; }            // GiaVIP - Giá VIP


        public string name { get; set; }

        public string description { get; set; }

        public string servicePrice_IdVP_Orcale { get; set; }
        public int? servicePrice_IdPK_Orcale { get; set; }
        public int? servicePrice_Type_Postgresql { get; set; }
        public int hide_Orcale { get; set; }
        public bool enable { get; set; }
    }
}
