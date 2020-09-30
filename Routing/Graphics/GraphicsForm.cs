using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Routing
{
    public partial class frm_grid : Form
    {
        private Bitmap grid;
        private PictureBox pb_grid;
        private Point cursorLocation;
        private Graphics gr;
        private int ROWS;
        private int COLS;
        private const int GRID_WIDTH = 1;
        private Color frameColor = System.Drawing.Color.White;
        private static Random rand=new Random();
        private const int BORDER = 15;
        private GridDrawer gridDrawer;
        private Drawer drawer;
        private List<Line> Lines = new List<Line>();

        struct Line
        {
            public int x1;
            public int y1;
            public int x2;
            public int y2;
            public Color color;
            public int width;
            public Line(Color color, int width, int x1, int y1, int x2, int y2)
            {
                this.color = color;
                this.width = width;
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
            }
        }

        public frm_grid()
        {
            InitPictureBox();
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(790, 650);
            //grid = new Bitmap(630, 630);
        }

        private void InitPictureBox()
        {
            pb_grid = new PictureBox();
            pb_grid.Size =new Size(630,630);
            pb_grid.BorderStyle = BorderStyle.FixedSingle;
            pb_grid.BackColor = frameColor;
            pb_grid.Location = new Point(150, 10);
            this.Controls.Add(pb_grid);
        }


        private void InitPicture(int rows, int cols, List<int> obstruct, List<List<Conductor>> traces)
        {
            this.ROWS = rows;
            this.COLS = cols;
            cursorLocation = new Point(0, 0);
            pb_grid.MouseDown += new MouseEventHandler(pb_grid_MouseDown);
            pb_grid.MouseMove += new MouseEventHandler(pb_grid_MouseMove);
            pb_grid.MouseWheel += new MouseEventHandler(pb_grid_MouseWheel);
            // pb_grid.Paint += new PaintEventHandler(pb_grid_Paint);

            //шаг сетки
            int step = 50;

            //битмап для сетки
           grid = new Bitmap((COLS - 1) * step , (ROWS - 1) * step );
            gr = Graphics.FromImage(grid);

            //gridDrawer - инструмкнт для рисования сетки
            drawer = new Drawer(gr);
            gridDrawer = new GridDrawer(pb_grid.Width,pb_grid.Height, drawer);

            Point coord = new Point(0, 0);
            Point coord1 = new Point(0, 0);

            //создание горизонтальных линий для сетки
            for (int y = 0; y < ROWS; y++)
                for (int x = 0; x < COLS - 1; x++)
                    Lines.Add(new Line(Color.Gray, GRID_WIDTH, x, y, x + 1, y));

            //создание вертикальных линий для сетки
            for (int x = 0; x < COLS; x++)
                for (int y = 0; y < ROWS- 1; y++)
                    Lines.Add(new Line(Color.Gray, GRID_WIDTH,  x, y,  x, y+1));

            //создание препятствий
            foreach (int o in obstruct)
            {
                coord.X = o % COLS;
                coord.Y = o / COLS;
                Lines.Add(new Line(frameColor, GRID_WIDTH,coord.X-1, coord.Y,  coord.X,  coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH, coord.X,  coord.Y, coord.X+1,  coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH, coord.X,  coord.Y-1, coord.X,  coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH,coord.X,  coord.Y,  coord.X,  coord.Y+1));
            }

            //создание цветных линий для прорисовки построенных трасс
            foreach (var trace in traces)
            {
                Color trace_color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                foreach (var cond in trace)
                {
                    coord.X = cond.FirstNode % COLS;
                    coord.Y = cond.FirstNode / COLS;
                    coord1.X = cond.SecondNode % COLS;
                    coord1.Y = cond.SecondNode / COLS;
                    Lines.Add(new Line(trace_color, 3,coord.X, coord.Y, coord1.X, coord1.Y));
                }
            }
        }

        private void Repaint()
        {
            Pen pen = new Pen(Color.Gray, 1);
            gridDrawer.Clear(frameColor);
            foreach (Line line in Lines)
            {
                pen.Color = line.color;
                pen.Width = line.width;
                gridDrawer.DrawLine(pen, line.x1, line.y1, line.x2, line.y2);
            }
            if (gridDrawer.Scale > 20)
                NumerateNodes();
            pb_grid.Image = grid;
        }


        private void NumerateNodes()
        {
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 7);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            for (int i = 0; i < ROWS*COLS; i++)
                gridDrawer.DrawString(i.ToString(), drawFont, drawBrush, i % COLS, i / COLS);
        }

        private void pb_grid_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                gridDrawer.Scale += 1;
            else
                gridDrawer.Scale -= 1;
            Repaint();
        }

        private void pb_grid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                cursorLocation.X = e.X;
                cursorLocation.Y = e.Y;
            }
        }

        private void pb_grid_MouseMove(object sender, MouseEventArgs e)
        {
                if (e.Button == MouseButtons.Left)
                {
                int diff_x = e.X - cursorLocation.X;
                int diff_y = e.Y - cursorLocation.Y;
                gridDrawer.Move(gridDrawer.Offset.X + diff_x, gridDrawer.Offset.Y + diff_y);
                    cursorLocation.X = e.X;
                    cursorLocation.Y = e.Y;
                Repaint();
                //pb_grid.Invalidate();
                }
        }

        //private void pb_grid_Paint(object sender, PaintEventArgs e)
        //{
        //    if (grid != null)
        //        e.Graphics.DrawImage(grid, gridDrawer.Offset);
        //}

        private Obstruct GenerateTestField(int range, double percentage)
        {
            Graph g = new Graph(range, range);
            Obstruct obs = new Obstruct(g);
            Random rnd = new Random();
            int amount = g.GetN();
            int count = (int)(amount * percentage);
            int obstacle = 0;

            for (int i = 0; i < count; i++)
            {
                do
                {
                    obstacle = rnd.Next(amount);
                } while (obs[obstacle] == true);
                obs[obstacle] = true;
            }
            return obs;
        }

        private List<int> GenerateObstruct(Obstruct obs, double percentage)
        {
            List<int> obstructs = new List<int>();
            Random rnd = new Random();
            int amount = obs.GetN();
            int obstacle = 0;
            int count = (int)(amount * percentage);
            for (int i = 0; i < count; i++)
            {
                do
                {
                    obstacle = rnd.Next(amount);
                } while (obs[obstacle] == true);
                obs[obstacle] = true;
                obstructs.Add(obstacle);
            }
            return obstructs;
        }

        private List<int[]> GenerateTrace (Obstruct g, int circ_count, int pins)
        {
            PerPut pp = new PerPut(g.GetN());
            Random rnd = new Random();
            List<int[]> circuits = new List<int[]>();
            int[] circ = null;
            int pin = 0;
            //цикл для генерации всех цепей
            for (int j = 0; j < circ_count; j++)
            {
                circ = new int[pins];
                //цикл для генерации контактов одной цепи
                for (int i = 0; i < pins; i++)
                {
                    do
                    {
                        pin = rnd.Next(g.GetN());
                    } while (g[pin] == true || pp.ContainLeft(pin));
                    circ[i] = pin;
                    pp.MoveLeft(pin);
                }
                circuits.Add(circ);
            }
            return circuits;
        }



        private void button1_Click(object sender, EventArgs e)
        {
           
            int range = 10;
            Graph g = new Graph(5, 6);
            Obstruct obs = new Obstruct(g);
            obs[20] = true;
            obs[21] = true;
            obs[15] = true;
            obs[9] = true;
            obs[3] = true;
            obs[16] = true;
            List<int> obstr = GenerateObstruct(obs, 0.1);
            Solver s1 = new Solver(g);
            //List<List<int>> sol = s1.FindPathOnSubgraph(g, new List<int[]> { new int[]{ 32, 27, 55, 68, 84 }, new int[] { 62, 87 } }, new int[] { 1, 2, 3 });
            List<List<int>> sol = s1.FindTrace(obs, new List<int[]> { new int[] { 8, 17}});
            foreach (var h in sol)
                foreach (int q in h)
                    Console.WriteLine($"{q} ");
            ////Console.WriteLine("OBSTRUCTS");
            ////foreach (int n in obstr)
            ////    Console.Write($"{n} ");
            ////Console.WriteLine();
            //foreach (int n in obstr)
            //    obs[n] = true;
            //List<int[]> circuits = GenerateTrace(obs, 10, 5);
            ////foreach (var trace in circuits)
            ////{
            ////    Console.WriteLine("GENERATED");
            ////    foreach (int n in trace)
            ////        Console.Write($"{n} ");
            ////    Console.WriteLine();
            ////}
            //Solver s = new Solver(obs);

            //Stopwatch sw = new Stopwatch();

            //sw.Start();
            ////List<List<Conductor>> CondTrace = s.FindPathOnSubgraph(obs, circuits, new int[] { 1000 });
            //sw.Stop();
            //Console.WriteLine($"Time {sw.ElapsedMilliseconds}");


            ////sw.Restart();
            ////s.FindTrace(obs, circuits);
            ////sw.Stop();
            ////Console.WriteLine($"Time {sw.ElapsedMilliseconds}");
            //foreach (var err in s.GetFailReport())
            //{
            //    if (err.Value != null)
            //        Console.WriteLine($"{err.Key} {err.Value.Count()}");
            //}

            ////InitPicture(range, range, obstr, CondTrace);
            ////Repaint();
        }
    }
}
