using ScottPlot;

using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace AI_LAB_2.Extensions
{
    public static class PlotExtensions
    {
        public static void AddScatterPoints(this Plot plot, IEnumerable<PointF> points, Color? color = null, float markerSize = 5, MarkerShape markerShape = MarkerShape.filledCircle, string label = null)
        {
            plot.AddScatterPoints(
                points.Select(p => (double)p.X).ToArray(),
                points.Select(p => (double)p.Y).ToArray(),
                color, markerSize, markerShape, label
            );
        }
    }
}
