using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace myapp.MVVM.Model
{
    public class FileHandler
    {
        

        public ObservableCollection<string> Hist { get; set; }
        public ObservableCollection<string> SearchHist { get; set; }
        public FileHandler()
        {
            Hist = new ObservableCollection<string>();
            SearchHist = new ObservableCollection<string>();


        }
        public ObservableCollection<string> find_history()
        {
            DirectoryInfo d = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory")); //Assuming Test is your Folder

            FileInfo[] Files = d.GetFiles("*.xml"); //Getting Text files
            string str = "";

            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;
                Hist.Add(file.Name);
            }
            return Hist;
        }

        public void find_files(string SearchWord)
        {
            //SearchWord = "something";
            String strPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory");
            String search = SearchWord;

            //LINQ
            var files = from file in Directory.GetFiles(strPath, "*", SearchOption.AllDirectories)
                        where File.ReadAllLines(file).Any(x => x.Contains("[" + search))
                        select file;
            string filename;
            SearchHist.Clear();
            foreach (var file in files)
            {
                filename = Path.GetFileNameWithoutExtension(file);
                filename = filename + ".xml";
                SearchHist.Add(filename);
            }
 
        }
        public string get_file(string filename)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory", filename);

        }
        public string get_strPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory");
        }

        public System.Collections.Generic.IEnumerable<string> get_file_linq(string strPath, string search)
        {
            var temp = from file in Directory.GetFiles(strPath, "*", SearchOption.AllDirectories)
                       where File.ReadAllLines(file).Any(x => x.Contains("[" + search))
                       select file;
            return temp;
        }
        public string get_file_without_extension(string file)
        {

            return Path.GetFileNameWithoutExtension(file); ;
        }
    }
}
