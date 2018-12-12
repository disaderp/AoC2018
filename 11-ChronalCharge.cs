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
		static int serialNum;
		static void Main(string[] args)
		{
			StreamReader sr = new StreamReader("input11.txt");
			serialNum = Int32.Parse(sr.ReadLine());

			int[,] fuel = new int[301, 301];
			for (int i = 1; i < 301; i++)
			{
				for(int j = 1; j < 301; j++)
				{
					fuel[i, j] = calcPower(i, j);
				}
			}

			//PART1
			int[] maxGrid = new int[2];
			int maxValue = 0;
			for (int i = 2; i < 300; i++)
			{
				for (int j = 2; j < 300; j++)
				{
					int sum = fuel[i - 1, j - 1] + fuel[i - 1, j] + fuel[i - 1, j + 1] +
						fuel[i, j - 1] + fuel[i, j] + fuel[i, j + 1] +
						fuel[i + 1, j - 1] + fuel[i + 1, j] + fuel[i + 1, j + 1];
					if(sum > maxValue)
					{
						maxValue = sum;
						maxGrid[0] = i;
						maxGrid[1] = j;
					}
				}
			}
			Console.WriteLine((maxGrid[0] - 1) + "," + (maxGrid[1] -1));
			//PART1

			//PART2
			int[] maxGrid2 = new int[2];
			int gridSize2 = 0;
			int maxValue2 = 0;
			for (int i = 1; i < 300; i++)
			{
				for (int j = 1; j < 300; j++)
				{
					for (int k = 1; i+k <= 301 && k+j <= 301; k++)
					{
						int sum=0;
						for (int z = 0; z < k && i+z < 301; z++)
						{
							for(int x = 0; x < k && j+x < 301; x++)
							{
								sum += fuel[i + z, j + x];
							}	
						}
						
						if (sum > maxValue2)
						{
							maxValue2 = sum;
							maxGrid2[0] = i;
							maxGrid2[1] = j;
							gridSize2 = k;
						}
					}
					
				}
			}
			Console.WriteLine(maxGrid2[0]+ "," + maxGrid2[1] + "," + gridSize2);
			//PART2
		}
		static int calcPower(int x, int y)
		{
			int rackId = 10 + x;
			int number = (rackId * y + serialNum) * rackId;
			int hundredDigit = (int)Math.Abs(number / 100 % 10);
			return hundredDigit - 5;
			
		}
	}
}
		

	

