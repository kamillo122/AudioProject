using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using NAudio.Wave;
using NAudio.WaveFormRenderer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace AudioProject
{
    public class Visualization
    {
        private readonly WaveFormRenderer waveFormRenderer;
        private readonly WaveFormRendererSettings soundCloudGrayTransparentBlocks = new SoundCloudBlockWaveFormSettings(System.Drawing.Color.FromArgb(196, 224, 225, 224), System.Drawing.Color.FromArgb(64, 224, 224, 224), System.Drawing.Color.FromArgb(196, 128, 128, 128),
                System.Drawing.Color.FromArgb(64, 128, 128, 128))
        {
            Name = "SoundCloud Gray Transparent Blocks",
            PixelsPerPeak = 4,
            SpacerPixels = 1,
            TopSpacerGradientStartColor = System.Drawing.Color.FromArgb(64, 224, 224, 224),
            BackgroundColor = System.Drawing.Color.Transparent,
            TopHeight = 60,
            BottomHeight = 60,
        };
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        public ImageSource ImageSourceFromBitmap(System.Drawing.Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }
        private void RenderWaveForm()
        {
            //if (audioFile == null) return;
            //RenderedImage.Source = null;
            //var peakProvider = new MaxPeakProvider();
            //RenderThreadFunc(peakProvider, soundCloudGrayTransparentBlocks);
        }
        private void RenderThreadFunc(IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            //Image image = null;
            //try
            //{
            //using (var waveStream = new AudioFileReader(Path.GetFullPath(AudioPaths[QueueIndex])))
            //{
            //image = waveFormRenderer.Render(waveStream, peakProvider, settings);
            //if (File.Exists(@"F:\studia\semestr_4\Programowanie IV\AudioProject\AudioProject\Images\rendered.PNG"))
            //{
            //RenderedImage.Source = null;
            //File.Delete(@"F:\studia\semestr_4\Programowanie IV\AudioProject\AudioProject\Images\rendered.PNG");
            //}
            //else
            //{
            // image.Save(@"F:\studia\semestr_4\Programowanie IV\AudioProject\AudioProject\Images\rendered.PNG", System.Drawing.Imaging.ImageFormat.Png);
            //}
            //}
            //}
            //catch (Exception ex)
            //{
            //MessageBox.Show(ex.Message);
            //}
            //FinishedRender(image);
        }
        private void FinishedRender(System.Drawing.Image image)
        {
            if (image == null) return;
            try
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(image);
                //RenderedImage.Source = bmp;
                //RenderedImage.Source = new BitmapImage(new Uri(@"F:\studia\semestr_4\Programowanie IV\AudioProject\AudioProject\Images\rendered.PNG"));
                image.Dispose();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
    }
}
