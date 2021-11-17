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
        private Image mainPicture;
        private string originalPath;
        private PictureBox[] filterPictures;
        private Label[] filterNames;
        public Form1()
        {
            InitializeComponent();
            filterPictures = new PictureBox[]{filterPic0, filterPic1, filterPic2,
                filterPic3, filterPic4, filterPic5,filterPic6, filterPic7, filterPic8};
            filterNames = new Label[] {filter0, filter1, filter2, filter3, filter4, filter5, filter6, filter7, filter8};
            for(int i=0; i < filterNames.Length; i++)
            {
                filterNames[i].Text = Filters.names[i];
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = Filters.names[i];
                item.Click += new EventHandler((s, ee) => 
                    ChooseFilter(filtersToolStripMenuItem.DropDownItems.IndexOf(item)));
                filtersToolStripMenuItem.DropDownItems.Add(item);
                filterPictures[i].Click += new EventHandler((s, ee) =>
                    ChooseFilter(filtersToolStripMenuItem.DropDownItems.IndexOf(item)));
            }
            var bmp = new Bitmap(ImageFilter.Properties.Resources.image);
            changeOriginalImage(bmp);
            ChooseFilter(0);
        }

        private void changeOriginalImage(Image image)
        {
            original = new Bitmap(image);
            Filters.SetOriginal(original);
            for (int i = 0; i < filterNames.Length; i++)
            {
                filterPictures[i].Image = Filters.FilterSmall(i);
            }
            updatePicture();
        }

        private int choosenFilter = 0;
        private void ChooseFilter(int filterInd)
        {
            filterNames[choosenFilter].BackColor = SystemColors.Control;
            choosenFilter = filterInd;
            filterNames[choosenFilter].BackColor = Color.Yellow;
            updatePicture();
        }

        private void updatePicture()
        {
            Image filtered = Filters.Filter(original, choosenFilter);
            Image brighthened;
            if (brightness == 0) brighthened = filtered;
            else brighthened = Filters.DrawAsBrightness(filtered, brightness);
            Image contrasted;
            if (contrast == 10) contrasted = brighthened;
            else contrasted = Filters.DrawAsContrast(brighthened, contrast);
            if (saturation == 10) mainPicture = contrasted;
            else mainPicture = Filters.DrawAsSaturation(contrasted, saturation);
            pictureBox1.Image = mainPicture;
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
            Image bm = Filters.Filter(mainPicture, choosenFilter);
            Console.WriteLine("Path is HERE: " + originalPath);
            bm.Save(originalPath);
            MessageBox.Show("Saved");
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image bm = Filters.Filter(mainPicture, choosenFilter);
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

        private float brightness = 0f;
        private void trackBarBrightness_ValueChanged(object sender, EventArgs e)
        {
            brightness = ((float)trackBarBrightness.Value-5f)/10f;
            updatePicture();
        }


        private float contrast = 1f;
        private void trackBarContrast_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarContrast.Value < 5)
            {
                contrast = ((float)trackBarContrast.Value + 2f) / 7f;
            }
            else if (trackBarContrast.Value > 5)
            {
                contrast = ((float)trackBarContrast.Value - 3f)/2f; 
            }
            else contrast = 1f;
            updatePicture();
        }


        private float saturation = 1f;
        private void trackBarSaturation_ValueChanged(object sender, EventArgs e)
        {
            if (trackBarSaturation.Value < 5)
            {
                saturation = ((float)trackBarSaturation.Value + 2f) / 7f;
            }
            else if (trackBarSaturation.Value > 5)
            {
                saturation = ((float)trackBarSaturation.Value - 3f) / 2f;
            }
            else saturation = 1f;
            updatePicture();
        }
    }
}
