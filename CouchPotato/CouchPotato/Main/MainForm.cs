using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using OpenTK.Input;

namespace com.CouchPotato.Main
{
    public partial class MainForm : Form
    {
        bool _IsMenuShown = false;

        System.Timers.Timer _Timer = new System.Timers.Timer();

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
            // 暫時只有 Joystick 1
            //_DefaultAxis = Joystick.GetState(1).GetAxis(JoystickAxis.Axis1);
            

            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(_Timer_Elapsed);

            _Timer.Interval = 250;
            _Timer.Start();

            osdChannelList1.ChannelList = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ChannelList.xml";
            osdChannelList1.initChannelList();

            axsopocx1.SendToBack();
            axsopocx1.Select();
            axsopocx1.SetFullscreen();
            axsopocx1.InitPlayer();
            axsopocx1.SetFullscreen();

            axsopocx1.Dock = DockStyle.Fill;
        }


        delegate void JoystickCallback(string _Direction);

        private void JoystickPressed(string _Button)
        {
            if (osdChannelList1.InvokeRequired)
            {
                JoystickCallback d = new JoystickCallback(JoystickPressed);
                this.Invoke(d, new object[] {_Button});
            }
            else
            {
                if (_Button == "L")
                {
                    axsopocx1.Stop();
                    _Timer.Stop();
                    this.Close();
                }
                if (_IsMenuShown)
                {
                    if (_Button == "UP")
                    {
                        osdChannelList1.channelUp();
                    }
                    else if (_Button == "DOWN")
                    {
                        osdChannelList1.channelDown();
                    }
                    else if (_Button == "A")
                    {
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
                    }
                    if (_Button == "B")
                    {
                        showOSD(false);
                    }
                }
                else
                {
                        showOSD(!_IsMenuShown && _Button!="B");
                }
            }
        }

        /// <summary>
        /// 讀取 Joystick input 用
        /// </summary>
        void _Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _Timer.Stop();

            try
            {
                var state = Joystick.GetState(1);

                int nAxis = (int) Math.Round(state.GetAxis(JoystickAxis.Axis1), 0);
                // 上下
                if (nAxis < 0)
                {
                    //Debug.Print(state.ToString());
                    JoystickPressed("DOWN");
                }
                else if (nAxis > 0)
                {
                    //Debug.Print(state.ToString());
                    JoystickPressed("UP");
                }

                // 其它按鍵
                // 先假設使用者不會笨到兩顆一起按 =.=

                // A: 選台
                if (state.IsButtonDown(JoystickButton.Button1))
                { 
                    JoystickPressed("A");
                }

                // B: 關閉 OSD
                if (state.IsButtonDown(JoystickButton.Button2))
                { 
                    JoystickPressed("B");
                }

                if (state.IsButtonDown(JoystickButton.Button6))
                {
                    JoystickPressed("L");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
            
            _Timer.Start();
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
                    try
                    {
                        axsopocx1.Stop();
                    }
                    catch (Exception ex)
                    { }
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
