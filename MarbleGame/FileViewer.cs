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

namespace MarbleGame
{
    public partial class FileViewer : Form
    {        public FileViewer()
        {
            InitializeComponent();
        }

        private string FullPath;

        private void FileViewer_Load(object sender, EventArgs e)
        {
            FullPath = Directory.GetCurrentDirectory();
            filepath();
            listItems();

        }

        private void setImages()
        {
            string folder;
            string marblefile;
            

        }

        private void listItems()
        {

            listView1.Clear();

            DirectoryInfo dirInfo;
            dirInfo = new DirectoryInfo(FullPath);

            FileInfo[] fileList = dirInfo.GetFiles();

            DirectoryInfo[] dirList = dirInfo.GetDirectories();
            
           

            for (int i = 0; i < dirList.Length; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = dirList[i].Name;
                lvi.ImageIndex = 1;
                listView1.Items.Add(lvi);
            }

            for (int i = 0; i < fileList.Length; i++)
            {
                ListViewItem lvi2 = new ListViewItem();
                lvi2.Text= fileList[i].Name;
                if (fileList[i].Extension == ".mrb")
                {
                    lvi2.ImageIndex = 2;
                }
                else
                    lvi2.ImageIndex = 0;
                
                listView1.Items.Add(lvi2);
            }



        }

        private void filepath()
        {
           
            textBox1.Text = FullPath;
          
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "0";
            label4.Text = "0";
            label6.Text = "0";
            label8.Text = "0";
            if (pbxPreview != null)
            {
                pbxPreview.Image = null;
            }

            DirectoryInfo dirInfo;
            
            dirInfo = new DirectoryInfo(FullPath);
            if (dirInfo.Parent != null)
            {
                DirectoryInfo parentDir = dirInfo.Parent;
                FullPath = parentDir.FullName;
                filepath();
                listItems();
            }
            else
            {
                MessageBox.Show("End of Directories");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(Directory.Exists(textBox1.Text))
                {
                    FullPath = textBox1.Text;
                    filepath();
                    listItems();

                    label2.Text = "0";
                    label4.Text = "0";
                    label6.Text = "0";
                    label8.Text = "0";
                    if(pbxPreview != null)
                    pbxPreview.Image = null;
                }
                else
                {
                    MessageBox.Show("Incorrect Path Entered");
                }
            }
        }

        public string rettextPath
        {
            set
            {
                textPath = value;
            }
            get
            {
                return textPath;
            }
        }
        public string tempzip
        {
            set
            {
                zipt = value;
            }
            get
            {
                return zipt;
            }
        }
        public string tempPath
        {
            set
            {
                picPath = value;
            }
            get
            {
                return picPath;
            }
        }

