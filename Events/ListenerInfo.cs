using System;

namespace TridentMc.Events
{
    public struct ListenerInfo
    {
        public Delegate Listener;
        public EventPriority EventPriority;
    }
}