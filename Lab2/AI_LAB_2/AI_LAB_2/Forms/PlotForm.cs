using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

using ScottPlot;

namespace AI_LAB_2.Forms
{
    public partial class PlotForm : Form
    {
        public PlotForm(Plot plot)
        {
            InitializeComponent();
            PlotPictureBox.Image = plot.Render();
        }

        public PlotForm(IEnumerable<Plot> plots)
        {
            InitializeComponent();
            PlotPictureBox.Image = plots.First().Render();

            int lastY = PlotPictureBox.Location.Y;

            IEnumerable<Plot> leftPlots = plots.Skip(1);
            foreach(Plot plot in leftPlots)
            {
                PictureBox picture = new PictureBox();

                picture.Size = PlotPictureBox.Size;
                picture.Location = new Point(
                    PlotPictureBox.Location.X,
                    lastY + PlotPictureBox.Size.Height
                );

                picture.Image = plot.Render();

                lastY = picture.Location.Y;

                this.Controls.Add(picture);
            }
        }
    }
}
