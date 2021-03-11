using Pong.Utility;
using System;
using System.Drawing;

namespace Pong.Entities
{
    public class Ball : AbstractEntity
    {
        #region Constructor

        public Ball(float acceleration, PointF location, float radius)
            : base(acceleration, location, new SizeF(radius, radius))
        {
            this._random = new Random();
        }

        #endregion

        #region Attributes and Properties

        public float Radius
        {
            get { return Size.Width / 2; }            
        }

        #endregion

        #region Private Fields

        private readonly Random _random;

        #endregion

        #region Private Methods

        private void Accelerate(out PointF nextLocation, out PointF nextSpeed, float deltaTime)
        {
            var nXSpeed = Speed.X + (Acceleration * deltaTime) * (Speed.X > 0 ? 1 : -1);
            var nYSpeed = Speed.Y + (Acceleration * deltaTime) * (Speed.Y > 0 ? 1 : -1);

            var nx = Location.X + (nXSpeed * deltaTime);
            var ny = Location.Y + (nYSpeed * deltaTime);

            nextLocation = new PointF(nx, ny);
            nextSpeed = new PointF(nXSpeed, nYSpeed);
        }

        private void TopDownCollision(ref PointF nextLocation, ref PointF nextSpeed)
        {
            if ((Location.Y - Radius) < 0)
            {
                nextLocation.Y = 0f + Radius;
                nextSpeed.Y *= -1f;
            }
            else if ((Location.Y + Radius) > 600)
            {
                nextLocation.Y = 600f - Radius;
                nextSpeed.Y *= -1f;
            }
        }

        private bool PlayerCollision(Racquet racquet, ref PointF nextLocation, ref PointF nextSpeed)
        {
            var xRect = racquet.Location.X - (racquet.Size.Width / 2f);
            var yRect = racquet.Location.Y - (racquet.Size.Height / 2f);
            var rectangle = new RectangleF(new PointF(xRect, yRect), racquet.Size);

            return PlayerHorizontalCollision(ref nextLocation, ref nextSpeed, rectangle) ||
                   PlayerVerticalCollision(ref nextLocation, ref nextSpeed, rectangle);
        }

        private bool PlayerHorizontalCollision(ref PointF nextLocation, ref PointF nextSpeed, RectangleF rectangle)
        {
            var left_direction = (nextLocation.X - Location.X) < 0;
            var right_direction = (nextLocation.X - Location.X) > 0;
            if (left_direction)
            {
                var intersection = MathFunctions.LinesIntersection(Location,
                    nextLocation,
                    new PointF(rectangle.Right + Radius, rectangle.Top - Radius),
                    new PointF(rectangle.Right + Radius, rectangle.Bottom + Radius));

                if (!intersection.IsEmpty)
                {
                    nextLocation.X = intersection.X;
                    nextSpeed.X *= -1f;
                    return true;
                }
            }
            else if (right_direction)
            {
                var intersection = MathFunctions.LinesIntersection(Location,
                    nextLocation,
                    new PointF(rectangle.Left - Radius, rectangle.Top - Radius),
                    new PointF(rectangle.Left - Radius, rectangle.Bottom + Radius));

                if (!intersection.IsEmpty)
                {
                    nextLocation.X = intersection.X;
                    nextSpeed.X *= -1f;
                    return true;
                }
            }

            return false;
        }

        private bool PlayerVerticalCollision(ref PointF nextLocation, ref PointF nextSpeed, RectangleF rectangle)
        {
            var up_direction = (nextLocation.Y - Location.Y) < 0;
            var down_direction = (nextLocation.Y - Location.Y) > 0;
            if (up_direction)
            {
                var intersection = MathFunctions.LinesIntersection(Location,
                    nextLocation,
                    new PointF(rectangle.Left - Radius, rectangle.Bottom + Radius),
                    new PointF(rectangle.Right + Radius, rectangle.Bottom + Radius));

                if (!intersection.IsEmpty)
                {
                    nextLocation.Y = intersection.Y;
                    nextSpeed.Y *= -1f;
                    return true;
                }
            }
            else if (down_direction)
            {
                var intersection = MathFunctions.LinesIntersection(Location,
                    nextLocation,
                    new PointF(rectangle.Left - Radius, rectangle.Top - Radius),
                    new PointF(rectangle.Right + Radius, rectangle.Top - Radius));

                if (!intersection.IsEmpty)
                {
                    nextLocation.Y = intersection.Y;
                    nextSpeed.Y *= -1f;
                    return true;
                }
            }

            return false;
        }

        private void Spin(Racquet racquet, ref PointF nextSpeed)
        {
            var xVarSpeed = (((float)_random.NextDouble() - .5f) * 2f) * (10f / 100f);
            var yVarSpeed = (float)_random.NextDouble() * (20f / 100f);

            if (racquet.Speed.Y < 0)
                yVarSpeed *= -1;

            var xSpeed = nextSpeed.X * (1 + xVarSpeed);
            var ySpeed = nextSpeed.Y * (1 + yVarSpeed);

            nextSpeed = new PointF(xSpeed, ySpeed);
        }

        #endregion

        #region Public Methods

        public void PrepareToServe(Racquet racquet)
        {
            float x;
            if (racquet.Location.X > 400)
                x = racquet.Location.X - Radius - (racquet.Size.Width / 2f);
            else
                x = racquet.Location.X + Radius + (racquet.Size.Width / 2f);

            var y = racquet.Location.Y;
            
            Speed = new PointF();
            Location = new PointF(x, y);
        }

        public void Render(Graphics graphics)
        {
            var halfSize = new SizeF(Size.Width / 2f, Size.Height / 2f);
            var brush = new SolidBrush(Color.White);
            var position = new PointF(Location.X - halfSize.Width, Location.Y - halfSize.Height);
            var rectangle = new RectangleF(position, Size);
            graphics.FillEllipse(brush, rectangle);
        }

        public void Serve(Racquet racquet, float serviceSpeed)
        {
            var xVarSpeed = (((float)_random.NextDouble() - .5f) * 2f) * (20f / 100f);
            xVarSpeed += 1f;
            var yVarSpeed = (((float)_random.NextDouble() - .5f) * 2f) * (20f / 100f);
            yVarSpeed += 1f;

            var xSpeed = serviceSpeed * xVarSpeed;
            var ySpeed = serviceSpeed * yVarSpeed;

            if (racquet.Location.X > 400)
                xSpeed *= -1f;

            if (racquet.Speed.Y == 0f)
                ySpeed *= (_random.NextDouble() > 0.5) ? 1f : -1f;
            else if (racquet.Speed.Y < 0f)
                ySpeed *= -1;


            Speed = new PointF(xSpeed, ySpeed);
        }
        
        public void Update(Racquet left, Racquet right, float deltaTime)
        {
            Accelerate(out var nextLocation, out var nextSpeed, deltaTime);
            TopDownCollision(ref nextLocation, ref nextSpeed);

            if (Speed.X < 0)
            {
                if (PlayerCollision(left, ref nextLocation, ref nextSpeed))
                    Spin(left, ref nextSpeed);
            }
            else
            {
                if (PlayerCollision(right, ref nextLocation, ref nextSpeed))
                    Spin(right, ref nextSpeed);
            }


            Location = nextLocation;
            Speed = nextSpeed;
        }

        #endregion
    }
}
