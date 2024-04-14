using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AudioProject
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly AudioPlayer Player;
        private readonly Visualization Visualization;
        private Path EqualizerPath = new Path();
        public SettingsWindow(AudioPlayer player, Visualization visualization)
        {
            InitializeComponent();
            ClrPickerBackground.SelectedColor = Colors.Black;
            AudioDeviceComboBox.DropDownClosed += OnSelectionChanged;
            Loaded += OnSettingsLoaded;
            Player = player;
            Visualization = visualization;
            EqualizerCanvas.Children.Add(EqualizerPath);
            EqualizerPath.Stroke = Brushes.AliceBlue;
            EqualizerPath.StrokeThickness = 2;
            DrawEqualizerCurve();
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
            Visualization.UpdateWaveFormColor();
        }
        private void DrawEqualizerCurve()
        {
          //TODO
        }
        private void Band1Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band1 = (float)e.NewValue;
            Player.UpdateEqualizer();
            DrawEqualizerCurve();
        }
        private void Band2Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band2 = (float)e.NewValue;
            Player.UpdateEqualizer();
            DrawEqualizerCurve();
        }
        private void Band3Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band3 = (float)e.NewValue;
            Player.UpdateEqualizer();
            DrawEqualizerCurve();
        }
        private void Band4Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band4 = (float)e.NewValue;
            Player.UpdateEqualizer();
            DrawEqualizerCurve();
        }
        private void Band5Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band5 = (float)e.NewValue;
            Player.UpdateEqualizer();
            DrawEqualizerCurve();
        }
        private void Band6Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band6 = (float)e.NewValue;
            Player.UpdateEqualizer();
            DrawEqualizerCurve();
        }
        private void Band7Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band7 = (float)e.NewValue;
            Player.UpdateEqualizer();
            DrawEqualizerCurve();
        }
        private void Band8Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band8 = (float)e.NewValue;
            Player.UpdateEqualizer();
            DrawEqualizerCurve();
        }
    }
}
