using System;
using UnityEngine;
using UniRx;

namespace ARena
{
	public abstract class MovementComponent : MonoBehaviour, IMovementInputProxy 
	{
		public abstract IObservable<Vector3> DestinationObservable();
	}
}

