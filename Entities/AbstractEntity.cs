using System.Drawing;

namespace Pong.Entities
{
    public abstract class AbstractEntity
    {
        #region Constructor

        public AbstractEntity(float acceleration, PointF location, SizeF size)
        {
            Acceleration = acceleration;
            Location = location;
            Size = size;
        }

        #endregion

        #region Attributes and Properties

        protected float Acceleration { get; set; }
        public PointF Location { get; protected set; }
        public SizeF Size { get; private set; }
        public PointF Speed { get; protected set; }

        #endregion
    }
}
