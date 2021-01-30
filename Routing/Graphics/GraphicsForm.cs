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
            int range = 10000;
            IGraph obs = new Obstruct(new Graph(range, range));
            IPolygon rect1 = new Rectangle(6000, 0, 10000, 4000);
            IPolygon rect2 = new Rectangle(2000, 0, 4000, 3000);
            IPolygon rect3 = new Rectangle(0, 4000, 5000, 7000);
            IPolygon rect4 = new Rectangle(7000, 5000, 10000, 10000);
            IPolygon union = new UnionPolygon();
            union.Add(rect1).Add(rect2).Add(rect3).Add(rect4);
            PolygonGraph polygonGraph = new PolygonGraph(obs);
            polygonGraph.RouteOn(union);
            int[] circ1 =
            {
                polygonGraph.ToNum(6250, 250,0),
                polygonGraph.ToNum(7750, 250,0),
                polygonGraph.ToNum(6750, 1250,0),
                polygonGraph.ToNum(9000, 500,0),
                polygonGraph.ToNum(9750, 250,0)
            };
            int[] circ2 =
            {
                polygonGraph.ToNum(6250, 2000,0),
                polygonGraph.ToNum(7250, 1750,0),
                polygonGraph.ToNum(8250, 1750,0),
                polygonGraph.ToNum(7500, 1000,0),
                polygonGraph.ToNum(8250, 1250,0)
            };
            int[] circ3 =
            {
                polygonGraph.ToNum(6500, 3500,0),
                polygonGraph.ToNum(6500, 2000,0),
                polygonGraph.ToNum(7500, 3500,0),
                polygonGraph.ToNum(7250, 2750,0),
                polygonGraph.ToNum(7750, 2750,0)
            };
            int[] circ4 =
            {
                polygonGraph.ToNum(7250, 500,0),
                polygonGraph.ToNum(8750, 1500,0),
                polygonGraph.ToNum(8750, 2750,0),
                polygonGraph.ToNum(8000, 2250,0),
                polygonGraph.ToNum(7000, 2250,0)
            };
            int[] circ5 =
            {
                polygonGraph.ToNum(9750, 1000,0),
                polygonGraph.ToNum(9750, 3750,0),
                polygonGraph.ToNum(9250, 2500,0),
                polygonGraph.ToNum(8000, 3750,0),
                polygonGraph.ToNum(8750, 3250,0)
            };
            int[] circ6 =
            {
                polygonGraph.ToNum(2400, 400,0),
                polygonGraph.ToNum(3800, 400,0),
                polygonGraph.ToNum(3800, 2800,0),
                polygonGraph.ToNum(3400, 2800,0),
                polygonGraph.ToNum(3600, 2000,0)
            };
            int[] circ7 =
            {
                polygonGraph.ToNum(2200, 0,0),
                polygonGraph.ToNum(2200, 800,0),
                polygonGraph.ToNum(3000, 800,0),
                polygonGraph.ToNum(2600, 1600,0),
                polygonGraph.ToNum(2400, 1200,0)
            };
            int[] circ8 =
            {
                polygonGraph.ToNum(2200, 2800,0),
                polygonGraph.ToNum(2800, 3000,0),
                polygonGraph.ToNum(3400, 3000,0),
                polygonGraph.ToNum(4000, 2400,0),
                polygonGraph.ToNum(2600, 2400,0)
            };
            int[] circ9 =
          {
                polygonGraph.ToNum(2600, 600,0),
                polygonGraph.ToNum(2600, 1800,0),
                polygonGraph.ToNum(2800, 1800,0),
                polygonGraph.ToNum(2200, 1600,0),
                polygonGraph.ToNum(3200, 1200,0)
            };
            int[] circ10 =
          {
                polygonGraph.ToNum(2200, 2600,0),
                polygonGraph.ToNum(2400, 2000,0),
                polygonGraph.ToNum(3400, 2400,0),
                polygonGraph.ToNum(2600, 2200,0),
                polygonGraph.ToNum(3000, 2800,0)
            };
            int[] circ11 =
        {
                polygonGraph.ToNum(400, 4600,0),
                polygonGraph.ToNum(2400, 4600,0),
                polygonGraph.ToNum(1200, 5400,0),
                polygonGraph.ToNum(2400, 5400,0),
                polygonGraph.ToNum(3200, 5000,0)
            };
            int[] circ12 =
        {
                polygonGraph.ToNum(600, 5600,0),
                polygonGraph.ToNum(1400, 4200,0),
                polygonGraph.ToNum(1800, 5600,0),
                polygonGraph.ToNum(1800, 4800,0),
                polygonGraph.ToNum(800, 6400,0)
            };
            int[] circ13 =
        {
                polygonGraph.ToNum(4600, 4800,0),
                polygonGraph.ToNum(3200, 4600,0),
                polygonGraph.ToNum(1600, 6000,0),
                polygonGraph.ToNum(2400, 6200,0),
                polygonGraph.ToNum(4600, 6200,0)
            };
            int[] circ14 =
        {
                polygonGraph.ToNum(4200, 4400,0),
                polygonGraph.ToNum(4200, 6400,0),
                polygonGraph.ToNum(4800, 5600,0),
                polygonGraph.ToNum(4000, 5800,0),
                polygonGraph.ToNum(2800, 5800,0)
            };

            int[] circ15 =
    {
                polygonGraph.ToNum(400, 6200,0),
                polygonGraph.ToNum(1600, 6800,0),
                polygonGraph.ToNum(4400, 6800,0),
                polygonGraph.ToNum(2800, 6400,0),
                polygonGraph.ToNum(3600, 6000,0)
            };

            int[] circ16 =
    {
                polygonGraph.ToNum(7600, 6000,0),
                polygonGraph.ToNum(9000, 6000,0),
                polygonGraph.ToNum(7600, 7600,0),
                polygonGraph.ToNum(8200, 8000,0),
                polygonGraph.ToNum(8600, 7000,0)
            };
            int[] circ17 =
    {
                polygonGraph.ToNum(7200, 5200,0),
                polygonGraph.ToNum(7200, 6200,0),
                polygonGraph.ToNum(8800, 5200,0),
                polygonGraph.ToNum(8400, 6400,0),
                polygonGraph.ToNum(8000, 5800,0)
            };

            int[] circ18 =
    {
                polygonGraph.ToNum(9600, 5800,0),
                polygonGraph.ToNum(7800, 6600,0),
                polygonGraph.ToNum(9200, 7000,0),
                polygonGraph.ToNum(8000, 7600,0),
                polygonGraph.ToNum(8800, 8400,0)
            };

            int[] circ19 =
    {
                polygonGraph.ToNum(7200, 9800,0),
                polygonGraph.ToNum(9800, 9800,0),
                polygonGraph.ToNum(9800, 8200,0),
                polygonGraph.ToNum(8200, 9200,0),
                polygonGraph.ToNum(9400, 9000,0)
            };
            int[] circ20 =
{
                polygonGraph.ToNum(7200, 7000,0),
                polygonGraph.ToNum(7200, 9200,0),
                polygonGraph.ToNum(8200, 8600,0),
                polygonGraph.ToNum(9800, 6400,0),
                polygonGraph.ToNum(9200, 9400,0)
            };

            List<int[]> circuits = new List<int[]>
            {
                circ1,
                circ2,
                circ3,
                circ4,
                circ5,
                circ6,
                circ7,
                circ8,
                circ9,
                circ10,
                circ11,
                circ12,
                circ13,
                circ14,
                circ15,
                circ16,
                circ17,
                circ18,
                circ19,
                circ20,
            };
            Solver s = new Solver(polygonGraph);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            s.FindTrace(polygonGraph, circuits);
            sw.Stop();
            Console.WriteLine("Время: " + sw.ElapsedMilliseconds);
        }
    }
}
