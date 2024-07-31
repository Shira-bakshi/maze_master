using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.classes
{
    public class MazeRequest
    {
        public MazeRequest()
        {
            Matrix_maze = new List<List<int>>();
        }

        public List<List<int>> Matrix_maze { get; set; }
        public int Transition { get; set; }
        public int Wall { get; set; }


    }

}
