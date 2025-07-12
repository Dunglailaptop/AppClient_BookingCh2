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
    public class SepcialtyViewModel : INotifyPropertyChanged
    {
        private readonly SepcialtyService _sepcialtyService;


        private BindingList<Sepcialty> _Sepcialty;

        private bool _isLoading;
        private string _searchText;
        private string _errorMessage;





        public BindingList<Sepcialty> Sepcialtys
        {
            get => _Sepcialty;
            set
            {
                _Sepcialty = value;
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


        public SepcialtyViewModel()
        {
            //serviuce
            _sepcialtyService = new SepcialtyService();
            _Sepcialty = new BindingList<Sepcialty>();

            // Khởi tạo commands
            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());


        }


        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var datares = await _sepcialtyService.GetSepcialtysAsync();

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




        // Dispose service khi không sử dụng
        public void Dispose()
        {
            _sepcialtyService?.Dispose();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
