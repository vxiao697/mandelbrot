using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace mandelbrot
{
    class Complex
    {
        //expressed in form a+bi, where a = re and b = im
        private double re;
        private double im;

        public Complex()
        {
            re = 0.0;
            im = 0.0;
        }

        public Complex(Complex complex)
        {
            re = complex.Real();
            im = complex.Imaginary();
        }

		public Complex(double real, double imag)
		{
			re = real;
			im = imag;
		}

		public double Real() { return re; }

		public double Imaginary() { return im; }

        public void Real(double real) { re = real; }

		public void Imaginary(double imag) { im = imag; }

		public void Add(Complex other)
		{
			re += other.re;
			im += other.im;
		}

		public void Subtract(Complex other)
		{
			re -= other.re;
			im -= other.im;
		}

		public void Multiply(Complex other)
		{
			double a = re;
			double b = im;
			double c = other.re;
			double d = other.im;

			re = a * c - b * d;
			im = a * d + b * c;
		}

		public void Divide(Complex other)
		{
			double a = re;
			double b = im;
			double c = other.re;
			double d = other.im;

			re = (a * c + b * d) / (c * c + d * d);
			im = (b * c - a * d) / (c * c + d * d);
		}

		public double MagnitudeSquared()
		{
			return re * re + im * im;
		}

		public void Print()
		{
			Console.WriteLine((re != 0 ? re.ToString() : "") + (im > 0 && re != 0 ? "+" : "") + (im != 0 ? im.ToString() + "i" : ""));
		}
	}

	public class ComplexTest
	{
		public void Run()
		{
			{
				Complex complex1 = new Complex(1.0, 1.0);
				Complex complex2 = new Complex(1.0, -1.0);
				complex1.Add(complex2);

				Debug.Assert(complex1.Real() == 2.0);
				Debug.Assert(complex1.Imaginary() == 0);
			}

			
			{
				Complex complex1 = new Complex(2.0, 3.0);
				Complex complex2 = new Complex(4.0, 5.0);
				complex1.Multiply(complex2);

				Debug.Assert(complex1.Real() == -7.0);
				Debug.Assert(complex1.Imaginary() == 22);
			}

			{
				Complex complex1 = new Complex(4.0, 2.0);
				Complex complex2 = new Complex(3.0, -1.0);
				complex1.Divide(complex2);

				Debug.Assert(complex1.Real() == 1.0);
				Debug.Assert(complex1.Imaginary() == 1);
			}
			
			/*{
				complex1.Subtract(complex2);
				complex1.Multiply(complex2);
				complex1.Print();

				Console.Write("Real = " + complex1.Real());
				Console.Write("\n Imaginary = " + complex1.Imaginary());
			}*/
		}
	};
}
