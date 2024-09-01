using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace Paint
{
    public class CustomStroke : Stroke
    {
        public CustomStroke(StylusPointCollection pts) : base(pts)
        {
            this.StylusPoints = pts;
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            if (drawingContext == null)
            {
                throw new ArgumentNullException("drawingContext");
            }
            if (null == drawingAttributes)
            {
                throw new ArgumentNullException("drawingAttributes");
            }
            DrawingAttributes originalDa = drawingAttributes.Clone();
            SolidColorBrush brush2 = new SolidColorBrush(drawingAttributes.Color);
            brush2.Freeze();
            //drawingContext.DrawRectangle(brush2, null, new Rect(GetTheLeftTopPoint(), GetTheRightBottomPoint()));

            StylusPoint stp = this.StylusPoints[0];
            StylusPoint sp = this.StylusPoints[1];
            double radius = System.Math.Sqrt(System.Math.Pow((double)(sp.X - stp.X), 2) + System.Math.Pow((double)(sp.Y - stp.Y), 2)) / 2.0;

            drawingContext.DrawEllipse(brush2, null, new Point((sp.X + stp.X) / 2.0, (sp.Y + stp.Y) / 2.0), radius, radius);
        }

        System.Windows.Point GetTheLeftTopPoint()
        {
            if (this.StylusPoints == null)
                throw new ArgumentNullException("StylusPoints");
            StylusPoint tmpPoint = new StylusPoint(double.MaxValue, double.MaxValue);
            foreach (StylusPoint point in this.StylusPoints)
            {
                if ((point.X < tmpPoint.X) || (point.Y < tmpPoint.Y))
                    tmpPoint = point;
            }
            return tmpPoint.ToPoint();
        }

        System.Windows.Point GetTheRightBottomPoint()
        {
            if (this.StylusPoints == null)
                throw new ArgumentNullException("StylusPoints");
            StylusPoint tmpPoint = new StylusPoint(0, 0);
            foreach (StylusPoint point in this.StylusPoints)
            {
                if ((point.X > tmpPoint.X) || (point.Y > tmpPoint.Y))
                    tmpPoint = point;
            }
            return tmpPoint.ToPoint();
        }
    }
}
