using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using model;
using System.Threading;
using System.Drawing;

namespace helper
{
    class CreateCaro : UserControl{ 
        public enum entity
        {
            player1 = 1,
            player2 = 2,
        }

   

        private static CreateCaro INSTANCE;

        public static readonly int PT = 10;
        public static readonly int PL = 10;

        public static readonly int SCol = 12;

        public static readonly int SRow = 12;

        public static readonly int SWidthBox = 30;

        public static readonly int SHeightBox = 30;

        public static readonly int nextSpace = 31;

        private static readonly int sizeWTablePanelCaro = (SWidthBox + (nextSpace - SWidthBox)) * SCol + (PL * 2);

        private static readonly int sizeHTablePanelCaro = (SHeightBox + (nextSpace - SHeightBox)) * SRow + (PT * 2);

        private bool start , over ;

        private int[,] board = new int[SRow + 1, SCol + 1];

        private Button[,] boardButtons = new Button[SRow + 1, SCol + 1];

        private Stack<history> histories = new Stack<history>();

        private int curent = 1;

        private int playerWin;

        private Player player1;

        private Player player2;

        private Bitmap bm;
        private Graphics myGbm;

        private Bitmap bm2;
        private Graphics myGbm2;

        public CreateCaro()
        {

            start = false;
            over = false;
            playerWin = 0;

            if (pTableCaro == null)
                 pTableCaro = new Panel();
            if (pTableControl == null)
                pTableControl = new FlowLayoutPanel();

            reBm();

            
            //UserPaint -> không còn phụ thuộc vào hdh nữa
            // để tránh lỗi tràn bộ nhớ  , hdh sẽ xem vùng nhớ còn đủ hay không thì nó mới thao tác
            // khi UserPaint = true , bỏ quyền quản lí của hdh
            this.SetStyle(ControlStyles.UserPaint , true);
            // tắt message từ windown , hdh m để thao tác vvẽ nhanh hơn
            this.SetStyle(ControlStyles.AllPaintingInWmPaint , true);
            // gấp đôi bộ nhớ đệm
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer , true);
             Console.WriteLine("start build");


        }





        public void setUpforPlayer1()
        {
            if (player1 == null)
            {
                player1 = new Player(lTime1 , this);
                player1.Icon = "X";
            }
            
            if (lTime1.InvokeRequired)
                lName1.Invoke((MethodInvoker)(() => lName1.Text = "Player 1"));
            else
                lName1.Text = "Player 1";

            if (lTime1.InvokeRequired)
                lTime1.Invoke((MethodInvoker)(() => lTime1.Text = "time " + player1.Time));
            else
                lTime1.Text = "time " + player1.Time;

            if (lUnder1.InvokeRequired)
                lUnder1.Invoke((MethodInvoker)(() => lUnder1.Text = "undo " + player1.Undo));
            else
                lUnder1.Text = "undo " + player1.Undo;
            
            if(lWin1.InvokeRequired)
                lWin1.Invoke((MethodInvoker)(() => lWin1.Text = "win " + player1.Win + "/" + player1.Total));
            else
                lWin1.Text = "win " + player1.Win + "/" + player1.Total;
        }

        public void setUpforPlayer2()
        {
            if (player2 == null)
            {
                player2 = new Player(lTime2 , this);
                player2.Icon = "O";
            }
            if (lTime2.InvokeRequired)
                lName2.Invoke((MethodInvoker)(() => lName2.Text = "Player 2"));
            else
                lName2.Text = "Player 2";
            
            if (lTime2.InvokeRequired)
                lTime2.Invoke((MethodInvoker)(() => lTime2.Text = "time " + player2.Time));
            else
                lTime2.Text = "time " + player2.Time;

            if (lUnder2.InvokeRequired)
                lUnder2.Invoke((MethodInvoker)(() => lUnder2.Text = "undo " + player2.Undo));
            else
                lUnder2.Text = "undo " + player2.Undo;

            if (lWin2.InvokeRequired)
                lWin2.Invoke((MethodInvoker)(() => lWin2.Text = "win " + player2.Win + "/" + player2.Total));
            else
                lWin2.Text = "win " + player2.Win + "/" + player2.Total;
        }

