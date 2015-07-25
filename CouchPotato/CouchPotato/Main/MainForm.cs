using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace com.CouchPotato.Main
{
    public partial class MainForm : Form
    {
        // Channels
        string[] _ChannelNames = { 
                                     "探索頻道", 
                                     "緯來日本台", 
                                     "三立台灣台", 
                                     "三立都會台", 
                                     "JET綜合台", 
                                     "中天娛樂" 
                                 };

        string[] _ChannelURLs = { 
                                    "sop://124.232.150.188:3920/5700", 
                                    "sop://124.232.150.188:3920/5733",
                                    "sop://124.232.150.188:3920/5706",
                                    "sop://124.232.150.188:3920/5707",
                                    "sop://124.232.150.188:3920/5708",
                                    "sop://124.232.150.188:3920/5709"
                                };

        Label[] _lblChannels = new Label[5];

        int _SelectedChannel = 0;
        int _CurrentChannel = -1;
        bool _IsMenuShown = false;
        int _TotalChannels = 6;

        
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form load... nothing to say
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            initOSDChannelList();
            setOSD();

            _SelectedChannel = 0;

            axsopocx1.SendToBack();
            axsopocx1.Select();
            axsopocx1.SetFullscreen();
            axsopocx1.InitPlayer();

            axsopocx1.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Initialize Channel Labels on OSD
        /// </summary>
        private void initOSDChannelList()
        {
            // Label Height
            int nHeight = 32;
            
            for (int i = 0; i < 5; i++)
            {
                _lblChannels[i] = new Label();
                Label lbl = _lblChannels[i];

                lbl.Parent = axsopocx1;
                lbl.Font = new Font(lbl.Font.FontFamily, 20);
                lbl.ForeColor = Color.FromArgb(0xffffff);

                lbl.BackColor = Color.FromArgb(0x30, 0x30, 0x30);
                lbl.AutoSize = false;
                lbl.BorderStyle = BorderStyle.FixedSingle;

                lbl.Location = new System.Drawing.Point(20, 20 + nHeight * i);
                lbl.Name = "label1";
                lbl.Size = new System.Drawing.Size(300, nHeight);
                lbl.Visible = false;
                this.Controls.Add(lbl);
            }

            // Bold & Yellow for selected channel
            _lblChannels[2].Font = new Font(_lblChannels[2].Font, FontStyle.Bold);
            _lblChannels[2].ForeColor = Color.FromArgb(0xff, 0xff, 0xab);
        }

        /// <summary>
        /// Show/Hide OSD
        /// </summary>
        /// <param name="_Visible"></param>
        private void showOSD(bool _Visible)
        {
            for (int i = 0; i < 5; i++)
            {
                _lblChannels[i].Visible = _Visible;
            }
            _IsMenuShown = _Visible;
        }

        // 觸發鍵盤事件


        /// <summary>
        /// Key Down Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axsopocx1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F4:
                    axsopocx1.Stop();
                    this.Close();
                    break;
                case Keys.Up:
                    if (_IsMenuShown)
                    {
                        _SelectedChannel -= 1;
                        setOSD();
                    }
                    else
                    {
                        showOSD(!_IsMenuShown);
                    }
                    break;

                case Keys.Down:
                    if (_IsMenuShown)
                    {
                        _SelectedChannel += 1;
                        setOSD();
                    }
                    else
                    {
                        showOSD(!_IsMenuShown);
                    }
                    break;
                case Keys.Enter:
                    if (_IsMenuShown)
                    {
                        if (_CurrentChannel != _SelectedChannel)
                        {
                            _CurrentChannel = _SelectedChannel;
                            axsopocx1.SetSopAddress(_ChannelURLs[_SelectedChannel]);
                            axsopocx1.Play();
                        }
                        showOSD(!_IsMenuShown);
                    }
                    else
                    {
                        showOSD(!_IsMenuShown);
                    }
                    break;
                case Keys.Escape:
                    showOSD(false);
                    break;
                default:
                    break;
            }
        }

        // 轉台
        /// <summary>
        /// Set OSD Channel List captions
        /// </summary>
        private void setOSD()
        {
            _SelectedChannel += _TotalChannels;
            _SelectedChannel %= _TotalChannels;

            // Offset: 2
            // Total menu items: 5
            // Total channels: 6
            for (int i = 0; i < 5; i++)
            {
                _lblChannels[i].Text = _ChannelNames[((_SelectedChannel + i - 2 + _TotalChannels) % _TotalChannels)];
            }
        }
    }
}
