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
    
        private BindingList<DepartMentAppointScheduling> _DepartMentAppointSchedulings;
        private BindingList<Room> _Room;
        private BindingList<Sepcialty> _Sepcialty;
        private BindingList<DepartMent> _DepartMent;
        private BindingList<Doctor> _Doctor;
        private BindingList<Examination> _Examination;
        private DepartMentAppointScheduling _selectedDepartMentAppointScheduling;
        private bool _isLoading;
        private string _searchText;
        private string _errorMessage;
        private int ZoneId;

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

        public DepartmentAppointSchedulingViewModel()
        {
            //serviuce
            _DepartMentAppointSchedulingService = new DepartMentAppointSchedulingService();
            _RoomService = new RoomService();
            _SepcialtyService = new SepcialtyService();
            _ExaminationService = new ExaminationService();
            _DoctorService = new DoctorService();
            _DepartMentService =new DepartMentService();
            // Bindinglist
            Sepcialtys = new BindingList<Sepcialty>();
            Examinations= new BindingList<Examination>();
            Doctors = new BindingList<Doctor>();
            DepartMents = new BindingList<DepartMent>();
            DepartMentAppointSchedulings = new BindingList<DepartMentAppointScheduling>();
            Rooms = new BindingList<Room>();
            // Khởi tạo commands
            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            LoadDataCommand_Room = new RelayCommand(async () => await LoadDataAsync_Room());
            LoadDataCommand_Examination = new RelayCommand(async () => await LoadDataAsync_Examination());
            LoadDataCommand_Doctor = new RelayCommand(async () => await LoadDataAsync_Doctor());
            LoadDataCommand_Sepcialty = new RelayCommand(async () => await LoadDataAsync_Sepcialty());
            LoadDataCommand_DepartMent = new RelayCommand(async () => await LoadDataAsync_DepartMent());
            //RefreshCommand = new RelayCommand(async () => await RefreshDataAsync());
            //DeleteCommand = new RelayCommand(DeleteDepartMentAppointScheduling, () => IsDepartMentAppointSchedulingSelected);
            //SearchCommand = new RelayCommand(SearchDepartMentAppointSchedulings);
            AddCommand = new RelayCommand(async () => await AddDePartMentAppointSchedulingAsync());
            //EditCommand = new RelayCommand(EditDepartMentAppointScheduling, () => IsDepartMentAppointSchedulingSelected);
        }
        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _DepartMentAppointSchedulingService.GetDepartMentAppointSchedulingsAsync();

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

                var datares = await _RoomService.GetRoomsAsync(Zone_Id);

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
                var ResultNotGetId = DepartMentAppointSchedulings.Where(x => x.Id == 0).ToList();
                var createdEmployee = await _DepartMentAppointSchedulingService.CreateDepartMentAppointSchedulingAsync(ResultNotGetId);

                if (createdEmployee == true)
                {
                    //Employees.Add(createdEmployee);
                    MessageBox.Show("Thêm nhân viên thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
        // Dispose service khi không sử dụng
        public void Dispose()
        {
            _DepartMentAppointSchedulingService?.Dispose();
        }
    

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
