using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

namespace DeviceHelper
{
    public class LeapHelper
    {
        private LeapListener listener;
        private readonly Controller controller;
        public bool IsConnected { get { return controller.IsConnected; }}

        public LeapListener Listener
        {
            get { return listener; }
        }

        public LeapHelper()
        {
            
            controller = new Controller();              
        }
        public void Init()
        {
            if (controller.IsConnected)
            {
                listener = new LeapListener();
                controller.AddListener(listener);
            }
        }
        
        public void Close()
        {
            try
            {
                if (listener != null)
                    controller.RemoveListener(listener);
                controller.Dispose();
            }
            catch
            {
              
            }
        }
    }
}
