using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
	class Program
	{
		static void Main(string[] args)
		{
			List<int[]> points = new List<int[]>();
			List<int[]> pointsOrig = new List<int[]>();
			StreamReader sr = new StreamReader("input10.txt");
			while (sr.Peek() != -1)
			{
				int[] t = new int[4];
				int[] t2 = new int[4];
				string line = sr.ReadLine();

				int start_index = line.IndexOf('<') + 1;
				int end_index = line.IndexOf('>');
				int length = end_index - start_index;
				string xy = line.Substring(start_index, length);

				t[0] = Int32.Parse(xy.Split(',')[0]);
				t[1] = Int32.Parse(xy.Split(',')[1]);

				start_index = line.LastIndexOf('<') + 1;
				end_index = line.LastIndexOf('>');
				length = end_index - start_index;
				xy = line.Substring(start_index, length);

				t[2] = Int32.Parse(xy.Split(',')[0]);
				t[3] = Int32.Parse(xy.Split(',')[1]);
				points.Add(t);
				t.CopyTo(t2, 0);//copy values not references
				pointsOrig.Add(t2);
			}

			//PART1
			int smallestI = 0;
			int smallestBox = Int32.MaxValue;
			for (int i = 0; i < 15000; i++)
			{
				int minx = Int32.MaxValue;
				int miny = Int32.MaxValue;
				int maxx = 0;
				int maxy = 0;
				
				foreach (int[] t in points)
				{
					t[0] += t[2];
					t[1] += t[3];
					if (t[0] > maxx) maxx = t[0];
					if (t[0] < minx) minx = t[0];

					if (t[1] > maxy) maxy = t[1];
					if (t[1] < miny) miny = t[1];
				}
				if (maxx - minx + maxy - miny < smallestBox)
				{
					smallestBox = maxx - minx + maxy - miny;
					smallestI = i;
				}
			}

			int minx2 = Int32.MaxValue;
			int miny2 = Int32.MaxValue;
			int maxx2 = 0;
			int maxy2 = 0;
			foreach (int[] t in pointsOrig)
			{
				t[1] += t[3] * (smallestI+1);
				t[0] += t[2] * (smallestI+1);
				if (t[0] > maxx2) maxx2 = t[0];
				if (t[0] < minx2) minx2 = t[0];
							   		  
				if (t[1] > maxy2) maxy2 = t[1];
				if (t[1] < miny2) miny2 = t[1];
			}
			Bitmap stars = new Bitmap(maxx2 - minx2 + 2, maxy2 - miny2 + 2);
			Graphics starGraphics = Graphics.FromImage(stars);
			Pen blackPen = new Pen(Brushes.Black);
			foreach (int[] p in pointsOrig)
			{
				starGraphics.DrawRectangle(blackPen, normalise(p[0], minx2, maxx2, maxx2 - minx2), normalise(p[1], miny2, maxy2, maxy2 - miny2), 1, 1);
			}
			stars.Save("stars.bmp");
			//PART1

			//PART2
			Console.WriteLine(smallestI+1);
			//PART2
		}
		static int normalise(int val, int min, int max, int rangemax)//rangemin = 0
		{
			return (int)(((float)(val - min) / (float)(max - min)) *(rangemax - 0) + 0);
		}
	}
}
		

	

