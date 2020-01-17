using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarbleGame
{
    public partial class stopWatch : UserControl
    {
        private float radius;
        private Point center;
        private BufferedGraphicsContext CurrentContext;
        private Point center2;
        private DateTime now;
        

        private bool running;
        DateTime startTime;
        TimeSpan currentTimePassed, pauseTime;

        public stopWatch()
        {
            InitializeComponent();
            
            running = false;
            if (Width < Height)
            {
                radius = Width / 2;
            }
            else
            {
                radius = Height / 2;
            }
            center = new Point(Width / 2, Height / 2);
            center2 = new Point(Width/2 , Height/ 3);
        }

        private void stopWatch_Load(object sender, EventArgs e)
        {
            CurrentContext = BufferedGraphicsManager.Current;
            System.Drawing.BufferedGraphics bg = CurrentContext.Allocate(CreateGraphics(), ClientRectangle);
            bg.Graphics.Clear(SystemColors.Window);

            
            DrawLines(bg.Graphics);
            DrawNumbers(bg.Graphics);
           DrawHands(bg.Graphics);
            bg.Render();
        }

        private void drawCircle(Graphics g)
        {
            Pen mypen4 = new Pen(Color.Black, radius * .02f);
            Pen mypen5 = new Pen(Color.Black, radius * 1f);

           // g.DrawEllipse(mypen4, Width/2, Height/4, 30, 30);
            //g.DrawLine(mypen5, center, center2);

        }

        private void DrawLines(Graphics g)
        {
            Pen myPen1 = new Pen(Color.Red, radius * 0.07f);
            Pen myPen2 = new Pen(Color.Black, radius * 0.05f);
            Pen myPen3 = new Pen(Color.Black, radius * 0.02f);
            Pen myPen6 = new Pen(Color.Black, radius * 0.04f);

            for (int i = 0; i < 4; i++)
            {
                float x = (float)Math.Cos(i * 90 * Math.PI / 180) * radius * .7f + center.X;
                float y = (float)Math.Sin(i * 90 * Math.PI / 180) * radius * .7f + center.Y;

                float x2 = (float)Math.Cos(i * 90 * Math.PI / 180) * radius * .8f + center.X;
                float y2 = (float)Math.Sin(i * 90 * Math.PI / 180) * radius * .8f + center.Y;

                g.DrawLine(myPen1, x, y, x2, y2);
            }

            for (int i = 0; i < 12; i++)
            {
                if (i % 3 != 0)
                {
                    float x = (float)Math.Cos(i * 30 * Math.PI / 180) * radius * .7f + center.X;
                    float y = (float)Math.Sin(i * 30 * Math.PI / 180) * radius * .7f + center.Y;

                    float x2 = (float)Math.Cos(i * 30 * Math.PI / 180) * radius * .8f + center.X;
                    float y2 = (float)Math.Sin(i * 30 * Math.PI / 180) * radius * .8f + center.Y;

                    ////draw minute marks inside small clock
                    //float x3 = (float)Math.Cos(i * 30 * Math.PI / 180)* radius/4 * .7f + center2.X;
                    //float y3 = (float)Math.Sin(i * 30 * Math.PI / 180) * radius/4 * .7f + center2.Y;

                    //float x4 = (float)Math.Cos(i * 30 * Math.PI / 180) * radius/4 * .8f + center2.X;
                    //float y4 = (float)Math.Sin(i * 30 * Math.PI / 180) * radius/4 * .8f + center2.Y;
                    //g.DrawLine(myPen6, x3, y3, x4, y4);

                    g.DrawLine(myPen2, x, y, x2, y2);
                    
                }
                //draw minute marks inside small clock
                float x3 = (float)Math.Cos(i * 30 * Math.PI / 180) * radius / 4 * .7f + center2.X;
                float y3 = (float)Math.Sin(i * 30 * Math.PI / 180) * radius / 4 * .7f + center2.Y;

                float x4 = (float)Math.Cos(i * 30 * Math.PI / 180) * radius / 4 * .8f + center2.X;
                float y4 = (float)Math.Sin(i * 30 * Math.PI / 180) * radius / 4 * .8f + center2.Y;
                g.DrawLine(myPen6, x3, y3, x4, y4);
            }

            for (int i = 0; i < 60; i++)
            {
                if (i % 5 != 0)
                {
                    float x = (float)Math.Cos(i * 6 * Math.PI / 180) * radius * .7f + center.X;
                    float y = (float)Math.Sin(i * 6 * Math.PI / 180) * radius * .7f + center.Y;

                    float x2 = (float)Math.Cos(i * 6 * Math.PI / 180) * radius * .8f + center.X;
                    float y2 = (float)Math.Sin(i * 6 * Math.PI / 180) * radius * .8f + center.Y;

                    g.DrawLine(myPen3, x, y, x2, y2);
                }
            }

        }

        private void DrawNumbers(Graphics g)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            Font numfont = new Font("Tahoma", radius * 0.11f, FontStyle.Bold);
            Brush numbrush = new SolidBrush(Color.Black);
            int num = 5;
            for (int i = 1; i <= 12; i++)
            {
                float x = (float)Math.Cos((i * 30 - 90) * Math.PI / 180) * radius * .9f + center.X;
                float y = (float)Math.Sin((i * 30 - 90) * Math.PI / 180) * radius * .9f + center.Y;

                g.DrawString(num.ToString(), numfont, numbrush, x, y, sf);
                num += 5;
            }
        }

        private void DrawHands(Graphics g)
        {
            Pen hourPen = new Pen(Color.Black, radius * 0.05f);
            Pen minutePen = new Pen(Color.Black, radius * 0.03f);
            Pen secondPen = new Pen(Color.Red, radius * 0.01f);

            // DateTime now = DateTime.Now;
            int hours = now.Hour;
            int minutes = 0;
            int seconds = 0;

           
            

            //float hx = (float)Math.Cos((hours * 30 - 90) * Math.PI / 180) * radius * .6f + center.X;
            //float hy = (float)Math.Sin((hours * 30 - 90) * Math.PI / 180) * radius * .6f + center.Y;

            float mx = (float)Math.Cos((currentTimePassed.Minutes * 6 - 90) * Math.PI / 180) * radius * .15f + center2.X;
            float my = (float)Math.Sin((currentTimePassed.Minutes * 6 - 90) * Math.PI / 180) * radius * .15f + center2.Y;

            float sx = (float)Math.Cos((currentTimePassed.Seconds * 6 - 90) * Math.PI / 180) * radius * .9f + center.X;
            float sy = (float)Math.Sin((currentTimePassed.Seconds * 6 - 90) * Math.PI / 180) * radius * .9f + center.Y;

           // g.DrawLine(hourPen, center.X, center.Y, hx, hy);
            g.DrawLine(minutePen, center2.X, center2.Y, mx, my);
            g.DrawLine(secondPen, center.X, center.Y, sx, sy);

        }

        private void stopWatch_Paint(object sender, PaintEventArgs e)
        {
            CurrentContext = BufferedGraphicsManager.Current;
            System.Drawing.BufferedGraphics bg = CurrentContext.Allocate(CreateGraphics(), ClientRectangle);
            bg.Graphics.Clear(Color.White);

           // DateTime now = DateTime.Now
            DrawLines(bg.Graphics);
            DrawNumbers(bg.Graphics);
            DrawHands(bg.Graphics);
            drawCircle(bg.Graphics);
            bg.Render();
        }

        private void stopWatch_Resize(object sender, EventArgs e)
        {
            if (Width < Height)
            {
                radius = Width / 2;
            }
            else
            {
                radius = Height / 2;
            }
            center = new Point(Width / 2, Height / 2);
            center2 = new Point(Width / 2, Height / 3);


            CurrentContext = BufferedGraphicsManager.Current;
            System.Drawing.BufferedGraphics bg = CurrentContext.Allocate(CreateGraphics(), ClientRectangle);
            bg.Graphics.Clear(Color.White);

            DrawLines(bg.Graphics);
            DrawNumbers(bg.Graphics);
            DrawHands(bg.Graphics);
            drawCircle(bg.Graphics);
            bg.Render();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CurrentContext = BufferedGraphicsManager.Current;
            System.Drawing.BufferedGraphics bg = CurrentContext.Allocate(CreateGraphics(), ClientRectangle);
            bg.Graphics.Clear(Color.White);

            currentTimePassed = DateTime.Now - startTime + pauseTime;

            DrawLines(bg.Graphics);
            DrawNumbers(bg.Graphics);
            DrawHands(bg.Graphics);
            drawCircle(bg.Graphics);
            bg.Render();
        }

        public void Start()
        {
            if (!running)
            {
                startTime = DateTime.Now;
                running = true;
                timer1.Enabled = true;
            }
            
        }

        public void Stop()
        {
            if (running)
            {
                currentTimePassed = TimeSpan.Zero;
                pauseTime = TimeSpan.Zero;
                timer1.Enabled = false;
                running = false;
            }
        }

        public void Pause()
        {
            if (running)
            {
                timer1.Enabled = false;
                running = false;
                pauseTime += DateTime.Now - startTime;
            }
        }

        public void Reset()
        {
            if (!running)
            {
                
                pauseTime = TimeSpan.Zero;
                currentTimePassed = TimeSpan.Zero;

                CurrentContext = BufferedGraphicsManager.Current;
                System.Drawing.BufferedGraphics bg = CurrentContext.Allocate(CreateGraphics(), ClientRectangle);
                bg.Graphics.Clear(SystemColors.Window);


                DrawLines(bg.Graphics);
                DrawNumbers(bg.Graphics);
                DrawHands(bg.Graphics);
                bg.Render();


            }
        }

        public int Seconds
        {
            get
            {
                return currentTimePassed.Seconds;
            }
        }
        public int Minutes
        {
            get
            {
                return currentTimePassed.Minutes;
            }
        }
    }
}
