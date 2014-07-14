using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace BionicLab10
{
    /// <summary>
    /// Логика взаимодействия для ViewLog.xaml
    /// </summary>
    public partial class ViewLog : Window
    {
        private XmlDocument _xmlDoc;
        public ViewLog()
        {
            InitializeComponent();
            _xmlDoc = new XmlDocument();
        }

        private void ViewLog_Load(object sender, EventArgs e)
        {
            _xmlDoc.Load("log.xml");
            XmlNodeList nodes = _xmlDoc.GetElementsByTagName("date");
            TreeViewLog.Items.Clear();
            TreeViewItem core = new TreeViewItem();
            core.Header = "log";
            List<TreeViewItem> firstLevel = new List<TreeViewItem>();
            int n = 0;
            foreach (XmlNode node in nodes)
            {
                firstLevel.Add(new TreeViewItem());
                firstLevel[n].Header = node.Attributes["id"].Value;
                List<TreeViewItem> secondLevel = new List<TreeViewItem>();
                int k=0;
                foreach (XmlNode m in node.ChildNodes)
                {
                    string str = m.Attributes["name"].Value + " " + m.Attributes["status"].Value;
                    secondLevel.Add(new TreeViewItem());
                    secondLevel[k].Header = str;
                    firstLevel[n].Items.Add(secondLevel[k]);
                    k++;
                }
                core.Items.Add(firstLevel[n]);
                n++;
            }
            TreeViewLog.Items.Add(core);
        }
    }
}
