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
		static HashSet<long> pots = new HashSet<long>();
		static void Main(string[] args)
		{
			int offset = 100;
			StreamReader sr = new StreamReader("input12.txt");
			List<Recipe> notes = new List<Recipe>();
			string initial = sr.ReadLine(); sr.ReadLine();//remove empty
			initial = initial.Substring(initial.IndexOf(':') + 2);
			for(int i = 0;i<initial.Length; i++)
			{
				if (initial[i] == '#')
				{
					pots.Add(i + offset);
				}
			}
			while (sr.Peek() != -1)
			{
				Recipe tmp = new Recipe();
				tmp.mostLeft = sr.Read() == '#' ? true : false;
				tmp.left = sr.Read() == '#' ? true : false;
				tmp.current = sr.Read() == '#' ? true : false;
				tmp.right = sr.Read() == '#' ? true : false;
				tmp.mostRight = sr.Read() == '#' ? true : false;
				sr.Read();sr.Read();sr.Read();sr.Read();//" => "
				tmp.result = sr.Read() == '#' ? true : false;
				sr.Read();//\n
				notes.Add(tmp);
			}

			//PART1
			for(int i =0; i < 20; i++)
			{
				HashSet<long> nextGenPots = new HashSet<long>();
				for (int j = 2; j < 298; j++)
				{
					Recipe match = new Recipe(pVal(j-2), pVal(j-1), pVal(j) , pVal(j+1) , pVal(j+2), true);
					if (notes.Contains(match))
					{
						nextGenPots.Add(j);
					}
				}
				pots = new HashSet<long>(nextGenPots);
			}
			int result = 0;
			foreach(int pot in pots)
			{
				result += pot - offset;
			}
			Console.WriteLine(result);
			//PART1

			//PART2
			pots = new HashSet<long>();
			for (int i = 0; i < initial.Length; i++)
			{
				if (initial[i] == '#')
				{
					pots.Add(i + offset);
				}
			}

			List<List<int>> set = new List<List<int>>();//every observed distribution
			List<int> movement = new List<int>();//how much first plant moved comparing to previous generation
			long last = pots.First();
			int eachMove = 0;
			int move = 0;
			long x;//need to remember when break occurs
			for (x = 0; x < 50000000000; x++)//should break much faster - if not, full simulation is performed(VERY SLOW)
			{
				HashSet<long> nextGenPots = new HashSet<long>();
				long min = pots.Min() - 3;//minimise search range
				long max = pots.Max() + 3;//minimise search range
				List<int> currentSet = new List<int>();
				bool first = false;
				int diff = 0;
				for (long j = min; j < max; j++)
				{
					Recipe match = new Recipe(pVal(j - 2), pVal(j - 1), pVal(j), pVal(j + 1), pVal(j + 2), true);
					if (notes.Contains(match))
					{
						nextGenPots.Add(j);//this generation
						if (!first)//calculate relative difference between each plant
						{
							currentSet.Add(0);
							first = true;
						}
						else
						{
							currentSet.Add(diff);
							diff = 0;
						}
					}
					diff++;
				}
				pots = new HashSet<long>(nextGenPots);//update pots
				bool breakFor = false;
				foreach (List<int> s in set)
				{
					if (s.SequenceEqual(currentSet))//ordered comparison
					{
						int lastMatch = set.IndexOf(s);
						move = (int)(x - lastMatch);//how many generations before the same distribution
						for(int k = lastMatch; k < x; k++)
						{
							eachMove += movement[k];//how much plants moved 
						}
						breakFor = true;
					}
				}
				if (breakFor) break;

				set.Add(currentSet);//for pattern finding

				movement.Add((int)(pots.First() - last));//calcualte movement
				last = pots.First();//assign new last
			}

			long result2 = 0;
			long diffr = (50000000000 - x - 1) * eachMove;//assuming move == 1
			foreach (long pot in pots)
			{
				result2 += (pot + diffr) - offset;
			}
			Console.WriteLine(result2);
			//PART2
		}
		struct Recipe
		{
			public Recipe(bool mL, bool l, bool c, bool r, bool mR, bool res)
			{
				mostLeft = mL;
				left = l;
				current = c;
				right = r;
				mostRight = mR;
				result = res;
			}
			public bool mostLeft;
			public bool left;
			public bool current;
			public bool right;
			public bool mostRight;
			public bool result;
		}
		static bool pVal(long index)
		{
			return pots.Contains(index);
		}
	}
}
		

	

