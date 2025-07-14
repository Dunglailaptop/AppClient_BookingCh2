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

namespace AppBookingND2.SystemModels.Zone.View
{
    public partial class Frm_Zone_List : DevExpress.XtraEditors.XtraForm
    {
        private readonly ZoneViewModel viewModel;
        public Frm_Zone_List()
        {
            InitializeComponent();
            viewModel = new ZoneViewModel();
            setupAsync();
        }
    }
}
namespace AppBookingND2.SystemModels.Zone.View
{
    public partial class Frm_Zone_List 
    {
        public async Task setupAsync()
        {
            await viewModel.LoadDataAsync();
            gridControl1.DataSource = viewModel.Zones;

        }
    }
}