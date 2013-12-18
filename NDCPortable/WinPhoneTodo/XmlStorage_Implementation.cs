using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDCPortable;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;

namespace WinPhoneTodo
{
    public class XmlStorageImplementation : IXmlStorage
    {
        public XmlStorageImplementation()
        {
        }

        public List<TodoItem> ReadXml(string filename)
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (fileStorage.FileExists(filename))
            {
                var serializer = new XmlSerializer(typeof(List<TodoItem>));
            
                //var fileReader = new StreamReader(new IsolatedStorageFileStream(filename, FileMode.Open, fileStorage));
                //string textFile = fileReader.ReadToEnd();
            
                using (var stream = new StreamReader(new IsolatedStorageFileStream(filename, FileMode.Open, fileStorage)))
                {
                    return (List<TodoItem>)serializer.Deserialize(stream);
                }
            }
            return new List<TodoItem>();
        }

        public void WriteXml(List<TodoItem> tasks, string filename)
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            var serializer = new XmlSerializer(typeof(List<TodoItem>));
            using (var writer = new StreamWriter(new IsolatedStorageFileStream(filename, FileMode.OpenOrCreate, fileStorage)))
            {
                serializer.Serialize(writer, tasks);
            }
        }
    }
}
