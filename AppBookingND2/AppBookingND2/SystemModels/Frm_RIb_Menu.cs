using AppBookingND2.SystemModels.Zone;
using AppBookingND2.View;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
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

namespace AppBookingND2
{
    public partial class Frm_RIb_Menu : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Frm_RIb_Menu()
        {
            InitializeComponent();
        }
        void OpenFrom(Type typefrom)
        {
            foreach (Form frm in MdiChildren)
            {
                if (frm.GetType() == typefrom)
                {
                    frm.Activate();
                    return;
                }
            }
            Form f = (Form) Activator.CreateInstance(typefrom);
            f.MdiParent = this;
            f.Show();
        }
        public void SetupFrom(XtraForm xtraForm)
        {

            xtraForm.MdiParent = this;
            xtraForm.Show();
        }
        private void Frm_RIb_Menu_Load(object sender, EventArgs e)
        {
       
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
           OpenFrom(typeof(Frm_Department));
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenFrom(typeof(Frm_Room));
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frm_Zone frm_Zone = new Frm_Zone();
            if (frm_Zone.ShowDialog() == DialogResult.OK)
            {
              
                OpenFrom(typeof(Frm_DepartmentAppointScheduling));
            }

           
        }
    }
}