using AppBookingND2.Helper;
using AppBookingND2.Model;
using AppBookingND2.Service;
using AppBookingND2.SystemModels.TimeSlot.View;
using AppBookingND2.ViewModel;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComboBoxItem = AppBookingND2.Model.ComboBoxItem;

namespace AppBookingND2.View
{
    public partial class Frm_DepartmentAppointScheduling : DevExpress.XtraEditors.XtraForm
    {
        public int id { get; set; }
        private DepartmentAppointSchedulingViewModel viewModel;
        private ZoneViewModel zoneViewModel;
        public Frm_DepartmentAppointScheduling()
        {
            InitializeComponent();
            viewModel = new DepartmentAppointSchedulingViewModel();
           
            SetupEventHandlers();
            ConfigureGridControl();
        }
     
        private void labelControl2_Click(object sender, EventArgs e)
        {

        }

        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !gridView1.IsNewItemRow(gridView1.FocusedRowHandle))
            {
                int currentColumnIndex = gridView1.FocusedColumn?.VisibleIndex ?? -1;
                int lastColumnIndex = gridView1.VisibleColumns.Count - 1;

                if (currentColumnIndex == lastColumnIndex)
                {
                    // Validation trước khi cho xuống dòng
                    if (IsCurrentRowValid())
                    {
                        keyAddnew();
                        gridView1.RefreshData();
                        gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                        gridView1.FocusedColumn = gridView1.VisibleColumns[0];
                        gridView1.ShowEditor();
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = true; // Không cho xuống dòng
                    }
                }
            }
        }

        private void gridView1_CellValueChanged_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "DateInWeek")
            {
                GridView view = sender as GridView;
                DateTime newDate = Convert.ToDateTime(e.Value);

                // Lấy thông tin ngày mới
                DateInfo dateDetails = DateHelper.GetDateDetails(newDate);

                // Cập nhật các cột khác trong cùng dòng
                view.SetRowCellValue(e.RowHandle, "Year", dateDetails.Year);
                view.SetRowCellValue(e.RowHandle, "Week", dateDetails.WeekOfYear);
                view.SetRowCellValue(e.RowHandle, "DayInWeek", dateDetails.DayOfWeekName);

                // Refresh để hiển thị thay đổi
                view.RefreshRow(e.RowHandle);
            }
        }

        private async void simpleButton2_Click(object sender, EventArgs e)
        {
            AddListDepartmentAppointScheduling();
        }
    }
    

}
namespace AppBookingND2.View
{ 
    partial class Frm_DepartmentAppointScheduling
    {  
        private void SetupEventHandlers()
        {
            DateTime now = DateTime.Now;
            DateInfo dateDetails = DateHelper.GetDateDetails(now);
            textEdit1.Text = dateDetails.Year.ToString();
            textEdit2.Text = dateDetails.WeekOfYear.ToString();
            viewModel.Zone_Id = zoneViewModel.id_zone;
            this.Load += async (s, e) =>
            {
                await LoadDataAndBind();
            };
        }
        private async Task LoadDataAndBind()
        {
            try
            {
                // Hiển thị loading
                gridControl1.UseWaitCursor = true;

                // Load data
                await viewModel.LoadDataAsync();
                await viewModel.LoadDataAsync_Room();
                await viewModel.LoadDataAsync_DepartMent();
                await viewModel.LoadDataAsync_Examination();
                await viewModel.LoadDataAsync_Doctor();
                await viewModel.LoadDataAsync_Sepcialty();
                // Bind data
                gridControl1.DataSource = viewModel.DepartMentAppointSchedulings;
                loadcomboboxDepartMentAlternativeAsync();
                loadcomboboxExaminationAlternativeAsync();
                loadcomboboxRoomAlternativeAsync();
                loadcomboboxSepcialtyAlternativeAsync();
                loadcomboboxDoctorAlternativeAsync();

                // Thêm button column CUỐI CÙNG
                AddButtonColumn();

                gridView1.RefreshData();
                gridView1.BestFitColumns();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                gridControl1.UseWaitCursor = false;
            }
        }
        private bool IsCurrentRowValid()
        {
            int rowHandle = gridView1.FocusedRowHandle;
            var currentObject = gridView1.GetRow(rowHandle) as DepartMentAppointScheduling;

            if (currentObject == null) return false;

            // Dictionary để mapping field và display name
                    var validationRules = new Dictionary<string, (object value, string displayName)>
            {
                { "Year", (currentObject.Year, "Năm") },
                { "Week", (currentObject.Week, "Tuần") },
                { "DayInWeek", (currentObject.DayInWeek, "Thứ") },
                { "DateInWeek", (currentObject.DateInWeek, "Ngày") },
                { "Specialtyid", (currentObject.Specialtyid, "Chuyên khoa") },
                { "RoomId", (currentObject.RoomId, "Phòng khám") },
                { "ExaminationId", (currentObject.ExaminationId, "Ca khám") },
                { "DoctorId", (currentObject.DoctorId, "Bác sĩ") },
                { "DepartmentHospitalId", (currentObject.DepartmentHospitalId, "Khoa phòng") }
            };

            foreach (var rule in validationRules)
            {
                var fieldName = rule.Key;
                var value = rule.Value.value;
                var displayName = rule.Value.displayName;

                // Check theo từng loại dữ liệu
                bool isInvalid = false;
                if (value is int intValue)
                {
                    isInvalid = intValue <= 0;
                }
                else if (value is string stringValue)
                {
                    isInvalid = string.IsNullOrEmpty(stringValue);
                }
                else if (value is DateTime dateValue)
                {
                    isInvalid = dateValue == DateTime.MinValue;
                }

                if (isInvalid)
                {
                    MessageBox.Show($"{displayName} không được để trống!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FocusToColumn(fieldName);
                    return false;
                }
            }

            return true;
        }
        private void FocusToColumn(string fieldName)
        {
            var column = gridView1.Columns[fieldName];
            if (column != null)
            {
                gridView1.FocusedColumn = column;
                gridView1.ShowEditor();
            }
        }
        private async Task AddListDepartmentAppointScheduling()
        {

            try
            {
                // Disable button để tránh multiple clicks
                simpleButton2.Enabled = false;

                // Hiển thị loading (optional)
                // loadingPanel.Visible = true;

                await viewModel.AddDePartMentAppointSchedulingAsync();

                // Thông báo thành công
                MessageBox.Show("Thêm lịch hẹn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Enable lại button
                simpleButton2.Enabled = true;

                // Ẩn loading
                // loadingPanel.Visible = false;
            }
        }
        public void keyAddnew()
        {
            // Kiểm tra xem có dòng nào được focus không
            if (gridView1.FocusedRowHandle < 0)
                return;

            try
            {
                // Lấy giá trị của các cột từ hàng hiện tại
                DateTime dateInWeek = Convert.ToDateTime(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DateInWeek"));
                int roomId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "RoomId"));
                int specialtyId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SpecialtyId"));
                int doctorId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DoctorId"));
                int ExaminationId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ExaminationId"));
                int DepartMentId = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DepartmentHospitalId"));
                // Thêm 1 ngày vào ngày hiện tại để tạo dòng mới
                DateTime nextDate = dateInWeek.AddDays(1);
                DateInfo dateDetails = DateHelper.GetDateDetails(nextDate);

                // Thêm dữ liệu mới
                viewModel.DepartMentAppointSchedulings.Add(new DepartMentAppointScheduling
                {
                    Id = 0,
                    Year = dateDetails.Year,
                    Week = dateDetails.WeekOfYear,
                    DayInWeek = dateDetails.DayOfWeekName,
                    DateInWeek = dateDetails.InputDate,
                    Total = 10,
                    Status = true,
                    Specialtyid = specialtyId,
                    RoomId = roomId,
                    ExaminationId = ExaminationId,
                    DoctorId = doctorId,
                    DepartmentHospitalId = DepartMentId,
                   
                });

                // Refresh grid
                gridControl1.RefreshDataSource();

                // Focus vào dòng mới
                gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                gridView1.SelectRow(gridView1.FocusedRowHandle);

                // Focus vào cột DateInWeek để người dùng có thể chỉnh sửa ngay
                gridView1.FocusedColumn = gridView1.Columns["DateInWeek"];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}");
            }
        }
        public async Task loadcomboboxRoomAlternativeAsync()
        {
            RepositoryItemLookUpEdit repoLookUpEdit = new RepositoryItemLookUpEdit();
            var doctorList = new List<ComboBoxItem>();
            BindingList<Room> listRoom = viewModel.Rooms;

            if (listRoom != null)
            {
                foreach (Room item in listRoom)
                {
                    doctorList.Add(new ComboBoxItem
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name
                    });
                }
            }

