using AppBookingND2.ViewModel;
using DevExpress.Utils.Extensions;
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

namespace AppBookingND2.SystemModels.Zone
{
    public partial class Frm_Zone : DevExpress.XtraEditors.XtraForm
    {
        private readonly ZoneViewModel zoneViewModel;
        public Frm_Zone()
        {
            InitializeComponent();
            zoneViewModel = new ZoneViewModel();
            setupAsync();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int selectedId = Convert.ToInt32(lookUpEdit1.EditValue);
            var lectedZone = zoneViewModel.Zones.FirstOrDefault(z => z.Id == selectedId);
            Console.WriteLine(lectedZone);
            zoneViewModel.id_zone = lectedZone.Id;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
namespace AppBookingND2.SystemModels.Zone
{
    partial class Frm_Zone
    {
      public async Task setupAsync()
        {
            zoneViewModel.LoadDataAsync();
            lookUpEdit1.Properties.DataSource = zoneViewModel.Zones;
            lookUpEdit1.Properties.DisplayMember = "Name";
            lookUpEdit1.Properties.ValueMember = "Id";
            lookUpEdit1.EditValue = zoneViewModel.Zones.FirstOrDefault()?.Id;
        }
    
    }
}