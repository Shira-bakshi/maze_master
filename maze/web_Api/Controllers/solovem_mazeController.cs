using BLL.classes;
using BLL.inter;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace web_Api.Controllers
{
    //[Route("web_api/[controller]")]
    //        [ApiController]
    public class solovem_mazeController : Controller
    {
        IbuildGraph bG;
        public solovem_mazeController(IbuildGraph bG)
        {
            this.bG = bG;
        }
        [HttpPost("s")]
        public ActionResult<string> s([FromBody] List<List<int>> mazeRequest)
        {
            return Ok("sssss");
        }

        [HttpPost("solveMatrix")]
        //public ActionResult<int[,]> solve(int[,] matrix_maze, int t, int wall)
        //{
        //    return Ok(bG.solveMatrix(matrix_maze,t,wall));
        //}
        public ActionResult<int[,]> SolveMatrix([FromBody] MazeRequest mazeRequest)
        {
            if (mazeRequest == null)
            {
                Console.WriteLine("++++++++++++++++++++++++++++++");
                return BadRequest("Invalid data.");
            }
            Console.WriteLine("Transition: " + mazeRequest.Transition);
            Console.WriteLine("Wall: " + mazeRequest.Wall);
            Console.WriteLine("MatrixMaze: " + mazeRequest.Matrix_maze.ToString());
            //foreach (var item in mazeRequest.Matrix_maze)
            //{
            //    foreach (var item1 in item)
            //    {
            //        Console.Write(item1);
            //    }
            //    Console.WriteLine();
            //}
            Console.WriteLine("_________");
            int Wall = mazeRequest.Wall;
            int Transition = mazeRequest.Transition;


            List<List<int>> listOfLists = mazeRequest.Matrix_maze;
            // Determine the size of the 2D array
            int rows = listOfLists.Count;
            int cols = listOfLists[0].Count;

            // Create the 2D array
            int[,] array2D = new int[cols, rows];

            // Fill the 2D array with values from the List<List<int>>
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array2D[j, i] = listOfLists[i][j];
                    //Console.Write(array2D[i,j]);
                }
                //Console.WriteLine();
            }

            // כאן תוסיף את הלוגיקה לפתרון המבוך


            //return Ok(array2D);
            Console.WriteLine("the new maze: lasttt");
            array2D = bG.solveMatrix(array2D, Transition, Wall);
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                     listOfLists[i][j]= array2D[i,j];
                    //Console.Write(listOfLists[i][j]);
                }
                //Console.WriteLine();
            }
            Console.WriteLine("finish!!");
            return Ok(listOfLists);
        }


    }

    
}
