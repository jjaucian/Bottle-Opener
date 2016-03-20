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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BottleOpener.ViewModels;
using System.Collections.Specialized;

namespace BottleOpener
{
    /// <summary>
    /// Interaction logic for LogMessagePanel.xaml
    /// </summary>
    public partial class LogMessagePanel : UserControl
    {
        public LogMessagePanel()
        {
            InitializeComponent();
            DataContext = new LogMessageViewModel();

            ((INotifyCollectionChanged)listBox.Items).CollectionChanged += ListBoxChanged;
        }

        private void ListBoxChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                listBox.ScrollIntoView(e.NewItems[0]);
            }

        }
    }


}
