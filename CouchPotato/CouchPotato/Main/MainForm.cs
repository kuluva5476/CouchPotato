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

        //System.Timers.Timer _Timer = new System.Timers.Timer();

        com.CouchPotato.GameController.Joystick _Joystick;

        string _CurrentPlaying = "";

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
            //this.Cursor.
            Cursor.Hide();

            _Joystick = new com.CouchPotato.GameController.Joystick();
            _Joystick.JoystickPressed += new com.CouchPotato.GameController.Joystick.JoystickPressedEventHandler(Joystick_JoystickPressed);
            //_Joystick.JoystickTrace += new com.CouchPotato.GameController.Joystick.JoystickTraceEventHandler(Joystick_JoystickTrace);
            _Joystick.Initialize();

            osdChannelList1.ChannelList = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ChannelList.xml";
            osdChannelList1.ThumbnailPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Thumbnails\\" ;
            osdChannelList1.initChannelList();

            axVLCPlugin21.volume = 100;
            label1.Text = "";

        }

        delegate void JoystickTraceCallback(JoystickTraceEventArgs e);

        void Joystick_JoystickTrace(JoystickTraceEventArgs e)
        {
            if (osdChannelList1.InvokeRequired)
            {
                JoystickTraceCallback d = new JoystickTraceCallback(Joystick_JoystickTrace);
                this.Invoke(d, new object[] { e });
            }
            else
            {
                label1.Text = e.TraceMessage;
            }
        }

        delegate void JoystickPressedCallback(JoystickPressedEventArgs e);

        /// <summary>
        /// Joystick Pressed event
        /// </summary>
        /// <param name="e">Buttons, DPad, DirectionHat</param>
        void Joystick_JoystickPressed(JoystickPressedEventArgs e)
        {
            if (osdChannelList1.InvokeRequired)
            {
                JoystickPressedCallback d = new JoystickPressedCallback(Joystick_JoystickPressed);
                this.Invoke(d, new object[] { e });
            }
            else
            {
                label1.Text = e.TraceMessage;
                if (e.Buttons.Start)
                //if (e.Buttons.L)
                {
                    //_Timer.Stop();
                    this.Close();
                }
                if (e.Buttons.L)
                {
                    int nVolume = axVLCPlugin21.volume - 5;
                    if (nVolume < 0)
                        nVolume = 0;
                    axVLCPlugin21.volume = nVolume;

                }
                if (e.Buttons.R)
                {
                    int nVolume = axVLCPlugin21.volume + 5;
                    if (nVolume > 100)
                        nVolume = 100;
                    axVLCPlugin21.volume = nVolume;

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
                            showOSD(!_IsMenuShown);
                            Application.DoEvents();
                            if (_CurrentPlaying != osdChannelList1.ChannelAddress)
                            {
                                axVLCPlugin21.playlist.clear();
                                axVLCPlugin21.playlist.add(osdChannelList1.ChannelAddress, null, null);
                                axVLCPlugin21.playlist.playItem(0);
                                _CurrentPlaying = osdChannelList1.ChannelAddress;
                            }
                        }
                    }
                    else if (e.Buttons.B)
                    {
                        showOSD(false);
                    }
                }
                else
                {
                    if (e.Buttons.A || e.Buttons.B || e.DPadDirection == DPad.Up || e.DPadDirection == DPad.Down || e.HatDirection == HatPosition.Up || e.HatDirection == HatPosition.Down )
                        showOSD(!_IsMenuShown || !e.Buttons.B);
                }
            }
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

        /// <summary>
        /// Form closing
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop playing
            try
            {
                //_Joystick.JoystickPressed -= 
                //_Joystick.Dispose();
                //axVLCPlugin21.playlist.stop();
                Application.Exit();
            }
            catch (Exception ex)
            {
                label1.Text = ex.Message;
            }
            // Release joystick (stop timer)
        }

        /// <summary>
        /// VLC Player Keydown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axVLCPlugin21_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            label1.Text = e.KeyCode.ToString();
            switch (e.KeyCode)
            {
                case Keys.F4:
                    //_Timer.Stop();
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
                case Keys.Subtract: case Keys.OemMinus:
                    int nVolume = axVLCPlugin21.volume - 5;
                    if (nVolume <= 0)
                        nVolume = 0;
                    axVLCPlugin21.volume = nVolume;
                    break;

                case Keys.Add: case Keys.Oemplus:
                    nVolume = axVLCPlugin21.volume + 5;
                    if (nVolume > 100)
                        nVolume = 100;
                    axVLCPlugin21.volume = nVolume;
                    break;

                case Keys.Oem5:
                    axVLCPlugin21.volume = 0;
                    break;
                
                case Keys.Enter:
                    if (_IsMenuShown)
                    {
                        showOSD(!_IsMenuShown);
                        Application.DoEvents();
                        if (_CurrentPlaying != osdChannelList1.ChannelAddress)
                        {
                            axVLCPlugin21.playlist.clear();
                            axVLCPlugin21.playlist.add(osdChannelList1.ChannelAddress, null, null);
                            axVLCPlugin21.playlist.playItem(0);
                        }
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

        //private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.F4:
        //            _Timer.Stop();
        //            this.Close();
        //            break;
        //        case Keys.Up:
        //            if (_IsMenuShown)
        //                osdChannelList1.channelUp();
        //            else
        //                showOSD(!_IsMenuShown);
        //            break;

        //        case Keys.Down:
        //            if (_IsMenuShown)
        //                osdChannelList1.channelDown();
        //            else
        //                showOSD(!_IsMenuShown);
        //            break;
        //        case Keys.Enter:
        //            if (_IsMenuShown)
        //            {
        //                showOSD(!_IsMenuShown);
        //                Application.DoEvents();
        //                if (_CurrentPlaying != osdChannelList1.ChannelAddress)
        //                {
        //                    axVLCPlugin21.playlist.clear();
        //                    axVLCPlugin21.playlist.add(osdChannelList1.ChannelAddress, null, null);
        //                    axVLCPlugin21.playlist.playItem(0);
        //                }
        //            }
        //            else
        //            {
        //                showOSD(!_IsMenuShown);
        //            }
        //            break;
        //        case Keys.Escape:
        //            showOSD(false);
        //            break;
        //        default:
        //            break;
        //    }

        //}

    }
}
