using System;
using System.Collections;
using System.Text;
using System.Reflection;
using OpenTK.Input;


namespace com.CouchPotato.GameController
{
    public enum DPad
    {
        Up, Right, Left, Down, None
    };

    public class Button
    {
        public bool A = false;
        public bool B = false;
        public bool X = false;
        public bool Y = false;
        public bool L = false;
        public bool R = false;
        public bool LB = false;
        public bool RB = false;
        public bool Select = false;
        public bool Start = false;

        public string ToString()
        {
            string sReturn = "";

            sReturn += " A:" + A.ToString();
            sReturn += " B:" + B.ToString();
            sReturn += " X:" + X.ToString();
            sReturn += " Y:" + Y.ToString();
            sReturn += " L:" + L.ToString();
            sReturn += " R:" + R.ToString();
            sReturn += " LB:" + LB.ToString();
            sReturn += " RB:" + RB.ToString();
            sReturn += " Select:" + Select.ToString();
            sReturn += " Start:" + Start.ToString();

            return sReturn;
        }
    }

    public class JoystickPressedEventArgs : EventArgs
    {
        public DPad DPadDirection;
        public Button Buttons;
        public HatPosition HatDirection;
    }

    public class Joystick
    {
        System.Timers.Timer _Timer = new System.Timers.Timer();

        public event JoystickPressedEventHandler JoystickPressed;

        protected virtual void OnJoystickPressed(JoystickPressedEventArgs e)
        {
            JoystickPressedEventHandler handler = JoystickPressed;
            if (handler != null)
                handler(e);
        }

        public delegate void JoystickPressedEventHandler(JoystickPressedEventArgs e);

        public void Initialize()
        {
            _Timer.Elapsed += new System.Timers.ElapsedEventHandler(_Timer_Elapsed);
            _Timer.Interval = 120;
            _Timer.Start();
        }

        public void Dispose()
        {
            _Timer.Stop();
        }

        void _Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _Timer.Stop();

            DPad[] oDirection = new DPad[4];
            Button oBtn = new Button();
            HatPosition[] oHatDirection = new HatPosition[4];

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    var state = OpenTK.Input.Joystick.GetState(i + 1);
                    if (!state.IsConnected)
                        continue;
                    System.Diagnostics.Debug.Print(state.ToString());

                    oBtn.A = state.IsButtonDown(JoystickButton.Button1);
                    oBtn.B = state.IsButtonDown(JoystickButton.Button2);
                    oBtn.X = state.IsButtonDown(JoystickButton.Button0);
                    oBtn.Y = state.IsButtonDown(JoystickButton.Button3);
                    oBtn.L = state.IsButtonDown(JoystickButton.Button6);
                    oBtn.R = state.IsButtonDown(JoystickButton.Button7);
                    oBtn.LB = false;
                    oBtn.RB = false;
                    oBtn.Select = state.IsButtonDown(JoystickButton.Button8);
                    oBtn.Start = state.IsButtonDown(JoystickButton.Button9);

                    JoystickHatState jhs = state.GetHat(JoystickHat.Hat0);
                    oHatDirection[i] = jhs.Position;

                    //jhs.Position
                    //state.GetHat(JoystickHat.Hat0) == JoystickHatState
                    //if (state.GetHat(JoystickHat.Hat0) == HatPosition.Up)
                    //    oDirection[i] = DPad.Up;



                    int nXAxis = (int)Math.Round(state.GetAxis(JoystickAxis.Axis0), 0);
                    int nYAxis = (int)Math.Round(state.GetAxis(JoystickAxis.Axis1), 0);

                    // 左邊磨菇頭...
                    if ((nXAxis == 0 && nYAxis == 0))
                    {
                        oDirection[i] = DPad.None;
                    }

                    if (nYAxis > 0)
                        oDirection[i] = DPad.Up;
                    if (nYAxis < 0)
                        oDirection[i] = DPad.Down;

                    if (oBtn.A || oBtn.B || oBtn.X || oBtn.Y || oBtn.L || oBtn.R || oBtn.LB || oBtn.RB || oBtn.Select || oBtn.Start || oDirection[i] != DPad.None || jhs.Position != HatPosition.Centered)
                    {
                        JoystickPressedEventHandler handler = JoystickPressed;
                        if (handler != null)
                        {
                            JoystickPressedEventArgs evt = new JoystickPressedEventArgs();

                            evt.DPadDirection = oDirection[i];
                            evt.Buttons = oBtn;
                            evt.HatDirection = oHatDirection[i];
                            handler(evt);
                        }
                    }
                }

            }
            catch (Exception ex){ }



            _Timer.Start();
        }


    }
}
