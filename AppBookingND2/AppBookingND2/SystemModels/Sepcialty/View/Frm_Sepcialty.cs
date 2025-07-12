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

namespace AppBookingND2.SystemModels.Sepcialty.View
{
    public partial class Frm_Sepcialty : DevExpress.XtraEditors.XtraForm
    {

        private readonly SepcialtyViewModel ViewModel;
        public Frm_Sepcialty()
        {
            InitializeComponent();
            ViewModel = new SepcialtyViewModel();
            setupAsync();
        }
    }
}

namespace AppBookingND2.SystemModels.Sepcialty.View
{
    public partial class Frm_Sepcialty 
    {
        public async Task setupAsync()
        {
            await ViewModel.LoadDataAsync();
            gridControl1.DataSource = ViewModel.Sepcialtys;

        }
    }
}