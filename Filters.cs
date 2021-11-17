using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;

namespace ImageFilter
{
    static class Filters 
    {
        public static readonly String[] names = new String[]{"Original", "Grayscale", "Sepia", "Negative", 
            "Black and White", "Polaroid", "Sunny", "Cold", "Sea"};
        private static Image original;

        public static void SetOriginal(Bitmap originalBig)
        {
            original = ResizeImage(originalBig, new Size(150,150));
                new Bitmap(150, 150, PixelFormat.Format32bppArgb);
        }

        public static Image FilterSmall(int index)
        {
            return Filter(original, index);
        }

        public static Image Filter(Image image, int filterInd)
        {
            switch (filterInd)
            {
                case 0: return image;
                case 1: return DrawAsGrayscale(image);
                case 2: return DrawAsSepiaTone(image);
                case 3: return DrawAsNegative(image);
                case 4: return DrawAsBlackAndWhite(image);
                case 5: return DrawAsPolaroid(image);
                case 6: return DrawAsSunny(image);
                case 7: return DrawAsCold(image);
                case 8: return DrawAsSea(image);
                default: return GetArgbCopy(image);
            }
        }


        public static Image ResizeImage(Image imgToResize, Size destinationSize)
        {
            var originalWidth = imgToResize.Width;
            var originalHeight = imgToResize.Height;

            //how many units are there to make the original length
            var hRatio = (float)originalHeight / destinationSize.Height;
            var wRatio = (float)originalWidth / destinationSize.Width;

            //get the shorter side
            var ratio = Math.Min(hRatio, wRatio);

            var hScale = Convert.ToInt32(destinationSize.Height * ratio);
            var wScale = Convert.ToInt32(destinationSize.Width * ratio);

            //start cropping from the center
            var startX = (originalWidth - wScale) / 2;
            var startY = (originalHeight - hScale) / 2;

            //crop the image from the specified location and size
            var sourceRectangle = new Rectangle(startX, startY, wScale, hScale);

            //the future size of the image
            var bitmap = new Bitmap(destinationSize.Width, destinationSize.Height);

            //fill-in the whole bitmap
            var destinationRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            //generate the new image
            using (var g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, destinationRectangle, sourceRectangle, GraphicsUnit.Pixel);
            }
            return bitmap;
        }


        private static Bitmap ApplyColorMatrix(Image sourceImage, ColorMatrix colorMatrix)
        {
            Bitmap bmp32BppSource = GetArgbCopy(sourceImage);
            Bitmap bmp32BppDest = new Bitmap(bmp32BppSource.Width, bmp32BppSource.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bmp32BppDest))
            {
                ImageAttributes bmpAttributes = new ImageAttributes();
                bmpAttributes.SetColorMatrix(colorMatrix);

                graphics.DrawImage(bmp32BppSource, new Rectangle(0, 0, bmp32BppSource.Width, bmp32BppSource.Height),
                                    0, 0, bmp32BppSource.Width, bmp32BppSource.Height, GraphicsUnit.Pixel, bmpAttributes);
            }
            bmp32BppSource.Dispose();
            return bmp32BppDest;
        }

        private static Bitmap GetArgbCopy(Image sourceImage)
        {
            Bitmap bmpNew = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bmpNew))
            {
                graphics.DrawImage(sourceImage, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), GraphicsUnit.Pixel);
                graphics.Flush();
            }
            return bmpNew;
        }

        public static Bitmap DrawAsBrightness(this Image sourceImage, float b)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                {
                            new float[]{1, 0, 0, 0, 0},
                            new float[]{0, 1, 0, 0, 0},
                            new float[]{0, 0, 1, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{b, b, b, 0, 1}
                                });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsContrast(this Image sourceImage, float c)
        {
            float t = (1f - c) / 2f;
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                {
                            new float[]{c, 0, 0, 0, 0},
                            new float[]{0, c, 0, 0, 0},
                            new float[]{0, 0, c, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{t, t, t, 0, 1}
                                });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsSaturation(this Image sourceImage, float s)
        {
            float sr = (1f - s)*0.3086f;
            float sg = (1f - s) * 0.6094f;
            float sb = (1f - s) * 0.0820f;
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                {
                            new float[]{sr+s, sr, sr, 0, 0},
                            new float[]{sg, sg+s, sg, 0, 0},
                            new float[]{sb, sb, sb+s, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{0, 0, 0, 0, 1}
                                });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsGrayscale(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                {
                            new float[]{.3f, .3f, .3f, 0, 0},
                            new float[]{.59f, .59f, .59f, 0, 0},
                            new float[]{.11f, .11f, .11f, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{0, 0, 0, 0, 1}
                                });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsSepiaTone(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                       {
                        new float[]{.393f, .349f, .272f, 0, 0},
                        new float[]{.769f, .686f, .534f, 0, 0},
                        new float[]{.189f, .168f, .131f, 0, 0},
                        new float[]{0, 0, 0, 1, 0},
                        new float[]{0, 0, 0, 0, 1}
                       });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsNegative(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                           {
                            new float[]{-1, 0, 0, 0, 0},
                            new float[]{0, -1, 0, 0, 0},
                            new float[]{0, 0, -1, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{1, 1, 1, 1, 1}
                           });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsBlackAndWhite(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                           {
                            new float[]{1.5f, 1.5f, 1.5f, 0, 0},
                            new float[]{1.5f,1.5f,1.5f,0,0},
                            new float[]{1.5f,1.5f,1.5f,0,0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{ -1,-1,-1,0,1 }
                           });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsPolaroid(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                           {
                            new float[]{1.438f, -0.062f, -0.062f, 0f, 0},
                            new float[]{-0.122f,1.378f,-0.122f,0f,0},
                            new float[]{-0.016f,-0.016f,1.483f,0f,0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{ -0.03f,0.05f,-0.02f,0f,1 }
                           });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsSunny(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                           {
                            new float[]{1, 0, 0, 0, 0},
                            new float[]{0, 1, 0, 0, 0},
                            new float[]{0, 0, 1, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{0.2f, 0.2f, 0, 0, 1}
                           });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsCold(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                           {
                            new float[]{-1, 0, 0, 0, 0},
                            new float[]{0, 1, 0, 0, 0},
                            new float[]{0, 0, 1, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{0, 0, 0.2f, 0, 1}
                           });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }

        public static Bitmap DrawAsSea(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                           {
                            new float[]{-1, 0, 0, 0, 0},
                            new float[]{0, 1, 0, 0, 0},
                            new float[]{0, 0, 1, 0, 0},
                            new float[]{0, 0, 0, 1, 0},
                            new float[]{0, 0.2f, 0, 0, 1}
                           });
            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
    }
}
