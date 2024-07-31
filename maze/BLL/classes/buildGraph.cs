using BLL.inter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AutoMapper;

namespace BLL.classes
{
    public class buildGraph:IbuildGraph
    {
        IbuildGraph Ib;
        //IMapper imapper;

        public graph graph = new graph();
        public string[] since = new string[] { "up", "down", "right", "left" };
        public int index_g = 0;
        public int[,] mazeMatrix;
        public void stam()
        {
            Console.WriteLine("it's work!");
        }
        public int[,] solveMatrix(int[,] mazeMatrix,int transition,int wall )
        {
            Console.WriteLine("++++++");
            this.mazeMatrix = mazeMatrix;
            int[,] wallx;
            int[,] wally;
            Vertex s = new Vertex();
            Vertex f = new Vertex();
            List<Vertex> lsx;
            List<Vertex> lsy;
            if (transition==wall && wall==1)
            {
               wallx = new int[mazeMatrix.GetLength(0), mazeMatrix.GetLength(1)];
               wally = new int[mazeMatrix.GetLength(0), mazeMatrix.GetLength(1)];
               build_graph(wallx, wally, s, f);
               lsx = graph.LValue;
               lsy = graph.sortY();
               findAllNeighbours(wallx, wally, lsx, lsy);
                Console.WriteLine("this g:+");
                Console.WriteLine(s.ToString());
                Console.WriteLine(f.ToString());
                //Console.WriteLine(graph.ToString());
            }
            else
            {
                build_graph1(s, f, transition, wall);
            }


            return solveMaze(graph);
        }
        public int[,] solveMaze(graph g)
        {
            List<int> startPath = new List<int>();
            List<int> endPath = new List<int>();

            Mutex mutex = new Mutex();
            // יצירת תהליכים
            Thread thread1 = new Thread(() =>
            {
                // פונקציה שתחשב את הנתיב מההתחלה לסוף
                startPath = graph.startAStar(0, graph.countdV - 1);
            });

            Thread thread2 = new Thread(() =>
            {
                // פונקציה שתחשב את הנתיב מהסוף להתחלה
                endPath = graph.endAStar(graph.countdV - 1, 0);
            });

            // הפעלת התהליכים
            thread1.Start();
            thread2.Start();

            // המתנה לסיום התהליכים
            thread1.Join();
            thread2.Join();
            Console.WriteLine("the threads:");
            for (int i = 0; i < startPath.Count; i++)
            {
                Console.Write(startPath[i] + "->");
            }
            Console.WriteLine();
            for (int i = 0; i < endPath.Count; i++)
            {
                Console.Write(endPath[i] + "->");
            }
            Console.WriteLine();
            return PrintPath(graph, startPath, endPath);
        }
        public  int[,] PrintPath(graph g, List<int> start, List<int> end)
        {
            int[,] tempMazeMatrix = mazeMatrix;
            for (int i = 0; i < start.Count - 1; i++)
            {
                tempMazeMatrix = print(g.LValue[start[i]], g.LValue[start[i + 1]], tempMazeMatrix);
            }
            for (int i = 0; i < end.Count - 1; i++)
            {
                tempMazeMatrix = print(g.LValue[end[i]], g.LValue[end[i + 1]], tempMazeMatrix);
            }
            Console.WriteLine("the maze solve:");
            //for (int i = 0; i < tempMazeMatrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < tempMazeMatrix.GetLength(1); j++)
            //    {
            //        Console.Write(tempMazeMatrix[i, j] + " ");
            //    }
            //    Console.WriteLine();

            //}
            return tempMazeMatrix;
        }
        public  int[,] print(Vertex v1, Vertex v2, int[,] maze)
        {
            if (v1.x_value == v2.x_value)
            {
                int min = v1.y_value < v2.y_value ? v1.y_value : v2.y_value;
                int max = v1.y_value > v2.y_value ? v1.y_value : v2.y_value;

                for (int j = min; j <= max; j++)
                {
                    maze[v1.x_value, j] = 3;
                }
            }
            else
            {
                int min = v1.x_value < v2.x_value ? v1.x_value : v2.x_value;
                int max = v1.x_value > v2.x_value ? v1.x_value : v2.x_value;

                for (int i = min; i <= max; i++)
                {
                    maze[i, v1.y_value] = 3;
                }
            }
            return maze;
        }
        //public bool is_1(int[,] mazeMatrix)
        //{
        //    for(int i = 0;i< mazeMatrix.GetLength(0);i++)
        //    {
        //        if (mazeMatrix[0,i] != mazeMatrix[1,i])
        //            return false;
        //    }
        //    return true;
        //}
        public  bool binaryFind(int[,] Iwall, int i, int IVarMIn, int IVarMax)
        {
            int countArr = 1;//המספר הראשון הוא 0 , במקרה שזהו פתח המבוך אז אז יש קיר בסוף
            while (countArr < Iwall.GetLength(1)-1  && Iwall[i, countArr] != 0)
                countArr++;

            int left = 0, right = countArr - 1;

            while (left <= right)
            {
                int middle = left + (right - left) / 2;

                if (Iwall[i, middle] > IVarMIn && Iwall[i, middle] < IVarMax)//בדיקה האם המספר בטווח
                    return true;
                else if (Iwall[i, middle] < IVarMIn)
                    left = middle + 1;
                else
                    right = middle - 1;
            }
            return false;
        }
        //פונקציה המקבלת קודקוד ושני קודקודים משני צדדיו ובודקת האם הם שכנים
        //אין בינהם קיר
        public void isNeighbors(int[,] wall, Vertex v, Vertex NRight, Vertex Nleft, char type_)
        {
            if (type_ == 'x')
            {
                if (NRight != null && binaryFind(wall, v.x_value, NRight.y_value, v.y_value) == false)
                    v.addNeighbours(NRight.index, v.y_value - NRight.y_value);
                if (Nleft != null && binaryFind(wall, v.x_value, v.y_value, Nleft.y_value) == false)
                    v.addNeighbours(Nleft.index, Nleft.y_value - v.y_value);
            }
            else
            {
                if (NRight != null && binaryFind(wall, v.y_value, NRight.x_value, v.x_value) == false)
                    v.addNeighbours(NRight.index, v.x_value - NRight.x_value);
                if (Nleft != null && binaryFind(wall, v.y_value, v.x_value, Nleft.x_value) == false)
                    v.addNeighbours(Nleft.index, Nleft.x_value - v.x_value);
            }
        }
        //פונקציה המקבלת תת של ליסט ומוצאת את השכנים של כל האיברים שבאותו ליסט
        public void find(int[,] wall, List<Vertex> lv, char type_)
        {
            for (int i = 0; i < lv.Count; i++)
            {
                if (i == 0 && i + 1 < lv.Count)
                    isNeighbors(wall, lv[i], null, lv[i + 1], type_);
                else if (i == lv.Count - 1 && i > 0)
                    isNeighbors(wall, lv[i], lv[i - 1], null, type_);
                else if (i > 0 && i < lv.Count)
                {
                    isNeighbors(wall, lv[i], lv[i - 1], lv[i + 1], type_);
                }
            }
        }
        public void findAllNeighbours(int[,] wallx, int[,] wally, List<Vertex> lSX, List<Vertex> lSY)
        {
            List<Vertex> temp;
            int i = 0;
            int vTemp = lSX[i].x_value;
            //מציאת שכנים לצדדים
            while (i < lSX.Count)
            {
                temp = lSX.Where(x => x.x_value == vTemp).ToList();
                temp.Sort((a, b) => a.y_value.CompareTo(b.y_value));
                find(wallx, temp, 'x');
                if (i + temp.Count < lSX.Count)
                {
                    vTemp = lSX[i + temp.Count].x_value;
                    i += temp.Count;
                }
                else
                    i = lSX.Count;
            }
            i = 0;
            vTemp = lSY[i].y_value;
            temp = new List<Vertex>();
            //מציאת שכנים למעלה ולמטה
            while (i < lSY.Count)
            {
                temp = lSY.Where(x => x.y_value == vTemp).ToList();
                temp.Sort((a, b) => a.x_value.CompareTo(b.x_value));
                find(wally, temp, 'y');

                if (i + temp.Count < lSX.Count)
                {
                    vTemp = lSY[i + temp.Count].y_value;
                    i += temp.Count;
                }
                else
                    i = lSY.Count;
            }
        }
         public void build_graph(int[,] wallx, int[,] wally, Vertex s, Vertex f)
         {
         
             Vertex t = new Vertex();
             int c = 0, c1 = 0;
             int indexGraph = 0;
             for (int i = 0; i < mazeMatrix.GetLength(0); i++)//מציאת התחלה וסיום 
             {
                 if (mazeMatrix[0, i] == 1)
                 {
                     t = new Vertex(0, i, indexGraph++);
                    s = t;
                     graph.add(s);
                 }
                 if (mazeMatrix[mazeMatrix.GetLength(0) - 1, i] == 1)
                 {
                     t = new Vertex(mazeMatrix.GetLength(1) - 1, i, 0);
                    f = t;
                 }
             }
            if (s.x_value == -1 || f.x_value == -1)
            {
                for (int i = 0; i < mazeMatrix.GetLength(0); i++)//מציאת התחלה וסיום 
                {
                    if (mazeMatrix[i, 0] == 1)
                    {
                        s = new Vertex(i, 0, indexGraph++);
                        graph.add(s);
                    }
                    if (mazeMatrix[mazeMatrix.GetLength(0) - 1, i] == 1)
                    {
                        f = new Vertex(i, mazeMatrix.GetLength(1) - 1, 0);
                    }
                }
            }


            bool flag = false;
             for (int i = 0; i < mazeMatrix.GetLength(0) - 1; i++)//מציאת שאר הצמתים
             {
                 c = 0;
                 c1 = 0;
                 for (int j = 0; j < mazeMatrix.GetLength(1) - 1; j++)
                 {
                     if (i > 0 && i < mazeMatrix.GetLength(0) - 1 && j > 0 && j < mazeMatrix.GetLength(1) - 1)
                         if (mazeMatrix[i, j] == 1)
                         {
                             flag = false;
                             //אם הנקודה היא רק מעבר בין צמתים היא לא צומת
                             if ((mazeMatrix[i - 1, j] == 1 && mazeMatrix[i + 1, j] == 1 &&
                                 mazeMatrix[i, j - 1] == 0 && mazeMatrix[i, j + 1] == 0) ||
                                 (mazeMatrix[i - 1, j] == 0 && mazeMatrix[i + 1, j] == 0 &&
                                 mazeMatrix[i, j - 1] == 1 && mazeMatrix[i, j + 1] == 1))
                                 flag = true;
                             // אם היא צומת
                                if (flag == false)
                                {
                                    Vertex v = new Vertex(i, j, indexGraph++);
                                    v.DistanacPoint(f, s);
                                    graph.add(v);
                                }
                            }
                        if (mazeMatrix[i, j] == 0)
                        {
                            wallx[i, c++] = j;

                        }
                        if (j< mazeMatrix.GetLength(0)-1 && mazeMatrix[j, i] == 0)
                        {
                            wally[i, c1++] = j;
                        }
                 }
             }
             f.index = indexGraph;
             s.DistanacPoint(f, s);
             f.DistanacPoint(f, s);
             graph.add(f);
         }
        public void build_graph1(Vertex s, Vertex f, int transition, int wall)
        {
            int indexGraph = 0;
            int c = 0;
            for (int i = 0; i < mazeMatrix.GetLength(0); i++)//מציאת התחלה וסיום 
            {
                if (s.x_value ==-1  && mazeMatrix[0, i] == 1)
                {
                    s = new Vertex(0, i + (transition / 2), index_g++);
                    //put_v(s, transition);
                    graph.add(s);
                    Console.WriteLine(s.ToString());
                    c = 1;
                    break;
                }
                if (s.x_value == -1 && mazeMatrix[i,0] == 1)
                {
                    s = new Vertex(i + (transition / 2),0,  index_g++);
                    //put_v(s, transition);
                    graph.add(s);
                    Console.WriteLine(s.ToString());
                    c = 4;
                    break;
                }
            }
                //if (f == null && mazeMatrix[mazeMatrix.GetLength(1) - wall, i] == 1)
                //{
                //    f = new Vertex(mazeMatrix.GetLength(1) - wall, i + (transition / 2), 0);
                //    //Console.WriteLine("---" + f.ToString());

                //}

            b_g(s, transition,wall ,f, c);
            //int index = graph2.findVertex(f);
            //Vertex temp = graph2.LValue[index];
            f.index = index_g++;
            graph.add(f);
            List<Edge> edges = f.getNeighbours();
            for (int i = 0; i < edges.Count; i++)
            {
                graph.LValue[edges[i].indexV].addNeighbours(f.index, edges[i].weight);
            }

        }
        public Vertex is_t(int x, int y, int transition,int wall, Vertex f, int c, int[] width)
        {
            Vertex v = null;
            int j = 0;
            int i_1 = 0, i_2 = 0, i_3 = 0, i_4 = 0;
            bool f_1 = false, f_2 = false, f_3 = false, f_4 = false;
            //Console.WriteLine(x+ ", " + y);
            while (j <= transition)
            {
                //x - transition < 0 ||
                if (x - transition < 0 || x + transition > mazeMatrix.GetLength(0) || mazeMatrix[x - j, y] != 1)//up
                    f_1 = true;//wall
                if (y - transition < 0 || y + transition > mazeMatrix.GetLength(1) || mazeMatrix[x, y + j] != 1)//right
                    f_2 = true;
                if (x + transition > mazeMatrix.GetLength(0) - 1 || mazeMatrix[x + j, y] != 1)//doun
                    f_3 = true;
                if (x - transition < 0 || y + transition > mazeMatrix.GetLength(1) || mazeMatrix[x, y - j] != 1)//left
                {
                    f_4 = true;
                    //Console.WriteLine(x - transition+ " " + y + transition +" godel: "+ mazeMatrix.GetLength(1)+" "+ mazeMatrix[x, y - j]);
                }

                j++;
            }
            //Console.WriteLine(f_1+" "+f_2+" "+ f_3+" "+f_4);
            if (f_1 != f_3 || f_2 != f_4)//הנק' היא צומת יש להוסיף אותה
            {
                //Console.WriteLine(x+" "+y);
                width[0]++;

                v = new Vertex(x, y, width[0]);
                if (x>mazeMatrix.GetLength(0)-wall||y>mazeMatrix.GetLength(1)-wall)
                {
                    f = v;
                }
                return v;
            }

            if (f_1 == f_3)// transition from right
            {
                if (c != 4 && f_4 == false)
                {
                    width[0]++;
                    v = is_t(x, y - transition, transition,wall, f, c, width);

                }
                if (c != 2 && f_2 == false)
                {
                    width[0]++;
                    v = is_t(x, y + transition, transition,wall, f, c, width);
                }
            }
            if (f_2 == f_4)
            {
                if (c != 3 && f_3 == false)
                {
                    width[0]++;
                    v = is_t(x + transition, y, transition,wall, f, c, width);
                }
                if (c != 1 && f_1 == false)
                {
                    width[0]++;
                    v = is_t(x - transition, y, transition,wall, f, c, width);
                }
            }
            if (x > mazeMatrix.GetLength(0) - wall || y > mazeMatrix.GetLength(1) - wall)
            {
                f = v;
            }
            return v;
        }
        public void b_g(Vertex s, int transition,int wall, Vertex f, int since)
        {
            Console.WriteLine(s.ToString());
            int c_x = s.x_value;
            int c_y = s.y_value;
            int i = 1;
            bool f_0 = false;
            bool is_w_1 = false, is_w_2 = false, is_w_3 = false, is_w_4 = false;
            while (i <= transition)
            {
                if (since == 1 || c_x - transition < 0 || mazeMatrix[c_x - i, c_y] == 0)//up
                    is_w_1 = true;
                if (since == 2 || c_y + transition > mazeMatrix.GetLength(1) - 1 || mazeMatrix[c_x, c_y + i] == 0)//right
                    is_w_2 = true;
                if (since == 3 || c_x + transition > mazeMatrix.GetLength(0) - 1 || mazeMatrix[c_x + i, c_y] == 0)//down
                    is_w_3 = true;
                if (since == 4 || c_y - transition < 0 || mazeMatrix[c_x, c_y - i] == 0)//left
                {
                    //Console.WriteLine(since + " " + (c_y - transition) + " "+ mazeMatrix[c_x, c_y - i]+"wall?? "+is_w_4);

                    is_w_4 = true;
                }
                i++;
            }
            //Console.WriteLine("current: " + is_w_1 + " " + is_w_2 + " " + is_w_3 + " " + is_w_4);
            //נקודה מימין לקודקוד הנוכחי היא מעבר המשך ממנה 
            // צריך לבדוק אם היא מעבר או צומת
            Vertex current1 = null, current2 = null, current3 = null, current4 = null;
            int[] width = { 0, 0 };
            int index;
            if (is_w_1 == false && since != 1)//down
            {
                current1 = is_t(c_x - transition, c_y, transition,wall, f, 3, width);
                if (current1 != null)
                {
                    index = graph.findVertex(current1.x_value, current1.y_value);
                    if (index != -1)
                        s.addNeighbours(index, width[0]);
                    else
                    if (f.x_value == current1.x_value && f.y_value == current1.y_value)
                    {
                        f.addNeighbours(s.index, width[0]);
                    }
                    else
                    {
                        s.addNeighbours(index_g, width[0]);
                        current1.index = index_g++;
                        current1.addNeighbours(s.index, width[0]);
                        graph.add(current1);
                        //b_g(current1, transition, wall, 3);
                        current1.father = 1;

                    }
                }
            }
            if (is_w_2 == false && since != 2)//left
            {
                current2 = is_t(c_x, c_y + transition, transition,wall ,f, 4, width);
                if (current2 != null)
                {
                    index = graph.findVertex(current2.x_value, current2.y_value);
                    if (index != -1)
                        s.addNeighbours(index, width[0]);
                    else
                    if (f.x_value == current2.x_value && f.y_value == current2.y_value)
                    {
                        f.addNeighbours(s.index, width[0]);
                    }
                    else
                    {
                        s.addNeighbours(index_g, width[0]);
                        current2.index = index_g++;
                        current2.addNeighbours(s.index, width[0]);
                        graph.add(current2);
                        //b_g(current2, transition, wall, 3);
                        current2.father = 1;
                    }
                }
            }
            if (is_w_3 == false && since != 3)//up 
            {
                current3 = is_t(c_x + transition, c_y, transition,wall, f, 1, width);
                //Console.WriteLine("-----------"+transition);
                if (current3 != null)
                {
                    index = graph.findVertex(current3.x_value, current3.y_value);
                    if (index != -1)
                        s.addNeighbours(index, width[0]);
                    else
                    if (f.x_value == current3.x_value && f.y_value == current3.y_value)
                    {
                        f.addNeighbours(s.index, width[0]);
                    }
                    else
                    {
                        s.addNeighbours(index_g, current3.index);
                        current3.index = index_g++;
                        current3.addNeighbours(s.index, width[0]);
                        graph.add(current3);
                        //b_g(current3, transition, wall, 3);
                        current3.father = 1;

                    }
                }
            }
            if (is_w_4 == false && since != 4)//right
            {
                current4 = is_t(c_x, c_y - transition, transition,wall, f, 2, width);

                if (current4 != null)
                {
                    index = graph.findVertex(current4.x_value, current4.y_value);
                    if (index != -1)
                        s.addNeighbours(index, width[0]);
                    else
                    if (f.x_value == current4.x_value && f.y_value == current4.y_value)
                    {
                        f.addNeighbours(s.index, width[0]);
                    }
                    else
                    {
                        s.addNeighbours(index_g, width[0]);
                        current4.index = index_g++;
                        current4.addNeighbours(s.index, width[0]);
                        current4.father = 1;
                        graph.add(current4);
                        //b_g(current4, transition, wall, 3);
                    }
                }
            }
            if (current1 != null && current1.father == 1)
                b_g(current1, transition,wall, f, 3);
            if (current2 != null && current2.father == 1)
                b_g(current2, transition,wall, f, 4);
            if (current3 != null && current3.father == 1)
                b_g(current3, transition,wall, f, 1);
            if (current4 != null && current4.father == 1)
                b_g(current4, transition,wall, f, 2);
        }
        public int[,] ConvertListTo2DArray(List<List<int>> listOfLists)
        {
            Console.WriteLine("commm");
            // Determine the size of the 2D array
            int rows = listOfLists.Count;
            int cols = listOfLists[0].Count;

            // Create the 2D array
            int[,] array2D = new int[rows, cols];

            // Fill the 2D array with values from the List<List<int>>
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array2D[i, j] = listOfLists[i][j];
                }
            }

            return array2D;
        }
    }
}