        private void caroSetUpTime()
        {
            Console.WriteLine("Start GAME");
            player1.startTime();
            player2.startTime();

            while (this.start && !this.over)
            {
                if(histories.Count > 0)
                {
                    history h = histories.Peek();
                    int x = h.p.X;
                    int y = h.p.Y;

                    if(this.curent == (int)entity.player1 &&
                       player1.Time == "00:00" &&
                       board[x,y] != this.curent) { 
                        this.start = false;
                        this.over = true;
                        player1.Run = false;
                        player2.Run = false;
                        this.playerWin = (int)entity.player2;
                        break;
                    }
                    else if(this.curent == (int)entity.player2 &&
                       player2.Time == "00:00" &&
                       board[x,y] != this.curent) { 
                        this.start = false;
                        this.over = true;
                        player1.Run = false;
                        player2.Run = false;
                        this.playerWin = (int)entity.player1;
                        break;
                    }
                     Icon icon = new Icon("D:\\c#############\\CaroForm3\\close-_1_.ico");
                    Point p1 = boardButtons[x, y].Location;
                    if (this.curent == (int)entity.player2)
                    { icon = new Icon("D:\\c#############\\CaroForm3\\o-_1_.ico"); }
                     myGbm.DrawIcon(icon , p1.X + 5 , p1.Y + 5);
                   
             

                     if(this.curent == (int)entity.player1) {                   
                        this.curent = (int)entity.player2;
                     }
                     else {
                        this.curent = (int)entity.player1;
                     }
                }

                lock (this)
                {
                    if (this.curent == (int)entity.player1)
                    {
                        lock (player1)
                        {
                            player1.Play = true;
                            player1.Time = Player.max_time;
                            Monitor.Pulse(player1);
                        }
                    }
                    else if (this.curent == (int)entity.player2)
                    {
                        lock (player2)
                        {
                            player2.Play = true;
                            player2.Time = Player.max_time;
                            Monitor.Pulse(player2);
                        }
                    }

                    Monitor.Wait(this);
                    
                    if(histories.Count == 0)
                    {
                        if(this.curent == (int)entity.player1)
                            this.playerWin = (int)entity.player2;
                        else
                            this.playerWin = (int)entity.player1;
                        this.start = false;
                        this.over = true;
                        player1.Run = false;
                        player2.Run = false;
                        break;
                    }
                }
            }
         
            if (this.playerWin == 3)
            {
                if (this.player1.Undo > this.player2.Undo)
                    this.playerWin = (int)entity.player2;
                else if (this.player1.Undo < this.player2.Undo)
                    this.playerWin = (int)entity.player1;
            }

            lock (player1)
            {
                Monitor.Pulse(player1);
            }

            lock (player2)
            {
                Monitor.Pulse(player2);
            }

            if(player1.Exit || player2.Exit)
                return;

            if(playerWin == (int)entity.player1)
            {
                MessageBox.Show("player 1 win!");
                player1.Win++;
            }
            else if(playerWin == (int)entity.player2)
            {
                MessageBox.Show("player 2 win!");
                player2.Win++;
            }else if(this.playerWin == 3)
            {
                MessageBox.Show("Draw!");
            }

            player1.Total++;
            player2.Total++;

            setUpforPlayer1();
            setUpforPlayer2();

            Console.WriteLine("END GAME");
        }

        private void runGame()
        {
            Thread thread = new Thread(new ThreadStart(caroSetUpTime));
            thread.Start();
        }

        public void refesh()
        {
            start = false;
            over = false;

            curent = 1;
            playerWin = 0;

            player1.reset();
            player2.reset();

            setUpforPlayer1();
            setUpforPlayer2();

            while(histories.Count > 0)
            {
                history h = histories.Pop();
                table.remove(ref h,ref board,ref boardButtons);
            }

            reBm();
            
            // call onPaint => draw again
           // this.Invalidate();
        }

        private void reBm()
        {
            this.BackgroundImage = null;
            this.bm = new Bitmap(sizeWTablePanelCaro , sizeHTablePanelCaro);
            this.myGbm = Graphics.FromImage(this.bm);
            guiCaro();
            this.BackgroundImage = (Bitmap)this.bm.Clone();
        }


