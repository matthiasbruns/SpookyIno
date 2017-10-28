using System;
using System.Collections.Generic;
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

	public static T GetOrCreateComponent<T>(this GameObject obj) where T : Component {
		T component = obj.GetComponent<T> ();

		if(component == null)
		{
			component = obj.AddComponent<T> ();
		}

		return component;
	}

	public static float x(this GameObject obj) {
		return obj.transform.position.x;
	}
	public static float y(this GameObject obj) {
		return obj.transform.position.y;
	}
	public static float z(this GameObject obj) {
		return obj.transform.position.z;
	}

	public static void GetInterfaces<T>(this GameObject obj, out List<T> interfaceList) where T : class {
		interfaceList = new List<T>();
		var components = obj.GetComponents<MonoBehaviour>();

		foreach(MonoBehaviour component in components) {
			if (component is T) {
                 interfaceList.Add(component as T);
             }
		}
	} 
}