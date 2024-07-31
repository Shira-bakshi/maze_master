using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.classes
{
    public class Edge
    {
        public int weight { get; set; }
        public int indexV { get; set; }

        public Edge(int indexV, int weight)
        {
            this.indexV = indexV;
            this.weight = weight;
        }
        public override string ToString()
        {
            return " index: " + "(" + indexV + ") " + "width: " + weight;
        }
        //public void addAdge(int w, Vertex u)
        //{

        //}
    }
}
