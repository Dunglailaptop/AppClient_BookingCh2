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

namespace AppBookingND2.SystemModels.ServicePrice.View
{
    public partial class Frm_ServicePrice : DevExpress.XtraEditors.XtraForm
    {
        private readonly ServicePriceViewModel viewModel;
        public Frm_ServicePrice()
        {
            InitializeComponent();
            viewModel = new ServicePriceViewModel();
            setupAsync();
        }
    }
}
namespace AppBookingND2.SystemModels.ServicePrice.View
{
    public partial class Frm_ServicePrice
    {
        public async Task setupAsync()
        {
            await viewModel.LoadDataAsync();
            gridControl1.DataSource = viewModel.ServicePrices;

        }
    }
}
