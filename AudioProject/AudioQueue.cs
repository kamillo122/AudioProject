using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioProject
{
    public static class AudioQueue
    {
        private static int QueueIndex = 0;
        private static List<string> AudioPaths = new List<string>();
        public static void OverridePaths(IEnumerable<string> paths)
        {
            if (!paths.Any()) { return; }
            AudioPaths = paths.ToList();
        }
        public static void UpdatePaths(string[] paths)
        {
            AudioPaths = paths.ToList();
            if (AudioPaths.Count <= 0) { return; }
            for (int i = 0; i < AudioPaths.Count; i++)
            {
                for (int j = 0; j < paths.Length; j++)
                {
                    if (!String.Equals(AudioPaths[i], paths[j]))
                    {
                        AudioPaths[i] = paths[j];
                    }
                }
            }
        }
        public static List<string> GetPaths()
        {
            return AudioPaths;
        }
        public static int GetQueueLength()
        {
            return AudioPaths.Count;
        }
        public static void AddItem(string item)
        {
            AudioPaths.Add(item);
            QueueIndex++;
        }
        public static void SetQueueIndex(int index)
        {
            if (AudioPaths.Count == 0)
            {
                return;
            }
            if (index >= AudioPaths.Count || QueueIndex <= 0)
            {
                return;
            }
            QueueIndex = index;
        }
        public static string GetCurrentAudio()
        {
            if (!AudioPaths.Any())
            {
                return String.Empty;
            }
            return AudioPaths[QueueIndex];
        }
        public static string GetNextAudio()
        {
            if (AudioPaths.Count == 0)
            {
                return String.Empty;
            }
            if (QueueIndex >= AudioPaths.Count || QueueIndex < 0)
            {
                QueueIndex = 0;
                return AudioPaths[QueueIndex];
            }
            QueueIndex++;
            return AudioPaths[QueueIndex];
        }
        public static string GetPrevAudio()
        {
            if (AudioPaths.Count == 0)
            {
                return String.Empty;
            }
            if (QueueIndex >= AudioPaths.Count || QueueIndex < 0)
            {
                QueueIndex = 0;
                return AudioPaths[QueueIndex];
            }
            QueueIndex--;
            return AudioPaths[QueueIndex];
        }
        public static string GetRandomAudio()
        {
            throw new NotImplementedException();
        }
    }
}
