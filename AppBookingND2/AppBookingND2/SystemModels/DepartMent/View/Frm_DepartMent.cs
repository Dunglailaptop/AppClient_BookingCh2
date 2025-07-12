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

namespace AppBookingND2.SystemModels.DepartMent.View
{
    public partial class Frm_DepartMent : DevExpress.XtraEditors.XtraForm
    {
        private readonly DepartMentViewModel viewModel;
        public Frm_DepartMent()
        {
            InitializeComponent();
            viewModel = new DepartMentViewModel();
            setupAsync();
        }
    }
}
namespace AppBookingND2.SystemModels.DepartMent.View
{
    public partial class Frm_DepartMent 
    {
        public async Task setupAsync()
        {
            await viewModel.LoadDataAsync();
            gridControl1.DataSource = viewModel.DepartMents;

        }
    }
}