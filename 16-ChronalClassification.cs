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
			var sr = new StreamReader("input16.txt");

			//PART1
			int count = 0;
			while (sr.Peek() != -1 && sr.Peek() != '\n')
			{
				int[] before = Array.ConvertAll(sr.ReadLine().Substring(9, 10).Split(','), int.Parse);
				int[] operation = Array.ConvertAll(sr.ReadLine().Split(' '), int.Parse);
				int[] after = Array.ConvertAll(sr.ReadLine().Substring(9, 10).Split(','), int.Parse);
				sr.ReadLine();//read empty

				int localCount = 0;
				int possCount = 0;
				Opcode poss = new Opcode();
				foreach(Opcode o in opcodes)
				{
					int[] regs = new int[4];
					before.CopyTo(regs, 0);

					o.execute(regs, operation[1], operation[2], operation[3]);
					if (regs.SequenceEqual(after))
					{
						localCount++;
						if(o.opcode == -1)
						{
							possCount++;
							poss = o;
						}
					}
				}

				if(possCount == 1)
				{
					opcodes[Array.FindIndex(opcodes, x => x.op == poss.op)].opcode = operation[0];
				}

				if (localCount >= 3) count++;

				if(sr.Peek() == '\n')//check if all opcodes resolved
				{
					foreach (Opcode o in opcodes)
					{
						Console.WriteLine(o.op + " : " + o.opcode);
					}
					foreach (Opcode o in opcodes)
					{
						if(o.opcode == -1)//if not resolved after first run 
						{
							sr.BaseStream.Position = 0;
							sr.DiscardBufferedData();
							count = 0;
							break;//restart
						}
					}
				}
			}

			Console.WriteLine(count);
			//PART1

			//PART2
			sr.ReadLine(); sr.ReadLine();//consume separator
			int[] programReg = new int[4];
			while (sr.Peek() != -1)
			{
				int[] operation = Array.ConvertAll(sr.ReadLine().Split(' '), int.Parse);
				Array.Find(opcodes, x => x.opcode == operation[0]).execute(programReg, operation[1], operation[2], operation[3]);
			}
			Console.WriteLine(programReg[0]);
			//PART2
		}

		static Opcode[] opcodes = {
			//addition
			new Opcode ("addr", (int[] reg, int a, int b, int c) => reg[c] = reg[a] + reg[b]),
			new Opcode ("addi", (int[] reg, int a, int b, int c) => reg[c] = reg[a] + b),
			//multiplication
			new Opcode ("mulr", (int[] reg, int a, int b, int c) => reg[c] = reg[a] * reg[b]),
			new Opcode ("muli", (int[] reg, int a, int b, int c) => reg[c] = reg[a] * b),
			//bitwise and
			new Opcode ("banr", (int[] reg, int a, int b, int c) => reg[c] = reg[a] & reg[b]),
			new Opcode ("bani", (int[] reg, int a, int b, int c) => reg[c] = reg[a] & b),
			//bitwise or
			new Opcode ("borr", (int[] reg, int a, int b, int c) => reg[c] = reg[a] | reg[b]),
			new Opcode ("bori", (int[] reg, int a, int b, int c) => reg[c] = reg[a] | b),
			//assignment
			new Opcode ("setr", (int[] reg, int a, int b, int c) => reg[c] = reg[a]),
			new Opcode ("seti", (int[] reg, int a, int b, int c) => reg[c] = a),
			//greater-than testing
			new Opcode ("gtir", (int[] reg, int a, int b, int c) => reg[c] = a > reg[b] ? 1 : 0),
			new Opcode ("gtri", (int[] reg, int a, int b, int c) => reg[c] = reg[a] > b ? 1 : 0),
			new Opcode ("gtrr", (int[] reg, int a, int b, int c) => reg[c] = reg[a] > reg[b] ? 1 : 0),
			//equality testing
			new Opcode ("eqir", (int[] reg, int a, int b, int c) => reg[c] = a == reg[b] ? 1 : 0),
			new Opcode ("eqri", (int[] reg, int a, int b, int c) => reg[c] = reg[a] == b ? 1 : 0),
			new Opcode ("eqrr", (int[] reg, int a, int b, int c) => reg[c] = reg[a] == reg[b] ? 1 : 0),
		};
	}
	struct Opcode
	{
		public int opcode;
		public string op;
		public Action<int[], int, int, int> execute;
		public Opcode(string _op, Action<int[], int, int, int> _ex)
		{
			opcode = -1;//dont know yet
			op = _op;
			execute = _ex;
		}
	}
}
