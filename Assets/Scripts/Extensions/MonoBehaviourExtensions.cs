using System;
using System.Collections;
using UnityEngine;

namespace Spooky
{
	public static class MonoBehaviourExtensions
	{
		
		public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action)
		{
			return monoBehaviour.StartCoroutine(InvokeImpl(action, 0));
		}

		public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
		{
			return monoBehaviour.StartCoroutine(InvokeImpl(action, time));
		}

		private static IEnumerator InvokeImpl(Action action, float time)
		{
			if (time > 0) {
				yield return new WaitForSeconds (time);
			}

			action();
		}
	}
}

