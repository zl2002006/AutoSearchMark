using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace HDevelop
{
    class Match
    {

        //private HWindow                       Window;
        //private HFramegrabber                 Framegrabber;
        //private int                           ImgWidth, ImgHeight;
        //private HRegion                       Rectangle, ModelRegion;
        //private double                        Row, Column;
        //private double                        Rect1Row, Rect1Col, Rect2Row, Rect2Col;
        //private double                        RectPhi, RectLength1, RectLength2;
        //private HalconDotNet.HWindowControl   WindowControl;
        //private HShapeModel                   ShapeModel;

        //private void WindowControl_HInitWindow(object sender, System.EventArgs e)
        //{
        //    string ImgType;

        //    Window = WindowControl.HalconWindow;
        //    Framegrabber = new HFramegrabber("File", 1, 1, 0, 0, 0, 0, "default",
        //        -1, "default", -1, "default",
        //        "board/board.seq", "default", 1, -1);
        //    Img = Framegrabber.GrabImage();
        //    Img.DispObj(Window);
        //    Img.GetImagePointer1(out ImgType, out ImgWidth, out ImgHeight);
        //    Window.SetDraw("margin");
        //    Window.SetLineWidth(3);
        //    Rectangle = new HRegion(188.0, 182, 298, 412);
        //    Rectangle.AreaCenter(out Row, out Column);
        //    Rect1Row = Row - 102;
        //    Rect1Col = Column + 5;
        //    Rect2Row = Row + 107;
        //    Rect2Col = Column + 5;
        //    RectPhi = 0;
        //    RectLength1 = 170;
        //    RectLength2 = 5;
        //}
            HShapeModel ShapeModel;
            HRegion ModelRegion;
            double rowOrg, colOrg;

            public void CreateModel(HImage Img, HWindow window, HWindow window1,double row1, double col1, double row2, double col2, double rowMark, double colMark)
		{
            HRegion rectangle = new HRegion(row1,col1,row2,col2);
            rectangle.AreaCenter(out rowOrg, out colOrg);
			HImage  ImgReduced=new HImage();
            //HRegion Rectangle1 = new HRegion();
            //HRegion Rectangle2 = new HRegion();

            //Window.SetColor("red");
            //Window.SetDraw("margin");
            //Window.SetLineWidth(3);
            //Rectangle.GenRectangle1(188.0, 182, 298, 412);
                ImgReduced = Img.ReduceDomain(rectangle);
                ImgReduced.InspectShapeModel(out ModelRegion, 1, 30);

            //Rectangle1.GenRectangle2(Rect1Row, Rect1Col, RectPhi, RectLength1, RectLength2);
            //Rectangle2.GenRectangle2(Rect2Row, Rect2Col, RectPhi, RectLength1, RectLength2);
			ShapeModel = new HShapeModel(ImgReduced, 4, 0, 0,
				0, "none", "use_polarity", 30, 10);
            ShapeModel.SetShapeModelOrigin(rowMark - rowOrg, colMark - colOrg);

            //window.SetColor("green");
            //window.SetDraw("margin");
            window.SetPart((int)row1, (int)col1, (int)row2, (int)col2);
            ModelRegion.DispObj(window);
            window1.SetPart((int)row1, (int)col1, (int)row2, (int)col2);
            ImgReduced.DispImage(window1);
            //Window.SetColor("blue");
            //Window.SetDraw("margin");
            //Rectangle1.DispObj(Window);
            //Rectangle2.DispObj(Window);
		}
        public void FindShapeModel(HImage Img, HWindow window, double row1, double col1, double row2, double col2, double rowMark, double colMark)
        {
                double            S1, S2;
                HTuple            RowCheck, ColumnCheck, AngleCheck, Score;
                HHomMat2D         Matrix = new HHomMat2D();
                HRegion           ModelRegionTrans;
                HTuple row1Check, col1Check;
                HTuple row2Check, col2Check;
                HRegion           Rectangle1 = new HRegion();
                //HRegion           Rectangle2 = new HRegion();
                //HMeasure          Measure1, Measure2;
                //HTuple            RowEdgeFirst1, ColumnEdgeFirst1;
                //HTuple            AmplitudeFirst1, RowEdgeSecond1;
                //HTuple            ColumnEdgeSecond1, AmplitudeSecond1;
                //HTuple            IntraDistance1, InterDistance1;
                //HTuple            RowEdgeFirst2, ColumnEdgeFirst2;
                //HTuple            AmplitudeFirst2, RowEdgeSecond2;
                //HTuple            ColumnEdgeSecond2, AmplitudeSecond2;
                //HTuple            IntraDistance2, InterDistance2;
                //HTuple            MinDistance;
                //int               NumLeads;

                //HSystem.SetSystem("flush_graphic", "false");
                //Img.GrabImage(Framegrabber);
                //Img.DispObj(Window);

                // Find the IC in the current image.
                S1 = HSystem.CountSeconds();
                ShapeModel.FindShapeModel(Img, 0, 
                    new HTuple(360).TupleRad().D, 
                    0.7, 1, 0.5, "least_squares",
                    4, 0.9, out RowCheck, out ColumnCheck,
                    out AngleCheck, out Score);
                S2 = HSystem.CountSeconds();
                if (RowCheck.Length == 1)
                {
                    //MatchingScoreLabel.Text = "Score: " +
                    //    String.Format("{0:F5}", Score.D);
                    // Rotate the model for visualization purposes.
                    //创建严格的仿射变换VectorAngleToRigid
                    Matrix.VectorAngleToRigid(new HTuple((row1+row2)/2), new HTuple((col1+col2)/2), new HTuple(0.0),
                        RowCheck-rowMark+rowOrg, ColumnCheck-colMark+colOrg, AngleCheck);
                    //根据Matrix变换ModelRegion
                    ModelRegionTrans = ModelRegion.AffineTransRegion(Matrix, "false");
                    window.SetColor("red");
                    window.SetDraw("margin");
                    window.SetLineWidth(2);
                    ModelRegionTrans.DispObj(window);
                    // Compute the parameters of the measurement rectangles.
                    Matrix.AffineTransPixel(new HTuple(row1), new HTuple(col1),
                        out row1Check, out col1Check);
                    Matrix.AffineTransPixel(new HTuple(row2), new HTuple(col2),
                        out row2Check, out col2Check);
                    Rectangle1.GenRectangle1(row1Check, col1Check, row2Check, col2Check);
                    window.SetColor("green");
                    window.SetDraw("margin");
                    window.SetLineWidth(1);
                    Rectangle1.DispObj(window);
                    window.SetColor("gold");
                    window.DispLine(RowCheck-15, ColumnCheck, RowCheck+15, ColumnCheck);
                    window.DispLine(RowCheck, ColumnCheck - 15, RowCheck, ColumnCheck + 45);
                    

                    // For visualization purposes, generate the two rectangles as
                    // regions and display them.
                    //Rectangle1.GenRectangle2(Rect1RowCheck.D, Rect1ColCheck.D,
                    //    RectPhi + AngleCheck.D,
                    //    RectLength1, RectLength2);
                    //Rectangle2.GenRectangle2(Rect2RowCheck.D, Rect2ColCheck.D,
                    //    RectPhi + AngleCheck.D,
                    //    RectLength1, RectLength2);
                    //window.SetColor("blue");
                    //window.SetDraw("margin");
                    //Rectangle1.DispObj(window);
                    //Rectangle2.DispObj(window);

                }
                //MatchingTimeLabel.Text = "Time: " +
                //    String.Format("{0,4:F1}", (S2 - S1)*1000) + "ms";
                //MatchingScoreLabel.Text = "Score: ";
                //{
                //    MatchingScoreLabel.Text = "Score: " +
                //        String.Format("{0:F5}", Score.D);
                //    // Rotate the model for visualization purposes.
                //    Matrix.VectorAngleToRigid(new HTuple(Row), new HTuple(Column), new HTuple(0.0), 
                //        RowCheck, ColumnCheck, AngleCheck);

                //    ModelRegionTrans = ModelRegion.AffineTransRegion(Matrix, "false");
                    //window.SetColor("green");
                    //window.SetDraw("fill");
                    ////ModelRegionTrans.DispObj(window);
        }
        

        //private void Timer_Tick(object sender, System.EventArgs e)
        //{
        //    Action();
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();
        //}

        //private void Action()
        //{
        //    double            S1, S2;
        //    HTuple            RowCheck, ColumnCheck, AngleCheck, Score;
        //    HHomMat2D         Matrix = new HHomMat2D();
        //    HRegion           ModelRegionTrans;
        //    HTuple            Rect1RowCheck, Rect1ColCheck;
        //    HTuple            Rect2RowCheck, Rect2ColCheck;
        //    HRegion           Rectangle1 = new HRegion();
        //    HRegion           Rectangle2 = new HRegion();
        //    HMeasure          Measure1, Measure2;
        //    HTuple            RowEdgeFirst1, ColumnEdgeFirst1;
        //    HTuple            AmplitudeFirst1, RowEdgeSecond1;
        //    HTuple            ColumnEdgeSecond1, AmplitudeSecond1;
        //    HTuple            IntraDistance1, InterDistance1;
        //    HTuple            RowEdgeFirst2, ColumnEdgeFirst2;
        //    HTuple            AmplitudeFirst2, RowEdgeSecond2;
        //    HTuple            ColumnEdgeSecond2, AmplitudeSecond2;
        //    HTuple            IntraDistance2, InterDistance2;
        //    HTuple            MinDistance;
        //    int               NumLeads;

        //    HSystem.SetSystem("flush_graphic", "false");
        //    Img.GrabImage(Framegrabber);
        //    Img.DispObj(Window);

        //    // Find the IC in the current image.
        //    S1 = HSystem.CountSeconds();
        //    ShapeModel.FindShapeModel(Img, 0, 
        //        new HTuple(360).TupleRad().D, 
        //        0.7, 1, 0.5, "least_squares",
        //        4, 0.9, out RowCheck, out ColumnCheck,
        //        out AngleCheck, out Score);
        //    S2 = HSystem.CountSeconds();
        //    MatchingTimeLabel.Text = "Time: " +
        //        String.Format("{0,4:F1}", (S2 - S1)*1000) + "ms";
        //    MatchingScoreLabel.Text = "Score: ";
      
            //if (RowCheck.Length == 1)
        //    {
        //        MatchingScoreLabel.Text = "Score: " +
        //            String.Format("{0:F5}", Score.D);
        //        // Rotate the model for visualization purposes.
        //        Matrix.VectorAngleToRigid(new HTuple(Row), new HTuple(Column), new HTuple(0.0), 
        //            RowCheck, ColumnCheck, AngleCheck);

        //        ModelRegionTrans = ModelRegion.AffineTransRegion(Matrix, "false");
        //        Window.SetColor("green");
        //        Window.SetDraw("fill");
        //        ModelRegionTrans.DispObj(Window);
        //        // Compute the parameters of the measurement rectangles.
        //        Matrix.AffineTransPixel(Rect1Row, Rect1Col, 
        //            out Rect1RowCheck, out Rect1ColCheck);
        //        Matrix.AffineTransPixel(Rect2Row, Rect2Col, 
        //            out Rect2RowCheck, out Rect2ColCheck);

        //        // For visualization purposes, generate the two rectangles as
        //        // regions and display them.
        //        Rectangle1.GenRectangle2(Rect1RowCheck.D, Rect1ColCheck.D, 
        //            RectPhi + AngleCheck.D, 
        //            RectLength1, RectLength2);
        //        Rectangle2.GenRectangle2(Rect2RowCheck.D, Rect2ColCheck.D, 
        //            RectPhi + AngleCheck.D, 
        //            RectLength1, RectLength2);
        //        Window.SetColor("blue");
        //        Window.SetDraw("margin");
        //        Rectangle1.DispObj(Window);
        //        Rectangle2.DispObj(Window);
        //        // Do the actual masurements.
        //        S1 = HSystem.CountSeconds();
        //        Measure1 = new HMeasure(Rect1RowCheck.D, Rect1ColCheck.D, 
        //            RectPhi + AngleCheck.D, 
        //            RectLength1, RectLength2, 
        //            ImgWidth, ImgHeight, "bilinear");
        //        Measure2 = new HMeasure(Rect2RowCheck.D, Rect2ColCheck.D, 
        //            RectPhi + AngleCheck.D, 
        //            RectLength1, RectLength2, 
        //            ImgWidth, ImgHeight, "bilinear");
        //        Measure1.MeasurePairs(Img, 2, 90,
        //            "positive", "all",
        //            out RowEdgeFirst1,
        //            out ColumnEdgeFirst1,
        //            out AmplitudeFirst1,
        //            out RowEdgeSecond1,
        //            out ColumnEdgeSecond1,
        //            out AmplitudeSecond1,
        //            out IntraDistance1,
        //            out InterDistance1);
        //        Measure2.MeasurePairs(Img, 2, 90,
        //            "positive", "all",
        //            out RowEdgeFirst2,
        //            out ColumnEdgeFirst2,
        //            out AmplitudeFirst2,
        //            out RowEdgeSecond2,
        //            out ColumnEdgeSecond2,
        //            out AmplitudeSecond2,
        //            out IntraDistance2,
        //            out InterDistance2);
        //        S2 = HSystem.CountSeconds();
        //        MeasureTimeLabel.Text = "Time: " +
        //            String.Format("{0,5:F1}", (S2 - S1)*1000) + "ms";
        //        Window.SetColor("red");
        //        Window.DispLine(RowEdgeFirst1 - RectLength2*Math.Cos(AngleCheck),
        //            ColumnEdgeFirst1 - RectLength2*Math.Sin(AngleCheck),
        //            RowEdgeFirst1 + RectLength2*Math.Cos(AngleCheck),
        //            ColumnEdgeFirst1 + RectLength2*Math.Sin(AngleCheck));
        //        Window.DispLine(RowEdgeSecond1 - RectLength2*Math.Cos(AngleCheck),
        //            ColumnEdgeSecond1 - RectLength2*Math.Sin(AngleCheck),
        //            RowEdgeSecond1 + RectLength2*Math.Cos(AngleCheck),
        //            ColumnEdgeSecond1 + RectLength2*Math.Sin(AngleCheck));
        //        Window.DispLine(RowEdgeFirst2 - RectLength2*Math.Cos(AngleCheck),
        //            ColumnEdgeFirst2 - RectLength2*Math.Sin(AngleCheck),
        //            RowEdgeFirst2 + RectLength2*Math.Cos(AngleCheck),
        //            ColumnEdgeFirst2 + RectLength2*Math.Sin(AngleCheck));
        //        Window.DispLine(RowEdgeSecond2 - RectLength2*Math.Cos(AngleCheck),
        //            ColumnEdgeSecond2 - RectLength2*Math.Sin(AngleCheck),
        //            RowEdgeSecond2 + RectLength2*Math.Cos(AngleCheck),
        //            ColumnEdgeSecond2 + RectLength2*Math.Sin(AngleCheck));
        //        NumLeads = IntraDistance1.Length + IntraDistance2.Length;
        //        MeasureNumLabel.Text = "Number of leads: " +
        //            String.Format("{0:D2}", NumLeads);
        //        MinDistance = InterDistance1.TupleConcat(InterDistance2).TupleMin();
        //        MeasureDistLabel.Text = "Minimum lead distance: " +
        //            String.Format("{0:F3}", MinDistance.D);
        //        HSystem.SetSystem("flush_graphic", "true");
        //        // Force the graphics window to be updated by displaying an empty circle.
        //        Window.DispCircle(-1.0, -1.0, 0.0);
        //    }
        //}
	}
}
