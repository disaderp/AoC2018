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
		static void Main(string[] args)
		{
			RailDirection[,] rails = new RailDirection[150, 150];
			List<Cart> carts = new List<Cart>();
			StreamReader sr = new StreamReader("input13.txt");
			int j = 0;
			while(sr.Peek() != -1)
			{
				string line = sr.ReadLine();
				for(int i =0; i< line.Length; i++)
				{
					if (line[i] == ' ') rails[i, j] = RailDirection.None;
					else if (line[i] == '|') rails[i, j] = RailDirection.Vertical;
					else if (line[i] == '-') rails[i, j] = RailDirection.Horizontal;
					else if (line[i] == '/') rails[i, j] = RailDirection.Right;
					else if (line[i] == '\\') rails[i, j] = RailDirection.Left;
					else if (line[i] == '+') rails[i, j] = RailDirection.Intersection;

					else if (line[i] == '>') { rails[i, j] = RailDirection.Horizontal; carts.Add(new Cart(i, j, CartDirection.Right)); }
					else if (line[i] == '<') { rails[i, j] = RailDirection.Horizontal; carts.Add(new Cart(i, j, CartDirection.Left)); }
					else if (line[i] == '^') { rails[i, j] = RailDirection.Vertical; carts.Add(new Cart(i, j, CartDirection.Up)); }
					else if (line[i] == 'v') { rails[i, j] = RailDirection.Vertical; carts.Add(new Cart(i, j, CartDirection.Down)); }
				}
				j++;
			}

			//PART1
			bool nCollision = true;
			bool first = false;
			List<Cart> toRemove = new List<Cart>();
			while (nCollision)
			{
				carts.Sort(compareCarts);//by rows then from left to right
				for(int i = 0; i < carts.Count; i++)
				{
					if (toRemove.Contains(carts[i])) continue;//cart removed
					Cart c = carts[i];
					if(c.dir == CartDirection.Left) c.x--;
					else if (c.dir == CartDirection.Right) c.x++;
					else if (c.dir == CartDirection.Up) c.y--;
					else if (c.dir == CartDirection.Down) c.y++;

					if(rails[c.x, c.y] == RailDirection.Left) // -----\-----
					{
						if (c.dir == CartDirection.Left) c.dir = CartDirection.Up;
						else if (c.dir == CartDirection.Right) c.dir = CartDirection.Down;
						else if (c.dir == CartDirection.Up) c.dir = CartDirection.Left;
						else if (c.dir == CartDirection.Down) c.dir = CartDirection.Right;
					}
					else if (rails[c.x, c.y] == RailDirection.Right) // ---/----
					{
						if (c.dir == CartDirection.Left) c.dir = CartDirection.Down;
						else if (c.dir == CartDirection.Right) c.dir = CartDirection.Up;
						else if (c.dir == CartDirection.Up) c.dir = CartDirection.Right;
						else if (c.dir == CartDirection.Down) c.dir = CartDirection.Left;
					}
					else if (rails[c.x, c.y] == RailDirection.Intersection) // ---+----
					{
						if (c.dir == CartDirection.Left)
						{
							if (c.lastTurn == CartDirection.Right)//turn left
							{
								c.dir = CartDirection.Down;
								c.lastTurn = CartDirection.Left;
							}
							else if (c.lastTurn == CartDirection.Left)//go straight
							{
								c.lastTurn = CartDirection.Up;
							}
							else if (c.lastTurn == CartDirection.Up)//turn right
							{
								c.dir = CartDirection.Up;
								c.lastTurn = CartDirection.Right;
							}
						}
						else if (c.dir == CartDirection.Right)
						{
							if (c.lastTurn == CartDirection.Right)//turn left
							{
								c.dir = CartDirection.Up;
								c.lastTurn = CartDirection.Left;
							}
							else if (c.lastTurn == CartDirection.Left)//go straight
							{
								c.lastTurn = CartDirection.Up;
							}
							else if (c.lastTurn == CartDirection.Up)//turn right
							{
								c.dir = CartDirection.Down;
								c.lastTurn = CartDirection.Right;
							}
						}
						else if (c.dir == CartDirection.Up)
						{
							if (c.lastTurn == CartDirection.Right)//turn left
							{
								c.dir = CartDirection.Left;
								c.lastTurn = CartDirection.Left;
							}
							else if (c.lastTurn == CartDirection.Left)//go straight
							{
								c.lastTurn = CartDirection.Up;
							}
							else if (c.lastTurn == CartDirection.Up)//turn right
							{
								c.dir = CartDirection.Right;
								c.lastTurn = CartDirection.Right;
							}
						}
						else if (c.dir == CartDirection.Down)
						{
							if (c.lastTurn == CartDirection.Right)//turn left
							{
								c.dir = CartDirection.Right;
								c.lastTurn = CartDirection.Left;
							}
							else if (c.lastTurn == CartDirection.Left)//go straight
							{
								c.lastTurn = CartDirection.Up;
							}
							else if (c.lastTurn == CartDirection.Up)//turn right
							{
								c.dir = CartDirection.Left;
								c.lastTurn = CartDirection.Right;
							}
						}
					}

					carts[i] = c;//update position

					for (int k = 0; k < carts.Count; k++)				
					{
						if(carts[k].x == c.x && carts[k].y == c.y && i != k && !toRemove.Contains(carts[k]))
						{
							if (!first)
							{
								//PART1
								first = true;
								Console.WriteLine(c.x + "," + c.y);
								//PART1
							}
							toRemove.Add(carts[k]);
							toRemove.Add(carts[i]);
						}
					}
				}

				//PART2
				if (toRemove.Count > 0)
				{
					foreach(Cart cc in toRemove)
					{
						carts.Remove(cc);
					}
					toRemove.Clear();
				}
				if (carts.Count == 1)
				{
					Cart x = carts[0];
					Console.WriteLine(x.x + "," + x.y);
					nCollision = false;
					break;
				}
				//PART2
			}
		}

		public struct Cart
		{
			public Cart(int _x, int _y, CartDirection _dir)
			{
				x = _x;
				y = _y;
				dir = _dir;
				lastTurn = CartDirection.Right;
			}
			public int x;
			public int y;
			public CartDirection dir;
			public CartDirection lastTurn;
		}
		static int compareCarts(Cart x, Cart y)
		{
			if (x.x > y.x) return 1;
			if (x.x < y.x) return -1;
			if (x.y > y.y) return 1;
			if (x.y < y.y) return -1;
			return 0;
		}
		public enum RailDirection
		{ 
			None,
			Vertical,// |
			Horizontal,// -
			Right,// /
			Left,// \
			Intersection// +
		};
		public enum CartDirection
		{
			Left,
			Right,
			Up,
			Down
		};
	}
}
		

	

