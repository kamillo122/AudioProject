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
        public void IncreaseQueueIndex()
        {
            QueueIndex++;
        }
        public void DecreaseQueueIndex()
        {
            QueueIndex--;
        }
        public void SetQueueIndex(int index)
        {
            QueueIndex = index;
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
                return String.Empty;
            }
            if (AudioPaths.Count == 1)
            {
                return AudioPaths[0];
            }
            return AudioPaths[QueueIndex];
        }
    }
}
