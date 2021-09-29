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
            SetCalculator sc = new SetCalculator(50, 50);

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


            int pos = 0;
            for (int i = 0; i < yPixels; i++)
            {
                for (int j = 0; j < xPixels; j++)
                {
                    iterate(pos);
                    pos++;
                }
            }

            print();
        }

        private void iterate(int pos)
        {
            int count = 0;
            Complex c = new Complex(grid[pos].complex);
            Complex z = new Complex(0, 0);

            do
            {
                z.Multiply(z);
                z.Add(c);

                count++;
            } while (IsDivergent(z) == false && count < 255);

            grid[pos].count = count;
        }

        private bool IsDivergent(Complex c)
        {
            if (c.MagnitudeSquared() < THRESHOLD)
                return false;
            else
                return true;
        }

        private void print()
        {
            int pos = 0;

            for (int i = 0; i < yPixels; i++)
            {
                for (int j = 0; j < xPixels; j++)
                {

                    if (grid[pos].count > 0 && grid[pos].count < 64)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    pos++;
                }

                Console.WriteLine();
            }
        }
    }
}
