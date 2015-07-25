using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace com.CouchPotato.Main
{
    public partial class MainForm : Form
    {
        bool _IsMenuShown = false;
        
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
            osdChannelList1.ChannelList = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ChannelList.xml";
            osdChannelList1.initChannelList();

            axsopocx1.SendToBack();
            axsopocx1.Select();
            axsopocx1.SetFullscreen();
            axsopocx1.InitPlayer();

            axsopocx1.Dock = DockStyle.Fill;
        }
        /// <summary>
        /// Show/Hide OSD
        /// </summary>
        /// <param name="_Visible"></param>
        private void showOSD(bool _Visible)
        {
            osdChannelList1.Visible = _Visible;
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
                        osdChannelList1.channelUp();
                    else
                        showOSD(!_IsMenuShown);
                    break;

                case Keys.Down:
                    if (_IsMenuShown)
                        osdChannelList1.channelDown();
                    else
                        showOSD(!_IsMenuShown);
                    break;
                case Keys.Enter:
                    if (_IsMenuShown)
                    {
                        // 頻道有變動才改
                        if (axsopocx1.SopAddress != osdChannelList1.ChannelAddress)
                        {
                            axsopocx1.SetSopAddress(osdChannelList1.ChannelAddress);
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
    }
}
