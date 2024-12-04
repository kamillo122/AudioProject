using NAudio.Extras;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace AudioProject
{
    public class AudioPlayer : IDisposable
    {
        private WaveOut outputDevice;
        private AudioFileReader audioFileReader;
        private Equalizer equalizer;
        private YouTubeAudioExtractor extractor = new YouTubeAudioExtractor();
        private readonly EqualizerBand[] bands;

        public event EventHandler<MaxSampleEventArgs> MaximumCalculated;

        public AudioPlayer()
        {
            bands = new EqualizerBand[]
                    {
                        new EqualizerBand {Bandwidth = 0.8f, Frequency = 100, Gain = 0},
                        new EqualizerBand {Bandwidth = 0.8f, Frequency = 200, Gain = 0},
                        new EqualizerBand {Bandwidth = 0.8f, Frequency = 400, Gain = 0},
                        new EqualizerBand {Bandwidth = 0.8f, Frequency = 800, Gain = 0},
                        new EqualizerBand {Bandwidth = 0.8f, Frequency = 1200, Gain = 0},
                        new EqualizerBand {Bandwidth = 0.8f, Frequency = 2400, Gain = 0},
                        new EqualizerBand {Bandwidth = 0.8f, Frequency = 4800, Gain = 0},
                        new EqualizerBand {Bandwidth = 0.8f, Frequency = 9600, Gain = 0},
                    };
        }
        public void UpdateEqualizer()
        {
            equalizer?.Update();
        }
        public float Band1
        {
            get => bands[0].Gain;
            set
            {
                if (bands[0].Gain != value)
                {
                    bands[0].Gain = value;
                }
            }
        }
        public float Band2
        {
            get => bands[1].Gain;
            set
            {
                if (bands[1].Gain != value)
                {
                    bands[1].Gain = value;
                }
            }
        }
        public float Band3
        {
            get => bands[2].Gain;
            set
            {
                if (bands[2].Gain != value)
                {
                    bands[2].Gain = value;
                }
            }
        }
        public float Band4
        {
            get => bands[3].Gain;
            set
            {
                if (bands[3].Gain != value)
                {
                    bands[3].Gain = value;
                }
            }
        }
        public float Band5
        {
            get => bands[4].Gain;
            set
            {
                if (bands[4].Gain != value)
                {
                    bands[4].Gain = value;
                }
            }
        }
        public float Band6
        {
            get => bands[5].Gain;
            set
            {
                if (bands[5].Gain != value)
                {
                    bands[5].Gain = value;
                }
            }
        }
        public float Band7
        {
            get => bands[6].Gain;
            set
            {
                if (bands[6].Gain != value)
                {
                    bands[6].Gain = value;
                }
            }
        }
        public float Band8
        {
            get => bands[7].Gain;
            set
            {
                if (bands[7].Gain != value)
                {
                    bands[7].Gain = value;
                }
            }
        }
        public void Load(string fileName)
        {
            Stop();
            CloseFile();
            EnsureDeviceCreated();
            if (fileName.IndexOf("youtube") != -1)
            {
                try
                {
                    byte[] audioBuffer = extractor.DownloadAudioToBuffer(fileName);
                    string runningPath = AppDomain.CurrentDomain.BaseDirectory;
                    string outputFilePath = string.Format("{0}Resources\\temp_audio.mp3", Path.GetFullPath(Path.Combine(runningPath, @"..\..\")));
                    File.WriteAllBytes(outputFilePath, audioBuffer);
                    OpenFile(outputFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                OpenFile(fileName);
            }
        }
        public static List<string> GetAudioDevices()
        {
            List<string> devices = new List<string>();
            for (int n = 0; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                devices.Add($"{n + 1}: {caps.ProductName}");
            }
            return devices;
        }
        public void Play()
        {
            if (outputDevice != null && audioFileReader != null && outputDevice.PlaybackState != PlaybackState.Playing)
            {
                outputDevice.Play();
            }
        }
        private void CloseFile()
        {
            audioFileReader?.Dispose();
            audioFileReader = null;
        }
        private void OpenFile(string fileName)
        {
            try
            {
                var inputStream = new AudioFileReader(fileName);
                audioFileReader = inputStream;
                var aggregator = new SampleAggregator(inputStream);
                aggregator.NotificationCount = inputStream.WaveFormat.SampleRate / 100;
                aggregator.MaximumCalculated += (s, a) => MaximumCalculated?.Invoke(this, a);
                equalizer = new Equalizer(aggregator, bands);
                outputDevice.Init(equalizer);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Problem opening file");
                CloseFile();
            }
        }
        private void EnsureDeviceCreated()
        {
            if (outputDevice == null)
            {
                CreateDevice();
            }
        }
        public void RecreateDevice(int deviceNumber)
        {
            outputDevice.Stop();
            outputDevice.Dispose();
            outputDevice = null;
            outputDevice = new WaveOut() { DesiredLatency = 200, DeviceNumber = deviceNumber };
        }
        public void SetVolume(float volume)
        {
            if (outputDevice == null)
            {
                return;
            }
            outputDevice.Volume = volume;
        }
        public void SetAudioTime(TimeSpan time)
        {
            if (audioFileReader == null)
            {
                return;
            }
            audioFileReader.CurrentTime = time;
        }
        private void CreateDevice()
        {
            outputDevice = new WaveOut { DesiredLatency = 200 };
        }
        public bool CheckDidDeviceCreated()
        {
            return outputDevice != null;
        }
        public bool CheckAudioStream()
        {
            return audioFileReader != null;
        }
        public void Pause()
        {
            outputDevice?.Pause();
        }

        public void Stop()
        {
            outputDevice?.Stop();
            if (audioFileReader != null)
            {
                audioFileReader.Position = 0;
            }
        }
        public double GetAudioTotalSeconds()
        {
            if (audioFileReader != null) return audioFileReader.TotalTime.TotalSeconds;
            else return 0;
        }
        public double GetAudioCurrentValueSeconds()
        {
            if (audioFileReader != null) return audioFileReader.CurrentTime.TotalSeconds;
            else return 0;
        }
        public long GetAudioPosition()
        {
            return audioFileReader.Position;
        }
        public void SetAudioPosition(long position)
        {
            if (audioFileReader == null)
            {
                return;
            }
            audioFileReader.Position = position;
        }
        public void Dispose()
        {
            Stop();
            CloseFile();
            outputDevice?.Dispose();
            outputDevice = null;
        }
    }
}