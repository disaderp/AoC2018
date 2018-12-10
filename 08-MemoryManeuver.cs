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
		static int sum = 0;
		static StreamReader sr = new StreamReader("input8.txt");
		static void Main(string[] args)
		{
			Node header = processNode();
			//PART1
			Console.WriteLine(sum);
			//PART1

			//PART2
			Console.WriteLine(nodeValue(header));
			//PART2
		}
		static Node processNode()
		{
			Node t = new Node();
			t.children = new List<Node>();
			t.metadata = new List<int>();
			t.nodes = readNextNum();
			t.meta = readNextNum();

			for(int i = 0; i < t.nodes; i++)
			{
				t.children.Add(processNode());
			}
			for(int i = 0; i< t.meta; i++)
			{
				int metaVal = readNextNum();
				sum += metaVal;
				t.metadata.Add(metaVal);
			}

			return t;
		}
		static int nodeValue(Node n)
		{
			if (n.nodes > 0)
			{
				int sum = 0;
				if (n.meta > 0)
				{	
					foreach(int index in n.metadata)
					{
						if(!(index == 0 || index - 1 >= n.nodes))
						{
							sum += nodeValue(n.children[index - 1]);
						}
					}
					
				}
				return sum;
			}
			else
			{
				int sum = 0;
				if (n.meta > 0)
				{
					foreach (int val in n.metadata)
					{
						sum += val;
					}
				}
				return sum;
			}
		}
		static int readNextNum()
		{
			string tmp = "";
			while (sr.Peek() != ' ' && sr.Peek() != -1)//space or eof
			{
				tmp += ((char)sr.Read()).ToString();
			}
			sr.Read();//consume space
			return Int32.Parse(tmp);
		}
		struct Node
		{
			public int nodes;
			public int meta;
			public List<Node> children;
			public List<int> metadata;
		}
	}
}
