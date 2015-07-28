using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;

namespace slimdx_gamepad
{
    /// <summary>
    /// Helper class for dealing with thumbstick normalization
    /// </summary>
    class Vector2
    {
        public double X;
        public double Y;

        public Vector2()
        {
            X = 1;
            Y = 1;
        }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator /(Vector2 vector, double denominator)
        {
            return new Vector2(vector.X / denominator, vector.Y / denominator);
        }

        public static Vector2 operator *(Vector2 vector, double operand)
        {
            return new Vector2(vector.X * operand, vector.Y * operand);
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public Vector2 Direction()
        {
            double magnitude = this.Length();
            if (magnitude == 0)
            {
                magnitude = 1;
            }
            return this / magnitude;
        }

        /// <summary>
        /// Normalizes the vector with a certain threshold
        /// </summary>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public Vector2 Normalize(short threshold)
        {
            double normalizedMagnitude = 0;
            if (this.Length() - threshold > 0)
            {
                normalizedMagnitude = Math.Min((this.Length() - threshold) / (short.MaxValue - threshold), 1);
            }
            return this.Direction() * normalizedMagnitude;
        }
    }
}
