using System;
using System.Collections.Generic;
using System.Linq;


public class EventManager
{
    private class Listener
    {
        public Action Callback;
        public int Priority;
    }

    private static Dictionary<string, List<Listener>> listeners = new();

    public static void AddListener(string eventName, Action callback, int priority = 100)
    {
        if (!listeners.ContainsKey(eventName))
            listeners[eventName] = new List<Listener>();

        listeners[eventName].Add(new Listener { Callback = callback, Priority = priority });
        listeners[eventName] = listeners[eventName].OrderBy(l => l.Priority).ToList();
    }

    public static void RemoveListener(string eventName, Action callback)
    {
        if (listeners.ContainsKey(eventName))
            listeners[eventName].RemoveAll(l => l.Callback == callback);
    }

    public static void Invoke(string eventName)
    {
        if (listeners.TryGetValue(eventName, out var list))
        {
            foreach (var listener in list)
                listener.Callback?.Invoke();
        }
    }
}
