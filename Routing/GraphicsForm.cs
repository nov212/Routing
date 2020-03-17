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
        private Bitmap bmp;
        private Graphics gr;
        private PictureBox pb_grid;
        private int frameLocation_x = 0;
        private int frameLocation_y = 0;
        private int cursor_x=0;
        private int cursor_y=0;
        private Bitmap sourcePicture;
        private Graphics sourcegr;
        private int SCALE;
        private int ROWS;
        private int COLS;
        private const int GRID_WIDTH = 1;
        private Color frameColor = System.Drawing.Color.White;
        private static Random rand=new Random();
        private const int ALINGMENT = 15;
        private List<int> obstruct;
        public frm_grid(int rows, int cols, int scale)
        {
            this.ROWS=rows;
            this.COLS=cols;
            this.SCALE = scale;
            obstruct = new List<int>();
            InitPictureBox();
            InitFrame();
        }

        private void InitFrame()
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(500,500);
            this.MouseWheel += new MouseEventHandler(frm_grid_MouseWheel);
        }

        private void InitPictureBox()
        {
            pb_grid = new PictureBox();
            pb_grid.Size = new System.Drawing.Size(500,500);
            pb_grid.BorderStyle = BorderStyle.FixedSingle;
            pb_grid.BackColor = frameColor;
            pb_grid.MouseDown += new MouseEventHandler(frm_grid_MouseDown);
            pb_grid.MouseMove += new MouseEventHandler(frm_grid_MouseMove);
            pb_grid.Paint += new PaintEventHandler(pb_grid_Paint);
            this.Controls.Add(pb_grid);
        }

        private void InitPicture()
        {
            bmp = new Bitmap((COLS - 1) * SCALE + 2 * ALINGMENT, (ROWS - 1) * SCALE + 2 * ALINGMENT);
            gr = Graphics.FromImage(bmp);
            Pen gridpen = new Pen(Color.Gray, GRID_WIDTH);
            for (int i = 0; i < COLS; i++)
                gr.DrawLine(gridpen, ALINGMENT + SCALE * i, 0, ALINGMENT + SCALE * i, bmp.Height);
            for (int i = 0; i < ROWS; i++)
                gr.DrawLine(gridpen, 0, ALINGMENT + SCALE * i, bmp.Width, ALINGMENT + SCALE * i);
            NumerateNodes();
            DrawObstruct();
            //pb_grid.Image = bmp;
        }

        //private void DrawGrid()
        //{
        //    Pen gridpen = new Pen(Color.Gray, GRID_WIDTH);
        //    for (int i = 0; i < COLS; i++)
        //       gr.DrawLine(gridpen, ALINGMENT + SCALE * i, 0, ALINGMENT + SCALE * i, bmp.Height);
        //    for (int i = 0; i < ROWS; i++)
        //       gr.DrawLine(gridpen, 0, ALINGMENT + SCALE * i, bmp.Width, ALINGMENT + SCALE * i);
        //    NumerateNodes();
        //    DrawObstruct();
        //    pb_grid.Image = bmp;
        //}

        private void NumerateNodes()
        {
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 7);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            for (int i = 0; i < COLS * ROWS; i++)
                gr.DrawString(i.ToString(), drawFont, drawBrush, (i % COLS) * SCALE, (i / COLS) * SCALE , drawFormat);
        }

        public void DrawLines(List<Conductor> obs)
        {
                System.Drawing.Color color = System.Drawing.Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                Pen NewPen = new Pen(color, 3);
                foreach (Conductor cond in obs)
                    gr.DrawLine(NewPen, (cond.FirstNode % COLS) * SCALE + ALINGMENT, (cond.FirstNode / COLS) * SCALE + ALINGMENT,
                        (cond.SecondNode % COLS) * SCALE + ALINGMENT, (cond.SecondNode / COLS) * SCALE + ALINGMENT);
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

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    DrawGrid();
        //}

        private void frm_grid_MouseWheel(object sender, MouseEventArgs e)
        {

                if (e.Delta > 0)
                {
                    if (SCALE < 70)
                        SCALE += 10;
                }
                else
                {
                    if (SCALE>20)
                        SCALE -= 10;
                }
                gr.Clear(frameColor);
                //DrawGrid();
        }

        private void frm_grid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                cursor_x = e.X;
                cursor_y = e.Y;
            }
        }

        private void frm_grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                int diff_x = e.X-cursor_x;
                int diff_y = e.Y - cursor_y;
                if ((frameLocation_x+diff_x)<=0 && (frameLocation_x + diff_x)>= pb_grid.Width- bmp.Width)
                    frameLocation_x += diff_x;
                if ((frameLocation_y + diff_y) <= 0 && (frameLocation_y + diff_y) >= pb_grid.Height - bmp.Height)
                    frameLocation_y += diff_y;
                cursor_x = e.X;
                cursor_y = e.Y;
                pb_grid.Invalidate();
            }
        }

        private void pb_grid_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(bmp, frameLocation_x, frameLocation_y);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitPicture();
        }
    }
}
