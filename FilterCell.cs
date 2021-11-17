using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageFilter
{
    class FilterCell : Panel
    {
        public FilterCell(String filterName, Bitmap original) : base()
        {
            this.Size = new Size(150, 180);
            this.Dock = DockStyle.None;
            this.Anchor = AnchorStyles.None;

            PictureBox picture = new PictureBox();
            picture.Image = original;
            picture.SizeMode = PictureBoxSizeMode.CenterImage;
            picture.Size = new Size(150, 150);
            picture.Location = new Point(0, 0);
            this.Controls.Add(picture);

            Label label = new Label();
            label.Text = filterName;
            label.Font = new Font("Microsoft Sans Serif", 12F);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Location = new Point(0, 150);
            label.Size = new Size(150, 30);
            this.Controls.Add(label);
        }
    }
}
