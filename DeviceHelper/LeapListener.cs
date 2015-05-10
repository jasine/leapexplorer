using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Leap;

namespace DeviceHelper
{
    public class LeapListener:Listener
    {
        //private Object thisLock = new Object();
        private DateTime lastTime;
        private void SafeWriteLine(String line)
        {
            //lock (thisLock)
            //{
                Debug.WriteLine(line);
            //}

        }


        public override void OnInit(Controller controller)
        {
            SafeWriteLine("Initialized");
            lastTime = DateTime.Now;
        }

        public override void OnConnect(Controller controller)
        {
            SafeWriteLine("Connected");
            controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            //controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

        }

        public override void OnDisconnect(Controller controller)
        {
            SafeWriteLine("Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            SafeWriteLine("Exited");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Leap.Frame frame = controller.Frame();

            if (!frame.Hands.IsEmpty)
            {               
                if (frame.Hands.Count==2&&frame.Fingers.Count>=2&&LeapFingerReady!=null)
                {                
                    Finger first, second;
                    if (frame.Fingers[0].Length > frame.Fingers[1].Length)
                    {
                        first = frame.Fingers[0];
                        second = frame.Fingers[1];                   
                    }
                    else
                    {
                        first = frame.Fingers[1];
                        second = frame.Fingers[0];
                    }
                    for (int i = 2; i < frame.Fingers.Count;i++ )
                    {
                        if (frame.Fingers[i].Length > second.Length && frame.Fingers[i].Length < first.Length)
                        {
                            second = frame.Fingers[i];
                        }
                        else if (frame.Fingers[i].Length > second.Length && frame.Fingers[i].Length > first.Length)
                        {
                            first = frame.Fingers[i];
                        }
                    }
                    if (first.Length>30&&second.Length>30)
                        LeapFingerReady(this,frame.Fingers[0],frame.Fingers[1]);
                }

            }

            // Get gestures




            
            GestureList gestures = frame.Gestures();
            for (int i = 0; i < gestures.Count; i++)
            {
                Gesture gesture = gestures[i];

                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPECIRCLE:
                        CircleGesture circle = new CircleGesture(gesture);

                        // Calculate clock direction using the angle between circle normal and pointable
                        if (circle.State == Gesture.GestureState.STATESTOP&&circle.Radius>15&&circle.Progress>0.5)
                        {
                            if(DateTime.Now-lastTime>TimeSpan.FromMilliseconds(1000))
                            { 
                                LeapCircleReady(this);
                                lastTime = DateTime.Now;
                            }
                        }
                       
                        break;
                    case Gesture.GestureType.TYPESWIPE:
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        
                        if (swipe.State == Gesture.GestureState.STATESTART&&LeapSwipeReady!=null)
                        {
                           // if (DateTime.Now - lastTime > TimeSpan.FromMilliseconds(600))
                            //{
                                if (swipe.Direction.x < -0.5)
                                {
                                    LeapSwipeReady(this, SwipeType.SwipeLeft);
                                    //lastTime = DateTime.Now;
                                }
                                else if (swipe.Direction.x > 0.5)
                                {
                                    LeapSwipeReady(this, SwipeType.SwipeRight);
                                    //lastTime = DateTime.Now;
                                }
                            //}
                            
                            //else if (swipe.Direction.z < -0.2)
                            //{
                            //    LeapSwipeReady(this, SwipeType.SwpieIn);
                            //}
                            //else if (swipe.Direction.z > 0.2)
                            //{
                            //    Thread.Sleep(300);
                            //    LeapSwipeReady(this, SwipeType.SwipeOut);
                            //}
                            

                        }
                        break;
                    case Gesture.GestureType.TYPEKEYTAP:
                        KeyTapGesture keytap = new KeyTapGesture(gesture);
                        SafeWriteLine("Tap id: " + keytap.Id
                                       + ", " + keytap.State
                                       + ", position: " + keytap.Position
                                       + ", direction: " + keytap.Direction);

                        break;
                    case Gesture.GestureType.TYPESCREENTAP:
                        ScreenTapGesture screentap = new ScreenTapGesture(gesture);
                        Finger first = screentap.Frame.Fingers[0];
                        for (int j = 1; j < frame.Fingers.Count; j++)
                        {
                            if ( frame.Fingers[j].Length > first.Length)
                            {
                                first = frame.Fingers[j];
                            }
                        }
                        if ( LeapTapScreenReady!=null&&screentap.State==Gesture.GestureState.STATESTOP&&first.Length>15)
                        {
                            if (DateTime.Now - lastTime > TimeSpan.FromMilliseconds(600))
                            {
                               LeapTapScreenReady(this);
                                lastTime = DateTime.Now;
                            }
                        }
                        break;
                }
            }


        }

        public delegate void SwipeEventHandler(object sender, SwipeType type);

        public event SwipeEventHandler LeapSwipeReady;

        public delegate void FingerEventHandler(object sender,Finger first,Finger second);

        public event FingerEventHandler LeapFingerReady;

        public delegate void CircleEventHandler(object sender);

        public event CircleEventHandler LeapCircleReady;

        public delegate void TypeScreenEventHandler(object sender);

        public event TypeScreenEventHandler LeapTapScreenReady;
    }
}
