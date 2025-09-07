namespace AppBookingND2.Network
{
    public static class ApiUrlConstants
    {
        // Base route prefix
        private const string ApiPrefix = "/api";

        // Doctor
        public static string D_List => $"{ApiPrefix}/doctor/list";
        public static string D_Detail(int id) => $"{ApiPrefix}/doctor/{id}";
        public static string D_Create => $"{ApiPrefix}/doctor/create";
        public static string D_Update(int id) => $"{ApiPrefix}/doctor/{id}";
        public static string D_Delete(int id) => $"{ApiPrefix}/doctor/{id}";

        // Department
        public static string De_List => $"{ApiPrefix}/department/list";
        public static string De_Detail(int id) => $"{ApiPrefix}/department/{id}";
        public static string De_Create => $"{ApiPrefix}/department/create";
        public static string De_Update(int id) => $"{ApiPrefix}/department/{id}";
        public static string De_Delete(int id) => $"{ApiPrefix}/department/{id}";

        // ExamType
        public static string Ex_List => $"{ApiPrefix}/exam-type/list";
        public static string Ex_Detail(int id) => $"{ApiPrefix}/exam-type/{id}";
        public static string Ex_Create => $"{ApiPrefix}/exam-type/create";
        public static string Ex_Update(int id) => $"{ApiPrefix}/exam-type/{id}";
        public static string Ex_Delete(int id) => $"{ApiPrefix}/exam-type/{id}";

        // Price
        public static string P_List => $"{ApiPrefix}/service-price/list";
        public static string P_Detail(int id) => $"{ApiPrefix}/service-price/{id}";
        public static string P_Create => $"{ApiPrefix}/service-price/create";
        public static string P_Update(int id) => $"{ApiPrefix}/Prservice-priceices/{id}";
        public static string P_Delete(int id) => $"{ApiPrefix}/service-price/{id}";

        // Room
        public static string R_List => $"{ApiPrefix}/room/list";
        public static string R_List_ZoneId(int ZoneId) => $"{ApiPrefix}/room/{ZoneId}";
        public static string R_Detail(int id) => $"{ApiPrefix}/room/{id}";
        public static string R_Create => $"{ApiPrefix}/room/create";
        public static string R_Update(int id) => $"{ApiPrefix}/room/{id}";
        public static string R_Delete(int id) => $"{ApiPrefix}/room/{id}";

        // Specialty
        public static string Se_List => $"{ApiPrefix}/specialty/list";
        public static string Se_Detail(int id) => $"{ApiPrefix}/specialty/{id}";
        public static string Se_Create => $"{ApiPrefix}/specialty/create";
        public static string Se_Update(int id) => $"{ApiPrefix}/specialty/{id}";
        public static string Se_Delete(int id) => $"{ApiPrefix}/specialty/{id}";
        // Zone
        public static string Z_List => $"{ApiPrefix}/zone/list";
        public static string Z_Detail(int id) => $"{ApiPrefix}/zone/{id}";
        public static string Z_Create => $"{ApiPrefix}/zone/create";
        public static string Z_Update(int id) => $"{ApiPrefix}/zone/{id}";
        public static string Z_Delete(int id) => $"{ApiPrefix}/zone/{id}";
        // Examination
        public static string Exam_List => $"{ApiPrefix}/examination/list";
        public static string Exam_Detail(int id) => $"{ApiPrefix}/examination/{id}";
        public static string Exam_Create => $"{ApiPrefix}/examination/create";
        public static string Exam_Update(int id) => $"{ApiPrefix}/examination/{id}";
        public static string Exam_Delete(int id) => $"{ApiPrefix}/examination/{id}";
        // Timeslot
        public static string T_List => $"{ApiPrefix}/time-slot/list";
        public static string T_Detail(int id) => $"{ApiPrefix}/time-slot/{id}";
        public static string T_Detail_ClinicSchedule(int ClinicScheduleId) => $"{ApiPrefix}/time-slot/{ClinicScheduleId}";
        public static string T_Create => $"{ApiPrefix}/time-slot/create";
        public static string T_Update(int id) => $"{ApiPrefix}/time-slot/{id}";
        public static string T_Delete(int id) => $"{ApiPrefix}/time-slot/{id}";
        //
        // ClinicSchedule
        public static string clinic_List(int Week,int Year, int ZoneId) => $"{ApiPrefix}/clinic-schedule/list?Week={Week}&Year={Year}&ZoneId={ZoneId}";
        public static string clinic_Detail(int id) => $"{ApiPrefix}/clinic-schedule/{id}";
        public static string clinic_Create => $"{ApiPrefix}/clinic-schedule/create";

    }
}
