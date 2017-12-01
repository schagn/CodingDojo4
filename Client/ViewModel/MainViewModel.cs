using GalaSoft.MvvmLight;
using Client.Communication;
using System.Collections.ObjectModel;

namespace Client.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private Communication.Client client;
        private bool isConnected = false;
        public string ChatName { get; set; }
        public string Message { get; set; }

        public ObservableCollection<string> ReceivedMessages { get; set; }

        public MainViewModel()
        {
            Message = "";
            ReceivedMessages = new ObservableCollection<string>();   
        }

        private void DisconnectClient()
        {
            isConnected = false;
            client.Close();
        }
    }
}