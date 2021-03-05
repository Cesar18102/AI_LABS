using AI_LAB_2.Extensions;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AI_LAB_2
{
    public class KMeansClusterizer
    {
        private static Random Random = new Random();

        public List<Dictionary<PointF, List<PointF>>> Clusterize(IEnumerable<PointF> data, int clustersCount)
        {
            List<Dictionary<PointF, List<PointF>>> history = new List<Dictionary<PointF, List<PointF>>>();
            List<PointF> centers = new List<PointF>();

            while(centers.Count != clustersCount)
            {
                int index = Random.Next(0, data.Count());
                PointF item = data.ElementAt(index);

                if (centers.Contains(item))
                    continue;

                centers.Add(item);
            }

            while(true)
            {
                Dictionary<PointF, List<PointF>> clusters = Distribute(data, centers);

                IEnumerable<PointF> emptyCenters = clusters.Where(cluster => cluster.Value.Count == 0).Select(kvp => kvp.Key);
                foreach (PointF emptyCenter in emptyCenters)
                {
                    Dictionary<PointF, PointF> nearestByClusters = clusters.ToDictionary(
                        cluster => cluster.Key,
                        cluster => cluster.Value.MinBy(p => GetDistance(p, emptyCenter))
                    );

                    KeyValuePair<PointF, PointF> nearest = nearestByClusters.MinBy(
                        kvp => GetDistance(kvp.Value, emptyCenter)
                    );

                    clusters[emptyCenter].Add(nearest.Value);
                    clusters[nearest.Key].Remove(nearest.Value);
                }

                Dictionary<PointF, List<PointF>> previousClusters = history.LastOrDefault();

                bool finish = previousClusters != null && clusters.Values.All(
                    points => previousClusters.Values.FirstOrDefault(
                        prevPoints => prevPoints.Except(points).Count() == 0
                    ) != null
                );

                history.Add(clusters);

                if (finish)
                    break;

                centers = clusters.Values.Select(cluster => GetCenter(cluster)).ToList();
            }

            return history;
        }

        private Dictionary<PointF, List<PointF>> Distribute(IEnumerable<PointF> data, IEnumerable<PointF> centers)
        {
            Dictionary<PointF, List<PointF>> clusters = centers.ToDictionary(
                center => center,
                canter => new List<PointF>()
            );

            foreach (PointF point in data)
            {
                PointF center = centers.MinBy(c => GetDistance(c, point));
                clusters[center].Add(point);
            }

            return clusters;
        }

        private float GetDistance(PointF point1, PointF point2)
        {
            return (float)Math.Sqrt(
                Math.Pow(point1.X - point2.X, 2.0) +
                Math.Pow(point1.Y - point2.Y, 2.0)
            );
        }

        private PointF GetCenter(IEnumerable<PointF> cluster)
        {
            return new PointF(
                cluster.Select(p => p.X).Average(),
                cluster.Select(p => p.Y).Average()
            );
        }
    }
}
