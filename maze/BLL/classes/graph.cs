namespace BLL.classes
{
    public class graph
    {
        public List<Vertex> LValue { get; set; }
        public int countdV = 0;

        public graph()
        {
            this.LValue = new List<Vertex>();
        }
        public void add(Vertex v)
        {
            this.LValue.Add(v);
            this.countdV++;
        }

        public int findVertex(int x, int y)
        {
            for (int i = 0; i < this.LValue.Count; i++)
                if (LValue[i].x_value == x && LValue[i].y_value == y)
                    return i;
            return -1;
        }
        public int findVertex(Vertex x)
        {
            for (int i = 0; i < this.LValue.Count; i++)
                if (LValue[i] == x)
                    return i;
            return -1;
        }


        public List<int> FindShortestPathBFS(int start, int end)
        {
            Queue<int> queue = new Queue<int>();

            List<int> shortPath = new List<int>();
            //זהו ממש מבנה של 
            //HASH
            Dictionary<int, int> parent = new Dictionary<int, int>();

            queue.Enqueue(start);
            parent[start] = -1;

            while (queue.Count > 0)
            {
                int currentV = queue.Dequeue();
                if (currentV == end)
                {
                    // Reconstruct shortest path
                    int vertex = end;
                    while (vertex != -1)
                    {
                        shortPath.Insert(0, vertex);
                        vertex = parent[vertex];//מחזיר את האבא שלו
                    }
                    return shortPath;
                }

                foreach (Edge e in this.LValue[currentV].Neighbours)
                {
                    int neighborV = e.indexV;
                    if (!parent.ContainsKey(neighborV))//בדיקה האם יש אבא אם לא 
                    {
                        parent[neighborV] = currentV;//תכניס לי במפתח של  הבן את האינדקס של האב
                        queue.Enqueue(neighborV);
                    }
                }
            }
            //מחזיר רשימה רקיה אם לא נמצא הסוף.
            return shortPath;
        }
        public List<Vertex> sortX()
        {
            List<Vertex> list = new List<Vertex>(this.LValue);
            list.Sort((a, b) => a.x_value.CompareTo(b.x_value));
            return list;

        }
        public List<Vertex> sortY()
        {
            List<Vertex> list = new List<Vertex>(this.LValue);
            list.Sort((a, b) => a.y_value.CompareTo(b.y_value));
            return list;
        }
        public List<int> startAStar(int start, int end)
        {
            List<int> path = new List<int>();
            PriorityQueue<Vertex> openSet = new PriorityQueue<Vertex>(countdV);
            PriorityQueue<Vertex> closeSet = new PriorityQueue<Vertex>(countdV);
            this.LValue[start].costStart = 0;
            this.LValue[start].costTotal = this.LValue[start].costEnd;
            openSet.Insert(this.LValue[start]);
            int cost = 0;
            while (!openSet.IsEmpty)
            {
                Vertex currentV = openSet.ExtractMin();
                if (currentV.index == end || currentV.visited)
                {
                    currentV = this.LValue[currentV.father];

                    while (currentV.index != start)
                    {
                        path.Add(currentV.index);
                        currentV = this.LValue[currentV.father];
                    }
                    path.Add(start);
                    path.Reverse();
                    return path;
                }
                //not goal:
                currentV.visited = true;

                closeSet.Insert(currentV);
                if (currentV.father != -1)
                    cost = this.LValue[currentV.father].costStart;
                foreach (Edge n in currentV.Neighbours)
                {
                    Vertex neighbor = this.LValue[n.indexV];
                    int newCost = cost + n.weight;
                    if (openSet.Contains(neighbor) || closeSet.Contains(neighbor))
                    {
                        if (neighbor.costStart > newCost)
                        {
                            neighbor.costStart = newCost;
                            neighbor.costTotal = neighbor.costEnd + neighbor.costStart;
                            neighbor.father = currentV.index;
                            if (openSet.Contains(neighbor))
                                openSet.update(neighbor);
                            if (closeSet.Contains(neighbor))
                                closeSet.update(neighbor);
                        }
                    }
                    else
                    {
                        neighbor.costStart = newCost;
                        neighbor.costTotal = neighbor.costEnd + neighbor.costStart;
                        neighbor.father = currentV.index;
                        openSet.Insert(neighbor);
                    }
                }

            }
            return new List<int>();
        }
        public List<int> endAStar(int start, int end)
        {
            List<int> path = new List<int>();
            PriorityQueue<Vertex> openSet = new PriorityQueue<Vertex>(countdV);
            PriorityQueue<Vertex> closeSet = new PriorityQueue<Vertex>(countdV);
            this.LValue[start].costStart = 0;
            this.LValue[start].costTotal = this.LValue[start].costEnd;
            openSet.Insert(this.LValue[start]);
            int cost = 0;
            while (!openSet.IsEmpty)
            {
                Vertex currentV = openSet.ExtractMin();
                if (currentV.index == end || currentV.visited)
                {
                    while (currentV.index != start)
                    {
                        path.Add(currentV.index);
                        currentV = this.LValue[currentV.fatherEnd];
                    }
                    path.Add(start);
                    path.Reverse();
                    return path;
                }
                //not goal:
                currentV.visited = true;

                closeSet.Insert(currentV);
                if (currentV.fatherEnd != -1)
                    cost = this.LValue[currentV.fatherEnd].costStart;
                foreach (Edge n in currentV.Neighbours)
                {
                    Vertex neighbor = this.LValue[n.indexV];
                    int newCost = cost + n.weight;
                    if (openSet.Contains(neighbor) || closeSet.Contains(neighbor))
                    {
                        if (neighbor.costStart > newCost)
                        {
                            neighbor.costStart = newCost;
                            neighbor.costTotal = neighbor.costEnd + neighbor.costStart;
                            neighbor.fatherEnd = currentV.index;
                            if (openSet.Contains(neighbor))
                                openSet.update(neighbor);
                            if (closeSet.Contains(neighbor))
                                closeSet.update(neighbor);
                        }
                    }
                    else
                    {
                        neighbor.costStart = newCost;
                        neighbor.costTotal = neighbor.costEnd + neighbor.costStart;
                        neighbor.fatherEnd = currentV.index;
                        openSet.Insert(neighbor);
                    }
                }

            }
            return new List<int>();
        }


        public int heuristic(Vertex a, Vertex b)
        {
            return Math.Abs(a.x_value - b.x_value) + Math.Abs(a.y_value - b.y_value);
        }
        public List<int> getPath(int end)
        {
            Stack<int> path = new Stack<int>();
            List<int> l = new List<int>();
            int current = end;
            while (current != 0)
            {
                path.Push(current);
                current = this.LValue[current].father;
            }
            path.Push(0);
            while (path.Count != 0)
            {
                l.Add(path.Pop());
            }
            return l;
        }
        public override string ToString()
        {
            string s = "";
            foreach (Vertex v in this.LValue)
            {
                s += v.ToString() + "\n";
            }
            return s;
        }



    }
}