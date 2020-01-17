using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ClassLibrary1;



namespace MarbleGame
{

    

    public partial class Form1 : Form
    {
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private Image marble;
        private PictureBox[,] twoDArray;
        private Grid[,] pbxGrid;
        private int marbles;
        private int size;
        private string picturepath;
        private string textpath;
        private string leaderPath;
        private string zipPath;
        private double ratio;
        private int leaderBoardCount;
        private double buttonWidth;
       private double buttonHeight;
        private int oldWidth;
        private int oldHeight;
        private int moves;
        private int time;
        private string identity;
        private bool puzzleChosen;
        private highScore temp;
        private List <highScore> leaderBoard;
        string TempDirectory;

        public Form1() 
        {
            
            oldHeight = this.Height;
            oldWidth = this.Width;
            puzzleChosen = false;

            

            InitializeComponent();
            


        }


        private void initBoard()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;

            string[] linesFromFile = System.IO.File.ReadAllLines(textpath);
            string[] pieces = linesFromFile[0].Split(' ');

            int lines = linesFromFile.Length - 1;
            size = Convert.ToInt32(pieces[0]);
            int sizew = marble.Width;
            int sizeh = marble.Height;
            moves = 0; 

            
            pbxGrid = new Grid[size, size];
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    this.pbxGrid[r, c] = new Grid();
                }
            }


            Int32.TryParse(pieces[1], out marbles);

            for (int x = 0, y = 0; x < marbles; x++, y++)
            {

                string[] coordinates = linesFromFile[x + 1].Split(' ');
                pbxGrid[Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1])].inputBall(y + 1);


            }

            for (int y = marbles, x = 0; y < marbles + marbles; y++, x++)
            {

                string[] coordinates = linesFromFile[y + 1].Split(' ');
                pbxGrid[Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1])].inputBall(-1 * (x + 1));

            }

            for (int z = marbles + marbles; z < lines; z++)
            {
                string[] coordinates = linesFromFile[z + 1].Split(' ');
                inputWalls(Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1]), Convert.ToInt32(coordinates[2]), Convert.ToInt32(coordinates[3]));
            }

            //outside walls
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {

                    if (r == 0)
                    {
                        pbxGrid[r, c].inputUp(true);
                    }
                    if (r == size - 1)
                    {
                        pbxGrid[r, c].inputDown(true);
                    }
                    if (c == 0)
                    {
                        pbxGrid[r, c].inputLeft(true);
                    }
                    if (c == size - 1)
                    {
                        pbxGrid[r, c].inputRight(true);
                    }
                }

            }

            if (sizew > sizeh)
            {
                ratio = sizew / (double)sizeh;
                buttonWidth = 250 / size;
                buttonHeight = buttonWidth / ratio;
            }
            else
            {
                ratio = sizeh / (double)sizew;
                buttonHeight = 250 / size;
                buttonWidth = buttonHeight / ratio;

            }

            twoDArray = new PictureBox[size, size];
            for (int r = 0, roffset = 0; r < size; r++)
            {
                for (int c = 0, coffset = 0; c < size; c++)
                {
                    this.twoDArray[r, c] = new PictureBox();
                    // this.twoDArray[r, c].Location = new System.Drawing.Point(12 + coffset, 12 + roffset);
                    this.twoDArray[r, c].Name = "btnr" + c.ToString() + "c" + r.ToString();
                    this.twoDArray[r, c].Size = new System.Drawing.Size((int)buttonWidth, (int)buttonHeight);
                    this.twoDArray[r, c].AutoSize = false;
                    this.twoDArray[r, c].SizeMode = PictureBoxSizeMode.StretchImage;
                    this.twoDArray[r, c].Margin = new System.Windows.Forms.Padding(0);
                    this.twoDArray[r, c].Dock = DockStyle.Fill;
                    //this.Controls.Add(this.twoDArray[r, c]);
                    insertImages(r, c);
                }
            }

            //layout panel 2

            if(marble.Width > marble.Height)
            {
                this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
                this.tableLayoutPanel2.ColumnCount = 1;
                this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
                this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
                this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
                this.tableLayoutPanel2.Name = "tableLayoutPanel2";
                this.tableLayoutPanel2.RowCount = 2;
                this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / (float)ratio));
                this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 - (100 / ((float)ratio))));
                this.tableLayoutPanel2.Size = new System.Drawing.Size(500, 500);
                this.tableLayoutPanel2.TabIndex = 0;
                this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            }
            else
            {
                this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
                this.tableLayoutPanel2.ColumnCount = 2;
                this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / (float)ratio));
                this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 - (100 / ((float)ratio))));
                this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
                this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
                this.tableLayoutPanel2.Name = "tableLayoutPanel2";
                this.tableLayoutPanel2.RowCount = 1;
                this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                this.tableLayoutPanel2.Size = new System.Drawing.Size(500, 500);
                this.tableLayoutPanel2.TabIndex = 0;
                this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            }
            

            //layout panel 3 
            float percent = Convert.ToSingle(100.0 / size);
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel3.ColumnCount = size;
            tableLayoutPanel3.RowCount = size;
            for (int x = 0; x < size; x++)
            {
                tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, percent));
                tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, percent));
            }
            tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);

            //PictureBox pictureBox;
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                   
                    tableLayoutPanel3.Controls.Add(twoDArray[row,col], col, row);
                }
                tableLayoutPanel2.Controls.Add(tableLayoutPanel3);

            }






        }

        private void insertImages(int r, int c)
        {
            
            int sizew = marble.Width;
            int sizeh = marble.Height;
            
            

            int displayWidth = (int)buttonWidth;
            int displayHeight = (int)buttonHeight;
            Bitmap bm = new Bitmap(displayWidth, displayHeight);
            Graphics g = Graphics.FromImage(bm);
            Rectangle rect = new Rectangle(0, 0, displayWidth, displayHeight);

            Pen bp = new Pen(Color.Black);
            Brush bf = new SolidBrush(Color.Yellow);
            Font f = new Font("Arial", 24.0f);
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            


            // Grid space is Empty
            if (pbxGrid[r,c].getBall() == 0)
            {
                //all walls
               if(pbxGrid[r, c].getLeft()==true && pbxGrid[r, c].getRight() == true && pbxGrid[r, c].getUp() == true && pbxGrid[r, c].getDown() == true)
                {
                    g.DrawImage(marble, rect, 1 * (marble.Width / 7), 2 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                    twoDArray[r, c].Image = bm;
                    return;
                }
               //no walls
               else if(pbxGrid[r, c].getLeft() == false && pbxGrid[r, c].getRight() == false && pbxGrid[r, c].getUp() == false && pbxGrid[r, c].getDown() == false)
                {
                    if(pbxGrid[r,c].getErr() == true)
                    {
                        g.DrawImage(marble, rect, 6 * (marble.Width / 7), 6 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                        twoDArray[r, c].Image = bm;
                        return;
                    }
                    g.DrawImage(marble, rect, 0 * (marble.Width / 7), 0 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                    twoDArray[r, c].Image = bm;
                    return;
                }
                else
                {
                    // start left wall or left only
                    if (pbxGrid[r, c].getLeft() == true)
                    {
                        //left, down
                        if (pbxGrid[r, c].getDown() == true)
                        {
                            //left, down, right
                            if(pbxGrid[r, c].getRight())
                            {

                                g.DrawImage(marble, rect, 0 * (marble.Width / 7), 3 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                                twoDArray[r, c].Image = bm;
                                return;                            }

                            g.DrawImage(marble, rect, 0 * (marble.Width / 7), 1 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //left, up
                        else if (pbxGrid[r, c].getUp() == true)
                        {
                            //left, up, right
                            if(pbxGrid[r, c].getRight() == true)
                            {
                                g.DrawImage(marble, rect, 6 * (marble.Width / 7), 1 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                                twoDArray[r, c].Image = bm;
                                return;
                            }

                            g.DrawImage(marble, rect, 5 * (marble.Width / 7), 0 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //left, right
                        else if (pbxGrid[r, c].getRight() == true)
                        {


                            g.DrawImage(marble, rect, 6 * (marble.Width / 7), 0 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //left wall only
                        g.DrawImage(marble, rect, 1 * (marble.Width / 7), 0 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                        twoDArray[r, c].Image = bm;
                        return;
                    }
                    //start right wall or right only
                    else if (pbxGrid[r, c].getRight() == true)
                    {
                        //right, down
                        if (pbxGrid[r, c].getDown() == true)
                        {

                            g.DrawImage(marble, rect, 2 * (marble.Width / 7), 1 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //right, up
                        else if (pbxGrid[r, c].getUp() == true)
                        {
                            //right, up, down
                            if (pbxGrid[r, c].getDown() == true)
                            {
                                g.DrawImage(marble, rect, 5 * (marble.Width / 7), 1 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                                twoDArray[r, c].Image = bm;
                                return;
                            }

                            g.DrawImage(marble, rect, 1 * (marble.Width / 7), 1 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //right wall only
                        g.DrawImage(marble, rect, 2 * (marble.Width / 7), 0 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                        twoDArray[r, c].Image = bm;
                        return;
                    }

                    //start up wall or up only
                    else if (pbxGrid[r, c].getUp() == true)
                    {
                        //up, down
                        if (pbxGrid[r, c].getDown() == true)
                        {
                           

                            g.DrawImage(marble, rect, 3 * (marble.Width / 7), 1 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //up only
                        g.DrawImage(marble, rect, 3 * (marble.Width / 7), 0 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                        twoDArray[r, c].Image = bm;
                        return;
                    }

                    //start down wall or down only
                    else if (pbxGrid[r, c].getDown() == true)
                    {

                        g.DrawImage(marble, rect, 4 * (marble.Width / 7), 0 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);

                        twoDArray[r, c].Image = bm;
                        return;
                    }
                }

               
            }
            // Grid space contains a ball
           else if (pbxGrid[r, c].getBall() >= 1)
            {
                //all walls
                if (pbxGrid[r, c].getLeft() == true && pbxGrid[r, c].getRight() == true && pbxGrid[r, c].getUp() == true && pbxGrid[r, c].getDown() == true)
                {
                    g.DrawImage(marble, rect, 5 * (marble.Width / 7), 6 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                    g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                    twoDArray[r, c].Image = bm;
                    return;
                }
                //no walls
                else if (pbxGrid[r, c].getLeft() == false && pbxGrid[r, c].getRight() == false && pbxGrid[r, c].getUp() == false && pbxGrid[r, c].getDown() == false)
                {
                    g.DrawImage(marble, rect, 4 * (marble.Width / 7), 4 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                    g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                    twoDArray[r, c].Image = bm;
                    return;
                }
                else
                {
                    // start left wall or left only
                    if (pbxGrid[r, c].getLeft() == true)
                    {
                        //left, down
                        if (pbxGrid[r, c].getDown() == true)
                        {
                            //left, down, right
                            if (pbxGrid[r, c].getRight())
                            {

                                g.DrawImage(marble, rect, 4 * (marble.Width / 7), 6 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                                g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                                twoDArray[r, c].Image = bm;
                                return;
                            }

                            g.DrawImage(marble, rect, 4 * (marble.Width / 7), 5 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //left, up
                        else if (pbxGrid[r, c].getUp() == true)
                        {
                            //left, up, right
                            if (pbxGrid[r, c].getRight() == true)
                            {
                                g.DrawImage(marble, rect, 3 * (marble.Width / 7), 6 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                                g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                                twoDArray[r, c].Image = bm;
                                return;
                            }

                            g.DrawImage(marble, rect, 2 * (marble.Width / 7), 5 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //left, right
                        else if (pbxGrid[r, c].getRight() == true)
                        {


                            g.DrawImage(marble, rect, 3 * (marble.Width / 7), 5 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //left wall only
                        g.DrawImage(marble, rect, 5 * (marble.Width / 7), 4 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                        g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                        twoDArray[r, c].Image = bm;
                        return;
                    }
                    //start right wall or right only
                    else if (pbxGrid[r, c].getRight() == true)
                    {
                        //right, down
                        if (pbxGrid[r, c].getDown() == true)
                        {

                            g.DrawImage(marble, rect, 6 * (marble.Width / 7), 5 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //right, up
                        else if (pbxGrid[r, c].getUp() == true)
                        {
                            //right, up, down
                            if (pbxGrid[r, c].getDown() == true)
                            {
                                g.DrawImage(marble, rect, 2 * (marble.Width / 7), 6 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                                g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                                twoDArray[r, c].Image = bm;
                                return;
                            }

                            g.DrawImage(marble, rect, 5 * (marble.Width / 7), 5 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //right wall only
                        g.DrawImage(marble, rect, 6 * (marble.Width / 7), 4 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                        g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                        twoDArray[r, c].Image = bm;
                        return;
                    }

                    //start up wall or up only
                    else if (pbxGrid[r, c].getUp() == true)
                    {
                        //up, down
                        if (pbxGrid[r, c].getDown() == true)
                        {


                            g.DrawImage(marble, rect, 0 * (marble.Width / 7), 6 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //up only
                        g.DrawImage(marble, rect, 0 * (marble.Width / 7), 5 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                        g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                        twoDArray[r, c].Image = bm;
                        return;
                    }

                    //start down wall or down only
                    else if (pbxGrid[r, c].getDown() == true)
                    {

                        g.DrawImage(marble, rect, 1 * (marble.Width / 7), 5 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                        g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                        twoDArray[r, c].Image = bm;
                        return;
                    }
                }

            }
            // Grid space contains a hole
           else if (pbxGrid[r, c].getBall() <= -1)
            {
                //all walls
                if (pbxGrid[r, c].getLeft() == true && pbxGrid[r, c].getRight() == true && pbxGrid[r, c].getUp() == true && pbxGrid[r, c].getDown() == true)
                {
                    g.DrawImage(marble, rect, 3 * (marble.Width / 7), 5 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                    g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                    twoDArray[r, c].Image = bm;
                    return;
                }
                //no walls
                else if (pbxGrid[r, c].getLeft() == false && pbxGrid[r, c].getRight() == false && pbxGrid[r, c].getUp() == false && pbxGrid[r, c].getDown() == false)
                {
                    g.DrawImage(marble, rect, 2 * (marble.Width / 7), 2 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                    g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                    twoDArray[r, c].Image = bm;
                    return;
                }
                else
                {
                    // start left wall or left only
                    if (pbxGrid[r, c].getLeft() == true)
                    {
                        //left, down
                        if (pbxGrid[r, c].getDown() == true)
                        {
                            //left, down, right
                            if (pbxGrid[r, c].getRight())
                            {

                                g.DrawImage(marble, rect, 2 * (marble.Width / 7), 4 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                                g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                                twoDArray[r, c].Image = bm;
                                return;
                            }

                            g.DrawImage(marble, rect, 2 * (marble.Width / 7), 3 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //left, up
                        else if (pbxGrid[r, c].getUp() == true)
                        {
                            //left, up, right
                            if (pbxGrid[r, c].getRight() == true)
                            {
                                g.DrawImage(marble, rect, 1 * (marble.Width / 7), 4 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                                g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                                twoDArray[r, c].Image = bm;
                                return;
                            }

                            g.DrawImage(marble, rect, 0 * (marble.Width / 7), 3 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //left, right
                        else if (pbxGrid[r, c].getRight() == true)
                        {


                            g.DrawImage(marble, rect, 1 * (marble.Width / 7), 3 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //left wall only
                        g.DrawImage(marble, rect, 3 * (marble.Width / 7), 2 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                        g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                        twoDArray[r, c].Image = bm;
                        return;
                    }
                    //start right wall or right only
                    else if (pbxGrid[r, c].getRight() == true)
                    {
                        //right, down
                        if (pbxGrid[r, c].getDown() == true)
                        {

                            g.DrawImage(marble, rect, 4 * (marble.Width / 7), 3 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }
                        //right, up
                        else if (pbxGrid[r, c].getUp() == true)
                        {
                            //right, up, down
                            if (pbxGrid[r, c].getDown() == true)
                            {
                                g.DrawImage(marble, rect, 0 * (marble.Width / 7), 4 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                                g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                                twoDArray[r, c].Image = bm;
                                return;
                            }

                            g.DrawImage(marble, rect, 3 * (marble.Width / 7), 3 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //right wall only

                        g.DrawImage(marble, rect, 4 * (marble.Width / 7), 2 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                        g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                        twoDArray[r, c].Image = bm;
                        return;
                    }

                    //start up wall or up only
                    else if (pbxGrid[r, c].getUp() == true)
                    {
                        //up, down
                        if (pbxGrid[r, c].getDown() == true)
                        {


                            g.DrawImage(marble, rect, 5 * (marble.Width / 7), 3 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                            g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                            twoDArray[r, c].Image = bm;
                            return;
                        }

                        //up only
                        g.DrawImage(marble, rect, 5 * (marble.Width / 7), 2 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                        g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                        twoDArray[r, c].Image = bm;
                        return;
                    }

                    //start down wall or down only
                    else if (pbxGrid[r, c].getDown() == true)
                    {
                        
                        g.DrawImage(marble, rect, 6 * (marble.Width / 7), 2 * (marble.Height / 7), marble.Width / 7, marble.Height / 7, GraphicsUnit.Pixel);
                        g.DrawString(pbxGrid[r, c].getBall().ToString(), f, bf, rect, sf);
                        twoDArray[r, c].Image = bm;
                        return;
                    }
                }


            }





            
        }

       


       private void inputWalls(int x1, int y1, int x2, int y2)
        {
            //first coordinates
            int r1 = x1;
            int c1 = y1;
            //second coordinates
            int r2 = x2;
            int c2 = y2;

            if (r1 == r2)
            {
                if (c1 > c2)
                {
                    pbxGrid[r1, c1].inputLeft(true);
                    pbxGrid[r2, c2].inputRight(true);
                }
                else
                {
                    pbxGrid[r1, c1].inputRight(true);
                    pbxGrid[r2, c2].inputLeft(true);
                }
            }
            else
            {
                if (r1 < r2)
                {
                    pbxGrid[r1, c1].inputUp(true);
                    pbxGrid[r2, c2].inputDown(true);
                }

                else
                {
                    pbxGrid[r1, c1].inputDown(true);
                    pbxGrid[r2, c2].inputUp(true);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] linesFromFile = System.IO.File.ReadAllLines("marble.txt");
            string[] pieces = linesFromFile[0].Split(' ');

            int lines = linesFromFile.Length - 1;
            int size = Convert.ToInt32(pieces[0]);
            int sizew = marble.Width;
            int sizeh = marble.Height;
           

            pbxGrid = new Grid[size, size];
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    this.pbxGrid[r, c] = new Grid();
                }
            }

                    Int32.TryParse(pieces[1], out marbles);

            for (int x = 0; x < marbles; x++)
            {
                string[] coordinates = linesFromFile[x + 1].Split(' ');
                this.pbxGrid[Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1])].inputBall(1);
            }

            for (int y = marbles; y < marbles + marbles; y++)
            {
                string[] coordinates = linesFromFile[y + 1].Split(' ');
                pbxGrid[Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1])].inputBall(-1);
            }

            for (int z = marbles + marbles; z < lines; z++)
            {
                string[] coordinates = linesFromFile[z + 1].Split(' ');
                inputWalls(Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1]), Convert.ToInt32(coordinates[2]), Convert.ToInt32(coordinates[3]));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void gameOver(bool win)
        {
            class11.Pause();
            if (win == true)
            {
                time = class11.Seconds + class11.Minutes * 60;
                identity = Environment.UserName;
                MessageBox.Show("You win!");
            }
            else
                MessageBox.Show("You Lose!");
            if (leaderBoard == null)
            {
                leaderBoard = new List<highScore>();
             

                 temp = new highScore();
                temp.identity = identity;
                temp.moves = moves;
                temp.time = time;

                addToList(temp);
                leaderBoard.Add(temp);
                leaderBoardCount++;
                

            }
            else
            {
                 temp = new highScore();
                temp.identity = identity;
                temp.moves = moves;
                temp.time = time;

                addToList(temp);
                leaderBoard.Add(temp);
                leaderBoardCount++;
                sort(leaderBoard);
            }

            if (leaderBoard != null)
            {
                IFormatter formatter = new BinaryFormatter();
                string PersonFile = leaderPath;
                string ArchiveFile = zipPath;

                using (FileStream stream = new FileStream(PersonFile, FileMode.Create))
                {

                    formatter.Serialize(stream, leaderBoard);

                }

                using (ZipArchive archive = ZipFile.Open(ArchiveFile, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry oldentry = archive.GetEntry("people.bin");
                    if (oldentry != null)
                    {
                        oldentry.Delete();
                    }


                    ZipArchiveEntry entry = archive.CreateEntry("people.bin");
                    using (Stream stream = entry.Open())
                    {
                        formatter.Serialize(stream, leaderBoard);
                    }

                }
            }

            int j = 0;
            try
            {
                while (j < leaderBoard.Count)
                {
                    listView1.Items[j].Remove();
                }
            }
            catch(ArgumentOutOfRangeException e)
            {

            }


            cleanPuzzle();
        }

       private void cleanPuzzle()
        {
            moves = 0;
            class11.Stop();

            tableLayoutPanel2.Dispose();
            tableLayoutPanel3.Dispose();

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {

                    this.twoDArray[r, c].Dispose();
                    this.twoDArray[r, c].Image = null;
                }
            }

            puzzleChosen = false;
        }
        private void rollLeft(object sender, EventArgs e)
        {
            moves += 1;
            for (int r = 0; r < size; r++)
            {
                for(int c = 1; c < size; c++)
                {
                    if(pbxGrid[r,c].getBall() > 0)
                    {
                        if (!Lroll(r, c))
                        {

                            for (int x = 0; x < size; x++)
                            {
                                for (int y = 0; y < size; y++)
                                {
                                   
                                    insertImages(x, y);
                                    
                                }
                            }
                            gameOver(false);
                            return;
                        }
                    }

                   
                }

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        insertImages(x, y);
                    }
                }

                if (marbles == 0)
                {
                    gameOver(true);
                }
            }
           
        }

        private bool Lroll(int r, int c)
        {
            if(pbxGrid[r,c].getBall() > 0)
            {
                while (c > 0)
                {
                    if(pbxGrid[r,c].getLeft()!= true && pbxGrid[r,c-1].getBall() == 0){
                        pbxGrid[r, c - 1].inputBall(pbxGrid[r, c].getBall());
                        pbxGrid[r, c].inputBall(0);

                    }
                    else if(pbxGrid[r,c].getLeft()!=true && pbxGrid[r,c-1].getBall() < 0)
                    {
                        if(pbxGrid[r,c].getBall() + pbxGrid[r,c-1].getBall() == 0)
                        {
                            pbxGrid[r, c].inputBall(0);
                            pbxGrid[r, c-1].inputBall(0);
                            marbles--;
                            return true;
                        }
                        else
                        {
                            pbxGrid[r, c].setErr(true);
                            pbxGrid[r, c].inputLeft(false);
                            pbxGrid[r, c].inputRight(false);
                            pbxGrid[r, c].inputUp(false);
                            pbxGrid[r, c].inputDown(false);
                            pbxGrid[r, c].inputBall(0);

                            pbxGrid[r, c - 1].setErr(true);
                            pbxGrid[r, c-1].inputLeft(false);
                            pbxGrid[r, c-1].inputRight(false);
                            pbxGrid[r, c-1].inputUp(false);
                            pbxGrid[r, c-1].inputDown(false);
                            pbxGrid[r, c-1].inputBall(0);

                            
                            return false;
                        }

                    }
                    else
                    {
                        return true;
                    }
                    c--;
                }
            }
            return true;
        }

        private void rollUp(object sender, EventArgs e)
        {
            moves += 1;
            for (int r = 1; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (pbxGrid[r, c].getBall() > 0)
                    {
                        if (!Uroll(r, c))
                        {

                            for (int x = 0; x < size; x++)
                            {
                                for (int y = 0; y < size; y++)
                                {
                                    insertImages(x, y);
                                }
                            }
                            gameOver(false);
                            return;
                        }
                    }


                }

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        insertImages(x, y);
                    }
                }

                if (marbles == 0)
                {
                    gameOver(true);
                }
            }



        }

        private bool Uroll(int r, int c)
        {
            if (pbxGrid[r, c].getBall() > 0)
            {
                while (r > 0)
                {
                    if (pbxGrid[r, c].getUp() != true && pbxGrid[r - 1, c].getBall() == 0)
                    {
                        pbxGrid[r-1, c].inputBall(pbxGrid[r, c].getBall());
                        pbxGrid[r, c].inputBall(0);

                    }
                    else if (pbxGrid[r, c].getUp() != true && pbxGrid[r - 1, c].getBall() < 0)
                    {
                        if (pbxGrid[r, c].getBall() + pbxGrid[r-1, c].getBall() == 0)
                        {
                            pbxGrid[r, c].inputBall(0);
                            pbxGrid[r-1, c].inputBall(0);
                            marbles--;
                            return true;
                        }
                        else
                        {
                            pbxGrid[r, c].setErr(true);
                            pbxGrid[r, c].inputLeft(false);
                            pbxGrid[r, c].inputRight(false);
                            pbxGrid[r, c].inputUp(false);
                            pbxGrid[r, c].inputDown(false);
                            pbxGrid[r, c].inputBall(0);

                            pbxGrid[r-1, c].setErr(true);
                            pbxGrid[r-1,c ].inputLeft(false);
                            pbxGrid[r-1, c].inputRight(false);
                            pbxGrid[r-1, c].inputUp(false);
                            pbxGrid[r-1, c ].inputDown(false);
                            pbxGrid[r-1, c ].inputBall(0);
                            return false;
                        }

                    }
                    else
                    {
                        return true;
                    }
                    r--;
                }
            }
            return true;

        }

        private void rollDown(object sender, EventArgs e)
        {
            moves += 1;
            for (int r = size -1; r >= 0; r--)
            {
                for (int c = 0; c < size; c++)
                {
                    if (pbxGrid[r, c].getBall() > 0)
                    {
                        if (!Droll(r, c))
                        {

                            for (int x = 0; x < size; x++)
                            {
                                for (int y = 0; y < size; y++)
                                {
                                    insertImages(x, y);
                                }
                            }
                            gameOver(false);
                            return;
                        }
                    }


                }

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        insertImages(x, y);
                    }
                }

                if (marbles == 0)
                {
                    gameOver(true);
                }
            }


        }

        private bool Droll(int r, int c)
        {
            if (pbxGrid[r, c].getBall() > 0)
            {
                while ( r < size)
                {
                    if (pbxGrid[r, c].getDown() != true && pbxGrid[r + 1, c].getBall() == 0)
                    {
                        pbxGrid[r + 1, c].inputBall(pbxGrid[r, c].getBall());
                        pbxGrid[r, c].inputBall(0);

                    }
                    else if (pbxGrid[r, c].getDown() != true && pbxGrid[r + 1, c].getBall() < 0)
                    {
                        if (pbxGrid[r, c].getBall() + pbxGrid[r + 1, c].getBall() == 0)
                        {
                            pbxGrid[r, c].inputBall(0);
                            pbxGrid[r + 1, c].inputBall(0);
                            marbles--;
                            return true;
                        }
                        else
                        {
                            pbxGrid[r, c].setErr(true);
                            pbxGrid[r, c].inputLeft(false);
                            pbxGrid[r, c].inputRight(false);
                            pbxGrid[r, c].inputUp(false);
                            pbxGrid[r, c].inputDown(false);
                            pbxGrid[r, c].inputBall(0);

                            pbxGrid[r+1, c].setErr(true);
                            pbxGrid[r+1, c].inputLeft(false);
                            pbxGrid[r+1, c].inputRight(false);
                            pbxGrid[r+1, c].inputUp(false);
                            pbxGrid[r+1, c].inputDown(false);
                            pbxGrid[r+1, c].inputBall(0);
                            return false;
                        }

                    }
                    else
                    {
                        return true;
                    }
                    r++;
                }
            }
            return true;
        }
    

        private void rollRight(object sender, EventArgs e)
        {
            moves += 1;
            for (int r = 0; r < size; r++)
            {
                for (int c = size - 1; c >= 0; c--)
                {
                    if (pbxGrid[r, c].getBall() > 0)
                    {
                        if (!Rroll(r, c))
                        {

                            for (int x = 0; x < size; x++)
                            {
                                for (int y = 0; y < size; y++)
                                {
                                    insertImages(x, y);
                                }
                            }
                            gameOver(false);
                            return;
                        }
                    }


                }

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        insertImages(x, y);
                    }
                }

                if (marbles == 0)
                {
                    gameOver(true);
                }
            }


        }

        private bool Rroll(int r, int c)
        {
            if (pbxGrid[r, c].getBall() > 0)
            {
                while (c < size)
                {
                    if (pbxGrid[r, c].getRight() != true && pbxGrid[r, c + 1].getBall() == 0)
                    {
                        pbxGrid[r, c + 1].inputBall(pbxGrid[r, c].getBall());
                        pbxGrid[r, c].inputBall(0);

                    }
                    else if (pbxGrid[r, c].getRight() != true && pbxGrid[r, c + 1].getBall() < 0)
                    {
                        if (pbxGrid[r, c].getBall() + pbxGrid[r, c + 1].getBall() == 0)
                        {
                            pbxGrid[r, c].inputBall(0);
                            pbxGrid[r, c + 1].inputBall(0);
                            marbles--;
                            return true;
                        }
                        else
                        {
                            pbxGrid[r, c].setErr(true);
                            pbxGrid[r, c].inputLeft(false);
                            pbxGrid[r, c].inputRight(false);
                            pbxGrid[r, c].inputUp(false);
                            pbxGrid[r, c].inputDown(false);
                            pbxGrid[r, c].inputBall(0);

                            pbxGrid[r, c + 1].setErr(true);
                            pbxGrid[r, c + 1].inputLeft(false);
                            pbxGrid[r, c + 1].inputRight(false);
                            pbxGrid[r, c + 1].inputUp(false);
                            pbxGrid[r, c + 1].inputDown(false);
                            pbxGrid[r, c + 1].inputBall(0);
                            return false;
                        }

                    }
                    else
                    {
                        return true;
                    }
                    c++;
                }
            }
            return true;
        }

       

        private void button5_Click(object sender, EventArgs e)
        {

            //read leaderboard in, edit while in listview, write out to file
            ClassLibrary1.Class2 fileViewer = new Class2();
            if (puzzleChosen)
                cleanPuzzle();
           if(fileViewer.ShowDialog() == DialogResult.OK)
            {
                puzzleChosen = true;
                picturepath = fileViewer.tempPath;
                textpath = fileViewer.rettextPath;
                if(fileViewer.tempLeader != null)
                leaderPath = fileViewer.tempLeader;
                zipPath = fileViewer.tempzip;

                TempDirectory = Directory.GetCurrentDirectory();

                leaderBoardCount = 0;
                // string PersonFile = Path.Combine(TempDirectory, "people.bin");



               
                    FileInfo PersonFileInfo = new FileInfo(leaderPath);
                

                if (PersonFileInfo.Exists == false)
                    {
                        leaderBoard = new List<highScore>();
                       
                    }
                    else
                    {
                        using (FileStream stream = new FileStream(leaderPath, FileMode.Open))
                        {
                            IFormatter formatter = new BinaryFormatter();

                            leaderBoard = (List<highScore>)formatter.Deserialize(stream);


                            foreach (highScore curScore in leaderBoard)
                            {
                                
                                addToList(curScore);
                            }
                        }
                    }
                
                

                marble = Image.FromFile(picturepath);
                initBoard();
                class11.Reset();
                class11.Start();
            }
        }
        private void sort(List <highScore> leader)
        {
            

            int l=0;
            try
            {
                while (listView1.Items[l] != null)
                {
                    listView1.Items[l].Remove();
                    
                }
            }
            catch(ArgumentOutOfRangeException e)
            {
                
                
            }
           
            leaderBoard = new List<highScore>(leader.OrderBy(highScore => highScore.moves).ThenBy(highScore => highScore.time));
           

           
        }

      
        private void RemoveDirectory()
        {
           

            DirectoryInfo dirInfo = new DirectoryInfo(TempDirectory);
            FileInfo[] fileList = dirInfo.GetFiles();
            foreach (FileInfo file in fileList)
            {
                File.Delete(file.FullName);
            }
            Directory.Delete(TempDirectory);
        }

        private void addToList(highScore score)
        {
            if (listView1.Items.Count < 5)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = score.identity; //(This is the 'name' property of our form)
                lvi.SubItems.Add(score.moves.ToString());
                lvi.SubItems.Add(score.time.ToString());

                listView1.Items.Add(lvi);
                leaderBoardCount++;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            int diffWidth = Math.Abs(oldWidth - this.Width);
            int diffHeight = Math.Abs(oldHeight - this.Height);

            if (diffWidth > diffHeight)
            {
                this.Height = this.Width - 77;
            }
            else
            {
                this.Width = this.Height + 77;
            }

            oldWidth = this.Width;
            oldHeight = this.Height;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (leaderBoard != null)
            {
                sort(leaderBoard);
                IFormatter formatter = new BinaryFormatter();
                string PersonFile = leaderPath;
                string ArchiveFile = zipPath;
            
                using (FileStream stream = new FileStream(PersonFile, FileMode.Create))
                {

                    formatter.Serialize(stream, leaderBoard);

                }

                using (ZipArchive archive = ZipFile.Open(ArchiveFile, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry oldentry = archive.GetEntry("people.bin");
                    if (oldentry != null)
                    {
                        oldentry.Delete();
                    }


                    ZipArchiveEntry entry = archive.CreateEntry("people.bin");
                    using (Stream stream = entry.Open())
                    {
                        formatter.Serialize(stream, leaderBoard);
                    }

                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel3 != null) { 
            tableLayoutPanel3.Show();
             }

            if (puzzleChosen)
            {
                class11.Start();
            }
            else
                MessageBox.Show("StopWatch Will Start after a puzzle is selected!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            class11.Stop();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            class11.Reset();

            if (class11.Seconds == 0)
            {
                moves = 0;

                tableLayoutPanel2.Dispose();
                tableLayoutPanel3.Dispose();

                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;

                for (int r = 0; r < size; r++)
                {
                    for (int c = 0; c < size; c++)
                    {

                        this.twoDArray[r, c].Dispose();
                        this.twoDArray[r, c].Image = null;
                    }
                }

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {

            class11.Pause();
            if (tableLayoutPanel3 != null) {
                tableLayoutPanel3.Hide();
                    }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("S " + class11.Seconds.ToString());
            MessageBox.Show(" M " + class11.Minutes.ToString());
            MessageBox.Show(moves.ToString());
        }

        private void class11_Load(object sender, EventArgs e)
        {

        }
    }

    class Grid
    {

        int ball = 0;
        bool upWall = false;
        bool rightWall = false;
        bool leftWall = false;
        bool downWall = false;
        bool err = false;

        public void inputBall(int x)
        {
            ball = x;
        }
        public void inputLeft(bool x)
        {
            leftWall = x;
        }

        public void inputUp(bool x)
        {
            upWall = x;
        }
        public void inputRight(bool x)
        {
            rightWall = x;
        }
        public void inputDown(bool x)
        {
            downWall = x;
        }

        public int getBall()
        {
            if (ball >= 1 || ball <= -1)
                return ball;
            else
                return 0;

        }
        public bool getLeft()
        {
            if (leftWall == true)
            {
                return true;
            }

            return false;
        }

        public bool getUp()
        {
            if (upWall == true)
            {
                return true;
            }

            return false;
        }
        public bool getRight()
        {
            if (rightWall == true)
            {
                return true;
            }

            return false;
        }
        public bool getDown()
        {
            if (downWall == true)
            {
                return true;
            }

            return false;
        }


        public void setErr(bool set)
        {
            err = set;
        }

        public bool getErr()
        {
            if (err == true)
            return true;
            else 
            return false;
        }

    }
}
