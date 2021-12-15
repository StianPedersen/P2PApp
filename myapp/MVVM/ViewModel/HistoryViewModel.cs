using myapp.Core;
using myapp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace myapp.MVVM.ViewModel
{

    class HistoryViewModel : ObservableObject
    {
        public ObservableCollection<string> History { get; set; }


        public ObservableCollection<string> SearchedHistory {
            get; set; }

        public RelayCommand SearchCommand
        {
            get;
            set;
        }
        FileHandler _fileHandler;
        private string _chosenFile = "";
        public string ChosenFile
        {
            get { return _chosenFile; }
            set
            {
                var filename = value;
                if (value != null)
                {
                    filename = _fileHandler.get_file(value);
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
            _fileHandler = new FileHandler();
            History = new ObservableCollection<string>();
            SearchedHistory = new ObservableCollection<string>();
            History = _fileHandler.find_history();
         


            SearchCommand = new RelayCommand(o => find_files());


        }
        public void find_files()
        {

            String strPath = _fileHandler.get_strPath();
            String search = SearchWord;


            var files = _fileHandler.get_file_linq(strPath, search);
            string filename;
            SearchedHistory.Clear();
            foreach (var file in files)
            {
                filename = _fileHandler.get_file_without_extension(file);
                filename = filename + ".xml";
                SearchedHistory.Add(filename);
            }
        }


    }
}
