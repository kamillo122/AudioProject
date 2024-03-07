﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AudioProject
{
    public class AudioPlayer : IDisposable
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFileReader;

        public void Load(string fileName)
        {
            Stop();
            CloseFile();
            EnsureDeviceCreated();
            OpenFile(fileName);
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
                outputDevice.Init(audioFileReader);
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
            outputDevice = new WaveOutEvent { DesiredLatency = 200 };
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