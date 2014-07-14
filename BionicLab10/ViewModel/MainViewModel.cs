using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using Ookii.Dialogs.Wpf;
using BionicLab10.Model;

namespace BionicLab10.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<FileViewModel> ListFiles { get; set; }
        public ListFiles ListFile { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand ChoiseDir1 { get; set; }
        public ICommand ChoiseDir2 { get; set; }
        public ICommand ClickSyncCommand { get; set; }
        public XmlDocument _xmlDoc;

        public MainViewModel()
        {
            ClickCommand = new Command(arg => ClickMethod());
            ClickSyncCommand = new Command(arg => ClickSyncMethod());
            ChoiseDir1 = new Command(arg => ChoiseDir1Method());
            ChoiseDir2 = new Command(arg => ChoiseDir2Method());
            ListFile = new Model.ListFiles();
            ListFiles = new ObservableCollection<FileViewModel>(ListFile.NewList.Select(f => new FileViewModel(f)));
            _xmlDoc = new XmlDocument();
            _xmlDoc.Load("log.xml");
            ClickMethod();
        }

        public string Dir1
        {
            get
            { return ListFile.Dir1; }
            set
            {
                if (value != ListFile.Dir1)
                {
                    ListFile.Dir1 = value;
                    OnPropertyChanged("Dir1");
                }
            }            
        }

        public string Dir2
        {
            get
            { return ListFile.Dir2; }
            set
            {
                if (value != ListFile.Dir2)
                {
                    ListFile.Dir2 = value;
                    OnPropertyChanged("Dir2");
                }
            }
        }

        private void ChoiseDir1Method()
        {
            VistaFolderBrowserDialog dialog1 = new VistaFolderBrowserDialog();
            dialog1.ShowDialog();
            Dir1 = dialog1.SelectedPath;
        }

        private void ChoiseDir2Method()
        {
            VistaFolderBrowserDialog dialog1 = new VistaFolderBrowserDialog();
            dialog1.ShowDialog();
            Dir2 = dialog1.SelectedPath;
        }

        private void ClickMethod()
        {
            if ((Dir1 != null ) && (Dir2 != null))
            {
                ListFiles.Clear();
                var dir = new DirectoryInfo(Dir1);
                XmlElement newElement = _xmlDoc.CreateElement("date");
                DateTime today = DateTime.Now;
                newElement.SetAttribute("id", today.ToString());
                
                foreach (FileInfo file in dir.GetFiles()) // извлекаем все файлы и кидаем их в список
                {
                    FileViewModel fileDir1 = new FileViewModel(new MyFile());
                    fileDir1.FileName = file.FullName.Substring(dir.ToString().Length + 1);
                    fileDir1.FileDate1 = File.GetLastWriteTime(file.FullName);

                    ListFiles.Add(fileDir1);
                }
                dir = new DirectoryInfo(Dir2);
                foreach (FileInfo file in dir.GetFiles()) // извлекаем все файлы и кидаем их в список
                {
                    FileViewModel fileDir2 = new FileViewModel(new MyFile());
                    fileDir2.FileName = file.FullName.Substring(dir.ToString().Length + 1);
                    fileDir2.FileDate2 = File.GetLastWriteTime(file.FullName);
                    int result = ListFiles.ToList<FileViewModel>().FindIndex(
                        delegate(FileViewModel LF)
                        {
                            return LF.FileName == fileDir2.FileName;
                        });

                    if (result != -1)
                        ListFiles[result].FileDate2 = fileDir2.FileDate2;
                    else
                        ListFiles.Add(fileDir2);
                }

                for (int i = 0; i < ListFiles.Count; i++)
                {
                    if (ListFiles[i].FileDate1 == ListFiles[i].FileDate2)
                        ListFiles[i].Status = "no changes";
                    else
                        if (ListFiles[i].FileDate1 == default(DateTime))
                        {

                            ListFiles[i].Status = "file was deleted";
                        }
                        else
                            if (ListFiles[i].FileDate2 == default(DateTime))
                                ListFiles[i].Status = "file was added";
                            else
                                if (ListFiles[i].FileDate1 > ListFiles[i].FileDate2)
                                    ListFiles[i].Status = "file newer on disk1";
                                else
                                    ListFiles[i].Status = "file newer on disk2";
                    ListFiles[i].Sync = false;


                    XmlElement elem = _xmlDoc.CreateElement("file");
                    elem.SetAttribute("name", ListFiles[i].FileName);
                    elem.SetAttribute("date_source", ListFiles[i].FileDate1.ToString());
                    elem.SetAttribute("date_distinct", ListFiles[i].FileDate2.ToString());
                    elem.SetAttribute("status", ListFiles[i].Status);
                    newElement.AppendChild(elem);
                    _xmlDoc.DocumentElement.AppendChild(newElement);
                    _xmlDoc.Save("log.xml");
                }
            }
            else
                ListFile.NewList = default(List<MyFile>);
        }

        public void ClickSyncMethod()
        {
            for (int i = 0; i < ListFiles.Count; i++)
            {
                if (ListFiles[i].Sync)
                {
                    switch (ListFiles[i].Status)
                    {
                        case "file was deleted":
                            File.Delete(Dir2+"\\"+ListFiles[i].FileName);
                            break;
                        case "file was added":
                            File.Copy((Dir1 + "\\" + ListFiles[i].FileName), (Dir2 + "\\" + ListFiles[i].FileName));
                            break;
                        case "file newer on disk1":
                            File.Copy((Dir1 + "\\" + ListFiles[i].FileName), (Dir2 + "\\" + ListFiles[i].FileName), true);
                            break;
                        case "file newer on disk2":
                            File.Copy((Dir2 + "\\" + ListFiles[i].FileName), (Dir1 + "\\" + ListFiles[i].FileName), true);
                            break;
                    }
                }
            }
            ClickMethod();
        }
    }
}
