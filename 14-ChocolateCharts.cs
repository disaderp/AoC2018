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
			StreamReader sr = new StreamReader("input14.txt");
			string countStr = sr.ReadLine();
			int count = Int32.Parse(countStr);

			//PART1
			Circle elf1 = new Circle(); elf1.Init();
			Circle elf2 = elf1.Next;
			Circle end = elf2;//keep track of last element

			for(int i = 0; i < count + 8; i++)
			{
				int score = elf1.Id + elf2.Id;
				int elf1Move = 1 + elf1.Id;
				int elf2Move = 1 + elf2.Id;

				int digit1 = score / 10;
				int digit2 = score % 10;

				if (digit1 != 0)
				{
					end.Insert(digit1);
					end = end.Next;
					i++;
				}
				if (i >= count + 8) break;
				end.Insert(digit2);
				end = end.Next;

				for (int j = 0;j < elf1Move; j++)
				{
					elf1 = elf1.Next;
				}
				for (int j = 0; j < elf2Move; j++)
				{
					elf2 = elf2.Next;
				}
			}
			string last = "";
			for(int i = 0; i < 10; i++)
			{
				last = end.Id + last;
				end = end.Prev;
			}
			Console.WriteLine(last);
			//PART1

			//PART2
			elf1 = new Circle(); elf1.Init();
			Circle firstNode = elf1;//keep first node for part 2
			elf2 = elf1.Next;
			end = elf2;//keep track of last element

			for (int i = 0; true; i++)
			{
				int score = elf1.Id + elf2.Id;
				int elf1Move = 1 + elf1.Id;
				int elf2Move = 1 + elf2.Id;

				int digit1 = score / 10;
				int digit2 = score % 10;

				if (digit1 != 0)
				{
					end.Insert(digit1);
					end = end.Next;
					i++;
				}

				end.Insert(digit2);
				end = end.Next;

				for (int j = 0; j < elf1Move; j++)
				{
					elf1 = elf1.Next;
				}
				for (int j = 0; j < elf2Move; j++)
				{
					elf2 = elf2.Next;
				}

				string last7 = "";
				for (int j = 0; j < 7; j++)
				{
					last7 = end.Id + last7;
					end = end.Prev;
				}
				end = end.Next.Next.Next.Next.Next.Next.Next;
				if(last7.Contains(countStr))
				{
					Console.WriteLine(i - countStr.Length + last7.IndexOf(countStr)+2);
					break;
				}
			}
			//PART2
		}
		static int mod(int x, int m)
		{
			int r = x % m;
			return r < 0 ? r + m : r;
		}
	}
	class Circle
	{
		private Circle _prev;
		private Circle _next;
		private int _num;
		public void Init ()
		{
			Circle next = new Circle();
			next.Id = 7;
			next.Prev = this;
			next.Next = this;

			this.Id = 3;
			this.Prev = next;
			this.Next = next;
		}

		public int Id
		{
			get { return _num; }
			set { _num = value; }
		}
		public Circle Prev
		{
			get { return _prev; }
			set { _prev = value; }
		}
		public Circle Next
		{
			get { return _next; }
			set { _next = value; }
		}
		public void Insert(int num)
		{
			Circle t = new Circle();
			t.Id = num;
			t.Prev = this;
			t.Next = this.Next;

			this.Next = t;
			this.Next.Next.Prev = t;
		}
		//public void RemovePrev()
		//{
		//	Circle toRem = this.Prev;
		//	toRem.Prev.Next = this;
		//	this.Prev = toRem.Prev;
		//	toRem = null;
		//}
	}
}
