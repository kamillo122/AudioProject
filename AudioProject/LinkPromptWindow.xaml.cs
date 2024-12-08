using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace AudioProject
{
    /// <summary>
    /// Interaction logic for LinkPromptWindow.xaml
    /// </summary>
    public partial class LinkPromptWindow : Window
    {
        public string Link { get; set; }
        private readonly YouTubeAudioExtractor extractor = new YouTubeAudioExtractor();
        public LinkPromptWindow()
        {
            InitializeComponent();
        }
        private void okButtonClick(object sender, RoutedEventArgs e)
        {
            if (LinkTextBox.Text != System.String.Empty)
            {
                Link = LinkTextBox.Text;
                var confirmSaving = System.Windows.Forms.MessageBox.Show("Are you want to save this audio?",
                                     "Saving audio",
                                     MessageBoxButtons.YesNo);
                if (confirmSaving == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                        saveFileDialog.Filter = "Audio (*.mp3)|*.mp3";
                        if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            byte[] audioBuffer = extractor.DownloadAudioToBuffer(Link, saveFileDialog.FileName);
                            AudioQueue.AddItem(saveFileDialog.FileName);
                            Dispatcher.Invoke(() => { ((MainWindow)Owner).UpdatePaths(); });
                            DialogResult = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        DialogResult = false;
                    }
                }
                else
                {
                    try
                    {
                        string path = Path.GetTempPath() + $"{Path.GetFileName(Link).Replace("watch?v=", "").Replace("_", "")}.mp3";
                        if (File.Exists(path))
                        {
                            AudioQueue.AddItem(path);
                            Dispatcher.Invoke(() => { ((MainWindow)Owner).UpdatePaths(); });
                            DialogResult = true;
                        }
                        else
                        {
                            byte[] audioBuffer = extractor.DownloadAudioToBuffer(Link, path);
                            AudioQueue.AddItem(path);
                            Dispatcher.Invoke(() => { ((MainWindow)Owner).UpdatePaths(); });
                            DialogResult = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        DialogResult = false;
                    }
                }
            }
        }

        private void cancelButtonClick(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
