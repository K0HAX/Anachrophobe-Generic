﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Timers;

namespace Anachrophobe
{
    static class EventMessenger
    {
        //public static event Action<object, TimerDatastore, bool, bool> Changed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="add"></param>
        /// <param name="del"></param>
        public static void SendMessage(object sender, TimerDatastore e, bool add, bool del)
        {
            //Changed(sender, e, add, del);
        }
    }
}
