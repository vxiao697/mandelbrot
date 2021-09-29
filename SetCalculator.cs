using System;
using System.Diagnostics;


namespace mandelbrot
{
    internal struct SetPixel
    {
        public Complex complex;
        public int count;
    }

    class Run
    {   
        static void Main(string[] args)
        {
            SetCalculator sc = new SetCalculator(10, 10);

            sc.FillGrid(1.25, -2.0, 2.5, 2.5);

            Debug.Assert(true);
        }
    }
    public class SetCalculator
    {
        const double THRESHOLD = 4.0;

        double left;
        double top;
        double width;
        double height;
        int xPixels;
        int yPixels;

        private SetPixel[] grid = null;

        public SetCalculator(int xPix, int yPix)
        {
            xPixels = xPix;
            yPixels = yPix;
            grid = new SetPixel[xPixels * yPixels];
        }

        void IntializeGrid()
        {
            int pos = 0;
            double xGap = width / xPixels;
            double yGap = height / yPixels;

            for (int i = 0; i < yPixels; i++)
            {
                double imaginary = top - i * yGap;
                for (int j = 0; j < xPixels; j++)
                {
                    double real = left + j * xGap;
                    grid[pos].complex = new Complex(real, imaginary);

                    pos++;
                }
            }
        }

        public void FillGrid(double top, double left, double width, double height)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;

            IntializeGrid();
        }

    }
}
