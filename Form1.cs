using ImageFilter.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageFilter
{
    public partial class Form1 : Form
    {
        private Bitmap original;
        private string originalPath;
        private PictureBox[] filterPictures;
        private Label[] filterNames;
        public Form1()
        {
            InitializeComponent();
            filterPictures = new PictureBox[]{pictureBox2, pictureBox3, pictureBox4,
                pictureBox5, pictureBox6, pictureBox7,pictureBox8, pictureBox9, pictureBox10};
            filterNames = new Label[] {label2, label3, label4, label5, label6, label7, label8, label9, label10};
            for(int i=0; i < 9; i++)
            {
                filterNames[i].Text = Filters.names[i];
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = Filters.names[i];
                item.Click += new EventHandler((s, ee) => 
                    ChooseFilter(filtersToolStripMenuItem.DropDownItems.IndexOf(item)));
                filtersToolStripMenuItem.DropDownItems.Add(item);
            }
            var bmp = new Bitmap(ImageFilter.Properties.Resources.image);
            changeOriginalImage(bmp);
            ChooseFilter(0);
        }

        private void changeOriginalImage(Image image)
        {
            original = new Bitmap(image);
            pictureBox1.Image = original;
            Filters.SetOriginal(original);
            for (int i = 0; i < filterNames.Length; i++)
            {
                filterPictures[i].Image = Filters.FilterSmall(i);
            }
        }

        private int choosenFilter = 0;
        private void ChooseFilter(int filterInd)
        {
            filterNames[choosenFilter].BackColor = SystemColors.Control;
            choosenFilter = filterInd;
            filterNames[choosenFilter].BackColor = Color.Yellow;
            pictureBox1.Image = Filters.Filter(original, choosenFilter);
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ChooseFilter(0);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ChooseFilter(1);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ChooseFilter(2);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ChooseFilter(3);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ChooseFilter(4);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            ChooseFilter(5);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ChooseFilter(6);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            ChooseFilter(7);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            ChooseFilter(8);
        }

        private void chooseImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                originalPath = openFileDialog1.FileName;
                using (Image img = Image.FromFile(originalPath))
                {
                    changeOriginalImage(img);
                    openFileDialog1.Dispose();
                    saveToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image bm = Filters.Filter(original, choosenFilter);
            Console.WriteLine("Path is HERE: " + originalPath);
            bm.Save(originalPath);
            MessageBox.Show("Saved");
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image bm = Filters.Filter(original, choosenFilter);
                bm.Save(saveFileDialog1.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/DosU13/Image-Filters");
        }
    }
}
