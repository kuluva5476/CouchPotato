using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using OpenTK.Input;
using com.CouchPotato.GameController;
namespace com.CouchPotato.Main
{
    public partial class MainForm : Form
    {
        bool _IsMenuShown = false;

        System.Timers.Timer _Timer = new System.Timers.Timer();

        com.CouchPotato.GameController.Joystick _Joystick;

        // Joystick Axis
        //float _DefaultAxis;

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
            _Joystick = new com.CouchPotato.GameController.Joystick();
            _Joystick.JoystickPressed += new com.CouchPotato.GameController.Joystick.JoystickPressedEventHandler(j_JoystickPressed);
            _Joystick.Initialize();

            osdChannelList1.ChannelList = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ChannelList.xml";
            osdChannelList1.initChannelList();

        }

        /// <summary>
        /// Joystick Pressed event
        /// </summary>
        /// <param name="e">Buttons, DPad, DirectionHat</param>
        void j_JoystickPressed(JoystickPressedEventArgs e)
        {
            if (osdChannelList1.InvokeRequired)
            {
                JoystickPressedCallback d = new JoystickPressedCallback(j_JoystickPressed);
                this.Invoke(d, new object[] { e });
            }
            else
            {
                if (e.Buttons.L)
                {
                    _Timer.Stop();
                    this.Close();
                }
                if (_IsMenuShown)
                {
                    if (e.DPadDirection == DPad.Up || e.HatDirection == HatPosition.Up)
                    {
                        osdChannelList1.channelUp();
                    }
                    else if (e.DPadDirection == DPad.Down || e.HatDirection == HatPosition.Down)
                    {
                        osdChannelList1.channelDown();
                    }
                    else if (e.Buttons.A)
                    {
                        if (_IsMenuShown)
                        {
                            axVLCPlugin21.playlist.clear();
                            axVLCPlugin21.playlist.add(osdChannelList1.ChannelAddress, null, null);
                            axVLCPlugin21.playlist.playItem(0);
                            showOSD(!_IsMenuShown);
                        }
                    }
                    if (e.Buttons.B)
                    {
                        showOSD(false);
                    }
                }
                else
                {
                    showOSD(!_IsMenuShown && !e.Buttons.B);
                }
            }

        }

        delegate void JoystickPressedCallback(JoystickPressedEventArgs e);

        /// <summary>
        /// Show/Hide OSD
        /// </summary>
        /// <param name="_Visible"></param>
        private void showOSD(bool _Visible)
        {
            osdChannelList1.Visible = _Visible;
            _IsMenuShown = _Visible;
        }


        /// <summary>
        /// Form closing
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop playing
            axVLCPlugin21.playlist.stop();
            // Release joystick (stop timer)
            _Joystick.Dispose();
        }

        /// <summary>
        /// VLC Player Keydown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axVLCPlugin21_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F4:
                    _Timer.Stop();
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
                        axVLCPlugin21.playlist.clear();
                        axVLCPlugin21.playlist.add(osdChannelList1.ChannelAddress, null, null);
                        axVLCPlugin21.playlist.playItem(0);

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
