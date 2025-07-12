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
    public class DoctorViewModel : INotifyPropertyChanged
    {
        private readonly DoctorService _DoctorService;


        private BindingList<Doctor> _Doctor;

        private bool _isLoading;
        private string _searchText;
        private string _errorMessage;





        public BindingList<Doctor> Doctors
        {
            get => _Doctor;
            set
            {
                _Doctor = value;
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



        // Commands
        public ICommand LoadDataCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }


        public DoctorViewModel()
        {
            //serviuce
            _DoctorService = new DoctorService();
            _Doctor = new BindingList<Doctor>();

            // Khởi tạo commands
            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());


        }


        public async Task LoadDataAsync()
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




        // Dispose service khi không sử dụng
        public void Dispose()
        {
            _DoctorService?.Dispose();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
