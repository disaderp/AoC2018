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
			List<Claim> claims = new List<Claim>();
			
			int maxX = 0;
			int maxY = 0;
			while (Console.In.Peek() != -1)
			{
				Claim tmp = new Claim();
				string[] claim = Console.ReadLine().Replace(":", "").Split(' ');
				tmp.id = Int32.Parse(claim[0].Replace("#", ""));
				tmp.startX = Int32.Parse(claim[2].Split(',')[0]);
				tmp.startY = Int32.Parse(claim[2].Split(',')[1]);
				tmp.lengthX = Int32.Parse(claim[3].Split('x')[0]);
				tmp.lengthY = Int32.Parse(claim[3].Split('x')[1]);
				claims.Add(tmp);

				int claimX = tmp.startX + tmp.lengthX;
				int claimY = tmp.startY + tmp.lengthY;

				if (claimX > maxX) { maxX = claimX; }
				if (claimY > maxY) { maxY = claimY; }
			}

			int[,] fabric = new int[maxX+1, maxY+1];

			foreach(Claim claim in claims)
			{
				for(int i = claim.startX; i < claim.startX + claim.lengthX; i++)
				{
					for(int j = claim.startY; j < claim.startY + claim.lengthY; j++)
					{
						if (fabric[i,j] == 0)
						{
							fabric[i, j] = claim.id;
						}
						else
						{
							fabric[i, j] = -1;//collision
						}
					}
				}
			}

			//PART2
			var watch = System.Diagnostics.Stopwatch.StartNew();
			int noCollisionID = 0;
			foreach (Claim claim in claims)
			{
				//bool collision = false;
				for (int i = claim.startX; i < claim.startX + claim.lengthX; i++)
				{
					for (int j = claim.startY; j < claim.startY + claim.lengthY; j++)
					{
						if (fabric[i, j] != claim.id)
						{
							//collision = true; //slower - 12ms
							goto CONTINUEFOREACH; //faster - 2ms
						}

					}
				}
				//if (! collision)
				noCollisionID = claim.id;
			CONTINUEFOREACH:;
			}
			watch.Stop();
			//PART2

			int collisions = 0;
			foreach(int el in fabric)
			{
				if (el == -1) collisions++;
			}

			Console.WriteLine(collisions);
			Console.WriteLine(noCollisionID);
			Console.WriteLine("Execution time: " + watch.ElapsedMilliseconds + "ms");

		}
	}
	struct Claim
	{
		public int id;

		public int startX;
		public int startY;

		public int lengthX;
		public int lengthY;
	}

}
