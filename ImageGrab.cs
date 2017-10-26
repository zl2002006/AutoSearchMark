using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.Threading;

namespace HDevelop
{
    class ImageGrab
    {
        //public HObject img = null;
        public HObject ho_img;
        private HTuple acqHandle;
        //public double Y { get; set; }
        //public double X { get; set; }
        public ImageGrab()
        {
            //this.Y = y;
            //this.X = x;
        }
        Thread th = null;

        public void ShowImg()
        {


                HOperatorSet.OpenFramegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb",
        -1, "false", "default", "0", -1, -1, out acqHandle);
                HOperatorSet.GrabImageStart(acqHandle, -1);

                th = new Thread(GrabImg);
                th.IsBackground = true;
                th.Start(); 
            //else
            //{

            //}
        }
        public void CloseGrab()
        {
            if (th != null)
            {
                th.Abort();
            }
            HOperatorSet.CloseFramegrabber(acqHandle);
            ho_img.Dispose();
        }
        //double a = 50;
        //double b = 0;
       void GrabImg()
        {
            //HWindow window = o as HWindow;
            while (true)
            {
                //if (ho_img != null)
                //{ ho_img.Dispose(); }
                HOperatorSet.GrabImageAsync(out ho_img, acqHandle, -1);
                //ho_img.Dispose();
                //HOperatorSet.MirrorImage(ho_img, out ho_img, "column");
                //HOperatorSet.DispObj(ho_img, window);
                //ho_img = img;
                //window.SetColor("sienna");
                //window.DispCross(this.Y/2,this.X/2,1000,0);
            }

        }
    }
}
