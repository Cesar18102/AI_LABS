using System;

namespace AI_LAB_2.Extensions
{
    public static class RandomExstensions
    {
        public static (float x, float y) NextNormalPair(this Random random, double stdev, double meanX, double meanY)
        {
            double r = random.NextDouble();
            double fi = random.NextDouble();

            double sqrtHelper = Math.Sqrt(-2 * Math.Log(r));

            double z0 = Math.Cos(2 * Math.PI * fi) * sqrtHelper;
            double z1 = Math.Sin(2 * Math.PI * fi) * sqrtHelper;

            float x = (float)(meanX + stdev * z0);
            float y = (float)(meanY + stdev * z1);

            return (x, y);
        }

        public static double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
}
