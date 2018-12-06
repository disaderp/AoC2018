using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
	class Program
	{
		static int dimX = 0;
		static int dimY = 0;
		static void Main(string[] args)
		{
			List<Point> points = new List<Point>();
			var sr = new StreamReader("input6.txt");

			int maxX = 0;
			int maxY = 0;
			while(sr.Peek() != -1)
			{
				Point t = new Point();
				string line = sr.ReadLine().Replace(" ", "");
				t.x = Int32.Parse(line.Split(',')[0]);
				t.y = Int32.Parse(line.Split(',')[1]);
				points.Add(t);
				if (maxX < t.x) maxX = t.x;
				if (maxY < t.y) maxY = t.y;
			}

			dimX = maxX + 1;
			dimY = maxY + 1;
			List<Point>[,] map = new List<Point>[dimX, dimY];
			List<Point> validPoints = new List<Point>(points);

			//PART1
			for (int i = 0; i < dimX; i++)
			{
				for(int j = 0; j < dimY; j++)
				{
					Point actual = new Point();
					actual.x = i;
					actual.y = j;
					int minDistance = Int32.MaxValue;
					List<Point> minPoints = new List<Point>();
					foreach(Point coord in points)
					{
						int pointDistance = distance(actual, coord);
						if(minDistance > pointDistance)
						{
							minPoints.Clear();
							minPoints.Add(coord);
							minDistance = pointDistance;
						}else if(minDistance == pointDistance)
						{
							minPoints.Add(coord);
						}
					}

					map[i, j] = new List<Point>(minPoints);
					if(isEdge(i, j))
					{
						foreach(Point p in minPoints)
						{
							validPoints.Remove(p);
						}
					}
				}
			}

			int maxField = 0;
			Point maxPoint = new Point();
			foreach(Point p in validPoints)
			{
				int pField = 0;
				for (int i = 0; i < dimX; i++)
				{
					for (int j = 0; j < dimY; j++)
					{
						if (map[i, j].Contains(p) && map[i,j].Count == 1)
						{
							pField++;
						}
					}
				}
				if(maxField < pField)
				{
					maxField = pField;
					maxPoint = p;
				}
			}
			Console.WriteLine(maxField);
			//PART1

			//PART2
			int regionArea = 0;
			for (int i = 0; i < dimX; i++)
			{
				for (int j = 0; j < dimY; j++)
				{
					int total = 0;
					Point actual = new Point();
					actual.x = i;
					actual.y = j;
					foreach(Point p in points)
					{
						total += distance(actual, p);
					}

					if(total < 10000) { regionArea++; }
				}
			}
			Console.WriteLine(regionArea);
			//PART2


		}

		static int distance(Point x, Point y)
		{
			return Math.Abs(x.x - y.x) + Math.Abs(x.y - y.y);
		}
		static bool isEdge(int x, int y)
		{
			if (x == 0 || y == 0 || x == dimX - 1 || y == dimY - 1) return true;
			return false;
		}
	}
	struct Point
	{
		public int x;
		public int y;
	}
}
