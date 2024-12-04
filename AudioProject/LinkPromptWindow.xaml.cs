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

namespace AudioProject
{
    /// <summary>
    /// Interaction logic for LinkPromptWindow.xaml
    /// </summary>
    public partial class LinkPromptWindow : Window
    {
        public string Link { get; set; }
        public LinkPromptWindow()
        {
            InitializeComponent();
        }
        private void okButtonClick(object sender, RoutedEventArgs e)
        {
            if (LinkTextBox.Text != String.Empty)
            {
                Link = LinkTextBox.Text;
                DialogResult = true;
            }
        }

        private void cancelButtonClick(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