        public string tempLeader
        {
            set
            {
                leaderBoard = value;
            }
            get
            {
                return leaderBoard;
            }

        }
        
        
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string content = listView1.SelectedItems[0].Text;
            
            
            if (content.Contains(".mrb"))
            {
                zipPath = Path.Combine(FullPath, content);

                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {

                    if (TempDirectory != null)
                    {
                        RemoveDirectory();
                    }

                    TempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                    Directory.CreateDirectory(TempDirectory);

                    

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(TempDirectory, entry.FullName), true);
                    }
                }
                     tempPath = Path.Combine(TempDirectory, "puzzle.jpg");
                     rettextPath = Path.Combine(TempDirectory, "puzzle.txt");
                tempzip = zipPath;
                if (Path.Combine(TempDirectory, "people.bin") != null)
                {
                    tempLeader = Path.Combine(TempDirectory, "people.bin");
                }
                else
                    tempLeader = null;
                this.DialogResult = DialogResult.OK;
                this.Close();
                
            }
            else if (content.Contains(".")){
                
                if(Directory.Exists(Path.Combine(FullPath, content))){
                    FullPath = Path.Combine(FullPath, content);
                    filepath();
                    listItems();
                }
            }
            else
            {
                FullPath = Path.Combine(FullPath, content);
                filepath();
                listItems();
            }


        }

      

        private string zipPath;
        private string picPath;
        private string textPath;
        private string leaderBoard;
        private string TempDirectory;
        private string zipt;
        private System.Windows.Forms.PictureBox pbxPreview;

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (listView1.SelectedItems != null)
            {
                string content = listView1.SelectedItems[0].Text;

                if (content.Contains(".mrb"))
                {
                    zipPath = Path.Combine(FullPath, content);

                    using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                    {

                        if (TempDirectory != null)
                        {
                            RemoveDirectory();
                        }

                        TempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                        Directory.CreateDirectory(TempDirectory);


                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            entry.ExtractToFile(Path.Combine(TempDirectory, entry.FullName), true);
                        }
                    }

                    string[] linesFromFile = System.IO.File.ReadAllLines(Path.Combine(TempDirectory, "puzzle.txt"));
                    string[] pieces = linesFromFile[0].Split(' ');

                    label2.Text = pieces[0];
                    label4.Text = pieces[1];
                    label6.Text = pieces[1];
                    label8.Text = pieces[2];


                    using (Image tempImage = Image.FromFile(Path.Combine(TempDirectory, "puzzle.jpg")))
                    {
                        int sizew = tempImage.Width;
                        int sizeh = tempImage.Height;
                        double ratio;
                        if (sizew > sizeh)
                        {
                            ratio = sizew / (double)sizeh;
                            sizeh = sizew / (int)ratio;
                        }
                        else
                        {
                            ratio = sizeh / (double)sizew;
                            sizew = sizeh / (int)ratio;

                        }


                        this.pbxPreview = new System.Windows.Forms.PictureBox();
                        this.pbxPreview.Location = new System.Drawing.Point(10, 40);
                        this.pbxPreview.Name = "pbxPreview";
                        this.pbxPreview.Size = new System.Drawing.Size(200, 200);
                        this.pbxPreview.TabIndex = 4;
                        this.pbxPreview.TabStop = false;
                        this.Controls.Add(this.pbxPreview);

                        Bitmap bm = new Bitmap(pbxPreview.Width, pbxPreview.Height);
                        Rectangle r = new Rectangle(0, 0, pbxPreview.Width, pbxPreview.Height);
                        Graphics g = Graphics.FromImage(bm);
                        // g.DrawImage(tempImage, r, 0, 0, tempImage.Width, tempImage.Height, GraphicsUnit.Pixel);
                        g.DrawImage(tempImage, r, 0, 0, sizew, sizeh, GraphicsUnit.Pixel);

                        pbxPreview.Image = bm;
                    }
                }
            }
        }

        private void RemoveDirectory()
        {
            pbxPreview.Dispose();
            pbxPreview.Image = null;
            label2.Text = "0";
            label4.Text = "0";
            label6.Text = "0";
            label8.Text = "0";

            DirectoryInfo dirInfo = new DirectoryInfo(TempDirectory);
            FileInfo[] fileList = dirInfo.GetFiles();
            foreach (FileInfo file in fileList)
            {
                File.Delete(file.FullName);
            }
            Directory.Delete(TempDirectory);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileToOpen = listView1.SelectedItems[0].Text;

            if (fileToOpen.Contains(".mrb"))
            {
                zipPath = Path.Combine(FullPath, fileToOpen);

                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {

                    if (TempDirectory != null)
                    {
                        RemoveDirectory();
                    }

                    TempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                    Directory.CreateDirectory(TempDirectory);


                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        entry.ExtractToFile(Path.Combine(TempDirectory, entry.FullName), true);
                    }
                }
                tempzip = zipPath;
                tempPath = Path.Combine(TempDirectory, "puzzle.jpg");
                rettextPath = Path.Combine(TempDirectory, "puzzle.txt");
                if (Path.Combine(TempDirectory, "people.bin") != null)
                {
                    tempLeader = Path.Combine(TempDirectory, "people.bin");
                }
                else
                    tempLeader = null;
                this.DialogResult = DialogResult.OK;
                this.Close();
               
            }
            else if (fileToOpen.Contains("."))
            {
                if (Directory.Exists(Path.Combine(FullPath, fileToOpen)))
                {
                    FullPath = Path.Combine(FullPath, fileToOpen);
                    filepath();
                    listItems();
                }
            }
            else
            {
                FullPath = Path.Combine(FullPath, fileToOpen);
                filepath();
                listItems();
            }
        }
    }
}
