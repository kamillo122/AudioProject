using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;

using System.Windows.Threading;
using System.Windows.Forms;

using NAudio.Extras;
using NAudio.Wave;
using Microsoft.Win32;
using System.Collections;
using System.Linq;

namespace AudioProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AudioPlayer player = new AudioPlayer();
        private readonly Visualization visualization;
        private bool userIsDraggingSlider = false;
        public bool WindowClosing = false;
        private readonly DispatcherTimer audioTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            visualization = new Visualization(canvas);
            audioTimer.Interval = TimeSpan.FromSeconds(1);
            audioTimer.Tick += timerAudioTick;
            audioTimer.Start();
            player.MaximumCalculated += OnMaximumCalculated;
            canvas.SizeChanged += visualization.WaveFormControlSizeChanged;
        }
        private void updateSlider()
        {
            sliProgress.Minimum = 0;
            sliProgress.Maximum = player.GetAudioTotalSeconds();
            sliProgress.Value = player.GetAudioCurrentValueSeconds();
        }
        private void timerAudioTick(object sender, EventArgs e)
        {
            if (player.CheckDidDeviceCreated() && player.CheckAudioStream() && !userIsDraggingSlider)
            {
                updateSlider();
            }
        }
        public void UpdatePaths()
        {
            lbFiles.Items.Clear();
            lbFiles.BeginInit();
            foreach (string path in AudioQueue.GetPaths())
            {
                lbFiles.Items.Add(Path.GetFileName(path));
            }
            lbFiles.EndInit();
        }
        private void btnOpenLinkClick(object sender, RoutedEventArgs e)
        {
            LinkPromptWindow linkPromptWindow = new LinkPromptWindow();
            linkPromptWindow.Owner = this;
            bool? result = linkPromptWindow.ShowDialog();
            if (result == true && linkPromptWindow.Link != String.Empty)
            {
                //player.Load(AudioQueue.GetCurrentAudio());
            }
        }
        private void btnOpenFilesClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Audio (*.mp3,*.wav,*aiff)|*.mp3;*.wav;*.aiff";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                AudioQueue.UpdatePaths(openFileDialog.FileNames);
                lbFiles.BeginInit();
                foreach (string filename in openFileDialog.FileNames)
                {
                    lbFiles.Items.Add(Path.GetFileName(filename));
                }
                lbFiles.EndInit();
                if (!player.CheckAudioStream() && !player.CheckDidDeviceCreated())
                {
                    player.Load(AudioQueue.GetCurrentAudio());
                }
            }
        }
        private void OnMaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            visualization.AddValue(e.MaxSample, e.MinSample);
        }
        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            if (AudioQueue.GetQueueLength() <= 0)
            {
                return;
            }
            if (PlayButton.Content == FindResource("Play"))
            {
                if (audioTimer.IsEnabled == false)
                {
                    audioTimer.Start();
                }
                if (AudioQueue.GetCurrentAudio() == String.Empty)
                {
                    PlayButton.Content = FindResource("Stop");
                    return;
                }
                if (!player.CheckAudioStream() && lbFiles.Items.Count > 0)
                {
                    try
                    {
                        player.Load(AudioQueue.GetNextAudio());
                    }
                    catch (FileNotFoundException ex)
                    {
                        System.Windows.Forms.MessageBox.Show("File not found", "Error");
                        throw ex;
                    }
                }
                if (player.CheckAudioStream() && player.CheckDidDeviceCreated())
                {
                    player.Play();
                    String file = Path.GetFileName(AudioQueue.GetCurrentAudio());
                    String text = "Current playing : " + file;
                    currentPlaying.Text = text;
                    PlayButton.Content = FindResource("Stop");
                }
            }
            else
            {
                PlayButton.Content = FindResource("Play");
                if (player.CheckDidDeviceCreated() && player.CheckAudioStream())
                {
                    player.Pause();
                    audioTimer.Stop();
                    currentPlaying.Text = String.Empty;
                }
                if (!player.CheckDidDeviceCreated() || !player.CheckAudioStream())
                {
                    player.Stop();
                    audioTimer.Stop();
                    currentPlaying.Text = String.Empty;
                }
            }
        }
        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            if (WindowClosing)
            {
                player?.Dispose();
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
                player.Stop();
                player.SetAudioPosition(0);
                updateSlider();
                lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
            }
            if (AudioQueue.GetCurrentAudio() != String.Empty)
            {
                visualization.Reset();
                player.Play();
                if (PlayButton.Content == FindResource("Play"))
                {
                    PlayButton.Content = FindResource("Stop");
                }
            }
        }
        private void NextClick(object sender, RoutedEventArgs e)
        {
            player.Stop();
            player.Load(AudioQueue.GetNextAudio());
            player.Play();
            String file = Path.GetFileName(AudioQueue.GetCurrentAudio());
            String text = "Current playing : " + file;
            currentPlaying.Text = text;
            visualization.Reset();
            if (PlayButton.Content == FindResource("Play"))
            {
                PlayButton.Content = FindResource("Stop");
            }
        }
        private void PrevClick(object sender, RoutedEventArgs e)
        {
            player.Stop();
            player.Load(AudioQueue.GetPrevAudio());
            player.Play();
            String file = Path.GetFileName(AudioQueue.GetCurrentAudio());
            String text = "Current playing : " + file;
            currentPlaying.Text = text;
            visualization.Reset();
            if (PlayButton.Content == FindResource("Play"))
            {
                PlayButton.Content = FindResource("Stop");
            }
        }
        private void SettingsButtonClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(player, visualization);
            settingsWindow.Owner = this;
            settingsWindow.Show();
        }
        private void lbFilesMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (lbFiles.SelectedItem != null)
            {
                player.Stop();
                AudioQueue.SetQueueIndex(lbFiles.SelectedIndex);
                player.Load(AudioQueue.GetCurrentAudio());
                String file = Path.GetFileName(AudioQueue.GetCurrentAudio());
                String text = "Current playing : " + file;
                currentPlaying.Text = text;
                visualization.Reset();
                player.Play();
                if (PlayButton.Content == FindResource("Play"))
                {
                    PlayButton.Content = FindResource("Stop");
                }
            }
        }
    }
}