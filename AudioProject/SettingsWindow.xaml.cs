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
        private readonly AudioPlayer Player;
        public SettingsWindow(AudioPlayer player)
        {
            InitializeComponent();
            //AudioDeviceComboBox.SelectionChanged += OnSelectionChanged;
            ClrPickerBackground.SelectedColor = Colors.Black;
            AudioDeviceComboBox.DropDownClosed += OnSelectionChanged;
            Loaded += OnSettingsLoaded;
            Player = player;
        }
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (AudioDeviceComboBox.SelectedItem != null && AudioDeviceComboBox.SelectedIndex != -1)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Player.RecreateDevice(AudioDeviceComboBox.SelectedIndex);
                });
            }
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
        private void ClrPickerBackgroundSelectedColorChanged(object sender, EventArgs e)
        {
            Application.Current.Resources["WindowBackgroudBrush"] = new SolidColorBrush(ClrPickerBackground.SelectedColor.Value);
        }
        private void ClrPickerForegroundSelectedColorChanged(object sender, EventArgs e)
        {
            Application.Current.Resources["WindowForegroundBrush"] = new SolidColorBrush(ClrPickerForeground.SelectedColor.Value);
        }
        private void ClrPickerWaveFormSelectedColorChanged(object sender, EventArgs e)
        {
            Application.Current.Resources["WaveFormBackgroundBrush"] = new SolidColorBrush(ClrPickerWaveForm.SelectedColor.Value);
        }
    }
}
