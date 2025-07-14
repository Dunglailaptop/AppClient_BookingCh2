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

namespace AppBookingND2.SystemModels.Room.View
{
    public partial class Frm_Room : DevExpress.XtraEditors.XtraForm
    {
        private readonly RoomViewModel viewModel;
        public Frm_Room()
        {
            InitializeComponent();
            viewModel = new RoomViewModel();
            setupAsync();
        }
    }
}
namespace AppBookingND2.SystemModels.Room.View
{
    public partial class Frm_Room 
    {
        public async Task setupAsync()
        {
            await viewModel.LoadDataAsync();
            gridControl1.DataSource = viewModel.Rooms;

        }
    }
}