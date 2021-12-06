using myapp.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp.MVVM.ViewModel
{

    class HistoryViewModel : ObservableObject
    {
        public ObservableCollection<string> History { get; set; }
        public HistoryViewModel()
        {
            History = new ObservableCollection<string>();
            DirectoryInfo d = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory")); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
            string str = "";

            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;
                History.Add(file.Name);
            }
        }

    }
}
