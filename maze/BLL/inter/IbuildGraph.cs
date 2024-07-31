using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.inter
{ 
    public interface IbuildGraph
    {
        public int[,] solveMatrix(int[,] mazeMatrix, int transition, int wall);
        public int[,] ConvertListTo2DArray(List<List<int>> listOfLists);
        public void stam();

    }
}



