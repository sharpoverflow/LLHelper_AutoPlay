using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace 截图分析器
{
    public partial class Form1 : Form
    {
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            end = true;
            isRun = false;
        }
        public Form1()
        {
            InitializeComponent();
            b = new Bitmap(width, height);
            g = Graphics.FromImage(b);
        }

        public Dictionary<byte, byte> pos2key;
        private string savePath = @"C:\Test\sif\";
        private string xyPath = @"C:\Test\sif\标定4.bmp";
        private int startX, startY;
        private int width = 1080;// Screen.PrimaryScreen.Bounds.Width;
        private int height = 720;// Screen.PrimaryScreen.Bounds.Height;
        private Bitmap b;
        private Graphics g;
        private int c = 0;
        private bool end = false;
        private bool isRun = false;

        private void button1_Click(object sender, EventArgs e)
        {
            pos2key = new Dictionary<byte, byte>();
            pos2key.Add(1, Win32API.Key32.Key_L);
            pos2key.Add(2, Win32API.Key32.Key_K);
            pos2key.Add(3, Win32API.Key32.Key_J);
            pos2key.Add(4, Win32API.Key32.Key_H);
            pos2key.Add(5, Win32API.Key32.Key_Space);
            pos2key.Add(6, Win32API.Key32.Key_F);
            pos2key.Add(7, Win32API.Key32.Key_D);
            pos2key.Add(8, Win32API.Key32.Key_S);
            pos2key.Add(9, Win32API.Key32.Key_A);

            Bitmap xyb = Image.FromFile(xyPath) as Bitmap;
            //MessageBox.Show(xyb.PixelFormat.ToString());
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            b = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(b);
            g.CopyFromScreen(0, 0, 0, 0, new Size(w, h));
            pictureBox1.Image = b;
            //MessageBox.Show(b.PixelFormat.ToString());


            List<Point> p = BmpColor.FindPic(0, 0, w, h, b, xyb);

            if (p == null || p.Count == 0)
            {
                MessageBox.Show("没找到");
                return;
            }

            startX = p[0].X - 1040;////1000
            startY = p[0].Y;
            b = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(b);
            g.CopyFromScreen(startX, startY, 0, 0, new Size(width, height));
            pictureBox1.Image = b;
        }

        private void CutScreen()
        {
            long time = DateTime.Now.Ticks;
            int i = 0;
            List<Bitmap> bl = new List<Bitmap>();
            while (!end)
            {
                b = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                g = Graphics.FromImage(b);
                g.CopyFromScreen(startX, startY, 0, 0, new Size(width, height));
                Thread.Sleep(1);
                i++;
                bl.Add(b);
                if (DateTime.Now.Ticks - time >= 10000000)
                {
                    time = DateTime.Now.Ticks;
                    label1.Text = i.ToString();
                    i = 0;
                    c++;
                    new Thread(SaveImage).Start(bl);
                    bl = new List<Bitmap>();
                }
            }
        }

        private void SaveImage(object obj)
        {
            int k = c;
            List<Bitmap> bl = obj as List<Bitmap>;
            for (int i = 0; i < bl.Count; i++)
            {
                Bitmap bp = bl[i];
                bp.Save(savePath + "/[" + k.ToString("000") + "][" + i.ToString("0000") + "].bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                bp.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Thread(CutScreen).Start();
            savePath += DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
        }


        private int radius = 61;
        private bool[,] range;
        private int rwidth = 9;
        private Vector2Int[] circleCenter = new Vector2Int[]
        {
            new Vector2Int(90,180),
            new Vector2Int(124,352),
            new Vector2Int(222,497),
            new Vector2Int(367,595),
            new Vector2Int(540,630),
            new Vector2Int(712,595),
            new Vector2Int(857,497),
            new Vector2Int(955,352),
            new Vector2Int(990,180),
        };

        private void button3_Click(object sender, EventArgs e)
        {
            Vector2Int startXY = new Vector2Int(startX, startY);
            Vector2Int radiusV = new Vector2Int(radius, radius);
            Vector2Int whhalfB = radiusV + new Vector2Int(rwidth, rwidth);
            Vector2Int whhalfS = radiusV - new Vector2Int(rwidth, rwidth);

            b = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(b);
            for (int i = 0; i < circleCenter.Length; i++)
            {
                g.CopyFromScreen(startXY + circleCenter[i] - radiusV, circleCenter[i] - radiusV, radiusV * 2);
            }
            for (int i = 0; i < circleCenter.Length; i++)
            {
                g.DrawEllipse(new Pen(Color.Red), new Rectangle(circleCenter[i] - radiusV - new Vector2Int(rwidth, rwidth), whhalfB * 2));
                g.DrawEllipse(new Pen(Color.Red), new Rectangle(circleCenter[i] - radiusV + new Vector2Int(rwidth, rwidth), whhalfS * 2));
            }

            range = new bool[radius * 2 + rwidth * 2, radius * 2 + rwidth * 2];
            for (int i = 0; i < range.GetLength(0); i++)
            {
                for (int t = 0; t < range.GetLength(1); t++)
                {
                    Vector2Int p = new Vector2Int(i, t) - whhalfB;
                    range[i, t] = p.length > radius - rwidth && p.length < radius + rwidth;
                }
            }

            pictureBox1.Image = b;

        }

        long stt;
        long cishut = 0;
        long precishu = 0;
        ScreenStateLogger ssl = new ScreenStateLogger();
        private void button4_Click(object sender, EventArgs e)
        {
            //5,124,0;
            isRun = true;
            stt = DateTime.Now.Ticks;

            ssl.ScreenRefreshed += OnBitmap;
            //new Thread(ThreadPlay).Start();
            ssl.Start();
        }

        Bitmap lastBitmap = null;
        private void OnBitmap(Bitmap nb)
        {
            //pictureBox1.Image = bitmap;
            if (lastBitmap != null)
            {
                lastBitmap.Dispose();
                lastBitmap = null;
            }
            lastBitmap = nb;

            int wh = radius * 2 + rwidth * 2;
            Vector2Int startXY = new Vector2Int(startX, startY);
            Vector2Int radiusV = new Vector2Int(radius, radius);
            Vector2Int whhalfB = radiusV + new Vector2Int(rwidth, rwidth);
            Vector2Int whhalfS = radiusV - new Vector2Int(rwidth, rwidth);
            Color pure = Color.FromArgb(0, 187, 0);
            int bwidth = nb.Width;
            BitmapData bData = nb.LockBits(new Rectangle(0, 0, nb.Width, nb.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* scan0 = (byte*)bData.Scan0.ToPointer();

                for (int k = 0; k < 9; k++)
                {
                    int sum = 0;//总数量
                    int fh = 0;//符合条件的数量
                    int fz = 25;//阈值
                    Vector2Int stv = circleCenter[k] - whhalfB + startXY;
                    for (int i = 0; i < wh; i++)
                    {
                        for (int t = 0; t < wh; t++)
                        {
                            if (range[i, t])
                            {
                                sum++;
                                int x = i + stv.x;
                                int y = t + stv.y;

                                int l = x + y * bwidth;
                                l *= 4;

                                if (ColorSim(pure, Color.FromArgb(scan0[l + 2], scan0[l + 1], scan0[l])) <= fz * fz * fz)
                                {
                                    fh++;
                                }
                            }
                        }
                    }
                    keyCtrl[k] = fh / (float)sum;
                }

            }

            //nb.UnlockBits(bData);
            cishut++;
            if (DateTime.Now.Ticks - stt > 10000000)
            {
                stt = DateTime.Now.Ticks;
                precishu = cishut;
                //Console.WriteLine(cishut.ToString("00000000"));
                cishut = 0;
            }
        }

        public unsafe void Test(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            int bytesPerPixel = 4;
            int maxPointerLenght = width * height * bytesPerPixel;
            int stride = width * bytesPerPixel;
            byte R = 0, G = 0, B = 0, A = 0;

            BitmapData bData = bmp.LockBits(
                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, bmp.PixelFormat);


            byte* scan0 = (byte*)bData.Scan0.ToPointer();

            for (int i = 0; i < maxPointerLenght; i += 4)
            {
                B = scan0[i + 0];
                G = scan0[i + 1];
                R = scan0[i + 2];
                A = scan0[i + 3];
            }
            bmp.UnlockBits(bData);
        }


        float[] keyCtrl = new float[9];

        void ThreadPlay(object obj)
        {
            while (isRun)
            {
                Thread.Sleep(1);
                for (int i = 0; i < 9; i++)
                {  
                    if (keyCtrl[i] > 0.10f)
                    {
                        Win32API.keybd_event(pos2key[(byte)(9 - i)], 0, 0, 0);
                    }
                    else
                    {
                        Win32API.keybd_event(pos2key[(byte)(9 - i)], 0, 2, 0);
                    }
                }
                string s = "";
                for (int i = 0; i < 9; i++)
                {
                    s += keyCtrl[i].ToString("F3").PadLeft(8, ' ');
                }
                //Console.WriteLine(s);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            isRun = false;
            ssl.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string s = "";
            for (int i = 0; i < 9; i++)
            {
                s += keyCtrl[i].ToString("F3").PadLeft(16, ' ');
            }
            s += "      帧率" + precishu;
            label2.Text = s;
        }



        //abs( r1 * r1 - r2 * r2 ) * 0.299 + abs( g1 * g1 - g2 * g2 ) + 0.587 + abs( b1 * b1 - b2 * b2 ) + 0.114
        float ColorSim(Color c1, Color c2)
        {
            //8000 return Math.Abs(c1.R * c1.R - c2.R * c2.R) * 0.299f + Math.Abs(c1.G * c1.G - c2.G * c2.G) * 0.587f + Math.Abs(c1.B * c1.B - c2.B * c2.B) * 0.114f;
            return (c1.R - c2.R) * (float)(c1.R - c2.R) + (c1.G - c2.G) * (float)(c1.G - c2.G) + (c1.B - c2.B) * (float)(c1.B - c2.B);
        }

    }
}
