using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication10
{
    public class Node
    {
        public Node(int number)
        {
            Number = number;
        }

        public Node()
        {
            Number = 0;
        }

        public int Number { get; set; }

        public List<Node> Nodes = new List<Node>();

        public override string ToString()
        {
            string result = string.Format("{0} : ", Number);
            foreach (var item in Nodes)
            {
                result += item.Number + " ";
            }

            return result;
        }
    }

    public class Graph
    {
        private int _countOfNodes;

        public int[,] Matrix;

        public List<Node> Nodes = new List<Node>();

        public int CountOfNodes
        {
            get { return _countOfNodes; }
        }

        public static void UIAdjacency(Graph g)
        {
            while (true)
            {
                Console.WriteLine(g.ToString());
                Console.WriteLine("Enter number of node");
                int number = int.Parse(Console.ReadLine());

                try
                {
                    g.AddNode(number);
                    Console.WriteLine(g.ToString());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Enter numbers of node to build relation");
                int number2 = int.Parse(Console.ReadLine());
                int number3 = int.Parse(Console.ReadLine());

                try
                {
                    g.AddRelation(g.Nodes[number2 - 1], g.Nodes[number3 - 1]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine(g.ToString());

                Console.WriteLine("Enter number of node to remove");
                int number4 = int.Parse(Console.ReadLine());

                try
                {
                    g.RemoveNode(number4);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public override string ToString()
        {
            string nodes = string.Empty;
            foreach (var item in Nodes)
            {
                nodes += item + Environment.NewLine;
            }

            return nodes;
        }

        // Суміжність
        public void GenerateNodesFromAdjacencyFile()
        {
            ReadFile("Matrix.txt");

            _countOfNodes = Matrix.GetLength(1);

            for (int i = 0; i < _countOfNodes; i++)
            {
                Nodes.Add(new Node(i + 1));
            }

            for (int i = 0; i < _countOfNodes; i++)
            {
                Nodes[i].Nodes.AddRange(getNodesFromRow(i + 1));
            }

        }

        // Інцидентність
        public void GenerateNodesFromIncidenceFile()
        {
            ReadFile("IncidenceMatrix.txt");
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                Nodes.Add(new Node(i + 1));
            }

            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (Matrix[i, j] == -1)
                    {
                        for (int k = 0; k < Matrix.GetLength(1); k++)
                        {
                            if (Matrix[i, k] == 1)
                            {
                                try
                                {
                                    AddRelation(Nodes[j], Nodes[k]);
                                }
                                catch
                                { }
                            }
                        }
                    }
                }
            }
        }

        public void AddNode(int node)
        {
            foreach (var item in Nodes)
            {
                if (item.Number == node)
                {
                    throw new Exception("This node is already exist");
                }
            }
            Nodes.Add(new Node(node));
        }

        public void RemoveNode(int node)
        {
            Node toRemove = null;
            foreach (var item in Nodes)
            {
                if (item.Number == node)
                {
                    toRemove = item;
                    break;
                }
            }
            if (toRemove == null)
                throw new NullReferenceException("Not found");

            foreach (var item in Nodes)
            {
                for (int i = 0; i < item.Nodes.Count; i++)
                {
                    if (item.Nodes[i] == toRemove)
                    {
                        item.Nodes.Remove(toRemove);
                    }
                }
            }

            Nodes.Remove(toRemove);
        }

        public void AddRelation(Node a, Node b)
        {
            if (a == b)
                throw new Exception("Can't build this relation");
            foreach (var item in a.Nodes)
            {
                if (item == b)
                {
                    throw new Exception("This relation is already exist");
                }
            }
            a.Nodes.Add(b);

        }

        private List<Node> getNodesFromRow(int number)
        {
            List<Node> nodes = new List<Node>();

            for (int i = 0; i < _countOfNodes; i++)
            {
                if (Matrix[i, number - 1] == 1)
                {
                    foreach (var item in Nodes)
                    {
                        if (item.Number == i + 1)
                        {
                            nodes.Add(item);
                            break;
                        }
                    }

                }
            }

            return nodes;
        }

        private void ReadFile(string path)
        {
            string[] s = File.ReadAllLines(path);
            int width = 1;
            foreach (var item in s[0])
            {
                if (item == '\t')
                    width++;
            }

            int height = s.Length;
            Matrix = new int[width, height];

            // пройтись по рядкам
            for (int i = 0; i < s.Length; i++)
            {
                int j = 0;
                // пройтись по елементам рядка
                for (int k = 0; k < s[i].Length; k++)
                {
                    if (s[i][k] != '\t')
                    {
                        if (s[i][k] == '-')
                        {
                            k++;
                            Matrix[j, i] = -1 * int.Parse(s[i][k].ToString());
                            j++;
                            continue;
                        }
                        Matrix[j, i] = int.Parse(s[i][k].ToString());
                        j++;
                    }
                }
            }
        }

    }
}
