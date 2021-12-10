using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace myapp.MVVM.Model
{
    class SaveChat
    {
        public SaveChat()
        {

        }
        public void SaveChatFunction(ObservableCollection<string> Messages)
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChatHistory"));
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dateAndTime = DateTime.Now;
            var date = dateAndTime.ToString("yyyyMMddHHmmss");
            string filename = $@"{date}.xml";



            using (var sw = new StreamWriter(Path.Combine(docPath, "ChatHistory", filename)))
            {
                foreach (string line in Messages)
                {
                    sw.WriteLine(line);
                }
            }
        }

        

    }
}
