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
			Dictionary<char, List<char>> prerequisites = new Dictionary<char, List<char>>();
			List<char> states = new List<char>();
			var sr = new StreamReader("input7.txt");

			while(sr.Peek() != -1)
			{
				string[] line = sr.ReadLine().Split(' ');
				if (!prerequisites.ContainsKey(line[7][0]))//to char
				{
					prerequisites.Add(line[7][0], new List<char>());
				}
				prerequisites[line[7][0]].Add(line[1][0]);
				if(!states.Contains(line[1][0])) states.Add(line[1][0]);
				if(!states.Contains(line[7][0])) states.Add(line[7][0]);
			}


			//PART1
			List<char> completed = new List<char>();
			List<char> possible = new List<char>();

			for (int i = 0; i < states.Count; i++)
			{
				foreach (char s in states.Except(completed).Except(possible))
				{
					bool prerequisiteCompleted = true;
					if (prerequisites.ContainsKey(s))
					{
						foreach (char pre in prerequisites[s])
						{
							if (!completed.Contains(pre))
							{
								prerequisiteCompleted = false;
								break;
							}
						}
					}
					if (prerequisiteCompleted)
					{
						possible.Add(s);
					}
				}

				possible.Sort();
				completed.Add(possible[0]);
				possible.RemoveAt(0);

			}
			completed.ForEach(i => Console.Write(i));
			Console.WriteLine();
			//PART1

			//PART2
			completed = new List<char>();
			possible = new List<char>();
			Dictionary<char, int> workers = new Dictionary<char, int>();

			for (int i = 0; ; i++)
			{
				foreach (char s in states.Except(completed).Except(possible))
				{
					if ( workers.ContainsKey(s))
					{
						continue;
					}
					bool prerequisiteCompleted = true;
					if (prerequisites.ContainsKey(s))
					{
						foreach (char pre in prerequisites[s])
						{
							if (!completed.Contains(pre))
							{
								prerequisiteCompleted = false;
								break;
							}
						}
					}
					if (prerequisiteCompleted)
					{
						possible.Add(s);
					}
				}


				possible.Sort();

				while (workers.Count < 5 && possible.Count > 0)
				{
					workers.Add(possible[0], possible[0] - 4);
					possible.RemoveAt(0);
				}

				if (workers.Count > 0)
				{
					for (int j = workers.Count - 1; j >= 0; j--)
					{
						if (workers.ElementAt(j).Value == 1)
						{
							completed.Add(workers.ElementAt(j).Key);
							workers.Remove(workers.ElementAt(j).Key); 

						}
						else
						{
							workers[workers.ElementAt(j).Key]--;
						}
					}
				}

				if(completed.Count == states.Count)
				{
					Console.WriteLine(i+1);
					break;
				}

			}
			//PART2


		}
	}
}
