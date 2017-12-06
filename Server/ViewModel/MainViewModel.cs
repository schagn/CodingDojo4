using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Server.Communication;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using CodingDojo4_DataHandler;

namespace Server.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private Communication.Server server;
        private const string ip = "127.0.0.1";
        private const int port = 8080;
        private bool isConnected = false;
        private DataHandler dataHandler;

        public ObservableCollection<string> Clients { get; set; }

        public ObservableCollection<string> Messages { get; set; }

        public string SelectedUser { get; set; }

        public int CountReceivedMessages { get { return Messages.Count; } }

        public RelayCommand StartBtnClick { get; set; }

        public RelayCommand StoppBtnClick { get; set; }
        public RelayCommand DropUserBtnClick { get; set; }
        public RelayCommand SaveToLogBtnClick { get; set; }


        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
            Clients = new ObservableCollection<string>();
            dataHandler = new DataHandler();

            StartBtnClick = new RelayCommand(
                () =>
                {
                    server = new Communication.Server(ip, port, UpdateGUIWithNewMessage);
                    server.StartAccepting();
                    isConnected = true;
                },
                () =>
                {
                    return (!isConnected);
                }
                );

            StoppBtnClick = new RelayCommand(
               () =>
               {
                   server.StopAccepting();
                   isConnected = false;
               },
               () =>
               {
                   return (isConnected);
               }
               );

            DropUserBtnClick = new RelayCommand(
               () =>
               {
                   server.DisconnectSpecificClient(SelectedUser);
                   Clients.Remove(SelectedUser);
               },
               () =>
               {
                   return (SelectedUser != null);
               }
               );

            SaveToLogBtnClick = new RelayCommand(
               () =>
               {
                   dataHandler.Save(Messages.ToList());
                   RaisePropertyChanged("LogFiles");
                   
               },
               () =>
               {
                   return (isConnected);
               }
               );

        }

        public void UpdateGUIWithNewMessage (string message)
        {
            App.Current.Dispatcher.Invoke(
                () =>
                {
                    string name = message.Split(':')[0];
                    if(!Clients.Contains(name))
                    {
                        Clients.Add(name);
                    }
                    Messages.Add(message);

                    RaisePropertyChanged("CountReceivedMessages");
                }

                );
        }
    }
}