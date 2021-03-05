using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

using ScottPlot;

using AI_LAB_2.Extensions;
using AI_LAB_2.Forms;

namespace AI_LAB_2
{
    public class Program
    {
        private const int CLUSTER_COUNT = 3;

        private const double STD_DEV = 0.6;
        private const int POINT_COUNT = 380;

        private const double MIN_CENTER_DISTANCE = 2;
        private const double MAX_CENTER_DISTANCE = 5;

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

            PlotForm generatedDataPlotForm = new PlotForm(generatedDataPlot);
            generatedDataPlotForm.ShowDialog();


            Plot grayDataPlot = new Plot();

            grayDataPlot.AddScatterPoints(allPoints.ToArray(), label: "Gray points");
            grayDataPlot.Legend();

            PlotForm grayDataPlotForm = new PlotForm(grayDataPlot);
            grayDataPlotForm.ShowDialog();

            KMeansClusterizer clusterizer = new KMeansClusterizer();

            List<Dictionary<PointF, List<PointF>>> clusterizingHistory = clusterizer.Clusterize(allPoints, CLUSTER_COUNT);
            
            PlotForm resultPlotForm = new PlotForm(CreateClusterizingPlot(clusterizingHistory.Last()));
            resultPlotForm.ShowDialog();

            PlotForm historyForm = new PlotForm(clusterizingHistory.Select(c => CreateClusterizingPlot(c)).ToList());
            historyForm.ShowDialog();

            Console.ReadLine();
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
