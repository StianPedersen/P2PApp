using myapp.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace myapp.MVVM.ViewModel
{

    class HistoryViewModel : ObservableObject
    {
        public ObservableCollection<string> History { get; set; }
        public ObservableCollection<string> SearchedHistory { get; set; }

        public RelayCommand SearchCommand { get; set; }
        private string _chosenFile = "";
        public string ChosenFile
        {
            get { return _chosenFile; }
            set
            {
                var filename = value;
                //DirectoryInfo d = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory"));
                if (value != null)
                {
                    filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory", value);

                    string xmlString = System.IO.File.ReadAllText(filename);
                    _chosenFile = xmlString;
                    OnPropertyChanged();
                }
            }
        }

        public string _searchWord = "";
        public string SearchWord
        {
            get { return _searchWord; }
            set
            {
                _searchWord = value;
                OnPropertyChanged();
            }
        }


        public HistoryViewModel()
        {
            History = new ObservableCollection<string>();
            SearchedHistory = new ObservableCollection<string>();
            SearchCommand = new RelayCommand(o => find_files());
            init_history();    
            
     
        }
        public void init_history()
        {
            DirectoryInfo d = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory")); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
            string str = "";
            
            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;
                History.Add(file.Name);
            }
        }

        public void find_files()
        {
            //SearchWord = "something";
            String strPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory");
            String search = SearchWord;

            var files = from file in Directory.GetFiles(strPath, "*", SearchOption.AllDirectories)
                        where File.ReadAllLines(file).Any(x => x.Contains("[" + search))
                        select file;
            string filename;
            SearchedHistory.Clear();
            foreach (var file in files)
            {
                filename = Path.GetFileNameWithoutExtension(file);
                filename = filename + ".xml";
                SearchedHistory.Add(filename);
            }
        }

    }
}
