using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
	class Program
	{
		static void Main(string[] args)
		{
			int count2 = 0;
			int count3 = 0;
			while (Console.In.Peek() != -1)
			{
				bool line2 = false;
				bool line3 = false;
				string line = new string(Console.ReadLine().OrderBy(c => c).ToArray()) + " ";//different last char - for easier checks in loop

				int count = 1;
				char ch = line[0];
				for (int i = 1; i < line.Length; i++)
				{
					if (ch == line[i])
					{
						count++;
					}
					else
					{
						if (count == 2)
						{
							line2 = true;
						}
						else if (count == 3)
						{
							line3 = true;
						}
						ch = line[i];
						count = 1;
					}

				}

				if (line2) count2++;
				if (line3) count3++;
			}
			Console.WriteLine(count2 * count3);
		}
	}
}
