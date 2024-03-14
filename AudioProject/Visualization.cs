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
        /*
        int renderPosition;
        double yTranslate = 40;
        double yScale = 40;
        double xScale = 2;
        int blankZone = 10;

        readonly Polygon waveForm = new Polygon();
        private Canvas mainCanvas;

        public Visualization(Canvas canvas)
        {
            mainCanvas = canvas;
            waveForm.Stroke = Brushes.WhiteSmoke;
            waveForm.StrokeThickness = 1;
            waveForm.Fill = new SolidColorBrush(Colors.WhiteSmoke);
            //mainCanvas.Children.Add(waveForm);
            //mainCanvas.Children.Add(topLine);
            //mainCanvas.Children.Add(bottomLine);
        }
        public void WaveFormControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // We will remove everything as we are going to rescale vertically
            renderPosition = 0;
            ClearAllPoints();

            yTranslate = mainCanvas.ActualHeight / 2;
            yScale = mainCanvas.ActualHeight / 2;
        }

        private void ClearAllPoints()
        {
            waveForm.Points.Clear();
        }

        private int Points
        {
            get { return waveForm.Points.Count / 2; }
        }

        public void AddValue(float maxValue, float minValue)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                int visiblePixels = (int)(mainCanvas.ActualWidth / xScale);
                if (visiblePixels > 0)
                {
                    CreatePoint(maxValue, minValue);

                    if (renderPosition > visiblePixels)
                    {
                        renderPosition = 0;
                    }
                    int erasePosition = (renderPosition + blankZone) % visiblePixels;
                    if (erasePosition < Points)
                    {
                        double yPos = SampleToYPosition(0);
                        waveForm.Points[erasePosition] = new Point(erasePosition * xScale, yPos);
                        waveForm.Points[BottomPointIndex(erasePosition)] = new Point(erasePosition * xScale, yPos);
                    }
                }
            });
        }

        private int BottomPointIndex(int position)
        {
            return waveForm.Points.Count - position - 1;
        }

        private double SampleToYPosition(float value)
        {
            return yTranslate + value * yScale;
        }

        private void CreatePoint(float topValue, float bottomValue)
        {
            double topYPos = SampleToYPosition(topValue);
            double bottomYPos = SampleToYPosition(bottomValue);
            double xPos = renderPosition * xScale;
            if (renderPosition >= Points)
            {
                int insertPos = Points;
                waveForm.Points.Insert(insertPos, new Point(xPos, topYPos));
                waveForm.Points.Insert(insertPos + 1, new Point(xPos, bottomYPos));
            }
            else
            {
                waveForm.Points[renderPosition] = new Point(xPos, topYPos);
                waveForm.Points[BottomPointIndex(renderPosition)] = new Point(xPos, bottomYPos);
            }
            renderPosition++;
        }

        /// <summary>
        /// Clears the waveform and repositions on the left
        /// </summary>
        public void Reset()
        {
            renderPosition = 0;
            ClearAllPoints();
        }
        */
        int renderPosition;
        double yTranslate = 40;
        double yScale = 40;
        int blankZone = 10;

        readonly Polyline topLine = new Polyline();
        readonly Polyline bottomLine = new Polyline();
        private Canvas mainCanvas;

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
            // We will remove everything as we are going to rescale vertically
            renderPosition = 0;
            ClearAllPoints();

            yTranslate = mainCanvas.ActualHeight / 2;
            yScale = mainCanvas.ActualHeight / 2;
        }

        private void ClearAllPoints()
        {
            topLine.Points.Clear();
            bottomLine.Points.Clear();
        }

        public void AddValue(float maxValue, float minValue)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                int pixelWidth = (int)mainCanvas.ActualWidth;
                if (pixelWidth > 0)
                {
                    CreatePoint(maxValue, minValue);

                    if (renderPosition > mainCanvas.ActualWidth)
                    {
                        renderPosition = 0;
                    }
                    int erasePosition = (renderPosition + blankZone) % pixelWidth;
                    if (erasePosition < topLine.Points.Count)
                    {
                        double yPos = SampleToYPosition(0);
                        topLine.Points[erasePosition] = new Point(erasePosition, yPos);
                        bottomLine.Points[erasePosition] = new Point(erasePosition, yPos);
                    }
                }
            });
        }

        private double SampleToYPosition(float value)
        {
            return yTranslate + value * yScale;
        }

        private void CreatePoint(float topValue, float bottomValue)
        {
            double topLinePos = SampleToYPosition(topValue);
            double bottomLinePos = SampleToYPosition(bottomValue);
            if (renderPosition >= topLine.Points.Count)
            {
                topLine.Points.Add(new Point(renderPosition, topLinePos));
                bottomLine.Points.Add(new Point(renderPosition, bottomLinePos));
            }
            else
            {
                topLine.Points[renderPosition] = new Point(renderPosition, topLinePos);
                bottomLine.Points[renderPosition] = new Point(renderPosition, bottomLinePos);
            }
            renderPosition++;
        }

        /// <summary>
        /// Clears the waveform and repositions on the left
        /// </summary>
        public void Reset()
        {
            renderPosition = 0;
            ClearAllPoints();
        }
    }
}