        public void guiCaro()
        {
            drawCheoDoc();
            drawCheoNgang();
            
        }

        private void drawCheoDoc()
        {
            Graphics g = this.CreateGraphics();

            int mix = Math.Min(SRow, SCol);
            int max = Math.Max(SRow, SCol);

            Pen pen = new Pen(Brushes.Black);
            pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            if (SRow < SCol)
            {
                mix = SCol;
                max = SRow;
            }

            int start = PT;
            for (int i = 1; i <= mix + 1; i++)
            {
                int end = PL;
                for (int j = 1; j <= max; j++)
                {
                    PointF p1 = new PointF(start - 1, end - 1);
                    PointF p2 = new PointF(start - 1, end + 30);
                   // g.DrawLine(pen, p1, p2);
                    // save in bitmap
                    myGbm.DrawLine(pen,p1,p2);
                 
                    end += (SHeightBox + (nextSpace - SHeightBox));
                }
                start += (SHeightBox + (nextSpace - SHeightBox));
            }

        }


        private void drawCheoNgang()
        {

            Graphics g = this.CreateGraphics();
            int mix = Math.Min(SRow, SCol);
            int max = Math.Max(SRow, SCol);

            Pen pen = new Pen(Brushes.Black);
            pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            if (SRow < SCol)
            {
                mix = SCol;
                max = SRow;
            }

            int start = PT;
            for (int i = 1; i <= max + 1; i++)
            {
                int end = PL;
                for (int j = 1; j <= mix; j++)
                {
                    PointF p1 = new PointF(end - 1, start - 1);
                    PointF p2 = new PointF(end + 30, start - 1);
                  //  g.DrawLine(pen, p1, p2);
                    // save in bitmap
                    myGbm.DrawLine(pen,p1,p2);
               
                    end += (SWidthBox + (nextSpace - SWidthBox));
                }
                start += (SHeightBox + (nextSpace - SHeightBox));
            }
        }

        private Stack<history> Clone()
        {
            var arr = new history[histories.Count];
            histories.CopyTo(arr, 0);
            Array.Reverse(arr);
            return new Stack<history>(arr);
        }

        private void drawInHistory()
        {
            if (histories == null) return;

            Stack<history> cloneHistory = Clone();

            while (cloneHistory.Count > 0)
            {
                history top = cloneHistory.Pop();
                entity curentNow = (entity)top.player;
                drawText(curentNow, top.p);
            }
        //    Console.WriteLine("his = " + histories.Count);
        }

        private void drawText(entity e ,Point p)
        {
            Graphics g = this.CreateGraphics();
            Icon icon = new Icon("D:\\c#############\\CaroForm3\\close-_1_.ico");
            Point p1 = boardButtons[p.X , p.Y].Location;
            if (e == entity.player2)
            { icon = new Icon("D:\\c#############\\CaroForm3\\o-_1_.ico"); }
            g.DrawIcon(icon  , p1.X + 5 , p1.Y + 5);
        }

