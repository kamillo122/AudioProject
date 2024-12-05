using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace AudioProject
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly AudioPlayer Player;
        private readonly Visualization Visualization;
        private System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
        public SettingsWindow(AudioPlayer player, Visualization visualization)
        {
            InitializeComponent();
            ClrPickerBackground.SelectedColor = Colors.Black;
            ClrPickerForeground.SelectedColor = Colors.WhiteSmoke;
            ClrPickerWaveForm.SelectedColor = Colors.GreenYellow;
            AudioDeviceComboBox.DropDownClosed += OnSelectionChanged;
            Loaded += OnSettingsLoaded;
            Player = player;
            Visualization = visualization;
            DrawEqualizerCurve();
        }
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (AudioDeviceComboBox.SelectedItem != null && AudioDeviceComboBox.SelectedIndex != -1)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (AudioDeviceComboBox.SelectedIndex != -1)
                    {
                        Player.RecreateDevice(AudioDeviceComboBox.SelectedIndex);
                    }
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
            path.Stroke = Application.Current.Resources["WaveFormBackgroundBrush"] as SolidColorBrush;
            Visualization?.UpdateWaveFormColor();
        }
        private void DrawEqualizerCurve()
        {
            // Clear any existing shapes from the canvas
            equalizerCanvas.Children.Clear();

            // Define the width and height of the canvas
            double canvasWidth = equalizerCanvas.ActualWidth;
            double canvasHeight = equalizerCanvas.ActualHeight;

            // Equalizer band values from Band1 to Band8
            List<double> bandValues = new List<double>
            {
                Player.Band1,
                Player.Band2,
                Player.Band3,
                Player.Band4,
                Player.Band5,
                Player.Band6,
                Player.Band7,
                Player.Band8
            };

            // Scale the values to fit within the canvas height
            double maxBandValue = bandValues.Max();
            double minBandValue = bandValues.Min();
            double scale = canvasHeight / (maxBandValue - minBandValue);

            // Create a Path to represent the curve
            path.Stroke = Application.Current.Resources["WaveFormBackgroundBrush"] as SolidColorBrush;  // Set the color of the curve (same as the waveform color)
            path.StrokeThickness = 2;

            // Create a PathGeometry to define the curve
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            // Start point of the curve (leftmost point)
            pathFigure.StartPoint = new Point(0, canvasHeight - (bandValues[0] - minBandValue) * scale);

            // Create a collection of points to represent the curve
            PolyLineSegment polyLineSegment = new PolyLineSegment();

            for (int i = 1; i < bandValues.Count; i++)
            {
                // Calculate the x-coordinate for each band (spread evenly across the width of the canvas)
                double x = (canvasWidth / (bandValues.Count - 1)) * i;

                // Calculate the y-coordinate based on the band value (scaled to fit the canvas height)
                double y = canvasHeight - (bandValues[i] - minBandValue) * scale;

                // Add the point to the PolyLineSegment
                polyLineSegment.Points.Add(new Point(x, y));
            }

            // Add the PolyLineSegment to the PathFigure
            pathFigure.Segments.Add(polyLineSegment);

            // Add the PathFigure to the PathGeometry
            pathGeometry.Figures.Add(pathFigure);

            // Set the Path's Data to the PathGeometry
            path.Data = pathGeometry;

            // Add the Path to the equalizerCanvas
            equalizerCanvas.Children.Add(path);
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

        private void btnOpenPlaylist_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Playlist (*.txt)|*.txt;";
            openFileDialog.CheckFileExists = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] paths = File.ReadAllLines(openFileDialog.FileName);
                AudioQueue.UpdatePaths(paths);
                //((MainWindow)this.Owner).UpdatePaths();
            }
        }

        private void btnSavePlaylist_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Playlist (*.txt)|*.txt;";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //File.WriteAllLines(saveFileDialog.FileName, AudioQueue.GetAudioPaths());
            }
        }
    }
}
