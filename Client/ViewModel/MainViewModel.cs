using GalaSoft.MvvmLight;
using Client.Communication;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace Client.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private Communication.Client client;
        private bool isConnected = false;
        public string ChatName { get; set; }
        public string Message { get; set; }

        public RelayCommand ConnectBtnClick { get; set; }

        public RelayCommand SendBtnClick { get; set; }

        public ObservableCollection<string> ReceivedMessages { get; set; }

        public MainViewModel()
        {
            Message = "";
            ReceivedMessages = new ObservableCollection<string>();
            ConnectBtnClick = new RelayCommand(
                () =>
                {
                    isConnected = true;
                    client = new Communication.Client("127.0.0.1", 8080, DisconnectClient, new Action<string>(NewMessageReceived));
                    
                },
                () =>
                {
                    return (!isConnected);
                } );

            SendBtnClick = new RelayCommand(
                () =>
                {
                    isConnected = true;
                    client.Send(ChatName + ": " + Message);
                    ReceivedMessages.Add("YOU: " + Message);
                },
                () =>
                {
                    return (isConnected && Message.Length >= 1);
                });

        }

        private void DisconnectClient()
        {
            isConnected = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private void NewMessageReceived(string message)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ReceivedMessages.Add(message);
                //RaisePropertyChanged("ReceivedMessages");
            });

        }
    }
}