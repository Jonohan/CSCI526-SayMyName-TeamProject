using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCenter : BaseManager<EventCenter>
{   
    /// <summary>
    /// Key: event name, e.g. MonsterIsDead, PlayerLevelsUp, GameOver, etc.
    /// Value: UnityAction&lt;object&gt;. Object, being the base type, can be any reference type(class object instance, List) or value type(int, enum, struct).
    /// </summary>
    private Dictionary< string, UnityAction<object> > eventDict = new Dictionary< string, UnityAction<object> >();
    
    /// <summary>
    /// Add an event listener
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="action">Delegate methods that will be executed in case this event happens</param>
    public void AddEventListener(string eventName, UnityAction<object> action)
    {   
        Debug.Log("EventCenter: AddEventListener - " + eventName);
        // Case 1: this event exists in the event dict
        if (eventDict.ContainsKey(eventName))
        {
            eventDict[eventName] += action;
        }
        // Case 2: this event doesn't exist in the event dict yet. 
        else
        {
            eventDict.Add(eventName, action);
        }
        // Debug.Log("EventList: ");
        // foreach (var key in eventDict.Keys)
        //     Debug.Log(key);
        // Debug.Log("EventList Ends. ");
    }
    
    
    /// <summary>
    /// Remove an event listener to prevent memory leak or errors. Example use is to call this method in OnDestroy() for a monobehaviour obj. 
    /// </summary>
    /// <param name="eventName">Name of the event</param>
    /// <param name="action">Delegate methods that would no longer be executed in case this event happens</param>
    public void RemoveEventListener(string eventName, UnityAction<object> action)
    {
        if (eventDict.ContainsKey(eventName))
            eventDict[eventName] -= action;
    }
    
    
   
    public void TriggerEvent(string eventName, object info)
    {   
        // check if there is a corresponding event listener
        if (eventDict.ContainsKey(eventName))
        {   
            Debug.Log("EventCenter: TriggerEvent - " + eventName);
            // eventDict[eventName].Invoke(info); also works! is the same as the line below. 
            eventDict[eventName](info); 
        }
    }

    internal void TriggerEvent(string v)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Remove all events in the event center. Example use: call this when switching between scenes.
    /// </summary>
    public void ClearEvents()
    {
        eventDict.Clear();
    }
    
}
