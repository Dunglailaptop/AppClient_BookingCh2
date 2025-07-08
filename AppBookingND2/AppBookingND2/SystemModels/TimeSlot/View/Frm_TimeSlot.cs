using AppBookingND2.ViewModel;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppBookingND2.SystemModels.TimeSlot.View
{
    public partial class Frm_TimeSlot : DevExpress.XtraEditors.XtraForm
    {
        private readonly TimeSlotViewModel viewModel;
        public Frm_TimeSlot()
        {
            InitializeComponent();
            viewModel = new TimeSlotViewModel();
           
        }

        private void Frm_TimeSlot_Load(object sender, EventArgs e)
        {
            Setup();
        }
    }
}

namespace AppBookingND2.SystemModels.TimeSlot.View
{
    partial class Frm_TimeSlot
    {
        public int DepartMentAppointSchedulingId{ get; set; }

        public int total { get; set; }

        public int spacemintues { get; set; }
        private async Task Setup()
        {
            viewModel.DepartMentAppointSchedulingId = DepartMentAppointSchedulingId;
            await viewModel.LoadDataAsyncByDepartMentAppointSchedulingId();

            // Kiểm tra data đã load chưa
            if (viewModel.TimeSlots != null && viewModel.TimeSlots.Count > 0)
            {
                gridControl1.DataSource = viewModel.TimeSlots;
                label1.Text = "Tổng số ca:" + viewModel.Totals;
                label2.Text = "Tổng số phút:" + spacemintues.ToString();
            }
            else
            {
                // Xử lý khi không có data
                gridControl1.DataSource = null;
                label1.Text = "Tổng số ca: 0";
                label2.Text = "Tổng số phút: 0";
            }
        }
    }
}
