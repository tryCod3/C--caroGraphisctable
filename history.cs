using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace model
{
    public class history
    {

        public Point p;
        public int player;



        public history() { }

        public history(Point p , int player)
        {
            this.p = p;
            this.player = player;
        }

    }
}
