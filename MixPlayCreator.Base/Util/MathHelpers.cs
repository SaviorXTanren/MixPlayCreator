using System;

namespace MixPlayCreator.Base.Util
{
    public static class MathHelper
    {
        public static int Clamp(int number, int min, int max)
        {
            return Math.Min(Math.Max(number, min), max);
        }
    }
}
