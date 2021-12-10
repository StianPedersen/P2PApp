using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using myapp.MVVM.Model;
using myapp.Core;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;


namespace myapp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject 
    {
       
        /* Commands */
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        
        private Server _server;
        private Program _listener;
        private SaveChat _saveChat;

        private readonly DelegateCommand _createServerCommand;
        public ICommand CreateServerCommand => _createServerCommand;

        public RelayCommand ConnectToServerCommand { get; set; }
        
        public RelayCommand SendMessageCommand { get; set; }
        public RelayCommand SendBuzzCommand { get; set; }
        public RelayCommand SaveChatCommand { get; set; }
        public RelayCommand ShowHistoryCommand { get; set; }

        private string _name = "Default";
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        
        private string _ip = "127.0.0.1";
        public string Ip 
        {
            get { return _ip; }
            set 
            { 
                _ip = value; 
                OnPropertyChanged(); }
            }

        private int _port = 8300;
        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        private int _width = 1200;
        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
        private int _height = 650;
        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }


        private string _message;
        public string Message
        {
            get { return _message; }
            set 
            {
                _message = value;
                OnPropertyChanged(); 
            }

        }

        
        public MainViewModel()
        {
            
            Users = new ObservableCollection<UserModel> ();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _saveChat = new SaveChat();
            
            _server.connectedEvent += UserConnected;
            _server.msgRecievedEvent += MessageRecieved;
            _server.disconnectEvent += RemoveUser;
            _server.noListenerEvent += NoListener;
            _server.ShakeScreenEvent += ShakeScreen;
            
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(_ip, _port, _name), o => !string.IsNullOrEmpty(_name));
            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
            SendBuzzCommand = new RelayCommand(o => _server.SendBuzz());
            SaveChatCommand = new RelayCommand(o => SaveChat());
            ShowHistoryCommand = new RelayCommand(o => ShowHistory());
            _createServerCommand = new DelegateCommand(OnCreateServerCommand);
          
        }

        private bool AcceptConnection()
        {
            string message = "CONNECT?";
            string caption = "ACCEPT CONNECTION";
            // Displays the MessageBox.
            // Kan köras await!
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                // Closes the parent form.
                return true;
            }
            return false;
        }
        private void UserConnected()
        {
            
            var newuser = new UserModel { Username = _server.PacketReader.ReadMessage(), UID = _server.PacketReader.ReadMessage() };
            
           if(!Users.Any(x => x.UID == newuser.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(newuser));

            }
        }
        private void MessageRecieved()
        {
            var msg = _server.PacketReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));


        }
        private void NoListener()
        {
            MessageBox.Show("Nobody listening to this port", "Error", MessageBoxButton.OK);
        }
        private void ShakeScreen()
        {
            System.Media.SystemSounds.Asterisk.Play();
        }
        private void RemoveUser()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
            

        }
        private void OnCreateServerCommand(object CommandParameter)
        {
           


                Task.Run(() =>
                {

                    Program _listener = new Program();
                    _listener.acceptEvent += AcceptConnection;
                    _listener.Function(_ip, _port); //while loop
            });
                _server.ConnectToServer(_ip, _port, _name);
            
           
        }


        //Shyffla ut
        private void SaveChat()
        {
                _saveChat.SaveChatFunction(Messages);         
        }

        private void ShowHistory()
        {

            Window _hist = new HistoryWindow();
            _hist.Show();

        }

    }

    
}
