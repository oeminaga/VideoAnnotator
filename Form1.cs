using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoAnnotatot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string _current_File = String.Empty;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] _txt = File.ReadAllLines("labels.txt");
            toolStripComboBox1.Items.AddRange(_txt);
            toolStripComboBox1.SelectedIndex = 0;

        }
        private void videoAnnotationControl1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripSeparator1_Click(object sender, EventArgs e)
        {
            using (AboutBox1 frm = new AboutBox1())
            {
                frm.ShowDialog();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_current_File))
            {
                this.videoAnnotationControl1.SaveAnnotation(!this.checkBox1.Checked);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            using (var browserdialog = new FolderBrowserDialog())
            {
                DialogResult dr = browserdialog.ShowDialog();
                if (dr == DialogResult.OK && !string.IsNullOrWhiteSpace(browserdialog.SelectedPath))
                {
                    string[] files = Directory.GetFiles(browserdialog.SelectedPath);
                    List<string> _video_files = new List<string>();
                    foreach (string file in files)
                    {
                        if (file.EndsWith(".mp4") || file.EndsWith(".wmf") || file.EndsWith(".avi") || file.EndsWith(".mov") || file.EndsWith(".mp3") || file.EndsWith(".mpg") | file.EndsWith(".mpeg"))
                        _video_files.Add(file);
                    }
                    
                    var _video_files_ = _video_files.ToArray();
                    this.listBox1.Items.AddRange(_video_files_);
                    int _length = this.listBox1.Items.Count;
                    for (int i=0; i<_length; i++)
                    {
                        var fl = new FileInfo((string)this.listBox1.Items[i]);
                        string _fl_ =fl.FullName.Replace(fl.Extension, ".json");
                        if (File.Exists(_fl_))
                        {
                            this.listBox1.SetItemChecked(i, true);
                        }

                    }
                    this.Refresh();
                    _current_File = String.Empty;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int _INDX = this.listBox1.Items.IndexOf(_current_File);
            Debug.WriteLine(_INDX);
            if (_current_File != String.Empty)
            {
                this.videoAnnotationControl1.SaveAnnotation(!this.checkBox1.Checked);
                
                if (_INDX == -1)
                {
                    return;
                }
                var fl_b = new FileInfo(_current_File);
                string _fl_b = fl_b.FullName.Replace(fl_b.Extension, ".json");
                if (File.Exists(_fl_b))
                {
                    this.listBox1.SetItemChecked(_INDX, true);
                }
                else
                {
                    this.listBox1.SetItemChecked(_INDX, false);
                }

            }
            if (this.listBox1.SelectedIndex == -1)
            {
                
                _current_File = String.Empty;
                this.videoAnnotationControl1.Filename = _current_File;
                this.toolStripLabel1.Text = _current_File;
                return;
            }
            
            _current_File = (string)this.listBox1.SelectedItem;
            var fl = new FileInfo(_current_File);
            string _fl_ = fl.FullName.Replace(fl.Extension, ".json");
            if (File.Exists(_fl_))
            {
                this.listBox1.SetItemChecked(this.listBox1.SelectedIndex, true);
            }
            else
            {
                this.listBox1.SetItemChecked(this.listBox1.SelectedIndex, false);
            }
            this.videoAnnotationControl1.Filename = _current_File;
            this.toolStripLabel1.Text = _current_File;
            this.videoAnnotationControl1.LoadAnnotation();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.videoAnnotationControl1.Label = (string)toolStripComboBox1.SelectedItem;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (_current_File != String.Empty)
            {
                this.videoAnnotationControl1.SaveAnnotation(!this.checkBox1.Checked);

            }
        }
    }
}
