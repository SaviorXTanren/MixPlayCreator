using System;

namespace MixPlayCreator.Base.Util
{
    public static class MathHelper
    {
        public static int Clamp(int number, int min, int max)
        {
            return Math.Min(Math.Max(number, min), max);
        }

        public static double Clamp(double number, double min, double max)
        {
            return Math.Min(Math.Max(number, min), max);
        }
    }
}
