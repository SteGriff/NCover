using System;
using System.ComponentModel;

namespace SvnTracker.Lang
{
    
    public static class EventHandlerExtentions
    {
        public static void Raise(this EventHandler handler, object sender)
        {
            Raise(handler, sender, EventArgs.Empty);
        }
        public static void Raise(this PropertyChangedEventHandler handler, object sender, PropertyChangedEventArgs args)
        {
            if (handler != null)
                handler(sender, args);
        }
        
        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public static void Raise<TA>(this EventHandler handler, object sender, TA args)
            where TA : EventArgs
        {
            if (handler != null)
            {
                handler(sender, args);
            }
        }
    }
}
