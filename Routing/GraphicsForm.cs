using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Routing
{
    public partial class frm_grid : Form
    {
        private Bitmap bmp;
        private Graphics gr;
        private PictureBox pb_grid;
        private int SCALE;
        private int ROWS;
        private int COLS;
        private const int GRID_WIDTH = 1;
        private Color frameColor = System.Drawing.Color.White;
        private const int ALINGMENT = 15;
        public frm_grid(int rows, int cols, int scale)
        {
            this.ROWS=rows;
            this.COLS=cols;
            this.SCALE = scale;
            InitPictureBox();
            InitFrame();
            DrawGrid();
        }

        private void InitFrame()
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size((COLS-1)*SCALE+2*ALINGMENT, (ROWS-1)*SCALE+2*ALINGMENT);
        }

        private void InitPictureBox()
        {
            pb_grid = new PictureBox();
            pb_grid.Size = new System.Drawing.Size((COLS-1)*SCALE+2*ALINGMENT, (ROWS-1)*SCALE+2*ALINGMENT);
            pb_grid.BorderStyle = BorderStyle.FixedSingle;
            pb_grid.BackColor = frameColor;
            this.Controls.Add(pb_grid);
            bmp = new Bitmap(pb_grid.Width, pb_grid.Height);
            gr = Graphics.FromImage(bmp);
        }


        private void DrawGrid()
        {
           Pen gridpen = new Pen(Color.Gray, GRID_WIDTH);
            for (int i = 0; i < COLS ; i++)
                gr.DrawLine(gridpen, ALINGMENT + SCALE * i, 0, ALINGMENT + SCALE * i, pb_grid.Height);
            for (int i = 0; i < ROWS ; i++)
                gr.DrawLine(gridpen, 0, ALINGMENT+SCALE*i, pb_grid.Width, ALINGMENT+SCALE*i);
            NumerateNodes();
            pb_grid.Image = bmp;
        }

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
            Random rand = new Random();
                System.Drawing.Color color = System.Drawing.Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                Pen NewPen = new Pen(color, 3);
            //foreach (List<Conductor> trace in obs)
            //{
                foreach (Conductor cond in obs)
                    gr.DrawLine(NewPen, (cond.FirstNode % COLS) * SCALE + ALINGMENT, (cond.FirstNode / COLS) * SCALE + ALINGMENT,
                        (cond.SecondNode % COLS) * SCALE + ALINGMENT, (cond.SecondNode / COLS) * SCALE + ALINGMENT);
            //}
        }

        public void DrawObstruct(int upLeft, int downRight)
        {
            int firstX = upLeft % COLS;
            int firstY = upLeft / COLS;
            int secondX = downRight % COLS;
            int secondY = downRight / COLS;
            for (int i = firstX; i <= secondX; i++)
                for (int j = firstY; j <= secondY; j++)
                {
                    //линия влево
                    gr.DrawLine(new Pen(frameColor, GRID_WIDTH), i * SCALE + ALINGMENT, j * SCALE + ALINGMENT, (i - 1) * SCALE + ALINGMENT + GRID_WIDTH, j * SCALE + ALINGMENT);
                    //линия вправо
                    gr.DrawLine(new Pen(frameColor, GRID_WIDTH), i * SCALE + ALINGMENT, j * SCALE + ALINGMENT, (i + 1) * SCALE + ALINGMENT - GRID_WIDTH, j * SCALE + ALINGMENT);
                    //линия вниз
                    gr.DrawLine(new Pen(frameColor, GRID_WIDTH), i * SCALE + ALINGMENT, j * SCALE + ALINGMENT, i * SCALE + ALINGMENT, (j + 1) * SCALE + ALINGMENT - GRID_WIDTH);
                    //линия вверх
                    gr.DrawLine(new Pen(frameColor, GRID_WIDTH), i * SCALE + ALINGMENT, j * SCALE + ALINGMENT, i * SCALE + ALINGMENT, (j - 1) * SCALE + ALINGMENT + GRID_WIDTH);
                }
        }


        private void frm_grid_Load(object sender, EventArgs e)
        {

        }
    }
}
