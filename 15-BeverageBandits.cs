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
		public static bool[,] map = new bool[32, 32];
		public static List<Unit> units = new List<Unit>();
		static void Main(string[] args)
		{
			StreamReader sr = new StreamReader("input15.txt");
			List<Unit> unitsCopy = new List<Unit>();
			for(int i = 0; i< 32; i++)
			{
				for(int j = 0; j < 32; j++)
				{
					char el = (char)sr.Read();
					if(el == '#')
					{
						map[i, j] = true;
					}else if(el != '.')
					{
						Unit t = new Unit(el, i, j, 3);
						Unit t2 = new Unit(el, i, j, 3);
						units.Add(t);
						unitsCopy.Add(t2);
					}
				}
				sr.Read();//newline
			}

			//PART1
			print();
			for(int j = 0; true ; j++)
			{
				units.Sort(Unit.compareUnits);
				units.Reverse();//descending for easier iteration through list with removed elements

				for(int i = units.Count-1; i >= 0; i--)
				{
					Unit a = units[i];

					a.Move();

					a.Attack();

					if(units.Exists(x => x.hp <= 0))
					{
						units.Remove(units.Find(x => x.hp <= 0));
						i--;
					}

				}

				print();

				bool lastTeam = units[0].team;
				bool teamWon = true;
				int hpLeft = 0;
				foreach (Unit x in units)
				{
					if (x.team != lastTeam)
					{
						teamWon = false;
						break;
					}
					hpLeft += x.hp;
				}
				if (teamWon)
				{
					Console.WriteLine(hpLeft * j);
					break;
				}	
			}
			//PART1

			//PART2
			int dmg = 4;
			bool ending = true;
			while (ending)
			{
				units.Clear();
				foreach(Unit x in unitsCopy)
				{
					Unit tmp = new Unit(x.team ? 'G' : 'E', x.x, x.y, dmg);
					units.Add(tmp);
				}
				dmg++;
				bool nelfDeath = true;
				for (int j = 0; nelfDeath; j++)
				{
					units.Sort(Unit.compareUnits);
					units.Reverse();//descending for easier iteration through list with removed elements

					for (int i = units.Count - 1; i >= 0; i--)
					{
						if (units[i] == null) continue;
						Unit a = units[i];

						a.Move();

						a.Attack();

						if (units.Exists(x => x.hp <= 0))
						{
							if (!units.Find(x => x.hp <= 0).team) nelfDeath = false;//elf died, restart game
							units.Remove(units.Find(x => x.hp <= 0));
							i--;
						}

					}

					print();

					bool lastTeam = units[0].team;
					bool teamWon = true;
					int hpLeft = 0;
					foreach (Unit x in units)
					{
						if (x.team != lastTeam)
						{
							teamWon = false;
							break;
						}
						hpLeft += x.hp;
					}
					if (teamWon)
					{
						Console.WriteLine(hpLeft * j);
						ending = false;
						break;
					}
				}
			}
			//PART2
		}

		static void print()
		{
			Console.Clear();
			for(int i = 0; i < 32; i++)
			{
				for(int j = 0; j < 32; j++)
				{
					if(units.Exists(x => x.x == i && x.y == j))
					{
						Console.Write(units.Find(x => x.x == i && x.y == j).team ? "G" : "E");
					}
					else if(map[i,j]) Console.Write("#");
					else Console.Write(".");
				}
				Console.WriteLine();
			}
			//Console.ReadKey(true);
		}
	}

	class Unit 
	{
		public bool team;
		public int x;
		public int y;
		public int hp;
		public int dmg;
		public Unit(char _team, int _x, int _y, int _dmg)
		{
			team = _team == 'G' ? true : false;//GOBLIN = 1, ELF = 0
			x = _x;
			y = _y;
			hp = 200;
			dmg = 3;
			if (!team) dmg = _dmg;
		}
		public void Move()
		{
			//checek if enemy in range already 
			if (Program.units.Exists(x => x.x == this.x - 1 && x.y == this.y && x.team != this.team) ||//TOP
				(Program.units.Exists(x => x.x == this.x && x.y == this.y - 1 && x.team != this.team)) || //LEFT
				(Program.units.Exists(x => x.x == this.x && x.y == this.y + 1 && x.team != this.team)) ||//RIGHT
				(Program.units.Exists(x => x.x == this.x + 1 && x.y == this.y && x.team != this.team)) )//BOTTOM
			{
				return;//no move required
			}

			List<Point> pointsInRange = this.inRange();//find in range

			int min = Int32.MaxValue;//find closest
			List<Point []> closestPoints = new List<Point []>();
			foreach (Point x in pointsInRange)
			{
				DistPoint dis = distance(new Point(this.x, this.y), x);
				if (min > dis.dist && dis.dist != -1)
				{
					Point[] t = new Point[2];
					closestPoints.Clear();
					min = dis.dist;
					t[0] = new Point(dis.x, dis.y);//step
					t[1] = new Point(x.x, x.y);//original
					closestPoints.Add(t);
				}
				else if (min == dis.dist)
				{
					Point[] t = new Point[2];
					t[0] = new Point(dis.x, dis.y);//step
					t[1] = new Point(x.x, x.y);//original
					closestPoints.Add(t);
				}
			}

			if (closestPoints.Count > 0)//check if move avaliable
			{
				closestPoints.Sort(comparePoints); //sort by original
				
				this.x = closestPoints[0][0].x;
				this.y = closestPoints[0][0].y;//MOVE
			}
		}
		public void Attack()
		{
			int lowestHealth = Int32.MaxValue;
			Point loc = new Point();
			if(Program.units.Exists(x => x.x == this.x -1 && x.y == this.y && x.team != this.team))//TOP
			{
				int hp = Program.units.Find(x => x.x == this.x - 1 && x.y == this.y).hp;
				lowestHealth = hp;
				loc.x = this.x - 1;
				loc.y = this.y;
			}
			if (Program.units.Exists(x => x.x == this.x  && x.y == this.y - 1 && x.team != this.team))//LEFT
			{
				int hp = Program.units.Find(x => x.x == this.x  && x.y == this.y - 1).hp;
				if(hp < lowestHealth)
				{
					lowestHealth = hp;
					loc.x = this.x;
					loc.y = this.y -1 ;
				}
			}
			if (Program.units.Exists(x => x.x == this.x  && x.y == this.y + 1 && x.team != this.team))//RIGHT
			{
				int hp = Program.units.Find(x => x.x == this.x  && x.y == this.y + 1).hp;
				if (hp < lowestHealth)
				{
					lowestHealth = hp;
					loc.x = this.x;
					loc.y = this.y + 1;
				}
			}
			if (Program.units.Exists(x => x.x == this.x + 1 && x.y == this.y && x.team != this.team))//BOTTOM
			{
				int hp = Program.units.Find(x => x.x == this.x + 1 && x.y == this.y).hp;
				if (hp < lowestHealth)
				{
					lowestHealth = hp;
					loc.x = this.x + 1;
					loc.y = this.y;
				}
			}
			if (lowestHealth < Int32.MaxValue)
			{
				Program.units.Find(x => x.x == loc.x && x.y == loc.y).hp -= this.dmg;
			}
			
		}
		public static int compareUnits(Unit x, Unit y)
		{
			if (x.x > y.x) return 1;
			if (x.x < y.x) return -1;
			if (x.y > y.y) return 1;
			if (x.y < y.y) return -1;
			return 0;
		}
		public static int comparePoints(Point[] x, Point[] y)
		{
			if (x[1].x > y[1].x) return 1;
			if (x[1].x < y[1].x) return -1;
			if (x[1].y > y[1].y) return 1;
			if (x[1].y < y[1].y) return -1;
			return 0;
		}
		static DistPoint distance(Point a, Point b)
		{
			int[,] dist = initNegative();
			dist[b.x, b.y] = 0;

			bool change = true;
			int x = 0;
			while (change)//modified dijkstra
			{
				change = false;
				for (int i = 0; i < 32; i++)
				{
					for (int j = 0; j < 32; j++)
					{
						if (dist[i, j] == x)
						{
							if(pathFree(new Point(i -1,j)) && (dist[i -1,j] == -1))
							{
								dist[i - 1, j] = x + 1;
								change = true;
							}
							if (pathFree(new Point(i , j -1)) && (dist[i , j-1] == -1))
							{
								dist[i , j-1] = x + 1;
								change = true;
							}
							if (pathFree(new Point(i, j+1)) && (dist[i, j+1] == -1))
							{
								dist[i, j+1] = x + 1;
								change = true;
							}
							if (pathFree(new Point(i + 1, j)) && (dist[i + 1, j] == -1))
							{
								dist[i + 1, j] = x + 1;
								change = true;
							}
						}
					}
				}
				x++;

				if (dist[a.x - 1, a.y] != -1)
					return new DistPoint(a.x -1, a.y, dist[a.x - 1, a.y]);
				if (dist[a.x, a.y-1] != -1)
					return new DistPoint(a.x, a.y -1 , dist[a.x, a.y-1]);
				if (dist[a.x, a.y+1] != -1)
					return new DistPoint(a.x, a.y + 1, dist[a.x, a.y+1]);
				if (dist[a.x + 1, a.y] != -1)
					return new DistPoint(a.x +1 , a.y, dist[a.x + 1, a.y]);
			}
			return new DistPoint(-1, -1,-1);//not reachable
		}
		static int[,] initNegative()
		{
			int[,] tmp = new int[32, 32];
			for(int i = 0; i < 32; i++)
			{
				for(int j = 0; j < 32; j++)
				{
					tmp[i, j] = -1;
				}
			}
			return tmp;
		}
		static bool pathFree(Point a)
		{
			if (!Program.map[a.x, a.y] && !Program.units.Exists(x => x.x == a.x && x.y == a.y)) return true;
			return false;
		}
		List<Point> inRange()
		{
			List<Point> tmp = new List<Point>();
			foreach(Unit x in Program.units)
			{
				if (x.team == this.team) continue;//find only enemies in range
				if(pathFree(new Point(x.x - 1, x.y)))//TOP SPOT FREE
				{
					Point t = new Point();
					t.x = x.x - 1;
					t.y = x.y;
					tmp.Add(t);
				}
				if (pathFree(new Point(x.x, x.y - 1)))//LEFT SPOT FREE
				{
					Point t = new Point();
					t.x = x.x;
					t.y = x.y - 1;
					tmp.Add(t);
				}
				if (pathFree(new Point(x.x, x.y + 1)))//RIGHT SPOT FREE
				{
					Point t = new Point();
					t.x = x.x;
					t.y = x.y + 1;
					tmp.Add(t);
				}
				if (pathFree(new Point(x.x + 1, x.y)))//BOTTOM SPOT FREE
				{
					Point t = new Point();
					t.x = x.x + 1;
					t.y = x.y;
					tmp.Add(t);
				}
			}
			return tmp;
		}
		public struct Point
		{
			public Point(int _x, int _y)
			{
				x = _x;
				y = _y;
			}
			public int x;
			public int y;
		}
		public struct DistPoint
		{
			public int x;
			public int y;
			public int dist;
			public DistPoint(int _x, int _y, int _dist)
			{
				x = _x;
				y = _y;
				dist = _dist;
			}
		}
	}

}
