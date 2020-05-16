using System;
using System.Collections.Generic;
using System.Linq;

namespace TridentMc.Events
{
    public class EventManager
    {
        private Dictionary<Type, List<Delegate>> _listeners = new Dictionary<Type, List<Delegate>>();

        public delegate void Listener<in T>(T theEvent) where T : IEvent;

        public void Listen<T>(Listener<T> listener) where T : IEvent
        {
            var type = typeof(T);
            if (!_listeners.ContainsKey(type))
            {
                _listeners.Add(type, new List<Delegate>());
            }

            _listeners[type].Add(listener);
        }

        public void FireEvent(IEvent theEvent)
        {
            var type = theEvent.GetType();
            if (!_listeners.ContainsKey(type))
            {
                return;
            }

            foreach (var listener in _listeners[type])
            {
                listener.DynamicInvoke(theEvent);
            }
        }
    }
}
