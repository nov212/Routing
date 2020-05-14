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
            int step = 50;
           grid = new Bitmap((COLS - 1) * step , (ROWS - 1) * step );
            gr = Graphics.FromImage(grid);
            drawer = new Drawer(gr);
            gridDrawer = new GridDrawer(pb_grid.Width,pb_grid.Height, drawer);
            Point coord = new Point(0, 0);
            Point coord1 = new Point(0, 0);
            for (int y = 0; y < ROWS; y++)
                for (int x = 0; x < COLS - 1; x++)
                    Lines.Add(new Line(Color.Gray, GRID_WIDTH, x, y, x + 1, y));
            for (int x = 0; x < COLS; x++)
                for (int y = 0; y < ROWS- 1; y++)
                    Lines.Add(new Line(Color.Gray, GRID_WIDTH,  x, y,  x, y+1));
            foreach (int o in obstruct)
            {
                coord.X = o % COLS;
                coord.Y = o / COLS;
                Lines.Add(new Line(frameColor, GRID_WIDTH,coord.X-1, coord.Y,  coord.X,  coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH, coord.X,  coord.Y, coord.X+1,  coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH, coord.X,  coord.Y-1, coord.X,  coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH,coord.X,  coord.Y,  coord.X,  coord.Y+1));
            }

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



        private void button1_Click(object sender, EventArgs e)
        {
            List<int[]> circuits = new List<int[]>();
            List<int> exist = new List<int>();
            List<int> obstr = new List<int>();
            int range = 10;
            Graph g = new Graph(range, range);
            Obstruct obs = new Obstruct(g);
            Solver s = new Solver(obs);
            int point = 0;
            int obsPoints = (int)(range * range * 0.1);
            Random rnd = new Random();
            for (int i = 0; i < obsPoints; i++)
            {
                point = rnd.Next(range * range);
                while (obs[point] == true)
                    point = rnd.Next(range * range);
                obs[point] = true;
                obstr.Add(point);
            }

            for (int i = 0; i < 10; i++)
            {
                int[] circuit = new int[5];
                for (int j = 0; j < 5; j++)
                {
                    point = rnd.Next(obs.GetN());
                    while (obs[point] == true || exist.Contains(point))
                        point = rnd.Next(range * range);
                    exist.Add(point);
                    circuit[j] = point;
                }
                circuits.Add(circuit);
            }
            foreach (int[] circ in circuits)
            {
                foreach (int n in circ)
                    System.Console.Write("{0} ", n);
                System.Console.WriteLine();
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            s.FindTrace(obs, circuits);
            sw.Stop();
            Console.WriteLine($"Time {sw.ElapsedMilliseconds}");
            InitPicture(range, range, obstr, s.GetTrace());
            Repaint();
        }
    }
}
