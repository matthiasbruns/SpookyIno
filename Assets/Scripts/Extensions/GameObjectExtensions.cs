using System;
using UnityEngine;

namespace ARena
{
	public static class GameObjectExtensions
	{
		public static T GetRequiredComponent<T>(this GameObject obj) where T : MonoBehaviour
		{
			T component = obj.GetComponent<T> ();

			if(component == null)
			{
				Debug.LogError("Expected to find component of type " 
					+ typeof(T) + " but found none", obj);
			}

			return component;
		}

		public static T GetOrCreateComponent<T>(this GameObject obj) where T : MonoBehaviour
		{
			T component = obj.GetComponent<T> ();

			if(component == null)
			{
				component = obj.AddComponent<T> ();
			}

			return component;
		}

		public static Optional<T> GetComponentSave<T>(this GameObject obj) where T : MonoBehaviour {
			return new Optional<T>(obj.GetComponent<T> ());
		}

		public static Optional<T[]> GetComponentsSave<T>(this GameObject obj) where T : MonoBehaviour {
			return new Optional<T[]>(obj.GetComponents<T> ());
		}
	
	}
}