            repoLookUpEdit.DataSource = doctorList;
            repoLookUpEdit.ValueMember = "Id";
            repoLookUpEdit.DisplayMember = "Name";
            repoLookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            repoLookUpEdit.NullText = "";
            gridControl1.RepositoryItems.Add(repoLookUpEdit);
            gridView1.Columns["RoomId"].ColumnEdit = repoLookUpEdit;

            // Sử dụng GridView event thay vì Repository event
            gridView1.CellValueChanged += GridView1_CellValueChanged;
        }

        private void GridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "RoomId")
            {
                var currentItem = gridView1.GetRow(e.RowHandle) as DepartMentAppointScheduling;
                if (currentItem != null)
                {
                    var selectedId = e.Value?.ToString();
                    if (!string.IsNullOrEmpty(selectedId))
                    {
                        currentItem.RoomId = int.Parse(selectedId);
                        Console.WriteLine($"Room ID updated: {currentItem.RoomId}");
                    }
                }
            }
        }
        public async Task loadcomboboxExaminationAlternativeAsync()
        {
            RepositoryItemLookUpEdit repoLookUpEdit = new RepositoryItemLookUpEdit();
            var doctorList = new List<ComboBoxItem>();
            BindingList<Examination> listRoom = viewModel.Examinations;

            if (listRoom != null)
            {
                foreach (Examination item in listRoom)
                {
                    doctorList.Add(new ComboBoxItem
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name
                    });
                }
            }

            repoLookUpEdit.DataSource = doctorList;
            repoLookUpEdit.ValueMember = "Id";
            repoLookUpEdit.DisplayMember = "Name";
            repoLookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            repoLookUpEdit.NullText = "";
            gridControl1.RepositoryItems.Add(repoLookUpEdit);
            gridView1.Columns["ExaminationId"].ColumnEdit = repoLookUpEdit;

            // Sử dụng GridView event thay vì Repository event
            gridView1.CellValueChanged += GridView1_CellValueChanged_Examination;
        }

        private void GridView1_CellValueChanged_Examination(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ExaminationId")
            {
                var currentItem = gridView1.GetRow(e.RowHandle) as DepartMentAppointScheduling;
                if (currentItem != null)
                {
                    var selectedId = e.Value?.ToString();
                    if (!string.IsNullOrEmpty(selectedId))
                    {
                        currentItem.ExaminationId = int.Parse(selectedId);
                        Console.WriteLine($"Room ID updated: {currentItem.RoomId}");
                    }
                }
            }
        }
        public async Task loadcomboboxDoctorAlternativeAsync()
        {
            RepositoryItemLookUpEdit repoLookUpEdit = new RepositoryItemLookUpEdit();
            var doctorList = new List<ComboBoxItem>();
            BindingList<Doctor> listRoom = viewModel.Doctors;

            if (listRoom != null)
            {
                foreach (Doctor item in listRoom)
                {
                    doctorList.Add(new ComboBoxItem
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name
                    });
                }
            }

            repoLookUpEdit.DataSource = doctorList;
            repoLookUpEdit.ValueMember = "Id";
            repoLookUpEdit.DisplayMember = "Name";
            repoLookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            repoLookUpEdit.NullText = "";
            gridControl1.RepositoryItems.Add(repoLookUpEdit);
            gridView1.Columns["DoctorId"].ColumnEdit = repoLookUpEdit;

            // Sử dụng GridView event thay vì Repository event
            gridView1.CellValueChanged += GridView1_CellValueChanged_Doctor;
        }

        private void GridView1_CellValueChanged_Doctor(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "DoctorId")
            {
                var currentItem = gridView1.GetRow(e.RowHandle) as DepartMentAppointScheduling;
                if (currentItem != null)
                {
                    var selectedId = e.Value?.ToString();
                    if (!string.IsNullOrEmpty(selectedId))
                    {
                        currentItem.DoctorId = int.Parse(selectedId);
                        Console.WriteLine($"Room ID updated: {currentItem.RoomId}");
                    }
                }
            }
        }
        public async Task loadcomboboxSepcialtyAlternativeAsync()
        {
            RepositoryItemLookUpEdit repoLookUpEdit = new RepositoryItemLookUpEdit();
            var doctorList = new List<ComboBoxItem>();
            BindingList<Sepcialty> listRoom = viewModel.Sepcialtys;

            if (listRoom != null)
            {
                foreach (Sepcialty item in listRoom)
                {
                    doctorList.Add(new ComboBoxItem
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name
                    });
                }
            }

            repoLookUpEdit.DataSource = doctorList;
            repoLookUpEdit.ValueMember = "Id";
            repoLookUpEdit.DisplayMember = "Name";
            repoLookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            repoLookUpEdit.NullText = "";
            gridControl1.RepositoryItems.Add(repoLookUpEdit);
            gridView1.Columns["Specialtyid"].ColumnEdit = repoLookUpEdit;

            // Sử dụng GridView event thay vì Repository event
            gridView1.CellValueChanged += GridView1_CellValueChanged_Specialty;
        }

        private void GridView1_CellValueChanged_Specialty(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Specialtyid")
            {
                var currentItem = gridView1.GetRow(e.RowHandle) as DepartMentAppointScheduling;
                if (currentItem != null)
                {
                    var selectedId = e.Value?.ToString();
                    if (!string.IsNullOrEmpty(selectedId))
                    {
                        currentItem.Specialtyid = int.Parse(selectedId);
                        Console.WriteLine($"Room ID updated: {currentItem.RoomId}");
                    }
                }
            }
        }
        public async Task loadcomboboxDepartMentAlternativeAsync()
        {
            RepositoryItemLookUpEdit repoLookUpEdit = new RepositoryItemLookUpEdit();
            var doctorList = new List<ComboBoxItem>();
            BindingList<DepartMent> listRoom = viewModel.DepartMents;

            if (listRoom != null)
            {
                foreach (DepartMent item in listRoom)
                {
                    doctorList.Add(new ComboBoxItem
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name
                    });
                }
            }

            repoLookUpEdit.DataSource = doctorList;
            repoLookUpEdit.ValueMember = "Id";
            repoLookUpEdit.DisplayMember = "Name";
            repoLookUpEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            repoLookUpEdit.NullText = "";
            gridControl1.RepositoryItems.Add(repoLookUpEdit);
            gridView1.Columns["DepartmentHospitalId"].ColumnEdit = repoLookUpEdit;

            // Sử dụng GridView event thay vì Repository event
            gridView1.CellValueChanged += GridView1_CellValueChanged_DepartmentHospital;
        }

        private void GridView1_CellValueChanged_DepartmentHospital(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "DepartmentHospitalId")
            {
                var currentItem = gridView1.GetRow(e.RowHandle) as DepartMentAppointScheduling;
                if (currentItem != null)
                {
                    var selectedId = e.Value?.ToString();
                    if (!string.IsNullOrEmpty(selectedId))
                    {
                        currentItem.DepartmentHospitalId = int.Parse(selectedId);
                        Console.WriteLine($"Room ID updated: {currentItem.RoomId}");
                    }
                }
            }
        }
        // Cấu hình GridControl columns
        private void ConfigureGridControl()
        {
            gridView1.RowHeight = 35;  // Tăng từ ~22 lên 35
            gridView1.Appearance.Row.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
            gridView1.Appearance.Row.Options.UseFont = true;
            gridView1.Appearance.HeaderPanel.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;

            gridView1.OptionsBehavior.Editable = true;
            gridView1.OptionsBehavior.AllowAddRows = DefaultBoolean.True;
            gridView1.OptionsView.NewItemRowPosition = NewItemRowPosition.Bottom;
            gridView1.OptionsBehavior.KeepFocusedRowOnUpdate = false; // Cho phép di chuyển focu

            // Thêm button column
           
            //// Hoặc nếu muốn hiển thị nhưng không cho edit cho đến khi valid
            //gridView1.OptionsView.ShowNewItemRow = true;

        }
        private void AddButtonColumn()
        {
            var columnExists = gridView1.Columns.Any(c => c.Caption == "Thao tác" || c.FieldName == "btnColumn");
            if (columnExists)
                return;

            GridColumn buttonColumn = gridView1.Columns.AddVisible("btnColumn", "Thao tác");
            buttonColumn.Name = "ThaoTacButtonColumn";
            buttonColumn.UnboundType = UnboundColumnType.String;
            buttonColumn.Width = 120; // Tăng chiều rộng
            buttonColumn.OptionsColumn.AllowEdit = true;
            buttonColumn.OptionsColumn.ShowCaption = true;

            RepositoryItemButtonEdit buttonEdit = new RepositoryItemButtonEdit();
            buttonEdit.TextEditStyle = TextEditStyles.HideTextEditor;
            buttonEdit.BorderStyle = BorderStyles.NoBorder;

            // Sử dụng icon thay vì text để tiết kiệm không gian
            EditorButton editButton = new EditorButton(ButtonPredefines.Glyph);
            editButton.Caption = "Xem";
            editButton.Tag = "Edit";
            editButton.Kind = ButtonPredefines.Glyph;
            // Nếu có icon, thêm vào đây
            //editButton.Image = Properties.Resources.;

            EditorButton deleteButton = new EditorButton(ButtonPredefines.Glyph);
            deleteButton.Caption = "Xóa";
            deleteButton.Tag = "Delete";
            deleteButton.Kind = ButtonPredefines.Glyph;
            // deleteButton.Image = Properties.Resources.delete_icon;

            buttonEdit.Buttons.Add(deleteButton);
            buttonEdit.Buttons.Add(editButton);

            buttonColumn.ColumnEdit = buttonEdit;
            buttonEdit.ButtonClick += ButtonEdit_ButtonClick;
            gridControl1.RepositoryItems.Add(buttonEdit);
        }

        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                int rowHandle = gridView1.FocusedRowHandle;
                if (rowHandle < 0) return;

                // Lấy dữ liệu từ row
                object rowData = gridView1.GetRow(rowHandle);

                switch (e.Button.Tag?.ToString())
                {
                    case "Edit":
                        EditRecord(rowHandle);
                        break;
                    case "Delete":
                        DeleteRecord(rowHandle);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void EditRecord(int rowHandle)
        {
            int Id = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id"));
            int Total = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Total"));
            int SpaceMintues = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "SpaceMinutes"));
            Frm_TimeSlot frm = new Frm_TimeSlot();
            frm.total = Total;
            frm.DepartMentAppointSchedulingId = Id;
            frm.spacemintues = SpaceMintues;
            frm.ShowDialog();
            

        }

        private void DeleteRecord(int rowHandle)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gridView1.DeleteRow(rowHandle);
                MessageBox.Show("Đã xóa thành công!");
            }
        }
    }

}