using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using model;
using System.Drawing;

namespace helper
{
    class point<T , S>
    {
        public T x { get; set; }

        public S y { get; set; }

        public point() { }
    
        public point(T x , S y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class table
    {

        public static bool canMove(Button curent)
        {
            return String.IsNullOrEmpty(curent.Text);
        }

        public static void undo(ref Stack<history> histories , ref int[,] board , ref Button[,] boardButtons)
        {
            if (histories.Count == 0)
                return;
            history curent = histories.Pop();
            remove(ref curent ,ref board , ref boardButtons);
        }

        public static void remove(ref history curent, ref int[,] board, ref Button[,] boardButtons)
        {
            int x = curent.p.X;
            int y = curent.p.Y;
            board[x, y] = 0;
            boardButtons[x, y].Text = "";
            boardButtons[x, y].BackColor = System.Drawing.Color.White;
        }


        private static void makeLight(ref List<point<int, int>> listLight , ref Button[,] boardButtons , ref Graphics g)
        {
           
            if (listLight == null || listLight.Count < 5)
                return;

            listLight.Sort(delegate (point<int , int> a , point<int , int> b)
            {
                if(a.x.CompareTo(b.x) == 0)
                {
                    return a.y.CompareTo(b.y);
                }

                return a.x.CompareTo(b.x);
            });

            /*
            foreach(point<int , int> p in listLight)
            {
                Console.WriteLine(p.x + "--" + p.y);
            }
            */

            int sz = listLight.Count;
          //  Console.WriteLine(listLight[0].x + " - " + listLight[0].y);
           // Console.WriteLine(listLight[sz - 1].x + " - " + listLight[sz - 1].y);

            Point x1 = boardButtons[listLight[0].x, listLight[0].y].Location;
            Point x2 = boardButtons[listLight[sz - 1].x, listLight[sz - 1].y].Location;

            Point point1 = new Point(x1.X , x1.Y);
            Point point2 = new Point(x2.X , x2.Y);

            point1.X += 15;
            point1.Y += 15;
            point2.X += 15;
            point2.Y += 15;

            g.DrawLine(new Pen(Color.FromArgb(255, 20, 147), 2), point1, point2);
        }

        public static List<point<int, int>> roadScoreCrossUpAndDown(int x , int y , int row , int col , ref int[,] board , ref Button[,] boardButtons , int sign , ref Graphics g)
        {
            List<point<int , int>> downAndUp = new List<point<int, int>>();

            // let down
            int i = x;
            int j = y;
            while(i <= col) // i + 1  j 
            {
                if (board[i, j] != sign)
                    break;
                downAndUp.Add(new point<int, int>(i , j));
                i++;
            }

            // let up
            i = x - 1;
            j = y;
            while (i >= 1) // i - 1  j
            {
                if (board[i, j] != sign)
                    break;
                downAndUp.Add(new point<int, int>(i, j));
                i--;
            }

            makeLight(ref downAndUp, ref boardButtons , ref g);

            return downAndUp;
        }

        public static List<point<int, int>> roadScoreLeftAdnRight(int x, int y, int row, int col, ref int[,] board, ref Button[,] boardButtons , int sign, ref Graphics g)
        {
            List<point<int, int>> LeftAdnRight = new List<point<int, int>>();

            // let left
            int i = x;
            int j = y;
            while (j >= 1) // i , j - 1
            {
                if (board[i, j] != sign)
                    break;
                LeftAdnRight.Add(new point<int, int>(i, j));
                j--;
            }

            // let right
            i = x;
            j = y + 1;
            while (j <= col) // i , j + 1
            {
                if (board[i, j] != sign)
                    break;
                LeftAdnRight.Add(new point<int, int>(i, j));
                j++;
            }

            makeLight(ref LeftAdnRight, ref boardButtons , ref g);

            return LeftAdnRight;
        }

        public static List<point<int, int>> roadScoreDm(int x, int y, int row, int col, ref int[,] board, ref Button[,] boardButtons, int sign, ref Graphics g)
        {
            List<point<int, int>> Dm = new List<point<int, int>>();

            // let dm up
            int i = x;
            int j = y;
            while (i >= 1 && j <= col) // i - 1, j + 1
            {
                if (board[i, j] != sign)
                    break;
                Dm.Add(new point<int, int>(i, j));
                i--;
                j++;
            }

            // let dm dowm
            i = x + 1;
            j = y - 1;
            while (i <= row && j >= 1) // i + 1 , j - 1
            {
                if (board[i, j] != sign)
                    break;
                Dm.Add(new point<int, int>(i, j));
                i++;
                j--;
            }

            makeLight(ref Dm, ref boardButtons , ref g);

            return Dm;
        }


        public static List<point<int, int>> roadScoreDs(int x, int y, int row, int col, ref int[,] board, ref Button[,] boardButtons, int sign, ref Graphics g)
        {
            List<point<int, int>> Ds = new List<point<int, int>>();

            // let ds up
            int i = x;
            int j = y;
            while (i >= 1 && j >= 1) // i - 1, j - 1
            {
                if (board[i, j] != sign)
                    break;
                Ds.Add(new point<int, int>(i, j));
                i--;
                j--;
            }

            // let ds dowm
            i = x + 1;
            j = y + 1;
            while (i <= row && j <= col) // i + 1 , j + 1
            {
                if (board[i, j] != sign)
                    break;
                Ds.Add(new point<int, int>(i, j));
                i++;
                j++;
            }

            makeLight(ref Ds, ref boardButtons , ref g);

            return Ds;
        }


    }
}
