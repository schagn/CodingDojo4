using GalaSoft.MvvmLight;
using Server.Communication;
using System.Collections.ObjectModel;

namespace Server.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private Communication.Server server;
        private const string ip = "127.0.0.1";
        private const int port = 8080;
        private bool isConnected = false;

        public ObservableCollection<ClientHandler> Clients { get; set; }

        public ObservableCollection<string> Messages { get; set; }

        public string SelectedUser { get; set; }

        public int CountReceivedMessages { get { return Messages.Count; } }


        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
            Clients = new ObservableCollection<ClientHandler>();
        }

        public void UpdateGUIWithNewMessage (string message)
        {

        }
    }
}