using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication10
{


    class Program
    { 

        static void Main(string[] args)
        {
            Graph g = new Graph();
            g.GenerateNodesFromIncidenceFile();
            Graph.UIAdjacency(g);

            Console.ReadKey();
        }
    }
}
