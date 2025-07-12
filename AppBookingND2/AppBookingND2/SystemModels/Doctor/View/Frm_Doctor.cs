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

namespace AppBookingND2.SystemModels.Doctor.View
{
    public partial class Frm_Doctor : DevExpress.XtraEditors.XtraForm
    {
        private readonly DoctorViewModel viewModel;
        public Frm_Doctor()
        {
            InitializeComponent();
            viewModel = new DoctorViewModel();
            setupAsync();
        }
    }
}
namespace AppBookingND2.SystemModels.Doctor.View
{
    public partial class Frm_Doctor 
    {
        public async Task setupAsync()
        {
            await viewModel.LoadDataAsync();
            gridControl1.DataSource = viewModel.Doctors;

        }
    }
}