        private void forBoard()
        {
           
            int start = PT;
            for (int i = 1; i <= SRow; i++)
            {
                int end = PL;
                for (int j = 1; j <= SCol ; j++)
                {
                    Button b = new Button()
                    {
                        Width = 30,
                        Height = 30,
                        Location = new Point(end, start),
                        Name = i + " " + j,
                        BackColor = Color.White,
                    };
                 //   b.Click += new MouseEventArgs(this.click_handel);
                    board[i, j] = 0;
                    boardButtons[i, j] = b;
               //     pTableCaro.Controls.Add(b);
                    end += (SWidthBox + (nextSpace - SWidthBox));
                }
                start += (SHeightBox + (nextSpace - SHeightBox));
            }
          //  pTableCaro.BackColor = Color.Red;

        }

       
        private void guiControl()
        {            
            // control
            FlowLayoutPanel flControl = new FlowLayoutPanel();
            pControl = new Panel();
            bStart = new Button()
            {
                Text = "Start"
            };
            bClear = new Button()
            {
                Text = "Clear"
            };
            bUndo = new Button()
            {
                Text = "Undo"
            };
            bClear.Click += new EventHandler(this.click_bClear);
            bUndo.Click += new EventHandler(this.click_bUndo);
            bStart.Click += new EventHandler(this.click_bStart);
            flControl.FlowDirection = FlowDirection.TopDown;
            flControl.Controls.Add(bStart);
            flControl.Controls.Add(bClear);
            flControl.Controls.Add(bUndo);
            pControl.Controls.Add(flControl);

            // player 1
            FlowLayoutPanel flPlayer1 = new FlowLayoutPanel();
            pInfo1 = new Panel();
            lName1 = new Label();
            lTime1 = new Label();
            lUnder1 = new Label();
            lWin1 = new Label();
            setUpforPlayer1();
            flPlayer1.FlowDirection = FlowDirection.TopDown;
            flPlayer1.Controls.Add(lName1);
            flPlayer1.Controls.Add(lTime1);
            flPlayer1.Controls.Add(lUnder1);
            flPlayer1.Controls.Add(lWin1);
            pInfo1.Controls.Add(flPlayer1);

            // player 2
            FlowLayoutPanel flPlayer2 = new FlowLayoutPanel();
            pInfo2 = new Panel();
            lName2 = new Label();
            lTime2 = new Label();
            lUnder2 = new Label();
            lWin2 = new Label();
            setUpforPlayer2();
            flPlayer2.FlowDirection = FlowDirection.TopDown;
            flPlayer2.Controls.Add(lName2);
            flPlayer2.Controls.Add(lTime2);
            flPlayer2.Controls.Add(lUnder2);
            flPlayer2.Controls.Add(lWin2);
            pInfo2.Controls.Add(flPlayer2);

            // all
            pTableControl.FlowDirection = FlowDirection.TopDown ;
            pTableControl.WrapContents = false;
            pTableControl.AutoScroll = true;
            pTableControl.Controls.Add(pControl);
            pTableControl.Controls.Add(pInfo1);
            pTableControl.Controls.Add(pInfo2);
        }

        public void setup()
        {
            this.Size = new Size(sizeWTablePanelCaro, sizeHTablePanelCaro);
           
            // this.BackColor = Color.Red;
            // pTableCaro.Size = new Size(sizeWTablePanelCaro, sizeHTablePanelCaro);
            //  pTableCaro.BackColor = Color.Red;

            pTableControl.Location = new Point(sizeWTablePanelCaro + 10 , 0);
            pTableControl.Height = Math.Max(sizeHTablePanelCaro , 300);
            forBoard();
            guiControl();
           
        }


        private void click_bClear(Object sender , EventArgs e)
        {
            if(this.over)
                refesh();
        }

        private Point getPoint(MouseEventArgs e)
        {
            int x1 = (int)((e.X - PL) / (SWidthBox + (nextSpace - SWidthBox))); 
            int y2 = (int)((e.Y - PT) / (SWidthBox + (nextSpace - SWidthBox)));
            return new Point(y2 + 1, x1 + 1);
        }

        private bool inRange(Point p)
        {
            return (p.X >= 1 && p.X <= SRow && p.Y >= 1 && p.Y <= SCol);
        }

        private bool inRange(MouseEventArgs e)
        {
            return (e.X >= PL && e.X <= sizeWTablePanelCaro - PL && e.Y >= PT && e.Y <= sizeHTablePanelCaro - PT);
        }

        public void click_handel(Object sender , MouseEventArgs e)
        {

            Point p = getPoint(e);
              Console.WriteLine(getPoint(e).ToString());
           
            if (!inRange(e))
                return;
            
                     
            if (board[p.X , p.Y] > 0 || !this.start)
                return;
          
            entity curentPlayer = (entity)this.curent;
           
            if (histories.Count > 0)
            {
                history idx = histories.Peek();
                int x = idx.p.X;
                int y = idx.p.Y;
                if (board[x, y] == this.curent)
                    return;
            }
            // khi click vao thi khong danh
            switch (curentPlayer)
            {
                case entity.player1:
                   // player1.move(curent);
                   // table.save(entity.player1, ref histories, ref board, ref boardButtons, ref curent);
                    drawText(curentPlayer, p);
                    board[p.X, p.Y] = (int)curentPlayer;
                    histories.Push(new history(p ,(int)curentPlayer));                  
                    break;
                case entity.player2:
                  //  player2.move(curent);
                //table.save(entity.player2, ref histories, ref board, ref boardButtons, ref curent);
                    drawText(curentPlayer, p);
                    board[p.X, p.Y] = (int)curentPlayer;
                    histories.Push(new history(p, (int)curentPlayer));                   
                    break;
                default:
                    MessageBox.Show("curent Player hiện tại chưa được setting!");
                    break;
            }

            lock (this)
            {
                bool iswin = isWin();
                if (iswin)
                {
                    this.start = false;
                    this.over = true;
                    player1.Run = false;
                    player2.Run = false;
                    player2.Play = true;
                    player1.Play = true;
                    this.playerWin = this.curent;
                    Monitor.Pulse(this);
                }

                if (this.start && isOver())
                {
                    this.playerWin = 3;
                    this.start = false;
                    this.over = true;
                    player1.Run = false;
                    player2.Run = false;
                    player2.Play = true;
                    player1.Play = true;
                    Monitor.Pulse(this);
                }
            }
        }

