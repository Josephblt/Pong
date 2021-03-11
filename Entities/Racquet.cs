using System;
using System.Drawing;

namespace Pong.Entities
{
    public class Racquet : AbstractEntity
    {
        #region Constructor

        public Racquet(float acceleration, PointF location, SizeF size) 
            : base(acceleration, location, size)
        {
        }

        #endregion

        #region Private Methods

        private void TopDownCollision(ref float nextLocation, ref float nextSpeed)
        {
            if (nextSpeed < 0 && (Location.Y - (Size.Height / 2f)) <= 0f)
                nextLocation = 0f + (Size.Height / 2f);
            else if (nextSpeed > 0 && (Location.Y + (Size.Height / 2f)) >= 600f)
                nextLocation = 600f - (Size.Height / 2f);
        }

        #endregion

        #region Public Methods

        public void Render(Graphics graphics)
        {
            var halfSize = new SizeF(Size.Width / 2f, Size.Height / 2f);
            var brush = new SolidBrush(Color.White);
            var position = new PointF(Location.X - halfSize.Width, Location.Y - halfSize.Height);
            var rectangle = new RectangleF(position, Size);
            graphics.FillRectangle(brush, rectangle);
        }

        
        public void Update(float targetHeight, float deltaTime)
        {
            var tDistance = targetHeight - Location.Y;
            float ySpeed;
            if (tDistance < 0f)
                ySpeed = -Acceleration;
            else
                ySpeed = Acceleration;

            var nDistance = (ySpeed * deltaTime);
            if (Math.Abs(nDistance) > Math.Abs(tDistance))
            {
                nDistance = tDistance;
                ySpeed = 0f;
            }
                
            var nextY = Location.Y + nDistance;
            TopDownCollision(ref nextY, ref ySpeed);

            Speed = new PointF(0f, ySpeed);
            Location = new PointF(Location.X, nextY);
        }

        #endregion
    }
}
