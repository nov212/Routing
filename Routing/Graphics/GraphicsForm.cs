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
        private static Random rand = new Random();
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
            pb_grid.Size = new Size(630, 630);
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
            grid = new Bitmap((COLS - 1) * step, (ROWS - 1) * step);
            gr = Graphics.FromImage(grid);

            //gridDrawer - инструмкнт для рисования сетки
            drawer = new Drawer(gr);
            gridDrawer = new GridDrawer(pb_grid.Width, pb_grid.Height, drawer);

            Point coord = new Point(0, 0);
            Point coord1 = new Point(0, 0);

            //создание горизонтальных линий для сетки
            for (int y = 0; y < ROWS; y++)
                for (int x = 0; x < COLS - 1; x++)
                    Lines.Add(new Line(Color.Gray, GRID_WIDTH, x, y, x + 1, y));

            //создание вертикальных линий для сетки
            for (int x = 0; x < COLS; x++)
                for (int y = 0; y < ROWS - 1; y++)
                    Lines.Add(new Line(Color.Gray, GRID_WIDTH, x, y, x, y + 1));

            //создание препятствий
            foreach (int o in obstruct)
            {
                coord.X = o % COLS;
                coord.Y = o / COLS;
                Lines.Add(new Line(frameColor, GRID_WIDTH, coord.X - 1, coord.Y, coord.X, coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH, coord.X, coord.Y, coord.X + 1, coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH, coord.X, coord.Y - 1, coord.X, coord.Y));
                Lines.Add(new Line(frameColor, GRID_WIDTH, coord.X, coord.Y, coord.X, coord.Y + 1));
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
                    Lines.Add(new Line(trace_color, 3, coord.X, coord.Y, coord1.X, coord1.Y));
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
            for (int i = 0; i < ROWS * COLS; i++)
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
            if (e.Button == MouseButtons.Left)
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
            //int range = 10000;
            //bool[] config = new bool[range * range];
            //GenerateCircuitsOnRect(config, new Graph(range, range), 1, 3, 9000, 7500, 9500, 8000);
            //GenerateCircuitsOnRect(config, new Graph(range, range), 1, 4, 7000, 7500, 7500, 8000);
            //GenerateCircuitsOnRect(config, new Graph(range, range), 1, 3, 4000, 6000, 4500, 6500);
            Test5();
        }

        static IGraph GenerateGragh(int range, int obstructs, bool[] config)
        {
            Obstruct obs = new Obstruct(new Graph(range, range));
            Random r = new Random();
            int index = 0;
            int iter = obstructs;
            while (iter > 0)
            {
                index = r.Next(0, range * range);
                if (config[index] == false)
                {
                    config[index] = true;
                    obs[index] = true;
                    iter--;
                }
            }
            return obs;
        }

        static List<int[]> GenerateCircuitsOnRect(bool[] config, IGraph g, int circCount, int pinCount, int startRow, int startCol, int endRow, int endCol)
        {
            Random r = new Random();
            int cc = circCount;
            int pc = pinCount;
            int index;
            int row = 0;
            int col = 0;
            List<int[]> circuits = new List<int[]>();
            for (int i = 0; i < circCount; i++)
            {
                int[] circuit = new int[pinCount];
                pc = pinCount;
                while (pc > 0)
                {
                    row = r.Next(startRow, endRow + 1);
                    col = r.Next(startCol, endCol + 1);
                    index = g.ToNum(row, col, 0);
                    if (config[index] == false)
                    {
                        config[index] = true;
                        Console.WriteLine(g.GetRow(index) + ", " + g.GetCol(index));
                        circuit[pc - 1] = index;
                        pc--;
                    }
                }
                circuits.Add(circuit);
            }
            return circuits;
        }

        private void ManualTest()
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
            IGraph justGraph = new Graph(range, range);
            Solver s = new Solver(polygonGraph);
            Solver s1 = new Solver(justGraph);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            s.FindTrace(justGraph, circuits);
            sw.Stop();
            Console.WriteLine("Время для декоратора: " + sw.ElapsedMilliseconds);
            sw.Restart();
            s1.FindTrace(justGraph, circuits);
            sw.Stop();
            Console.WriteLine("Время для обычного метода: " + sw.ElapsedMilliseconds);
        }

        private void Test2()
        {
            int range = 10000;
            IGraph anotherJustGraph = new Graph(10000, 10000);
            Rectangle rectangle = new Rectangle(0, 0, 999, 999);
            Rectangle rectangle1 = new Rectangle(9000, 0, 9999, 999);
            Rectangle rectangle2 = new Rectangle(0, 9000, 999, 9999);
            Rectangle rectangle3 = new Rectangle(9000, 9000, 9999, 9999);
            Rectangle rectangle4 = new Rectangle(1000, 1000, 8999, 8999);

            UnionPolygon anotherUP = new UnionPolygon();
            anotherUP.Add(rectangle).Add(rectangle1).Add(rectangle2).Add(rectangle3).Add(rectangle4);
            PolygonGraph pg = new PolygonGraph(anotherJustGraph);
            pg.RouteOn(anotherUP);

            bool[] config = new bool[range * range];

            List<int[]> circ = GenerateCircuitsOnRect(config, anotherJustGraph, 1, 20, 1000, 1000, 8999, 8999);
            foreach (var t in circ)
            {
                foreach (int n in t)
                    Console.WriteLine(n + "  ");
                //Console.WriteLine();
            }

            Solver s1 = new Solver(anotherJustGraph);
            //Solver s2 = new Solver(pg);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            s1.FindTrace(pg, circ);
            sw.Stop();
            Console.WriteLine("Время для декоратора: " + sw.ElapsedMilliseconds);
            //sw.Start();
            //s2.FindTrace(anotherJustGraph, circ);
            //sw.Stop();
            //Console.WriteLine("Время для обычного метода: " + sw.ElapsedMilliseconds);
        }

        private void Test3()
        {
            bool[] config = new bool[10000 * 10000];
            IGraph testGraph = GenerateGragh(10000, 0, config);
            List<int[]> c1 = GenerateCircuitsOnRect(config, testGraph, 10, 5, 0, 0, 2000, 4000);
            List<int[]> c2 = GenerateCircuitsOnRect(config, testGraph, 10, 5, 4000, 500, 4999, 3500);
            List<int[]> c = c1.Concat(c2).ToList();


            Rectangle r1 = new Rectangle(0, 0, 2000, 4000);
            Rectangle r2 = new Rectangle(4000, 500, 4999, 3500);
            UnionPolygon unionPolygon = new UnionPolygon();
            unionPolygon.Add(r1).Add(r2);
            PolygonGraph pg = new PolygonGraph(testGraph);
            pg.RouteOn(unionPolygon);

            foreach (var t in c)
            {
                foreach (int n in t)
                    Console.Write(n + "  ");
                Console.WriteLine();
            }
            Stopwatch sw = new Stopwatch();
            Solver s1 = new Solver(testGraph);
            Solver s2 = new Solver(pg);
            sw.Start();
            s1.FindTrace(testGraph, c);
            sw.Stop();
            Console.WriteLine("Время: " + sw.ElapsedMilliseconds);
            sw.Start();
            s2.FindTrace(pg, c);
            sw.Stop();
            Console.WriteLine("Время: " + sw.ElapsedMilliseconds);
        }

        private void Test4()
        {
            int range = 10000;
            IGraph simpleGraph = new Graph(range, range);
            Rectangle r1 = new Rectangle(2000, 0, 5000, range - 1);
            Rectangle r2 = new Rectangle(5000, 3000, range-1, 6000);
            UnionPolygon up = new UnionPolygon();
            up.Add(r1).Add(r2);
            int[] circ1 =
            {
                simpleGraph.ToNum(2100, 100,0),
                simpleGraph.ToNum(3000, 50, 0),
                simpleGraph.ToNum(4000, 340,0),
                simpleGraph.ToNum(3100, 8000, 0),
                simpleGraph.ToNum(4000, 9000,0),
                simpleGraph.ToNum(5000, 7000,0),
                simpleGraph.ToNum(2000, 1000,0),
                simpleGraph.ToNum(2000, 1500,0),
                simpleGraph.ToNum(2000, 3000,0),
            };

            int[] circ2 =
            {
                simpleGraph.ToNum(2100, 8000,0),
                simpleGraph.ToNum(2900, 8000, 0),
                simpleGraph.ToNum(4900, 8000,0),
                simpleGraph.ToNum(6000, 5900, 0),
                simpleGraph.ToNum(7100, 5900,0),
                simpleGraph.ToNum(9000, 5900,0),
                simpleGraph.ToNum(2100, 6000,0),
                simpleGraph.ToNum(2100, 6500,0),
                simpleGraph.ToNum(2100, 7000,0),
            };

            List<int[]> circuits = new List<int[]> { circ1, circ2 };
            PolygonGraph pg = new PolygonGraph(simpleGraph);
            pg.RouteOn(up);
            Solver s1 = new Solver(pg);
            Solver s2 = new Solver(simpleGraph);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            s1.FindTrace(pg, circuits);
            sw.Stop();
            Console.WriteLine("Время для декоратора: " + sw.ElapsedMilliseconds);
            sw.Start();
            s2.FindTrace(simpleGraph, circuits);
            sw.Stop();
            Console.WriteLine("Время для обычного метода: " + sw.ElapsedMilliseconds);
        }

        void Test5()
        {
            int range = 10000;
            bool[] config = new bool[range * range];
            IGraph simpleGraph = new Graph(range, range);
            Rectangle r11 = new Rectangle(500, 500, 7000, 1500);
            Rectangle r12 = new Rectangle(6000, 500, 7000, 4500);

            Rectangle r21 = new Rectangle(2000, 3000, 5000, 4000);

            Rectangle r31 = new Rectangle(0, 8500, 4500, 9500);
            Rectangle r32 = new Rectangle(3500, 7000, 4500, 9500);

            Rectangle r41 = new Rectangle(2500, 4500, 8500, 5500);
            Rectangle r42 = new Rectangle(7500, 2000, 8500, 5500);

            Rectangle r51 = new Rectangle(500, 2000, 3500, 2500);
            Rectangle r52 = new Rectangle(500, 2500, 1000, 3000);
            Rectangle r53 = new Rectangle(1500, 2000, 2000, 6500);
            Rectangle r54 = new Rectangle(1500, 6000, 3500, 6500);

            Rectangle r61 = new Rectangle(9000, 1000, 9500, 7000);
            Rectangle r62 = new Rectangle(6000, 6500, 9500, 7000);
            Rectangle r63 = new Rectangle(8000, 7000, 8500, 8500);

            Rectangle r71 = new Rectangle(4000, 6000, 5500, 6500);
            Rectangle r72 = new Rectangle(5000, 6000, 5500, 9500);
            Rectangle r73 = new Rectangle(5000, 9000, 9500, 9500);
            Rectangle r74 = new Rectangle(9000, 7500, 9500, 9500);
            Rectangle r75 = new Rectangle(7000, 7500, 7500, 9500);

            UnionPolygon up = new UnionPolygon();
            up.Add(r11).Add(r12);
            up.Add(r21);
            up.Add(r31).Add(r32);
            up.Add(r41).Add(r42);
            up.Add(r51).Add(r52).Add(r53).Add(r54);
            up.Add(r61).Add(r62).Add(r63);
            up.Add(r71).Add(r72).Add(r73).Add(r74).Add(r75);
            int[] circ1 =
            {
                simpleGraph.ToNum(510, 510,0),
                simpleGraph.ToNum(600, 1000, 0),
                simpleGraph.ToNum(550, 900,0), 
                simpleGraph.ToNum(1100, 1400, 0),
                simpleGraph.ToNum(890, 650,0),
                simpleGraph.ToNum(6050, 3500,0),
                simpleGraph.ToNum(6700, 4000,0),
                simpleGraph.ToNum(6350, 3910,0),
                simpleGraph.ToNum(6900, 2500,0),
                simpleGraph.ToNum(6600, 4100,0),
            };

            int[] circ2 =
            {
                simpleGraph.ToNum(2992, 3319,0),
                simpleGraph.ToNum(2867, 3824, 0),
                simpleGraph.ToNum(2702, 3219,0),
                simpleGraph.ToNum(2558, 3106, 0),
                simpleGraph.ToNum(2697, 3470,0),
                simpleGraph.ToNum(4373, 3501,0),
                simpleGraph.ToNum(4662, 3067,0),
                simpleGraph.ToNum(4309, 3678,0),
                simpleGraph.ToNum(4704, 3449,0),
                simpleGraph.ToNum(4776, 3279,0),
            };

            int[] circ3 =
            {

                simpleGraph.ToNum(550, 9255,0),
                simpleGraph.ToNum(373, 9183, 0),
                simpleGraph.ToNum(969, 8640,0),
                simpleGraph.ToNum(525, 8649, 0),
                simpleGraph.ToNum(478, 9448,0),
                simpleGraph.ToNum(4050, 7755,0),
                simpleGraph.ToNum(3873, 7683,0),
                simpleGraph.ToNum(4469, 7140,0),
                simpleGraph.ToNum(4025, 7149,0),
                simpleGraph.ToNum(3978, 7948,0),
            };

            int[] circ4 =
            {
                simpleGraph.ToNum(2537, 4887,0),
                simpleGraph.ToNum(3181, 4777, 0),
                simpleGraph.ToNum(2844, 4648,0),
                simpleGraph.ToNum(2763, 4519, 0),
                simpleGraph.ToNum(2572, 5228,0),
                simpleGraph.ToNum(7537, 2387,0),
                simpleGraph.ToNum( 8181, 2277,0),
                simpleGraph.ToNum(7844, 2148,0),
                simpleGraph.ToNum(7763, 2019,0),
                simpleGraph.ToNum(7572, 2728,0),
            };

            int[] circ5 =
            {
                simpleGraph.ToNum(3472, 2492,0),
                simpleGraph.ToNum(3471, 2342, 0),
                simpleGraph.ToNum(972, 2992,0),
                simpleGraph.ToNum(971, 2842, 0),
                simpleGraph.ToNum(858, 2595,0),
                simpleGraph.ToNum(802, 2657,0),
                simpleGraph.ToNum(3358, 2095,0),
                simpleGraph.ToNum(3472, 6492,0),
                simpleGraph.ToNum(3471, 6342,0),
                simpleGraph.ToNum(3358, 6095,0),
            };

            int[] circ6 =
           {
                simpleGraph.ToNum(9306, 1487,0),
                simpleGraph.ToNum(9049, 1339, 0),
                simpleGraph.ToNum(6306, 6987,0),
                simpleGraph.ToNum(9170, 1022, 0),
                simpleGraph.ToNum(6049, 6839,0),
                simpleGraph.ToNum(6170, 6522,0),
                simpleGraph.ToNum(6471, 6566,0),
                simpleGraph.ToNum(8306, 8487,0),
                simpleGraph.ToNum(8049, 8339,0),
                simpleGraph.ToNum(8170, 8022,0),
            };

            int[] circ7 =
           {
                simpleGraph.ToNum(9089, 7817,0),
                simpleGraph.ToNum(9242, 7781, 0),
                simpleGraph.ToNum(7089, 7817,0),
                simpleGraph.ToNum(9250, 7565, 0),
                simpleGraph.ToNum(7242, 7781,0),
                simpleGraph.ToNum(7250, 7565,0),
                simpleGraph.ToNum(7481, 7636,0),
                simpleGraph.ToNum(4089, 6317,0),
                simpleGraph.ToNum(4242, 6281,0),
                simpleGraph.ToNum(4250, 6065,0),
            };

            List<int[]> circuits = new List<int[]> {circ1, circ2, circ3, circ4, circ5, circ6, circ7 };

            foreach (var circ in circuits)
                foreach (int n in circ)
                    config[n] = true;
            simpleGraph = GenerateGragh(range, 50/100 * range * range, config);

            PolygonGraph pg = new PolygonGraph(simpleGraph);
            pg.RouteOn(up);
            Solver s1 = new Solver(pg);
            Solver s2 = new Solver(simpleGraph);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            s1.FindTrace(pg, circuits);
            sw.Stop();
            Console.WriteLine("Время для декоратора: " + sw.ElapsedMilliseconds);
            
            sw.Start();
            s2.FindTrace(simpleGraph, circuits);
            sw.Stop();
            Console.WriteLine("Время для обычного метода: " + sw.ElapsedMilliseconds);
            Console.WriteLine("Нереализованные цепи: " + s2.GetFailReport().Count);
        }



    }
}
