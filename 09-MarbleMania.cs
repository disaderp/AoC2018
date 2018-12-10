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
		static StreamReader sr = new StreamReader("input9.txt");
		static void Main(string[] args)
		{
			string data = sr.ReadLine();
			int players = Int32.Parse(data.Split(' ')[0]);
			int last = Int32.Parse(data.Split(' ')[6]);

			//PART1
			List<int> marbles = new List<int>(); marbles.Add(0);
			int current = 0;
			int[] playerScore = new int[players];
			int currentPlayer = 0;
			int nextMarble = 1;
			while (true)
			{
				if (nextMarble % 23 == 0) //lucky marble
				{
					int score = marbles[mod(current - 7, marbles.Count)] + nextMarble;
					playerScore[currentPlayer] += score;

					current = mod(current - 7, marbles.Count);
					marbles.RemoveAt(current);
					current = mod(current, marbles.Count);
		
					nextMarble++;
				}
				else
				{
					current = mod(current + 2, marbles.Count);
					marbles.Insert(current, nextMarble);//O(n)
					nextMarble++;
				}
				currentPlayer = mod(currentPlayer + 1 , players);
				if (nextMarble == last) break;//last marble score
			}
			Console.WriteLine(playerScore.Max());
			//PART1

			//PART2
			Circle circle = new Circle(); circle.Init();//much faster - with custom list
			long [] playerScore2 = new long [players];
			int currentPlayer2 = 0;
			int nextMarble2 = 1;
			while (true)
			{	
				if (nextMarble2 % 23 == 0) //lucky marble
				{
					circle = circle.Prev.Prev.Prev.Prev.Prev.Prev;
					int score = circle.Prev.Id + nextMarble2;
					playerScore2[currentPlayer2] += score;

					circle.RemovePrev();

					nextMarble2++;
				}
				else
				{
					circle = circle.Next;
					circle.Insert(nextMarble2);//O(1)
					circle = circle.Next;
					nextMarble2++;
				}
				currentPlayer2 = mod(currentPlayer2 + 1, players);
				if (nextMarble2 == (last * 100)) break;//last marble score
			}
			Console.WriteLine(playerScore2.Max());
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
			this.Id = 0;
			this.Prev = this;
			this.Next = this;
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
		public void RemovePrev()
		{
			Circle toRem = this.Prev;
			toRem.Prev.Next = this;
			this.Prev = toRem.Prev;
			toRem = null;
		}
	}
}
