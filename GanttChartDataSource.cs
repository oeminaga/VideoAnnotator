using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.IO;
namespace VideoAnnotatot
{
    public class LabelWithColor
    {
        private Brush _color;
        private string _label;
        public Brush Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }
    }
    public class GanttChartItem
    {
        private List<string> _Label = new List<String>();
        private Rectangle _StartPoint = new Rectangle();
        private Rectangle _EndPoint = new Rectangle();
        private Rectangle _Block = new Rectangle();
        private double _StartTime = 0;
        private double _EndTime = 0;
        private double _duration = 0;
        private double _fps = 0;

        private double _max_width = 0;
        public List<string> Label
        { get { return _Label; } set { _Label = value; } }
        public Rectangle StartPoint { get { return _StartPoint; } set { _StartPoint = value; } }
        public Rectangle EndPoint { get { return _EndPoint; } set { _EndPoint = value; } }
        public Rectangle Block { get { return _Block; } set { _Block = value; } }
        public double StartTime { get { return _StartTime; } set { _StartTime = value; } }

        public double Duration { get { return _duration; } set { _duration = value; } }

        public double MaxWidth { get { return _max_width; } set { _max_width = value; } }

        public double FPS { get { return _fps; } set { _fps = value; } }

        public double EndTime { get { return _EndTime; } set { _EndTime = value; } }
        public GanttChartItem() { }
        public GanttChartItem(Point pt, int width, double duration, string label, double fps,double max_width)
        {
            _StartPoint = new Rectangle(new Point(pt.X - width, 0), new Size(width, 50));
            _EndPoint = new Rectangle(new Point(pt.X + width * 1, 0), new Size(width, 50));
            _Block = new Rectangle(new Point(pt.X - width, 0), new Size(width * 2, 50));
            _duration = duration;
            _fps = fps;
            _max_width= max_width;
            _Label.Add(label);

        }
    }
    public class GanttChartDataSource
    {
        int width = 7;
        internal List<GanttChartItem> _collection = new List<GanttChartItem>();
        int _selected_index = -1;
        public GanttChartItem ganttChartItem = new GanttChartItem();
        public string move_which = "";
        public Point hit_point = new Point(-1, -1);
        public int distance_from_corner = 0;
        public bool MouseDown(Point pt, double duration, string label, double fps, double max_width)
        {
            bool _status = false;
            int _counter = 0;
            foreach (GanttChartItem itm in _collection)
            {
                if (itm.StartPoint.Contains(pt))
                {
                    ganttChartItem = itm;
                    _status = true;
                    move_which = "start";
                    hit_point = pt;
                    _selected_index = _counter;
                    distance_from_corner = pt.X - ganttChartItem.StartPoint.X;
                    break;
                }
                else if (itm.EndPoint.Contains(pt))
                {
                    ganttChartItem = itm;
                    _status = true;
                    move_which = "end";
                    hit_point = pt;
                    _selected_index = _counter;
                    distance_from_corner = pt.X - ganttChartItem.EndPoint.X;
                    break;
                }
                else if (itm.Block.Contains(pt))
                {
                    ganttChartItem = itm;
                    _status = true;
                    move_which = "block";
                    hit_point = pt;
                    _selected_index = _counter;
                    distance_from_corner = pt.X - ganttChartItem.Block.X;
                    break;
                }
                _counter += 1;

            }

            if (_status == false)
            {

                AddNewItem(pt, duration, label, fps, max_width);
                _status = true;
            }

            return _status;
        }
        public bool AddNewItem(Point pt, double duration, string label, double fps, double max_width)
        {
            GanttChartItem _new_itm = new GanttChartItem(pt, width, duration, label, fps, max_width);
            _collection.Add(_new_itm);
            return true;

        }
        public string MouseHover(Point pt)
        {
            
            foreach (GanttChartItem itm in _collection)
            {
                if (itm.Block.Contains(pt))
                {
                    var _sb = new StringBuilder();
                    foreach (string lb in itm.Label)
                    {
                        _sb.AppendLine(lb);
                    }
                    return _sb.ToString();
                }
            }
            return "";
        }

        public bool MouseMove(Point pt, int max_width = 1000)
        {
            bool _status = false;
            switch (move_which)
            {
                case "start":
                    {
                        Point new_x = new Point(pt.X - this.distance_from_corner, 0);
                        if (new_x.X + this.width >= this.ganttChartItem.EndPoint.X)
                        {
                            new_x = new Point(this.ganttChartItem.EndPoint.X - width, 0);
                        }
                        if (new_x.X <= 0)
                        {
                            new_x = new Point(0, 0);
                        }
                        int _counter = 0;
                        foreach (GanttChartItem itm in this._collection)
                        {
                            if (_counter != this._selected_index && itm.Block.IntersectsWith(new Rectangle(new Point(new_x.X, 0), new Size(this.ganttChartItem.EndPoint.X - new_x.X + width, 50))))
                            {
                                new_x = new Point(this.ganttChartItem.StartPoint.X, 0);
                                break;
                            }
                            _counter += 1;
                        }
                        this.ganttChartItem.StartPoint = new Rectangle(new_x, new Size(width, 50));
                        this.ganttChartItem.Block = new Rectangle(new Point(new_x.X, 0), new Size(this.ganttChartItem.EndPoint.X - this.ganttChartItem.StartPoint.X + width, 50));
                        _status = true;
                        // this.hit_point = pt;
                        break;
                    }
                case "end":
                    {
                        Point new_x = new Point(pt.X - this.distance_from_corner, 0);
                        if (new_x.X <= this.ganttChartItem.StartPoint.X + width)
                        {
                            new_x = new Point(this.ganttChartItem.StartPoint.X + width, 0);
                        }
                        if (new_x.X >= max_width)
                        {
                            new_x = new Point(max_width - width, 0);
                        }
                        int _counter = 0;
                        foreach (GanttChartItem itm in this._collection)
                        {
                            if (_counter != this._selected_index && itm.Block.IntersectsWith(new Rectangle(new Point(this.ganttChartItem.Block.X, 0), new Size(new_x.X - this.ganttChartItem.StartPoint.X + width, 50))))
                            {
                                new_x = new Point(this.ganttChartItem.EndPoint.X, 0);
                                break;
                            }
                            _counter += 1;
                        }
                        this.ganttChartItem.EndPoint = new Rectangle(new_x, new Size(width, 50));
                        this.ganttChartItem.Block = new Rectangle(new Point(this.ganttChartItem.Block.X, 0), new Size(this.ganttChartItem.EndPoint.X - this.ganttChartItem.StartPoint.X + width, 50));
                        _status = true;
                        break;
                    }
                case "block":
                    {
                        Point new_x = new Point(pt.X - this.distance_from_corner, 0);
                        int delta_ = pt.X - this.hit_point.X;

                        Point new_pt = new Point(this.ganttChartItem.StartPoint.X + delta_, 0);
                        if ((new_pt.X + this.ganttChartItem.Block.Width) >= max_width)
                        {
                            new_pt.X = this.ganttChartItem.StartPoint.X;
                        }
                        if (new_pt.X <= 0)
                        {
                            new_pt.X = 0;
                        }
                        int _counter = 0;
                        foreach (GanttChartItem itm in this._collection)
                        {
                            if (_counter != this._selected_index && itm.Block.IntersectsWith(new Rectangle(new_pt, new Size(this.ganttChartItem.Block.Width, 50))))
                            {
                                new_pt = new Point(this.ganttChartItem.StartPoint.X, 0);
                                break;
                            }
                            _counter += 1;
                        }
                        this.ganttChartItem.StartPoint = new Rectangle(new_pt, new Size(width, 50));
                        this.ganttChartItem.Block = new Rectangle(new_pt, new Size(this.ganttChartItem.Block.Width, 50));
                        this.ganttChartItem.EndPoint = new Rectangle(new Point(this.ganttChartItem.Block.Width + this.ganttChartItem.Block.X - this.width, 0), new Size(width, 50));
                        this.hit_point = pt;
                        _status = true;
                        break;
                    }
            }
            return _status;
        }
        public bool MouseUp()
        {
            this.ganttChartItem = new GanttChartItem();
            this.hit_point = new Point(-1, -1);
            return true;
        }
        
        public void Save(string filename="")
        {
            int length = _collection.Count;
            if (length ==0) return;
            for(int i=0; i<length; i++)
            {
                var itm = _collection[i];
                itm.StartTime = ((double)itm.StartPoint.X / (double)itm.MaxWidth) * itm.Duration;
                itm.EndTime = ((double)itm.EndPoint.X / (double)itm.MaxWidth) * itm.Duration;
                _collection[i] = itm;
            }
            string jsonString = JsonConvert.SerializeObject(_collection);
            
            using (StreamWriter sw = File.CreateText(filename))
                {
                    sw.Write(jsonString);
                }
            Debug.WriteLine(jsonString);
        }
        public void Load(string filename = "")
        {
            using (StreamReader sr = File.OpenText(filename))
            {
                string _txt = sr.ReadToEnd();
                this._collection = (List<GanttChartItem>)JsonConvert.DeserializeObject<IEnumerable<GanttChartItem>>(_txt);
                Debug.WriteLine(sr.ReadToEnd());
            }
        }
    }
}
