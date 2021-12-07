using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoAnnotatot
{
    public partial class GantChart : UserControl
    {
        public GanttChartDataSource manager = new GanttChartDataSource();
        private Point _point = new Point();
        public GantChart()
        {
            InitializeComponent();
            _Color_Label.Add("tumor", Brushes.DarkRed);
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);

        }
        Point _PlayerSign = new Point(0, 0);
        public Point PlayerSign
        {
            get { return _PlayerSign; }
            set { _PlayerSign = value;
                this.Update();
            }
        }
        private double _duration = 0;
        public double duration
        {
            get { return _duration; }
            set { _duration = value;}
        }

        private double _fps = 0;
        public double FPS
        {
            get { return _fps; }
            set { _fps = value; }
        }

        private string _label = "";
        public string label
        {
            get { return _label; }
            set { _label = value; }
        }
        public Dictionary<string, Brush> _Color_Label = new Dictionary<string, Brush>();
        private void GantChart_Paint(object sender, PaintEventArgs e)
        {
                foreach (GanttChartItem item in this.manager._collection)
                {
                    e.Graphics.FillRectangle(Brushes.Gray, item.Block);

                    e.Graphics.FillRectangle(Brushes.Red, item.StartPoint);
                    e.Graphics.DrawRectangle(Pens.Gray, item.StartPoint);
                    e.Graphics.FillRectangle(Brushes.Red, item.EndPoint);
                    e.Graphics.DrawRectangle(Pens.Gray, item.EndPoint);

                }
                e.Graphics.FillRectangle(Brushes.Green, new Rectangle(PlayerSign, new Size(5, 50)));
        }

        private void GantChart_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    manager.MouseDown(e.Location, _duration, _label, _fps, this.Width);
                    this.Refresh();
                }

                if (e.Button == MouseButtons.Right)
                {
                    int length = manager._collection.Count;
                    for (int i = 0; i < length; i++)
                    {
                        if (manager._collection[i].Block.Contains(e.Location))
                        {
                            DialogResult dr = MessageBox.Show("Do you want to delete this?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.Yes)
                            {
                                manager._collection.RemoveAt(i);
                            }
                        }
                    }
                    this.Refresh();
                }
            } catch{

            }
        }

        private void GantChart_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this, manager.MouseHover(this._point));
        }

        private void GantChart_MouseMove(object sender, MouseEventArgs e)
        {
            _point = e.Location;
            if (e.Button == MouseButtons.Left) { this.manager.MouseMove(e.Location, this.Width); this.Refresh(); }

        }

        private void GantChart_Load(object sender, EventArgs e)
        {
            
        }

        private void GantChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { this.manager.MouseUp(); }
        }

        private void GantChart_Resize(object sender, EventArgs e)
        {
            this.Height = 50;
        }
    }
}
