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
			int sum = 0;
			while (Console.In.Peek() != -1)
			{
				sum += Int32.Parse( Console.ReadLine());
			}
			Console.WriteLine(sum);
		}
	}
}
