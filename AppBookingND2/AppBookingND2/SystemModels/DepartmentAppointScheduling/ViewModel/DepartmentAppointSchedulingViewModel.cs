using AppBookingND2.Helper;
using AppBookingND2.Model;
using AppBookingND2.Service;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace AppBookingND2.ViewModel
{
    public class DepartmentAppointSchedulingViewModel: INotifyPropertyChanged
    {
        private readonly DepartMentAppointSchedulingService _DepartMentAppointSchedulingService;
        private readonly RoomService _RoomService;
        private readonly SepcialtyService _SepcialtyService;
        private readonly DepartMentService _DepartMentService;
        private readonly DoctorService _DoctorService;
        private readonly ExaminationService _ExaminationService;
        private readonly ExamTypeService _ExamTypeService;
    
        private BindingList<DepartMentAppointScheduling> _DepartMentAppointSchedulings;
        private BindingList<Room> _Room;
        private BindingList<Sepcialty> _Sepcialty;
        private BindingList<DepartMent> _DepartMent;
        private BindingList<Doctor> _Doctor;
        private BindingList<Examination> _Examination;
        private BindingList<ComboboxDateInWeek> _ComboboxDateInWeek;
        private BindingList<ExamType> _Examtype;
        private DepartMentAppointScheduling _selectedDepartMentAppointScheduling;
         
        private bool _isLoading;
        private string _searchText;
        private string _errorMessage;
        private int ZoneId;
        private int _Year;
        private int _Week;
        public BindingList<DepartMentAppointScheduling> DepartMentAppointSchedulings
        {
            get => _DepartMentAppointSchedulings;
            set
            {
                _DepartMentAppointSchedulings = value;
                OnPropertyChanged();
            }
        }
        // lấy combobox room
        public BindingList<Room> Rooms
        {
            get => _Room;
            set
            {
                _Room = value;
                OnPropertyChanged();
            }
        }

        public BindingList<ExamType> ExamTypes
        {
            get => _Examtype;
            set
            {
                _Examtype = value;
                OnPropertyChanged();
            }
        }

        public BindingList<DepartMent> DepartMents
        {
            get => _DepartMent;
            set
            {
                _DepartMent = value;
                OnPropertyChanged();
            }
        }
        public BindingList<Examination> Examinations
        {
            get => _Examination;
            set
            {
                _Examination = value;
                OnPropertyChanged();
            }
        }
        public BindingList<Sepcialty> Sepcialtys
        {
            get => _Sepcialty;
            set
            {
                _Sepcialty = value;
                OnPropertyChanged();
            }
        }
        public BindingList<Doctor> Doctors
        {
            get => _Doctor;
            set
            {
                _Doctor = value;
                OnPropertyChanged();
            }
        }

        public BindingList<ComboboxDateInWeek> ComboboxDateInWeeks
        {
            get => _ComboboxDateInWeek;
            set
            {
                _ComboboxDateInWeek = value;
                OnPropertyChanged();
            }
        }
        public int Zone_Id
        {
            get => ZoneId;
            set
            {
                ZoneId = value;
                OnPropertyChanged();
            }
        }

        public DepartMentAppointScheduling SelectedDepartMentAppointScheduling
        {
            get => _selectedDepartMentAppointScheduling;
            set
            {
                _selectedDepartMentAppointScheduling = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDepartMentAppointSchedulingSelected));
            }
        }

       

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }


        public int Year
        {
            get => _Year;
            set
            {
                _Year = value;
                OnPropertyChanged();
            }
        }

        public int Week
        {
            get => _Week;
            set
            {
                _Week = value;
                OnPropertyChanged();
            }
        }

        public bool IsDepartMentAppointSchedulingSelected => SelectedDepartMentAppointScheduling != null;

        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        //Libary Hospital
        public ICommand LoadDataCommand_Room { get; }

        public ICommand LoadDataCommand_Examination { get; }
        public ICommand LoadDataCommand_Doctor { get; }
        public ICommand LoadDataCommand_Sepcialty { get; }
        public ICommand LoadDataCommand_DepartMent { get; }

        public ICommand LoadDataCommand_Examtype { get; }
        public ICommand LoadDataCommand_ComboboxDateInWeek { get; }
        public DepartmentAppointSchedulingViewModel()
        {
            //serviuce
            _DepartMentAppointSchedulingService = new DepartMentAppointSchedulingService();
            _RoomService = new RoomService();
            _SepcialtyService = new SepcialtyService();
            _ExaminationService = new ExaminationService();
            _DoctorService = new DoctorService();
            _DepartMentService =new DepartMentService();
            _ExamTypeService = new ExamTypeService();
            // Bindinglist
            Sepcialtys = new BindingList<Sepcialty>();
            Examinations= new BindingList<Examination>();
            Doctors = new BindingList<Doctor>();
            DepartMents = new BindingList<DepartMent>();
            DepartMentAppointSchedulings = new BindingList<DepartMentAppointScheduling>();
            Rooms = new BindingList<Room>();
            ComboboxDateInWeeks = new BindingList<ComboboxDateInWeek>();
            ExamTypes = new BindingList<ExamType>();
            // Khởi tạo commands
            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            LoadDataCommand_Room = new RelayCommand(async () => await LoadDataAsync_Room());
            LoadDataCommand_Examination = new RelayCommand(async () => await LoadDataAsync_Examination());
            LoadDataCommand_Doctor = new RelayCommand(async () => await LoadDataAsync_Doctor());
            LoadDataCommand_Sepcialty = new RelayCommand(async () => await LoadDataAsync_Sepcialty());
            LoadDataCommand_DepartMent = new RelayCommand(async () => await LoadDataAsync_DepartMent());
            LoadDataCommand_ComboboxDateInWeek = new RelayCommand(async () => await LoadComboboxDateInWeek());
            LoadDataCommand_Examtype = new RelayCommand(async () => await LoadDataAsync_ExamType());
            //RefreshCommand = new RelayCommand(async () => await RefreshDataAsync());
            //DeleteCommand = new RelayCommand(DeleteDepartMentAppointScheduling, () => IsDepartMentAppointSchedulingSelected);
            //SearchCommand = new RelayCommand(SearchDepartMentAppointSchedulings);
            AddCommand = new RelayCommand(async () => await AddDePartMentAppointSchedulingAsync());
            //EditCommand = new RelayCommand(EditDepartMentAppointScheduling, () => IsDepartMentAppointSchedulingSelected);
        }

        public async Task LoadComboboxDateInWeek()
        {
            // Lấy 7 ngày trong tuần 28/2025
            List<DateTime> days = WeekHelper.GetWeekDays(Year, Week);

            // Lấy text đã format
            List<string> dayTexts = WeekHelper.GetWeekDaysFormatted(2025, 28);

            // Bind vào ComboBox
            var comboItems = WeekHelper.GetWeekDaysForComboBox(Year, Week);

            ComboboxDateInWeeks.Clear();
            foreach(var item in days)
            {
                ComboboxDateInWeek date = new ComboboxDateInWeek();
                date.Id = item;
                date.Date = item;
                ComboboxDateInWeeks.Add(date);
            }
        }

       

        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _DepartMentAppointSchedulingService.GetDepartMentAppointSchedulingsAsync(Year,Week,Zone_Id);

                DepartMentAppointSchedulings.Clear();
                foreach (var item in datares)
                {
                    DepartMentAppointSchedulings.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        public async Task LoadDataAsync_Room()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _RoomService.GetRoomByZoneIdAsync(Zone_Id);

                Rooms.Clear();
                foreach (var item in datares)
                {
                    Rooms.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadDataAsync_Examination()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _ExaminationService.GetExaminationsAsync();

                Examinations.Clear();
                foreach (var item in datares)
                {
                    Examinations.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        public async Task LoadDataAsync_Doctor()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _DoctorService.GetDoctorsAsync();

                Doctors.Clear();
                foreach (var item in datares)
                {
                    Doctors.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }


        public async Task LoadDataAsync_ExamType()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _ExamTypeService.GetExamTypesAsync();

                ExamTypes.Clear();
                foreach (var item in datares)
                {
                    ExamTypes.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        public async Task LoadDataAsync_Sepcialty()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _SepcialtyService.GetSepcialtysAsync();

                Sepcialtys.Clear();
                foreach (var item in datares)
                {
                    Sepcialtys.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        public async Task LoadDataAsync_DepartMent()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _DepartMentService.GetDepartMentsAsync();

                DepartMents.Clear();
                foreach (var item in datares)
                {
                    DepartMents.Add(item);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }


        public async Task AddDePartMentAppointSchedulingAsync()
        {
            try
            {
                IsLoading = true;

                // Lọc danh sách chưa có ID (có thể là mới tạo, chưa lưu vào DB)
                var resultNotGetId = DepartMentAppointSchedulings
                    .Where(x => x.Id == 0)
                    .ToList();

                if (!resultNotGetId.Any())
                {
                    MessageBox.Show("Không có lịch hẹn mới để thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Chuyển đổi từng phần tử sang dạng CinicscheduleCreate
                var scheduleCreates = resultNotGetId.Select(x => new CinicscheduleCreate
                {
                    DateInWeek = x.DateInWeek,
                    Total = x.Total,
                    SpaceMinutes = x.SpaceMinutes,
                    SpecialtyId = x.Specialtyid,
                    RoomId = x.RoomId,
                    ExaminationId = x.ExaminationId,
                    DoctorId = x.DoctorId,
                    DepartmentHospitalId = x.DepartmentHospitalId,

                    // Bạn cần cập nhật thêm nếu có thông tin:
                    ExamTypeId = Convert.ToInt32(x.ExamTypeId),            // Gán mặc định hoặc từ dữ liệu khác nếu có
                    StartSlot = x.startSlot.ToString(),       // Ví dụ, hoặc lấy từ giao diện người dùng
                    EndSlot = x.endSlot.ToString(),         // Ví dụ
                    HoldSlot = x.HoldSlot              // Gán mặc định hoặc xử lý logic khác
                }).ToList();

                // Gọi service để tạo các lịch hẹn
                var created = await _DepartMentAppointSchedulingService.CreateDepartMentAppointSchedulingAsync(scheduleCreates);

                if (created)
                {
                    MessageBox.Show("Thêm lịch hẹn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể thêm lịch hẹn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm lịch hẹn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Dispose service khi không sử dụng
    
    

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
