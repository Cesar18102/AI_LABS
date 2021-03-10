using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.IO;

using ScottPlot;
using System;

namespace AI_LAB_2.Forms
{
    public partial class PlotForm : Form
    {
        public PlotForm(Plot plot, string label)
        {
            InitializeComponent();
            PlotPictureBox.Image = plot.Render();

            PlotPictureBox.Image.Save(Environment.CurrentDirectory + "/Out/" + label + ".jpg");
        }

        public PlotForm(IEnumerable<Plot> plots, string label)
        {
            int i = 1;

            InitializeComponent();
            PlotPictureBox.Image = plots.First().Render();
            PlotPictureBox.Image.Save(Environment.CurrentDirectory + "/Out/" + label + i + ".jpg");

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

                ++i;
                picture.Image.Save(Environment.CurrentDirectory + "/Out/" + label + i + ".jpg");
            }
        }
    }
}
