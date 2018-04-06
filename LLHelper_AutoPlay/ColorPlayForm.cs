using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LLHelper_AutoPlay
{
    public partial class ColorPlayForm : Form
    {
        const string calibrationImagePath = "calibration.bmp";

        private Setting setting;

        private bool isRun = false;

        private int fps = 0;
        private int fpsPre = 0;
        private long fpsPreTick = 0;

        private int gameX;
        private int gameY;
        private int gameWidth = 1280;
        private int gameHeight = 720;

        private int colorThreshold = 15;
        private int colorThreshold3;

        private int radiusOutside = 75;
        private int radiusInside = 50;
        private bool[,] radiusRange = null;
        private int radiusRangeSize = 0;
        private int radiusPointInterval = 4;
        private Vector2Int[] circleCenter = new Vector2Int[]
        {
            new Vector2Int(190,180),
            new Vector2Int(224,352),
            new Vector2Int(322,497),
            new Vector2Int(467,595),
            new Vector2Int(640,630),
            new Vector2Int(812,595),
            new Vector2Int(957,497),
            new Vector2Int(1055,352),
            new Vector2Int(1090,180),
        };

        private Vector2Int startXY;
        private Vector2Int radiusOutV;
        private Vector2Int radiusInV;

        private float[] keyCtrl = new float[9];

        private int[,] targetColor = new int[3, 3]
        {
            //smile
            {255,77,235},
            //pure
            {125,255,52},
            //cool
            {52,219,255},
        };

        private ScreenStateLogger ssl = new ScreenStateLogger();

        public ColorPlayForm(Setting setting)
        {
            InitializeComponent();
            this.setting = setting;

            radiusOutV = new Vector2Int(radiusOutside, radiusOutside);
            radiusInV = new Vector2Int(radiusInside, radiusInside);

            colorThreshold3 = colorThreshold * colorThreshold * colorThreshold;

            for (int i = 0; i < targetColor.GetLength(0); i++)
            {
                for (int t = 0; t < targetColor.GetLength(1); t++)
                {
                    targetColor[i, t] *= targetColor[i, t];
                }
            }

            ssl.onScreenRefreshed += OnScreenRefreshed;
        }

        private void Btn_GetPos_Click(object sender, EventArgs e)
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;

            if (!File.Exists(calibrationImagePath))
            {
                MessageBox.Show("未找到定位图文件calibration.bmp");
                MessageBox.Show("请参照文件夹中的calibrationSample.png来制作模拟器左上角的定位图,并保存在文件夹中,命名为calibration.bmp. \r\n注意需要保存为24位位图(*.bmp),模拟器分辨率为1280*720");
                return;
            }

            Bitmap calibration = Image.FromFile(calibrationImagePath) as Bitmap;
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bitmap);

            g.CopyFromScreen(0, 0, 0, 0, new Size(width, height));
            pictureBox1.Image = bitmap;

            List<Point> p = BmpColor.FindPic(0, 0, width, height, bitmap, calibration);

            if (p == null || p.Count == 0)
            {
                MessageBox.Show("桌面上找不到定位图");
                return;
            }

            gameX = p[0].X;
            gameY = p[0].Y;

            startXY = new Vector2Int(gameX, gameY);


            bitmap = new Bitmap(gameWidth, gameHeight, PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(gameX, gameY, 0, 0, new Size(gameWidth, gameHeight));
            pictureBox1.Image = bitmap;

            btn_GetPos.Text = "已定位";
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            Start();
        }

        public void Start()
        {
            if (btn_GetPos.Text != "已定位") return;

            btn_Start.Text = "运行中";
            isRun = true;


            Bitmap b = new Bitmap(gameWidth, gameHeight, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(b);
            for (int i = 0; i < circleCenter.Length; i++)
            {
                //g.CopyFromScreen(startXY + circleCenter[i] - radiusOutV, circleCenter[i] - radiusOutV, radiusOutV * 2);
                g.CopyFromScreen(gameX, gameY, 0, 0, new Size(gameWidth, gameHeight));
            }
            for (int i = 0; i < circleCenter.Length; i++)
            {
                g.DrawEllipse(new Pen(Color.Red), new Rectangle(circleCenter[i] - radiusOutV, radiusOutV * 2));
                g.DrawEllipse(new Pen(Color.Red), new Rectangle(circleCenter[i] - radiusInV, radiusInV * 2));
            }
            radiusRange = new bool[radiusOutside * 2, radiusOutside * 2];
            int sum = 0;
            for (int i = 0; i < radiusRange.GetLength(0); i++)
            {
                for (int t = 0; t < radiusRange.GetLength(1); t++)
                {
                    Vector2Int p = new Vector2Int(i, t) - radiusOutV;
                    radiusRange[i, t] = p.Length < radiusOutside && p.Length > radiusInside;
                    sum += radiusRange[i, t] ? 1 : 0;
                }
            }
            radiusRangeSize = sum;
            radiusRangeSize /= radiusPointInterval * radiusPointInterval;

            Console.WriteLine(radiusRangeSize);
            pictureBox1.Image = b;


            for (int i = 0; i < targetColor.GetLength(0); i++)
            {
                for (int t = 0; t < targetColor.GetLength(1); t++)
                {
                    Console.WriteLine(targetColor[i, t]);
                }
            }

            new Thread(ThreadPlay).Start();
            new Thread(ThreadPercent).Start();
            ssl.Start();
        }

        private void OnScreenRefreshed(IntPtr imagePtr, int width)
        {
            unsafe
            {
                byte* p0 = (byte*)imagePtr.ToPointer();

                for (int k = 0; k < 9; k++)
                {
                    int matches = 0;
                    int sum = 0;
                    Vector2Int leftTop = circleCenter[k] - radiusOutV + startXY;

                    for (int i = 0; i < radiusOutside * 2; i += radiusPointInterval)
                    {
                        for (int t = 0; t < radiusOutside * 2; t += radiusPointInterval)
                        {
                            if (radiusRange[i, t])
                            {
                                sum++;
                                int x = i + leftTop.x;
                                int y = t + leftTop.y;

                                int pos = x + y * width;
                                pos *= 4;

                                int r = p0[pos + 2];
                                int g = p0[pos + 1];
                                int b = p0[pos + 0];

                                for (int j = 0; j < 3; j++)
                                {
                                    float value = Math.Abs(r * r - targetColor[j, 0]) * 0.299f + Math.Abs(g * g - targetColor[j, 1]) * 0.587f + Math.Abs(b * b - targetColor[j, 2]) * 0.114f;
                                    if (value < colorThreshold3)
                                    {
                                        matches++;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //Console.WriteLine(matches + " " + sum);


                    keyCtrl[k] = matches / (float)sum;
                }
            }

            fpsPre++;
            if (DateTime.Now.Ticks - fpsPreTick > 10000000)
            {
                fpsPreTick = DateTime.Now.Ticks;
                fps = fpsPre;
                fpsPre = 0;
                try
                {
                    lab_FPS.Text = fps.ToString();
                }
                catch { }
            }
        }

        public void Stop()
        {
            btn_Start.Text = "启动";
            isRun = false;
            ssl.Stop();
            fps = 0;
            fpsPre = 0;
            lab_FPS.Text = "";
            lab_Percent.Text = "";
        }

        private void ThreadPlay()
        {
            while (isRun)
            {
                Thread.Sleep(1);
                for (int i = 0; i < 9; i++)
                {
                    if (keyCtrl[i] > 0.10f)
                    {
                        Win32API.keybd_event(setting.pos2key[(byte)(9 - i)], 0, 0, 0);
                    }
                    else
                    {
                        Win32API.keybd_event(setting.pos2key[(byte)(9 - i)], 0, 2, 0);
                    }
                }
            }
        }

        private void ThreadPercent()
        {
            while (isRun)
            {
                string s = "";
                for (int i = 0; i < 9; i++)
                {
                    s += keyCtrl[i].ToString("F3").PadRight(7, ' ');
                }

                try
                {
                    lab_Percent.Text = s;
                }
                catch { }
                Thread.Sleep(20);
            }
        }

        private void ColorPlayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRun = false;
        }
    }
}