        private void click_bUndo(object sender , EventArgs e)
        {
            if (!this.start || histories.Count == 0 )
                return;
          
            if (histories.Count > 0)
            {
                history idx = histories.Peek();
                int x = idx.p.X;
                int y = idx.p.Y;
                if (board[x,y] != this.curent)
                    return;
            }
        
            entity curentPlayer = (entity)this.curent;
            if (curentPlayer == entity.player1 && player1.Undo == Player.max_undo)
                return;
            if (curentPlayer == entity.player2 && player2.Undo == Player.max_undo)
                return;
            if (curentPlayer == entity.player1)
            {
                player1.Undo++;
                lUnder1.Text = "undo " + player1.Undo;
            }
            else if (curentPlayer == entity.player2)
            {
                player2.Undo++;
                lUnder2.Text = "undo " + player2.Undo;
            }
            table.undo(ref histories,ref board, ref boardButtons);
            /* khi xóa 1 node histories
             * gọi Invalidate() -> onPaint() để draw lại
             */
      
            // this.Invalidate();
             this.BackgroundImage = null;
             
            // this.CreateGraphics().Clear(Color.Blue);
            
            
            this.BackgroundImage = (Bitmap)bm.Clone();  
            
                     
         }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void click_bStart(object sender, EventArgs e)
        {
            if (!this.start && !this.over)
            {
                this.start = true;
                this.over = false;
                runGame();
            }
        }

        bool isWin()
        {
            if (histories.Count < 9)
                return false;

            Graphics g = this.CreateGraphics();

            history arrIdx = histories.Peek();
            int x = arrIdx.p.X;
            int y = arrIdx.p.Y;

            int sz1 = table.roadScoreCrossUpAndDown(x, y, SRow, SCol, ref board, ref boardButtons, this.curent ,ref g).Count;
            int sz2 = table.roadScoreLeftAdnRight(x, y, SRow, SCol, ref board, ref boardButtons, this.curent ,ref g).Count;
            int sz3 = table.roadScoreDm(x, y, SRow, SCol, ref board, ref boardButtons, this.curent , ref g).Count;
            int sz4 = table.roadScoreDs(x, y, SRow, SCol, ref board, ref boardButtons, this.curent , ref g).Count;

            return (sz1 >= 5 || sz2 >= 5 || sz3 >= 5 || sz4 >= 5);
        }

    
        bool isOver()
        {
            return histories.Count == (SCol * SRow);
        }
        

        private Panel pTableCaro;

        public Panel TableCaro
        {
            get { return pTableCaro; }
            set
            {
                if (pTableCaro == null)
                    pTableCaro = new Panel();
            }
        }

        private FlowLayoutPanel pTableControl;

        public FlowLayoutPanel TableControl
        {
            get { return pTableControl; }
            set
            {
                if (pTableControl == null)
                    pTableControl = new FlowLayoutPanel();
            }
        }

        private Panel pControl;
        private Button bStart;
        private Button bClear;
        private Button bUndo;

        private Panel pInfo1;
        private Label lName1;
        private Label lTime1;
        private Label lUnder1;
        private Label lWin1;

        private Panel pInfo2;
        private Label lName2;
        private Label lTime2;
        private Label lUnder2;
        private Label lWin2;

    }
}
