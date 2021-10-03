using System;
using System.Diagnostics;


namespace mandelbrot
{
    internal struct SetPixel
    {
        public Complex complex;
        public int count;
        public byte red;
        public byte green;
        public byte blue;
    }

    /*
    class Run
    {   
        static void Main(string[] args)
        {
            SetCalculator sc = new SetCalculator(50, 50);

            sc.FillGrid(1.25, -2.0, 2.5, 2.5);

            Debug.Assert(true);
        }
    }
    */

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

        public void GetColor(int pos, out byte r, out byte g, out byte b)
        {
            r = grid[pos].red;
            g = grid[pos].green;
            b = grid[pos].blue;
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

        public void FillGrid(double top, double left, double width, double height, char colorMode)
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

            pos = 0;
            for (int i = 0; i < yPixels; i++)
            {
                for (int j = 0; j < xPixels; j++)
                {
                    if(colorMode == 'g')
                    {
                        ApplyGrayScale(grid[pos].count, pos);
                    }
                    else if(colorMode == 'c')
                    {
                        ApplyColor(grid[pos].count, pos);
                    }
                    else if (colorMode == 'h')
                    {
                        ApplyHistogram(grid[pos].count, pos);
                    }
                    pos++;
                }
            }
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
                        Console.Write("**");
                    }
                    pos++;
                }

                Console.WriteLine();
            }
        }

        private void ApplyGrayScale(int count, int pos)
        {
            grid[pos].red = (byte)(256 - count);
            grid[pos].green = (byte)(256 - count);
            grid[pos].blue = (byte)(256 - count);
        }

        private void ApplyColor(int count, int pos)
        {
            if (count == 255)
            {
                grid[pos].red = 0;
                grid[pos].green = 0;
                grid[pos].blue = 0;
            }
            else if (count > 0 && count <= 85)
            {
                grid[pos].red = (byte)((double)(count * 3));
                grid[pos].green = (byte)(71 + (double)(count * 2.283));
                grid[pos].blue = (byte)(171 + (double)(count));
            }
            else if (count > 85 && count <= 170)
            {
                count = 170 - (count - 85);
                grid[pos].red = 255;
                grid[pos].green = (byte)(1.5 * (double)(170 - (count - 85)));
                grid[pos].blue = (byte)(3 * (double)(170 - count));
            }
            else
            {
                grid[pos].red = (byte)(1.5 * (double)(255 - count));
                grid[pos].green = (byte)(1.5 * (double)(255 - count));
                grid[pos].blue = 0;
            }
        }

        private void ApplyHistogram(int count, int pos)
        {
            if (count == 255)
            {
                grid[pos].red = 0;
                grid[pos].green = 0;
                grid[pos].blue = 0;
            }
            else
            {
                count = count % 8;

                switch (count)
                {
                    case 0:
                        {
                            grid[pos].red = 255;
                            grid[pos].green = 0;
                            grid[pos].blue = 0;
                        }
                        break;

                    case 1:
                        {
                            grid[pos].red = 255;
                            grid[pos].green = 127;
                            grid[pos].blue = 0;
                            break;
                        }
                    case 2:
                        {
                            grid[pos].red = 255;
                            grid[pos].green = 255;
                            grid[pos].blue = 0;
                            break;
                        }
                    case 3:
                        {
                            grid[pos].red = 0;
                            grid[pos].green = 255;
                            grid[pos].blue = 0;
                            break;
                        }
                    case 4:
                        {
                            grid[pos].red = 0;
                            grid[pos].green = 0;
                            grid[pos].blue = 255;
                            break;
                        }
                    case 5:
                        {
                            grid[pos].red = 0;
                            grid[pos].green = 0;
                            grid[pos].blue = 255;
                            break;
                        }
                    case 6:
                        {
                            grid[pos].red = 75;
                            grid[pos].green = 0;
                            grid[pos].blue = 130;
                            break;
                        }
                    case 7:
                        {
                            grid[pos].red = 143;
                            grid[pos].green = 0;
                            grid[pos].blue = 255;
                            break;
                        }
                }
            }
        }
    }
}
