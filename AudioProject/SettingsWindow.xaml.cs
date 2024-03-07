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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Loaded += OnSettingsLoaded;
        }
        public void OnSettingsLoaded(object sender, RoutedEventArgs e)
        {
            AudioDeviceComboBox.Items.Clear();
            List<string> devices = AudioPlayer.GetAudioDevices();
            if (devices.Count > 0)
            {
                foreach (string device in devices)
                {
                    AudioDeviceComboBox.Items.Add(device);
                }
                AudioDeviceComboBox.SelectedIndex = 0;
            }
        }
    }
}
