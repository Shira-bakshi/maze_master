using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.classes
{
    public class Vertex
    {
        public int x_value { get; set; }
        public int y_value { get; set; }
        public bool visited { get; set; }
        public int color { get; set; }//1-בתהליך, 2-סיים, 0-לא נכנס 
        public int father { get; set; }
        public int fatherEnd { get; set; }

        public List<Edge> Neighbours { get; set; }//מערך שכנים 

        public int costTotal { get; set; }
        public int costEnd { get; set; }
        public int costStart { get; set; }

        public int index { get; set; }

        public Vertex() 
        {
            this.x_value = -1;
            this.y_value = -1;
            this.index = -1;

        }
        public Vertex(int x_value, int y_value, int index)
        {
            this.x_value = x_value;
            this.y_value = y_value;
            this.index = index;
            this.Neighbours = new List<Edge>();
            this.father = -1;
            this.fatherEnd = -1;
            this.costTotal = 10000000;
            this.costEnd = 0;
            this.costStart = 0;
            this.color = 0;
        }
        public string GetValue()
        {
            return "(" + this.x_value + "," + this.y_value + ")";
        }
        //לא בטוח נחוץ
        public void SetValue(int x, int y_value)
        {
            this.x_value = x;
            this.y_value = y_value;
        }

        public void addNeighbours(int Indexneighbour, int weight)
        {
            Edge e = new Edge(Indexneighbour, weight);
            this.Neighbours.Add(e);
            //this.Neighbours[countN++]=edge;
        }
        public List<Edge> getNeighbours()
        {
            return this.Neighbours;
        }
        //public  void costCalculation(graph g, int index, int strat, int end)
        //{
        //    List<Edge> edges = g.LValue[index].Neighbours;
        //    foreach (Edge e in edges)
        //    {
        //        DistanacPoint(g.LValue[index], g.LValue[strat], g.LValue[end]);
        //    }

        //}
        public void DistanacPoint(Vertex end, Vertex s)
        {
            int x2 = Math.Abs(this.x_value - end.x_value);
            int y2 = Math.Abs(this.y_value - end.y_value);
            int x1 = Math.Abs(this.x_value - s.x_value);
            int y1 = Math.Abs(this.y_value - s.y_value);
            this.costStart = x1 + x2;
            this.costEnd = (x2 + y2);
        }

        public override string ToString()
        {
            string s = "";
            if (this.Neighbours != null)
            {
                foreach (Edge v in this.Neighbours)
                {
                    s += v.ToString() + ", ";
                }
            }
            return index + "(" + x_value + "," + y_value + ")" + " cost: " + costTotal + s;
        }



    }
}
