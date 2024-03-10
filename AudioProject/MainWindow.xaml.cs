using Microsoft.Win32;
using NAudio.Wave;
using NAudio.WaveFormRenderer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AudioProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AudioPlayer player = new AudioPlayer();
        private AudioQueue audioQueue = new AudioQueue();
        private Visualization PolygonVisualizer;
        private bool userIsDraggingSlider = false;
        public bool WindowClosing = false;
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timerTick;
            timer.Start();
            PolygonVisualizer = new Visualization(canvas);

        }
        private void timerTick(object sender, EventArgs e)
        {
            if (player.CheckDidDeviceCreated() && player.CheckAudioStream() && !userIsDraggingSlider)
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = player.GetAudioTotalSeconds();
                sliProgress.Value = player.GetAudioCurrentValueSeconds();
            }
        }
        private void btnOpenFilesClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Audio (*.mp3,*.wav,*aiff)|*.mp3;*.wav;*.aiff|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                audioQueue.UpdatePaths(openFileDialog.FileNames);
                lbFiles.BeginInit();
                foreach (string filename in openFileDialog.FileNames)
                {
                    lbFiles.Items.Add(Path.GetFileName(filename));
                }
                lbFiles.EndInit();
                player.Load(audioQueue.GetNextAudio());
            }
        }
        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            if (PlayButton.Content == FindResource("Play"))
            {
                if (audioQueue.GetNextAudio() == String.Empty)
                {
                    PlayButton.Content = FindResource("Stop");
                    return;
                }
                if (!player.CheckAudioStream() && lbFiles.Items.Count > 0)
                {
                    try
                    {
                        player.Load(audioQueue.GetNextAudio());
                    }
                    catch (FileNotFoundException ex)
                    {
                        MessageBox.Show("File not found", "Error");
                        throw ex;
                    }
                }
                if (player.CheckAudioStream() && player.CheckDidDeviceCreated())
                {
                    player.Play();
                    PlayButton.Content = FindResource("Stop");
                }
            }
            else
            {
                PlayButton.Content = FindResource("Play");
                if (player.CheckDidDeviceCreated() && player.CheckAudioStream())
                {
                    player.Pause();
                }
                if (!player.CheckDidDeviceCreated() || !player.CheckAudioStream())
                {
                    player.Stop();
                }
            }
        }
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            if (WindowClosing)
            {
                player.Dispose();
            }
        }
        private void sliVolumeValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (player.CheckDidDeviceCreated())
            {
                player.SetVolume((float)sliVolume.Value / 100f);
            }
        }
        private void sliProgressDragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgressDragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            player.SetAudioTime(TimeSpan.FromSeconds(sliProgress.Value));
        }
        private void sliProgressValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }
        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            WindowClosing = true;
            player.Dispose();
        }

        private void AgainButtonClick(object sender, RoutedEventArgs e)
        {
            if (player.CheckDidDeviceCreated() && player.CheckAudioStream() && player.GetAudioPosition() != 0)
            {
                player.SetAudioPosition(0);
            }
            if (audioQueue.GetNextAudio() != String.Empty)
            {
                player.Play();
                if (PlayButton.Content == FindResource("Play"))
                {
                    PlayButton.Content = FindResource("Stop");
                }
            }
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            audioQueue.IncreaseQueueIndex();
            player.Load(audioQueue.GetNextAudio());
            player.Play();
            if (PlayButton.Content == FindResource("Play"))
            {
                PlayButton.Content = FindResource("Stop");
            }
        }
        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            audioQueue.DecreaseQueueIndex();
            player.Load(audioQueue.GetNextAudio());
            player.Play();
            if (PlayButton.Content == FindResource("Play"))
            {
                PlayButton.Content = FindResource("Stop");
            }
            else
            {
                PlayButton.Content = FindResource("Play");
            }
        }
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
        void lbFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbFiles.SelectedItem != null)
            {
                player.Stop();
                audioQueue.SetQueueIndex(lbFiles.SelectedIndex);
                player.Load(audioQueue.GetNextAudio());
                player.Play();
            }
        }
    }
}