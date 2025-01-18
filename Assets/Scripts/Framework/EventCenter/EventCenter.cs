using System.Collections.Generic;
using UnityEngine.Events;


namespace MarkFramework
{
	/// <summary>
	/// Liskov substitution principle load
	/// </summary>
	public abstract class EventInfoBase{}

	/// <summary>
	/// include observer's function delegate class
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EventInfo<T>: EventInfoBase
	{
		//The function info is be recorded here
		public UnityAction<T> actions;
		
		public EventInfo(UnityAction<T> action)
		{
			actions += action;
		}
	}

	/// <summary>
	/// record delegate without parameters
	/// </summary>
	public class EventInfo: EventInfoBase
	{
		//The function info is be recorded here
		public UnityAction actions;
		
		public EventInfo(UnityAction action)
		{
			actions += action;
		}
	}

	/// <summary>
	/// 1. Dictionary
	/// 2. Delegate
	/// 3. Observer design pattern
	/// </summary>
	public class EventCenter : BaseManager<EventCenter>
	{
		private EventCenter()
		{
			
		}
		
		//key - events' names
		//value - delegates which listen to the event
		private Dictionary<E_EventType, EventInfoBase> eventDict = new Dictionary<E_EventType, EventInfoBase>();
		
		/// <summary>
		/// Add Listener (Remember to remove)
		/// </summary>
		/// <param name="everntName">event's name</param>
		/// <param name="action">delegates which listen to the event</param>
		public void AddEventListener<T>(E_EventType everntName, UnityAction<T> action)
		{
			if(eventDict.ContainsKey(everntName))
			{
				(eventDict[everntName] as EventInfo<T>).actions += action;
			}
			else
			{
				eventDict.Add(everntName, new EventInfo<T>(action));
			}
		}
		
		public void AddEventListener(E_EventType eventName, UnityAction action)
		{
			if(eventDict.ContainsKey(eventName))
			{
				(eventDict[eventName] as EventInfo).actions += action;
			}
			else
			{
				eventDict.Add(eventName, new EventInfo(action));
			}
		}
		
		/// <summary>
		/// Remove Listener
		/// </summary>
		/// <param name="name">event's name</param>
		/// <param name="action">delegates which listen to the event</param>
		public void RemoveEventListener<T>(E_EventType eventName, UnityAction<T> action)
		{
			if(eventDict.ContainsKey(eventName))
				(eventDict[eventName] as EventInfo<T>).actions -= action;
		}
		
		public void RemoveEventListener(E_EventType eventName, UnityAction action)
		{
			if(eventDict.ContainsKey(eventName))
				(eventDict[eventName] as EventInfo).actions -= action;
		}
		
		/// <summary>
		/// Trigger event
		/// </summary>
		/// <param name="name">Event's name</param>
		public void EventTrigger<T>(E_EventType eventName, T info)
		{
			if(eventDict.ContainsKey(eventName))
			{
				(eventDict[eventName] as EventInfo<T>).actions?.Invoke(info);
			}
		}
		
		public void EventTrigger(E_EventType eventName)
		{
			if(eventDict.ContainsKey(eventName))
			{
				(eventDict[eventName] as EventInfo).actions?.Invoke();
			}
		}
		
		/// <summary>
		/// Clear event center
		/// Used when change scenes
		/// </summary>
		public void Clear()
		{
			eventDict.Clear();
		}
	}
}
