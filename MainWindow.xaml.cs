using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;


namespace mandelbrot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		public MainWindow()
		{
			InitializeComponent();

			imageHeight = (int)this.mbimage.Height;
			imageWidth = (int)this.mbimage.Width;

			pf = PixelFormats.Rgb24;
			rawStride = (imageWidth * pf.BitsPerPixel + 7) / 8;
			pixelData = new byte[rawStride * imageHeight];

			cal = new SetCalculator(imageWidth, imageHeight);
		}

		void SetPixel(int x, int y, Color c, byte[] buffer, int rawStride)
		{
			int xIndex = x * 3;
			int yIndex = y * rawStride;
			buffer[xIndex + yIndex] = c.R;
			buffer[xIndex + yIndex + 1] = c.G;
			buffer[xIndex + yIndex + 2] = c.B;
		}

		public void update_size(object sender, RoutedEventArgs e)
		{

		}

		private void calculateClicked(object sender, RoutedEventArgs e)
		{
			this.Cursor = Cursors.Wait;
			Button btn = (Button)sender;
			btn.IsEnabled = false;

			try
			{
				top = Convert.ToDouble(topEdit.Text);
			}
			catch (FormatException)
			{
				top = 1.25;
			}

			try
			{
				left = Convert.ToDouble(leftEdit.Text);
			}
			catch (FormatException)
			{
				left = -2.0;

			}

			try
			{
				width = Convert.ToDouble(widthEdit.Text);
			}
			catch (FormatException)
			{
				width = 2.5;
			}

			try
			{
				height = Convert.ToDouble(heightEdit.Text);
			}
			catch (FormatException)
			{
				height = 2.5;
			}

			cal.FillGrid(top, left, width, height);
			int pos = 0;
			for (int y = 0; y < imageHeight; y++)
			{
				for (int x = 0; x < imageWidth; x++)
				{
					Color c = new Color();

					byte r, g, b;
					cal.GetColor(pos, out r, out g, out b);
					c.R = r;
					c.G = g;
					c.B = b;

					SetPixel(x, y, c, pixelData, rawStride);

					pos++;
				}
			}

			bitmap = BitmapSource.Create(imageWidth, imageHeight, 96, 96, pf, null, pixelData, rawStride);
			mbimage.Source = bitmap;

			this.Cursor = Cursors.Arrow;
			btn.IsEnabled = true;
		}

		private void mbimage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			oldTop = top;
			oldLeft = left;
			oldWidth = width;
			oldHeight = height;

			Point pt = e.GetPosition(this);
			double pX = pt.X;
			double pY = pt.Y;

			Debug.Write("X = " + pX + " Y = " + pY);
			double zoomValue = Convert.ToDouble(zoomEdit.Text);
			double zoomFactor = Math.Sqrt(zoomValue);

			double newWidth = width / zoomFactor;
			double newHeight = height / zoomFactor;

			double newLeft = (left + ((pX / imageWidth) * width)) - (newWidth / 2.0);
			double newTop = (top - ((pY / imageHeight) * height)) + (newHeight / 2.0);

			topEdit.Text = Convert.ToString(newTop);
			leftEdit.Text = Convert.ToString(newLeft);
			widthEdit.Text = Convert.ToString(newWidth);
			heightEdit.Text = Convert.ToString(newHeight);
		}

		double top = 1.25;
		double left = -2.0;
		double width = 2.5;
		double height = 2.5;

		double oldTop;
		double oldLeft;
		double oldWidth;
		double oldHeight;

		/*
		double top = 0.375;
		double left = -1.0;
		double width = 0.25;
		double height = 0.25;
		*/

		BitmapSource bitmap;
		PixelFormat pf;
		int imageWidth, imageHeight, rawStride;

		private void mbimage_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			topEdit.Text = Convert.ToString(oldTop);
			leftEdit.Text = Convert.ToString(oldLeft);
			widthEdit.Text = Convert.ToString(oldWidth);
			heightEdit.Text = Convert.ToString(oldHeight);
		}

		byte[] pixelData;
		SetCalculator cal = null;
	}
}
