using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoAnnotatot
{
    public partial class VideoAnnotationControl : UserControl
    {
        public VideoAnnotationControl()
        {
            InitializeComponent();
            this.axWindowsMediaPlayer1.uiMode = "None";
            this.axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
            
            //this.axWindowsMediaPlayer1.settings.autoStart = true;

        }

        private void AxWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {


            if (e.newState == (int)WMPLib.WMPPlayState.wmppsReady & this.axWindowsMediaPlayer1.Ctlcontrols.currentItem != null)
            {
                this.axWindowsMediaPlayer1.Ctlcontrols.play();
                
                this.gantChart1.Enabled = true;
                this.gantChart1.Refresh();
            }
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.gantChart1.duration = this.axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                this.gantChart1.FPS = this.axWindowsMediaPlayer1.network.encodedFrameRate;
                Debug.WriteLine(this.gantChart1.duration);
                this.gantChart1.Refresh();
                timer.Interval = 100;
                timer.Tick += new EventHandler(timer1_Tick);
                timer.Start();
            }
        }

        private void gantChart1_Load(object sender, EventArgs e)
        {


        }
        bool Show_Frame = false;
        private void gantChart1_MouseMove(object sender, MouseEventArgs e)
        {
            Debug.WriteLine(this.gantChart1.manager.move_which);
            if (_filename != String.Empty && this.gantChart1.manager.move_which!="" && this.gantChart1.manager.hit_point.X!=-1)
            {
                double _position = 0;
                if (this.gantChart1.manager.move_which == "start")
                {
                    _position = this.gantChart1.manager.ganttChartItem.StartPoint.X;
                }
                if (this.gantChart1.manager.move_which == "end")
                {
                    _position = this.gantChart1.manager.ganttChartItem.EndPoint.X;
                }
                if (this.gantChart1.manager.move_which == "block")
                {
                    _position = e.Location.X;
                }
                
                this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition = Math.Round(((double)_position / ((double)this.gantChart1.Width)) * this.axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration,1);
                Show_Frame = true;
                Debug.WriteLine(_position);
            }
            else
            {
                this.axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
        }

        private string _filename = String.Empty;
        public string Filename
        {
            set { _filename = value;
                if (_filename != String.Empty)
                {
                    this.axWindowsMediaPlayer1.URL = _filename;
                    this.gantChart1.manager._collection.Clear();
                    this.gantChart1.manager.move_which = "";
                    this.gantChart1.manager.distance_from_corner =-1;
                    this.gantChart1.manager.ganttChartItem = new GanttChartItem(); ;
                    this.gantChart1.manager.hit_point = new Point(-1,-1);
                    this.gantChart1.Enabled = true;


                }
                else { this.gantChart1.Enabled = false; }
            }
            get { return _filename; }
        }
        public string Label
        {
            set
            {
                this.gantChart1.label = value;
            }
            get { return this.gantChart1.label; }
        }

        private void gantChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (Show_Frame && e.Button == MouseButtons.Left)
            {
                this.axWindowsMediaPlayer1.Ctlcontrols.play();

                this.axWindowsMediaPlayer1.Ctlcontrols.pause();
            }
        }

        private void gantChart1_MouseUp(object sender, MouseEventArgs e)
        {
            //Show_Frame = false;
        }
        private Timer timer = new Timer();
        private void axWindowsMediaPlayer1_DoubleClickEvent(object sender, AxWMPLib._WMPOCXEvents_DoubleClickEvent e)
        {
            this.axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.gantChart1.PlayerSign = new Point((int)Math.Round((this.axWindowsMediaPlayer1.Ctlcontrols.currentPosition / this.axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration) * this.gantChart1.Width, 0), 0);
                this.gantChart1.duration = this.axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                this.gantChart1.FPS = this.axWindowsMediaPlayer1.network.encodedFrameRate;
                Debug.WriteLine(this.gantChart1.PlayerSign);
                this.gantChart1.Refresh();
            }

            else if (this.axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                this.gantChart1.PlayerSign = new Point(0, 0);
                timer.Stop();
                this.gantChart1.Refresh();

            }
        }
            
        private void axWindowsMediaPlayer1_PositionChange(object sender, AxWMPLib._WMPOCXEvents_PositionChangeEvent e)
        {
            if (this.axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                this.gantChart1.PlayerSign = new Point((int)Math.Round((e.oldPosition / this.axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration) * this.gantChart1.Width, 0), 0);

                this.gantChart1.Refresh();
            }
        }

        public void SaveAnnotation(bool ShowDialog)
        {
            var __filename_ = new System.IO.FileInfo(_filename);
            string _ext=__filename_.Extension;
            string _to_store_json =__filename_.FullName.Replace(_ext, ".json");
            //this.gantChart1.duration = this.axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
            //Debug.WriteLine(this.gantChart1.duration);
            //this.gantChart1.FPS = this.axWindowsMediaPlayer1.network.encodedFrameRate;
            if (System.IO.File.Exists(_to_store_json) & ShowDialog)
            {
                DialogResult dr = MessageBox.Show("Do you want to overwrite?", "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                if (dr == DialogResult.No) return;
            }
            Debug.WriteLine(_to_store_json);

            this.gantChart1.manager.Save(_to_store_json);
        }

        public void LoadAnnotation()
        {
            var __filename_ = new System.IO.FileInfo(_filename);
            string _ext = __filename_.Extension;
            string _to_store_json = __filename_.FullName.Replace(_ext, ".json");
            if (System.IO.File.Exists(_to_store_json))
            {
                this.gantChart1.manager.Load(_to_store_json);
            }
            this.gantChart1.Refresh();
        }
    }
}
