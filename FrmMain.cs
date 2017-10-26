using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Threading;

namespace HDevelop
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }


        //public ROIRectancle ROI { get; set; }
        bool isFind = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowImg();
            if (isFind)
            {
                Find(HobjectToHimage(ig.ho_img));
            }
        }





        private void btnTeach_Click(object sender, EventArgs e)
        {
            //isStart = false;
            //ig.ShowImg(hWindowControl1.HalconWindow, isStart);
            //SetPatten sp = new SetPatten();
            if (sp == null)
            {
                sp = new SetPatten();
            }
            sp.ShowDialog();
        }
        SetPatten sp = new SetPatten();
        ROIRectancle roi = null;
        //bool isStart = true;
        bool isShow = false;
        ImageGrab ig = null;
        private void FrmMain_Load(object sender, EventArgs e)
        {
            skinEngine1.SkinFile = @"C:\Users\1\Desktop\C#项目\皮肤控件\皮肤\Emerald\Emerald.ssk";
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            if (ig == null)
            {
                ig = new ImageGrab();
            }
            ig.ShowImg();
            Control.CheckForIllegalCrossThreadCalls = true;
            if (roi == null)
            {
                roi = new ROIRectancle(150, 250, 280, 450, 200, 390, 20, Direction.up, this);
            }
            sp.btnUp.Click += btnUp_Click;
            sp.btnDown.Click += btnDown_Click;
            sp.btnLeft.Click += btnLeft_Click;
            sp.btnRight.Click += btnRight_Click;
            sp.btnShow.Click += btnShow_Click;
            sp.btnSave.Click += btnSave_Click;
            sp.btnExecu.Click += btnExecu_Click;
            sp.btnSpeed.Click += btnSpeed_Click;
            //sp.btnPoi.Click += btnPoi_Click;

            //ig.
        }
        int speed=20;
        void btnSpeed_Click(object sender, EventArgs e)
        {
            if (sp.btnSpeed.Text == "快")
            {
                speed = 20;
            }
            else if (sp.btnSpeed.Text == "中")
            {
                speed = 5;
            }
            else if (sp.btnSpeed.Text == "慢")
            {
                speed = 1;
            }
        }
        void Find(HImage image)
        {
            match.FindShapeModel(image, hWindowControl1.HalconWindow, roi.Row1, roi.Col1, roi.Row2, roi.Col2, roi.RowMark, roi.ColMark);
        }
        void btnExecu_Click(object sender, EventArgs e)
        {
            Find(image);
        }
        Match match = null;
        private HImage HobjectToHimage(HObject hobject)
        {
            HImage image = new HImage();
            HTuple pointer, type, width, height;
            HOperatorSet.GetImagePointer1(hobject, out pointer, out type, out width, out height);
            image.GenImage1(type, width, height, pointer);
            return image;
        }

        HImage image = null;
        void btnSave_Click(object sender, EventArgs e)
        {
            //hWindowControl2.ImagePart = new Rectangle((int)roi.Row1, (int)roi.Col1, (int)roi.Row2,(int) roi.Col2);
            // hWindowControl2.HalconWindow.DispObj(img);
            //roi.CreateModel(img, hWindowControl2.HalconWindow);
            image = HobjectToHimage(img);

            //HImage image=ig.ho_img as HImage;
            if (match == null)
            {
                match = new Match();
            }
            hWindowControl2.HalconWindow.ClearWindow();
            hWindowControl3.HalconWindow.ClearWindow();
            match.CreateModel(image, hWindowControl2.HalconWindow,hWindowControl3.HalconWindow,roi.Row1, roi.Col1, roi.Row2, roi.Col2, roi.RowMark, roi.ColMark);
        }
        HObject img = null;

        void btnShow_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                img = ig.ho_img;
            }
            timer1.Enabled = false;
            isShow = !isShow;
            roi.Draw(hWindowControl1.HalconWindow, img, isShow);


        }



        void btnRight_Click(object sender, EventArgs e)
        {
            roi.Dir = Direction.right;
            if (sp.btnPoi.Text == "框.移动")
            {
                if (roi.Col2 < hWindowControl1.ImagePart.Width)
                {
                    roi.Speed = speed;
                }
                roi.Move();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }
            else if (sp.btnPoi.Text == "比例")
            {
                if (roi.Col1 > 0 && roi.Col2 < this.hWindowControl1.ImagePart.Width)
                {
                    roi.Speed = speed;
                }
                roi.ScaleMove();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }
            else if (sp.btnPoi.Text == "点.移动")
            {
                roi.PointMove();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }

        }

        void btnLeft_Click(object sender, EventArgs e)
        {
            roi.Dir = Direction.left;
            if (sp.btnPoi.Text == "框.移动")
            {
                if (roi.Col1 > 0)
                {
                    roi.Speed = speed;
                }
                roi.Move();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }
            else if (sp.btnPoi.Text == "比例")
            {
                if (roi.Col1 > roi.Col2)
                {
                    roi.Speed = speed;
                }
                roi.ScaleMove();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }
            else if (sp.btnPoi.Text == "点.移动")
            {
                roi.PointMove();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }

        }

        void btnDown_Click(object sender, EventArgs e)
        {
            roi.Dir = Direction.down;
            if (sp.btnPoi.Text == "框.移动")
            {
                if (roi.Row2 < hWindowControl1.ImagePart.Height)
                {
                    roi.Speed = speed;
                }
                roi.Move();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }
            else if (sp.btnPoi.Text == "比例")
            {
                if (roi.Row1 < roi.Row2)
                {
                    roi.Speed = speed;
                }
                roi.ScaleMove();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }
            else if (sp.btnPoi.Text == "点.移动")
            {
                roi.PointMove();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }

        }

        void btnUp_Click(object sender, EventArgs e)
        {
            roi.Dir = Direction.up;
            if (sp.btnPoi.Text == "框.移动")
            {
                if (roi.Row1 > 0)
                {
                    roi.Speed = speed;
                }
                roi.Move();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }
            else if (sp.btnPoi.Text == "比例")
            {
                if (roi.Row1 > 0 && roi.Row2 < this.hWindowControl1.ImagePart.Height)
                {
                    roi.Speed = speed;
                }
                roi.ScaleMove();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }
            else if (sp.btnPoi.Text == "点.移动")
            {
                roi.PointMove();
                roi.Draw(hWindowControl1.HalconWindow, img, isShow);
            }

        }
        double a, b;
        void ShowImg()
        {
            a = hWindowControl1.ImagePart.Height / 2;
            b = hWindowControl1.ImagePart.Width / 2;
            hWindowControl1.HalconWindow.DispObj(ig.ho_img);
            hWindowControl1.HalconWindow.SetColor("sienna");
            hWindowControl1.HalconWindow.DispCross(a, b, 1000, 0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //hWindowControl1.HalconWindow.ClearWindow();
            if (img != null)
            {
                img = null;
            }
            timer1.Enabled = true;
            isShow = false;
            isFind = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isFind = true;
        }

    }
}
