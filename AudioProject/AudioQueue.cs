using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioProject
{
    public class AudioQueue
    {
        private int QueueIndex = 0;
        private List<string> AudioPaths = new List<string>();
        public void UpdatePaths(string[] paths)
        {
            AudioPaths = paths.ToList();
        }
        public int GetQueueLength()
        {
            return AudioPaths.Count;
        }
        public void SetQueueIndex(int index)
        {
            if (AudioPaths.Count == 0)
            {
                return;
            }
            if (index >= AudioPaths.Count || QueueIndex < 0)
            {
                return;
            }
            QueueIndex = index;
        }
        public string GetCurrentAudio()
        {
            return AudioPaths[QueueIndex];
        }
        public string GetNextAudio()
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
        public string GetPrevAudio()
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
        public string GetRandomAudio()
        {
            throw new NotImplementedException();
        }
    }
}
