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
			Dictionary<int, int[]> guardAsleep = new Dictionary<int, int[]>();
			var sr = new StreamReader("input4.txt");

			string[] log = sr.ReadToEnd().TrimEnd('\n').Split('\n');
			Array.Sort(log);

			int currentGuard = 0;
			int sleepStart = 0;
			foreach (string line in log)
			{
				if (line.Contains("Guard"))
				{
					currentGuard = Int32.Parse(line.Split('#')[1].Split(' ')[0]);
					if (!guardAsleep.ContainsKey(currentGuard))
					{
						guardAsleep.Add(currentGuard, new int[60]);
					}
				}else if(line.Contains("falls asleep"))
				{
					sleepStart = Int32.Parse(line.Split(' ')[1].Split(':')[1].TrimEnd (']'));
				}
				else if(line.Contains ("wakes up"))
				{
					int sleepEnd = Int32.Parse(line.Split(' ')[1].Split(':')[1].TrimEnd(']'));
					for (int i = sleepStart; i < sleepEnd; i++)
					{
						guardAsleep[currentGuard][i]++;
					}
				}
			}


			//PART1
			int maxSleep = 0;
			int maxId = 0;
			int mostAsleepTime = 0;
			for(int i = 0; i< guardAsleep.Count; i++)
			{
				int sleep = 0;
				foreach(int time in guardAsleep.ElementAt(i).Value)
				{
					sleep += time;
				}
				if (sleep > maxSleep)
				{
					maxSleep = sleep;
					maxId = guardAsleep.ElementAt(i).Key;
					int mostAsleep = guardAsleep.ElementAt(i).Value.Max();
					mostAsleepTime = guardAsleep.ElementAt(i).Value.ToList().IndexOf(mostAsleep);
				}
			}
			//PART1

			//PART2
			int maxFreqSleep = 0;
			int maxFreqId = 0;
			int mostFreqAsleepTime = 0;
			for (int i = 0; i < guardAsleep.Count; i++)
			{
				int mostAsleep = guardAsleep.ElementAt(i).Value.Max();
				if (mostAsleep > maxFreqSleep)
				{
					maxFreqSleep = mostAsleep;
					maxFreqId = guardAsleep.ElementAt(i).Key;
					mostFreqAsleepTime = guardAsleep.ElementAt(i).Value.ToList().IndexOf(mostAsleep);
				}
			}
			//PART2

			Console.WriteLine(maxId);
			Console.WriteLine(mostAsleepTime);

			Console.WriteLine(maxFreqId);
			Console.WriteLine(mostFreqAsleepTime);

		}
	}
}
