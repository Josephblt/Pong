using System;
using System.Drawing;

namespace Pong.Utility
{
    public static class MathFunctions
    {
        #region Static Methods

        public static bool Equals(float valueOne, float valueTwo, float threshold = 0.001f)
        {
            return Math.Abs(valueOne - valueTwo) <= threshold;
        }

        public static bool EqualsOrLess(float valueOne, float valueTwo, float threshold = 0.001f)
        {
            return Equals(valueOne, valueTwo, threshold) ||
                   valueOne < valueTwo;
        }

        public static PointF LinesIntersection(PointF P1, PointF P2, PointF P3, PointF P4)
        {
            float d = (P4.Y - P3.Y) * (P2.X - P1.X) -
                      (P4.X - P3.X) * (P2.Y - P1.Y);

            float na = (P4.X - P3.X) * (P1.Y - P3.Y) -
                        (P4.Y - P3.Y) * (P1.X - P3.X);

            float nb = (P2.X - P1.X) * (P1.Y - P3.Y) -
                        (P2.Y - P1.Y) * (P1.X - P3.X);

            if (d == 0)
                return PointF.Empty;

            float ua = na / d;
            float ub = nb / d;

            if (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d)
            {
                var ptIntersection = new PointF
                {
                    X = P1.X + (ua * (P2.X - P1.X)),
                    Y = P1.Y + (ua * (P2.Y - P1.Y))
                };
                return ptIntersection;
            }

            return PointF.Empty; ;
        }

        #endregion
    }
}
