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
			var sr = new StreamReader("input1.txt");
			int sum = 0;
			List<int> input = new List<int>();

			//List<int> freqList = new List<int>(); //90 000ms execution time
			HashSet<int> freqList = new HashSet<int>(); // 24ms execution time

			var watch = System.Diagnostics.Stopwatch.StartNew();
			while (sr.Peek() != -1)
			{
				input.Add(Int32.Parse(sr.ReadLine()));
			}

			while (true)
			{
				foreach(int number in input)
				{
					sum += number;
					if (freqList.Contains(sum))
					{
						Console.WriteLine(sum);
						watch.Stop();
						Console.WriteLine("Execution time: " + watch.ElapsedMilliseconds + "ms");
						return;
					}
					freqList.Add(sum);
				}
			}
		}
	}
}
