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
		static void Main(string[] args)
		{
			List<string> boxes = new List<string>();
			var sr = new StreamReader("input2.txt");

			int count2 = 0;
			int count3 = 0;
			while (sr.Peek() != -1)
			{
				string t = sr.ReadLine();
				boxes.Add(t);

				//PART1
				bool line2 = false;
				bool line3 = false;
				string line = new string(t.OrderBy(c => c).ToArray()) + " ";//different last char - for easier checks in loop

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
				//PART1
			}

			Console.WriteLine(count2 * count3);

			foreach (string box1 in boxes)
			{
				foreach ( string box2 in boxes)
				{
					if (box1 == box2) continue;
					int diff = 0;
					for(int i = 0; i < box1.Length; i++)
					{
						if((box1[i] != box2[i]))
						{
							diff++;
						}
						if(diff > 1) { break; }//difference on more than 1 position
					}
					if(diff == 1)
					{
						for (int i = 0; i < box1.Length; i++)
						{
							if (box1[i] == box2[i])
								Console.Write(box1[i]);
						}
						return;
					}
				}
			}
		}
	}
}
