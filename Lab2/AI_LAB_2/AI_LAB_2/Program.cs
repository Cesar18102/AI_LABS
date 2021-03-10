using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

using ScottPlot;

using AI_LAB_2.Extensions;
using AI_LAB_2.Forms;

using Aglomera;
using Aglomera.D3;
using Aglomera.Linkage;

namespace AI_LAB_2
{
    public class Program
    {
        private const int CLUSTER_COUNT = 3;

        private const double STD_DEV = 0.6;
        private const int POINT_COUNT = 380;

        private const double MIN_CENTER_DISTANCE = 5;
        private const double MAX_CENTER_DISTANCE = 10;

        public static void Main(string[] args)
        {
            Plot generatedDataPlot = new Plot();
            Spawner spawner = new Spawner(STD_DEV);

            List<PointF> allPoints = new List<PointF>();

            for(int i = 0; i < CLUSTER_COUNT; ++i)
            {
                spawner.ResetCenter(MIN_CENTER_DISTANCE, MAX_CENTER_DISTANCE);

                PointF[] points = spawner.Spawn(POINT_COUNT);
                allPoints.AddRange(points);

                Color color = generatedDataPlot.GetNextColor();

                generatedDataPlot.AddScatterPoints(points, color, label: $"Points {i + 1}");
                generatedDataPlot.AddPoint(spawner.Center.X, spawner.Center.Y, color, 25);
            }

            generatedDataPlot.Legend();

            PlotForm generatedDataPlotForm = new PlotForm(generatedDataPlot, "source_data");
            generatedDataPlotForm.ShowDialog();

            Plot grayDataPlot = new Plot();

            grayDataPlot.AddScatterPoints(allPoints.ToArray(), label: "Gray points");
            grayDataPlot.Legend();

            PlotForm grayDataPlotForm = new PlotForm(grayDataPlot, "gray_data");
            grayDataPlotForm.ShowDialog();

            KMeansClusterizer clusterizer = new KMeansClusterizer();

            List<Dictionary<PointF, List<PointF>>> clusterizingHistory = clusterizer.Clusterize(allPoints, CLUSTER_COUNT);
            
            PlotForm resultPlotForm = new PlotForm(CreateClusterizingPlot(clusterizingHistory.Last()), "crusterized");
            resultPlotForm.ShowDialog();

            PlotForm historyForm = new PlotForm(clusterizingHistory.Select(c => CreateClusterizingPlot(c)).ToList(), "history_");
            historyForm.ShowDialog();

            CentroidLinkage<DataPoint> linkage = new CentroidLinkage<DataPoint>(
                new DissimilarityMetric(),
                cluster => new DataPoint(
                    cluster.Average(p => p.X),
                    cluster.Average(p => p.Y)
                )
            );
            AgglomerativeClusteringAlgorithm<DataPoint> algorithm = new AgglomerativeClusteringAlgorithm<DataPoint>(linkage);

            HashSet<DataPoint> dataPoints = allPoints.Select(p => new DataPoint(p)).ToHashSet();
            ClusteringResult<DataPoint> clusteringResult = algorithm.GetClustering(dataPoints);
            ClusterSet<DataPoint> result = clusteringResult[clusteringResult.Count - 3];

            Plot aglomeraPlot = new Plot();

            foreach (Cluster<DataPoint> resultCluster in result)
            {
                Color color = aglomeraPlot.GetNextColor();

                aglomeraPlot.AddScatterPoints(
                    resultCluster.Select(p => (double)p.X).ToArray(),
                    resultCluster.Select(p => (double)p.Y).ToArray(),
                    color
                );

                aglomeraPlot.AddPoint(
                    resultCluster.Select(p => p.X).Average(),
                    resultCluster.Select(p => p.Y).Average(),
                    color, 25
                );
            }

            PlotForm aglomeraForm = new PlotForm(aglomeraPlot, "aglomera");
            aglomeraForm.ShowDialog();

            clusteringResult.SaveD3DendrogramFile(Environment.CurrentDirectory + "/dendro.json");

            Console.ReadLine();
        }

        private class DissimilarityMetric : IDissimilarityMetric<DataPoint>
        {
            public double Calculate(DataPoint instance1, DataPoint instance2)
            {
                return Math.Sqrt(
                    Math.Pow(instance1.X - instance2.X, 2.0) +
                    Math.Pow(instance1.Y - instance2.Y, 2.0)
                );
            }
        }

        public struct DataPoint : IComparable<DataPoint>
        {
            public float X { get; set; }
            public float Y { get; set; }

            public DataPoint(float x, float y)
            {
                X = x;
                Y = y;
            }

            public DataPoint(PointF point)
            {
                X = point.X;
                Y = point.Y;
            }

            public int CompareTo(DataPoint other)
            {
                return (int)Math.Round(X == other.X ? Y - other.Y : X - other.X);
            }
        }

        private static Plot CreateClusterizingPlot(Dictionary<PointF, List<PointF>> data)
        {
            Plot plot = new Plot();

            foreach (KeyValuePair<PointF, List<PointF>> cluster in data)
            {
                Color color = plot.GetNextColor();

                plot.AddPoint(cluster.Key.X, cluster.Key.Y, color, 25);
                plot.AddScatterPoints(cluster.Value, color);
            }

            return plot;
        }
    }
}
