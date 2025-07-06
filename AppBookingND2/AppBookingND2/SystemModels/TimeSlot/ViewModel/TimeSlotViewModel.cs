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
    public class TimeSlotViewModel : INotifyPropertyChanged
    {
        private readonly TimeSlotService _TimeSlotService;


        private BindingList<TimeSlot> _TimeSlots;
    
        private bool _isLoading;
        private string _searchText;
        private string _errorMessage;

        private int _DepartMentAppointSchedulingId;



        public BindingList<TimeSlot> TimeSlots
        {
            get => _TimeSlots;
            set
            {
                _TimeSlots = value;
                OnPropertyChanged();
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

        public int DepartMentAppointSchedulingId
        {
            get => _DepartMentAppointSchedulingId;
            set
            {
                _DepartMentAppointSchedulingId = value;
                OnPropertyChanged();
            }
        }


        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }

        public ICommand LoadDataCommandByDepartMentAppointScheduling { get; }
        public TimeSlotViewModel()
        {
            //serviuce
            _TimeSlotService = new TimeSlotService();
            TimeSlots = new BindingList<TimeSlot>();
            // Khởi tạo commands
            LoadDataCommandByDepartMentAppointScheduling = new RelayCommand(async () => await LoadDataAsyncByDepartMentAppointSchedulingId());

           
        }


        public async Task LoadDataAsyncByDepartMentAppointSchedulingId()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _TimeSlotService.GetTimeSlotByDepartmentAppointSchedulingIdAsync(_DepartMentAppointSchedulingId);

                TimeSlots.Clear();
                foreach (var item in datares)
                {
                    TimeSlots.Add(item);
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
 
  

   
        // Dispose service khi không sử dụng
        public void Dispose()
        {
            _TimeSlotService?.Dispose();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
