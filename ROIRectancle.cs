using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace HDevelop
{
    enum Direction
    {
        up,
        down,
        left,
        right
    }
    class ROIRectancle
    {
        public double Row1 { get; set; }//左上角点y坐标
        public double Col1 { get; set; }//左上角点x坐标
        public double Row2 { get; set; }//右下角点y坐标
        public double Col2 { get; set; }//右下角点x坐标
        public double RowMark { get; set; }//mark点y坐标
        public double ColMark { get; set; }//mark点x坐标
        public int Speed { get; set; }//移动速度
        public Direction Dir { get; set; }//移动方向
        private FrmMain frm;
        public ROIRectancle(double row1, double col1, double row2, double col2, double rowMark, double colMark, int speed, Direction dir, FrmMain frm)
        {
            this.Row1 = row1;
            this.Col1 = col1;
            this.Row2 = row2;
            this.Col2 = col2;
            this.RowMark = rowMark;
            this.ColMark = colMark;
            this.Speed = speed;
            this.Dir = dir;
            this.frm = frm;
        }
        public void Draw(HWindow window, HObject img,bool isShow)
        {
            window.ClearWindow();
            HOperatorSet.DispObj(img,window);
            if (isShow)
            {
                window.SetLineWidth(2);
                window.SetDraw("margin");
                window.SetColor("green");
                window.DispRectangle1(Row1, Col1, Row2, Col2);
                window.SetColor("red");
                window.SetLineWidth(1);
                window.DispRectangle1(RowMark - 5, ColMark - 5, RowMark + 5, ColMark + 5);
                window.DispCross(RowMark, ColMark,35, 0);
                window.SetDraw("fill");
                window.SetColor("blue");
                window.DispRectangle1(Row1 - 6, Col1 - 6, Row1 + 6, Col1 + 6);
                window.DispRectangle1(Row1 - 6, Col2 - 6, Row1 + 6, Col2 + 6);
                window.DispRectangle1(Row2 - 6, Col1 - 6, Row2 + 6, Col1 + 6);
                window.DispRectangle1(Row2 - 6, Col2 - 6, Row2 + 6, Col2 + 6);
                //HOperatorSet.OpenFramegrabber()
            }
        }
        public void Move()//整体移动框框
        {
            switch (this.Dir)
            {
                case Direction.up:
                    Row1 -= this.Speed;
                    Row2 -= this.Speed;
                    break;
                case Direction.down:
                    Row1 += this.Speed;
                    Row2 += this.Speed;
                    break;
                case Direction.left:
                    Col1 -= this.Speed;
                    Col2 -= this.Speed;
                    break;
                case Direction.right:
                    Col1 += this.Speed;
                    Col2 += this.Speed;
                    break;
            }
            if (this.Row1 < 0)
            {
                Row1 += this.Speed;
                Row2 += this.Speed;
                this.Speed = 0;
            }
            else if (this.Row2 > frm.hWindowControl1.ImagePart.Height)
            {
                Row1 -= this.Speed;
                Row2 -= this.Speed;
                this.Speed = 0;
            }
            else if (this.Col1 < 0)
            {
                Col1 += this.Speed;
                Col2 += this.Speed;
                this.Speed = 0;
            }
            else if (this.Col2 > frm.hWindowControl1.ImagePart.Width)
            {
                Col1 -= this.Speed;
                Col2 -= this.Speed;
                this.Speed = 0;
            }
        }
        public void ScaleMove()
        {
            //this.Speed = 20;
            switch (this.Dir)
            {
                case Direction.up:
                    Row1 -= this.Speed;
                    Row2 += this.Speed;
                    break;
                case Direction.down:
                    Row1 += this.Speed;
                    Row2 -= this.Speed;
                    break;
                case Direction.left:
                    Col1 += this.Speed;
                    Col2 -= this.Speed;
                    break;
                case Direction.right:
                    Col1 -= this.Speed;
                    Col2 += this.Speed;
                    break;
            }
            if (Row1 < 0)
            {
                Row1 = 0;
                this.Speed = 0;

            }
            else if (Row2 > frm.hWindowControl1.ImagePart.Height)
            {
                //Row1 -= 0;
                Row2 = frm.hWindowControl1.ImagePart.Height;
                this.Speed = 0;
            }
            else if (Row1 >= Row2)
            {
                Row1 -= this.Speed;
                Row2 += this.Speed;
                this.Speed = 0;
            }
            else if (Col1 >= Col2)
            {
                Col1 -= this.Speed;
                Col2 += this.Speed;
                this.Speed = 0;
            }
            else if (Col1 < 0)
            {
                Col1 = 0;
                this.Speed = 0;

            }
            else if (Col2 > frm.hWindowControl1.ImagePart.Width)
            {
                //Row1 -= 0;
                Col2 = frm.hWindowControl1.ImagePart.Width;
                this.Speed = 0;
            }
        }//比例移动ROI
        public void PointMove()//移动Mark点
        {
            switch (this.Dir)
            {
                case Direction.up:
                    this.RowMark -= this.Speed;
                    break;
                case Direction.down:
                    this.RowMark += this.Speed;
                    break;
                case Direction.left:
                    this.ColMark -= this.Speed;
                    break;
                case Direction.right:
                    this.ColMark += this.Speed;
                    break;
            }
            if (this.RowMark < 0)
            {
                this.RowMark = 0;
            }
            else if (this.RowMark > frm.hWindowControl1.ImagePart.Height)
            {
                this.RowMark = frm.hWindowControl1.ImagePart.Height;
            }
            else if (this.ColMark < 0)
            {
                this.ColMark = 0;
            }
            else if (this.ColMark > frm.hWindowControl1.ImagePart.Width)
            {
                this.ColMark = frm.hWindowControl1.ImagePart.Width;
            }
        }
        //public HRegion GetRegion()
        //{
        //    HRegion region = new HRegion();
        //    region.GenRectangle1(this.Row1, this.Col1, this.Row2, this.Col2);
        //    return region;
        //}
       // HObject region, reduceRegion;
        //HObject reduceRegion;
        //HTuple hv_ModelID;
        //public void CreateModel(HObject ho_img,HWindow window)
        //{
        //    HOperatorSet.GenEmptyObj(out region);
        //    HOperatorSet.GenEmptyObj(out reduceRegion);
        //    region.Dispose();
        //    HOperatorSet.GenRectangle1(out region, (HTuple)this.Row1, (HTuple)this.Col1, (HTuple)this.Row2, (HTuple)this.Col2);
        //    reduceRegion.Dispose();
        //    HOperatorSet.ReduceDomain(ho_img, region, out reduceRegion);
        ////    HOperatorSet.CreateShapeModel(reduceRegion, "auto", 0, 0, "auto", "auto", "use_polarity",
        ////"auto", "auto", out hv_ModelID);
        ////    HOperatorSet.SetShapeModelOrigin(hv_ModelID, (HTuple)(RowMark - (Row1 + Row2) / 2), (HTuple)(ColMark - (Col1 + Col2) / 2));
        ////    HOperatorSet.WriteShapeModel(hv_ModelID, "mk.shm");
        //    //window.DispObj(reduceRegion);
        //    HOperatorSet.DispObj(reduceRegion, window);
        //}
    }
}
