using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using helper;

namespace model
{
    class Player 
    {
      
        public static int max_undo = 2;
        public static string max_time = "00:2";
        private Label lTime;
        private CreateCaro caro;
       
        private int undo = max_undo;
        private int win;
        private int total;
        private string time = max_time;
        private string icon;
        private int next;
        private bool RUN;
        private bool play;
        private bool exit;

        public Player()
        {
      
        }

        public Player(Label lTime , CreateCaro caro)
        {
            this.lTime = lTime;
            this.caro = caro;
            reset();
        }

        public void startTime()
        {
            ThreadStart myThread = new ThreadStart(run);
            Thread thread = new Thread(myThread);
            thread.Start();
        }

        public int Undo
        {
            get { return undo; }
            set { undo = value; }
        }

        public bool Exit
        {
            get { return exit; }
            set { exit = value; }
        }

        public int Win
        {
            get { return win; }
            set { win = value; }
        }

        public int Total
        {
            get { return total; }
            set { total = value; }
        }

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public int Next
        {
            get { return next; }
            set { next = value; }
        }

        public bool Run
        {
            get { return RUN; }
            set { RUN = value; }
        }

        public bool Play
        {
            get { return play; }
            set { play = value; }
        }

        public void reset()
        {
            undo = 0;
            time = max_time;
            lTime.Text = max_time;
            play = false;
            RUN = true;
            exit = false;
        }

        public void run()
        {
            Console.WriteLine("start time");
            while (RUN)
            {
                lock (this)
                {
                    if (!play)
                        Monitor.Wait(this);

                    if (time == "00:00")
                    {
                        lock (caro)
                        {
                            Monitor.Pulse(caro);
                        }
                        if(this.exit)
                            break;
                        Monitor.Wait(this);
                    }

                    if (!RUN)
                        break;

                    string[] s = time.Split(':');
                    int mini = int.Parse(s[1]);
                    int hour = int.Parse(s[0]);

                    if (mini == 0 && hour > 0)
                    {
                        mini = 59;
                        hour--;
                    }
                    else if (mini != 0)
                    {
                        mini--;
                    }

                    string pHead = hour.ToString();
                    if (pHead.Length == 1)
                        pHead = "0" + pHead;

                    string pTail = mini.ToString();
                    if (pTail.Length == 1)
                        pTail = "0" + pTail;

                    time = pHead + ":" + pTail;

                    try { 
                        lTime.Invoke((MethodInvoker)(() => lTime.Text = time)) ;
                    }
                    catch(Exception e)
                    {
                        time = "00:00";
                        exit = true;
                    }
                  /*  lTime.Invoke(new Action(() =>
                    {
                        lTime.Text = time;
                    }));*/

                    Thread.Sleep(1000);
                }
            }
            Console.WriteLine("end time");
        }

        public void move(Button curent) 
        {
            curent.Text = icon;
        }

  
    }
}
