using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AudioProject
{
    /// <summary>
    /// Interaction logic for PolygonWaveFormControl.xaml
    /// </summary>
    public class Visualization : Canvas, IWaveFormRenderer
    {
        double yTranslate = 20;
        double yScale = 20;

        readonly Polyline topLine = new Polyline();
        readonly Polyline bottomLine = new Polyline();
        private readonly Canvas mainCanvas;

        public Visualization(Canvas canvas)
        {
            mainCanvas = canvas;
            topLine.Stroke = Brushes.WhiteSmoke;
            bottomLine.Stroke = Brushes.WhiteSmoke;
            topLine.StrokeThickness = 1;
            bottomLine.StrokeThickness = 1;
            mainCanvas.Children.Add(topLine);
            mainCanvas.Children.Add(bottomLine);
        }
        public void WaveFormControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Reset();
            yTranslate = mainCanvas.ActualHeight / 2;
            yScale = mainCanvas.ActualHeight / 2;
        }

        public void AddValue(float maxValue, float minValue)
        {
            int pixelWidth = (int)mainCanvas.ActualWidth;
            if (pixelWidth <= 0)
                return;

            CreatePoint(maxValue, minValue);

            // Check if the number of points exceeds the width of the canvas
            if (topLine.Points.Count > pixelWidth)
            {
                // Remove the first point from both topLine and bottomLine
                topLine.Points.RemoveAt(0);
                bottomLine.Points.RemoveAt(0);

                // Update the x-coordinates of the remaining points to shift the waveform
                for (int i = 0; i < topLine.Points.Count; i++)
                {
                    topLine.Points[i] = new Point(i, topLine.Points[i].Y);
                    bottomLine.Points[i] = new Point(i, bottomLine.Points[i].Y);
                }
            }

        }

        private double SampleToYPosition(float value)
        {
            return yTranslate + value * yScale;
        }

        private void CreatePoint(float topValue, float bottomValue)
        {
            double topLinePos = SampleToYPosition(topValue);
            double bottomLinePos = SampleToYPosition(bottomValue);

            topLine.Points.Add(new Point(topLine.Points.Count, topLinePos));
            bottomLine.Points.Add(new Point(bottomLine.Points.Count, bottomLinePos));
        }

        /// <summary>
        /// Clears the waveform and repositions on the left
        /// </summary>
        public void Reset()
        {
            topLine.Points.Clear();
            bottomLine.Points.Clear();
        }
    }
}