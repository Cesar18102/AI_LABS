using System;
using System.Drawing;

using AI_LAB_2.Extensions;

namespace AI_LAB_2
{
    public class Spawner
    {
        private static Random Random = new Random();

        public PointF Center { get; private set; }
        public double StandardDeviation { get; private set; }

        public Spawner(double standardDeviation)
        {
            StandardDeviation = standardDeviation;

            Center = new PointF(
                (float)Random.NextDouble(),
                (float)Random.NextDouble()
            );
        }

        public void ResetCenter(double minDistanceFromPrevious, double maxDistanceFromPrevious)
        {
            Center = new PointF(
                (float)Random.NextDouble(minDistanceFromPrevious, maxDistanceFromPrevious),
                (float)Random.NextDouble(minDistanceFromPrevious, maxDistanceFromPrevious)
            );
        }

        public PointF[] Spawn(int count)
        {
            PointF[] points = new PointF[count];

            for (int i = 0; i < count; ++i)
            {
                (float x, float y) = Random.NextNormalPair(StandardDeviation, Center.X, Center.Y);
                points[i] = new PointF(x, y);
            }

            return points;
        }
    }
}
