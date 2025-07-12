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

namespace AppBookingND2.SystemModels.Examination.View
{
    public partial class Frm_Examination : DevExpress.XtraEditors.XtraForm
    {
        private readonly ExaminationViewModel viewModel; 
        public Frm_Examination()
        {
            InitializeComponent();
            viewModel = new ExaminationViewModel();
            setupAsync();
        }
    }
}
namespace AppBookingND2.SystemModels.Examination.View
{
    public partial class Frm_Examination 
    {
        public async Task setupAsync()
        {
            await viewModel.LoadDataAsync();
            gridControl1.DataSource = viewModel.Examinations;

        }
    }
}