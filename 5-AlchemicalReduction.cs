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
			List<char> polymer = new List<char>();
			var sr = new StreamReader("input5.txt");
			
			foreach(char c in sr.ReadToEnd())
			{
				polymer.Add(c);
			}
			polymer.Remove('\n');

			//PART1
			Console.WriteLine(polymerise(polymer).Count);
			//PART1

			//PART2
			int smallest = Int32.MaxValue;
			char cut;
			for(char c = 'a'; c <= 'z'; c++)
			{
				List<char> polycut = new List<char>(polymer);
				polycut.RemoveAll(item => item == c);
				polycut.RemoveAll(item => item == c - 32);
				int count = polymerise(polycut).Count;
				if (smallest > count)
				{
					smallest = count;
					cut = c;
				}
			}
			Console.WriteLine(smallest);
			//PART2


		}

		static List<char> polymerise(List<char> _polymer)
		{
			List<char> polymer = new List<char>(_polymer);//do not change original
			bool changed = true;
			while (changed)
			{
				changed = false;
				char prev = polymer[0];
				for (int i = 1; i < polymer.Count; i++)
				{
					if (polymer[i] - 32 == prev || polymer[i] + 32 == prev)
					{
						polymer.RemoveAt(i);
						polymer.RemoveAt(i - 1);
						changed = true;
						break;
					}
					prev = polymer[i];
				}
			}
			return polymer;
		}
	}
	
}
