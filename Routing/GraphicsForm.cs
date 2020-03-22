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

namespace Routing
{
    public partial class frm_grid : Form
    {
        private Bitmap grid;
        private Graphics gr;
        private PictureBox pb_grid;
        private Point cursorLocation;
        private Point frameLocation;
        private int SCALE=20;
        private int ROWS;
        private int COLS;
        private const int GRID_WIDTH = 1;
        private Color frameColor = System.Drawing.Color.White;
        private static Random rand=new Random();
        private const int ALINGMENT = 15;
        private List<int> obstruct;
        private List<List<Conductor>> traces;
        private List<System.Drawing.Color> traces_color;
        public frm_grid(int rows, int cols, List<int> obstruct, List<List<Conductor>> traces)
        {
            this.ROWS=rows;
            this.COLS=cols;
            this.obstruct = obstruct;
            cursorLocation = new Point(0, 0);
            frameLocation = new Point(0, 0);
            this.traces = traces;
            traces_color = new List<System.Drawing.Color>();
            for (int i = 0; i < traces.Count; i++)
                traces_color.Add(System.Drawing.Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)));
            InitPictureBox();
            InitPicture();
            InitFrame();
        }

        private void InitFrame()
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(790,650);
        }

        private void InitPictureBox()
        {
            pb_grid = new PictureBox();
            pb_grid.Size = new System.Drawing.Size(630,630);
            pb_grid.BorderStyle = BorderStyle.FixedSingle;
            pb_grid.BackColor = frameColor;
            pb_grid.MouseDown += new MouseEventHandler(pb_grid_MouseDown);
            pb_grid.MouseMove += new MouseEventHandler(pb_grid_MouseMove);
            pb_grid.Paint += new PaintEventHandler(pb_grid_Paint);
            pb_grid.MouseWheel += new MouseEventHandler(pb_grid_MouseWheel);
            pb_grid.Location = new Point(150, 10);
            this.Controls.Add(pb_grid);
        }

        private void InitPicture()
        {
            grid = new Bitmap((COLS - 1) * SCALE + 2 * ALINGMENT, (ROWS - 1) * SCALE + 2 * ALINGMENT);
            gr = Graphics.FromImage(grid);
            DrawGrid();
            DrawObstruct();
            NumerateNodes();
            DrawLines();
        }

        private void DrawGrid()
        {
            Pen gridpen = new Pen(Color.Gray, GRID_WIDTH);
            for (int i = 0; i < COLS; i++)
                gr.DrawLine(gridpen, ALINGMENT + SCALE * i, 0, ALINGMENT + SCALE * i, grid.Height);
            for (int i = 0; i < ROWS; i++)
                gr.DrawLine(gridpen, 0, ALINGMENT + SCALE * i, grid.Width, ALINGMENT + SCALE * i);
            NumerateNodes();
        }

        private void NumerateNodes()
        {
            if (SCALE < 30)
                return;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 7);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            for (int i = 0; i < COLS * ROWS; i++)
                gr.DrawString(i.ToString(), drawFont, drawBrush, (i % COLS) * SCALE, (i / COLS) * SCALE , drawFormat);
        }

        public void DrawLines()
        {
            int numColor = 0;
            Pen NewPen = new Pen(traces_color[0], 3);
            foreach (List<Conductor> trace in traces)
            {
                NewPen.Color = traces_color[numColor];
                foreach (Conductor cond in trace)
                    gr.DrawLine(NewPen, (cond.FirstNode % COLS) * SCALE + ALINGMENT, (cond.FirstNode / COLS) * SCALE + ALINGMENT,
                        (cond.SecondNode % COLS) * SCALE + ALINGMENT, (cond.SecondNode / COLS) * SCALE + ALINGMENT);
                numColor++;
            }
        }

        public void SetObstruct(int upLeft, int downRight)
        {
            int firstX = upLeft % COLS;
            int firstY = upLeft / COLS;
            int secondX = downRight % COLS;
            int secondY = downRight / COLS;
            for (int i = firstY; i <= secondY; i++)
                for (int j = firstX; j <= secondX; j++)
                    obstruct.Add(i * COLS + j);
        }

        private void DrawObstruct()
        {
            foreach (int obs in obstruct)
            {
                int X = obs % COLS;
                int Y = obs / COLS;
                //линия влево
                gr.DrawLine(new Pen(frameColor, GRID_WIDTH), X * SCALE + ALINGMENT, Y * SCALE + ALINGMENT, (X - 1) * SCALE + ALINGMENT + GRID_WIDTH, Y * SCALE + ALINGMENT);
                //линия вправо
                gr.DrawLine(new Pen(frameColor, GRID_WIDTH), X * SCALE + ALINGMENT, Y * SCALE + ALINGMENT, (X + 1) * SCALE + ALINGMENT - GRID_WIDTH, Y * SCALE + ALINGMENT);
                //линия вниз
                gr.DrawLine(new Pen(frameColor, GRID_WIDTH), X * SCALE + ALINGMENT, Y * SCALE + ALINGMENT, X * SCALE + ALINGMENT, (Y + 1) * SCALE + ALINGMENT - GRID_WIDTH);
                //линия вверх
                gr.DrawLine(new Pen(frameColor, GRID_WIDTH), X * SCALE + ALINGMENT, Y * SCALE + ALINGMENT, X * SCALE + ALINGMENT, (Y - 1) * SCALE + ALINGMENT + GRID_WIDTH);
            }
        }

        private void pb_grid_MouseWheel(object sender, MouseEventArgs e)
        {

            if (e.Delta > 0)
                SCALE += 10;
            else
                SCALE -= 10;
                if (SCALE < 10)
                SCALE = 10;
            else
            if (SCALE > 70)
                SCALE = 70;
            else
            {
                grid.Dispose();
                gr.Dispose();
                InitPicture();
                pb_grid.Invalidate();
            }
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
            if (e.Button==MouseButtons.Left)
            {
                int diff_x = e.X-cursorLocation.X;
                int diff_y = e.Y - cursorLocation.Y;
                if ((frameLocation.X+diff_x)<=0 && (frameLocation.X + diff_x)>= pb_grid.Width- grid.Width)
                    frameLocation.X += diff_x;
                if ((frameLocation.Y + diff_y) <= 0 && (frameLocation.Y + diff_y) >= pb_grid.Height - grid.Height)
                    frameLocation.Y += diff_y;
                cursorLocation.X = e.X;
                cursorLocation.Y = e.Y;
                pb_grid.Invalidate();
            }
        }

        private void pb_grid_Paint(object sender, PaintEventArgs e)
        {
            if (grid.Width>pb_grid.Width)
            if (frameLocation.X + grid.Width < pb_grid.Width)
                frameLocation.X = pb_grid.Width - grid.Width;
            if (grid.Height > pb_grid.Height)
                if (frameLocation.Y + grid.Height < pb_grid.Height)
                frameLocation.Y = pb_grid.Height - grid.Height;
            e.Graphics.DrawImage(grid, frameLocation);
        }

        public void FindPinLocation(int pin)
        {
            int col = pin % COLS;
            int row = pin / COLS;
            SCALE = 50;
            Invalidate();
            frameLocation.X = -col * SCALE - ALINGMENT + pb_grid.Width / 2;
            frameLocation.Y = -row * SCALE - ALINGMENT + pb_grid.Height / 2;
            Invalidate();
        }

        private Point RowCol(int n)
        {
            return new Point(n / COLS, n % COLS);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int[]> circuits = new List<int[]>();
            List<int> exist = new List<int>();
            List<int> obstr = new List<int>();
            int range = 100;
            Graph g = new Graph(100, 100);
            Obstruct obs = new Obstruct(g);
            Solver s = new Solver(obs);
            int point = 0;
            int obsPoints = (int)(100 * 100 * 0.1);
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
                s.PinConnect(obs, circ);
            frm_grid fg = new frm_grid(100, 100, obstr, s.GetTrace());
        }
    }
}
