using System;
using System.Diagnostics;
using System.IO;

namespace AudioProject
{
    public class YouTubeAudioExtractor
    {
        // Method to download audio and get it as a byte buffer
        public byte[] DownloadAudioToBuffer(string videoUrl)
        {
            // Path to yt-dlp executable
            string runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string ytdlpPath = string.Format("{0}Resources\\yt-dlp.exe", Path.GetFullPath(Path.Combine(runningPath, @"..\..\")));

            // Arguments for yt-dlp to download audio in best quality and extract it in MP3 format
            string arguments = $"-x --audio-format mp3 --output temp_audio.mp3 {videoUrl}";

            // Set up the ProcessStartInfo to run yt-dlp
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ytdlpPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Execute yt-dlp to download the audio file
            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }

            // Read the downloaded audio file into a byte array
            string audioFilePath = @"temp_audio.mp3";  // Temporary audio file path
            if (File.Exists(audioFilePath))
            {
                return File.ReadAllBytes(audioFilePath);
            }
            else
            {
                throw new Exception("Audio download failed or file not found.");
            }
        }
    }

}
