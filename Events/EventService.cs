using System;
using System.Collections.Generic;
using Serilog;
using TridentMc.Extentions;

namespace TridentMc.Events
{
    public class EventService : ISingletonDiService
    {
        private readonly Dictionary<Type, List<Delegate>> _listeners = new Dictionary<Type, List<Delegate>>();
        private readonly Queue<IEvent> _toFire = new Queue<IEvent>();

        public delegate void Listener<in T>(T theEvent) where T : IEvent;

        public void Listen<T>(Listener<T> listener) where T : IEvent
        {
            var type = typeof(T);
            Log.Verbose("New listener for {Type}", type);
            
            if (!_listeners.ContainsKey(type))
            {
                _listeners.Add(type, new List<Delegate>());
            }

            _listeners[type].Add(listener);
        }

        public void EnqueueEvent(IEvent theEvent)
        {
            _toFire.Enqueue(theEvent);
        }

        public void Tick() {
            foreach (var theEvent in _toFire)
            {
                var type = theEvent.GetType();
                if (!_listeners.ContainsKey(type))
                {
                    return;
                }
                
                Log.Verbose("Firing event {Type}", type);

                foreach (var listener in _listeners[type])
                {
                    listener.DynamicInvoke(theEvent);
                }
            }
        }
    }
}
