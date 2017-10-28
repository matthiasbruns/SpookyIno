using System;
using UnityEngine;

public static class GameObjectExtensions
{
	public static T GetRequiredComponent<T>(this GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T> ();

		if(component == null)
		{
			Debug.LogError("Expected to find component of type " 
				+ typeof(T) + " but found none", obj);
		}

		return component;
	}

	public static T GetOrCreateComponent<T>(this GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T> ();

		if(component == null)
		{
			component = obj.AddComponent<T> ();
		}

		return component;
	}
}