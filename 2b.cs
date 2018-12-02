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
			List<string> boxes = new List<string>();
			while (Console.In.Peek() != -1)
			{
				boxes.Add(Console.ReadLine());
			}

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
