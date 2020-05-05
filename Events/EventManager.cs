using System;
using System.Collections.Generic;
using System.Linq;

namespace TridentMc.Events
{
    public class EventManager
    {
        private Queue<IEvent> _beforeTickQueuedEvents = new Queue<IEvent>();
        private Queue<IEvent> _afterTickQueuedEvents = new Queue<IEvent>();
        private Dictionary<Type, List<ListenerInfo>> _listeners = new Dictionary<Type, List<ListenerInfo>>();

        public delegate void Listener<in T>(T theEvent) where T : IEvent;
        
        public void Listen<T>(EventPriority priority, Listener<T> listener) where T : IEvent
        {
            var type = typeof(T);
            if (!_listeners.ContainsKey(type))
            {
                _listeners.Add(type, new List<ListenerInfo>());
            }

            var info = new ListenerInfo();
            info.Listener = listener;
            info.EventPriority = priority;
            _listeners[type].Add(info);
        }
        
        public void FireEvent(IEvent theEvent)
        {
            var type = theEvent.GetType();
            if (!_listeners.ContainsKey(type))
            {
                return;
            }

            foreach (var listenerInfo in _listeners[type].Where(l => l.EventPriority == EventPriority.Immediate))
            {
                listenerInfo.Listener.DynamicInvoke(theEvent);
            }

            _beforeTickQueuedEvents.Enqueue(theEvent);
        }

        public void Tick()
        {
            for (int i = 0; i < _beforeTickQueuedEvents.Count; i++)
            {
                IEvent theEvent = _beforeTickQueuedEvents.Dequeue();

                foreach (var listenerInfo in _listeners[theEvent.GetType()].Where(l => l.EventPriority == EventPriority.BeforeTick))
                {
                    listenerInfo.Listener.DynamicInvoke(theEvent);
                }
                
                _afterTickQueuedEvents.Enqueue(theEvent);
            }
        }

        public void Tock()
        {
            for (int i = 0; i < _afterTickQueuedEvents.Count; i++)
            {
                IEvent theEvent = _afterTickQueuedEvents.Dequeue();
                
                foreach (var listenerInfo in _listeners[theEvent.GetType()].Where(l => l.EventPriority == EventPriority.AfterTick))
                {
                    listenerInfo.Listener.DynamicInvoke(theEvent);
                }
            }
        }
    }
}
