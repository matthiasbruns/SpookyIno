using System;
using UnityEngine;
using UniRx;

namespace ARena
{
	public interface IMovementInputProxy
	{
		 IObservable<Vector3> DestinationObservable();
	}
}

