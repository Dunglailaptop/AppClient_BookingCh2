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
            label1.Text = "Tổng số ca:" + total.ToString();
            label2.Text = "Tổng số phút:" + spacemintues.ToString();
            viewModel.DepartMentAppointSchedulingId = DepartMentAppointSchedulingId;
            viewModel.LoadDataAsyncByDepartMentAppointSchedulingId();
            gridControl1.DataSource = viewModel.TimeSlots;
        }
    }
}
