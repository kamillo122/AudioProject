﻿using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly AudioQueue AudioQueue;
        public SettingsWindow(AudioPlayer player, Visualization visualization, AudioQueue audioQueue)
        {
            InitializeComponent();
            ClrPickerBackground.SelectedColor = Colors.Black;
            AudioDeviceComboBox.DropDownClosed += OnSelectionChanged;
            Loaded += OnSettingsLoaded;
            Player = player;
            Visualization = visualization;
            AudioQueue = audioQueue;
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
            /*
            PathFigure myPathFigure = new PathFigure();
            // Calculate the center of the canvas
            double centerX = EqualizerCanvas.ActualWidth / 2;
            double centerY = EqualizerCanvas.ActualHeight / 2;

            myPathFigure.StartPoint = new Point(0, centerY);

            PointCollection myPointCollection = new PointCollection(8);
            myPointCollection.Add(new Point(centerX + 100, centerY - Player.Band1));
            myPointCollection.Add(new Point(centerX + 125, centerY - Player.Band2));
            myPointCollection.Add(new Point(centerX + 150, centerY - Player.Band3));
            myPointCollection.Add(new Point(centerX + 175, centerY - Player.Band4));
            myPointCollection.Add(new Point(centerX + 200, centerY - Player.Band5));
            myPointCollection.Add(new Point(centerX + 225, centerY - Player.Band6));
            myPointCollection.Add(new Point(centerX + 250, centerY - Player.Band7));
            myPointCollection.Add(new Point(centerX + 275, centerY - Player.Band8));

            PolyBezierSegment myBezierSegment = new PolyBezierSegment();
            myBezierSegment.Points = myPointCollection;

            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            myPathSegmentCollection.Add(myBezierSegment);

            myPathFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            EqualizerPath.Data = myPathGeometry;
            */
        }
        private void Band1Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band1 = (float)e.NewValue;
            Player.UpdateEqualizer();
        }
        private void Band2Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band2 = (float)e.NewValue;
            Player.UpdateEqualizer();
        }
        private void Band3Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band3 = (float)e.NewValue;
            Player.UpdateEqualizer();
        }
        private void Band4Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band4 = (float)e.NewValue;
            Player.UpdateEqualizer();
        }
        private void Band5Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band5 = (float)e.NewValue;
            Player.UpdateEqualizer();

        }
        private void Band6Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band6 = (float)e.NewValue;
            Player.UpdateEqualizer();
 
        }
        private void Band7Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band7 = (float)e.NewValue;
            Player.UpdateEqualizer();
      
        }
        private void Band8Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Band8 = (float)e.NewValue;
            Player.UpdateEqualizer();
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
                ((MainWindow)this.Owner).UpdatePaths();
            }
        }

        private void btnSavePlaylist_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Playlist (*.txt)|*.txt;";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllLines(saveFileDialog.FileName, AudioQueue.GetAudioPaths());
            }
        }
    }
}
