using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace ARena {
	
	public abstract class MovementControlComponent : MonoBehaviour {
		
		public abstract IObservable<Vector3> DestinationObservable();

	}
}
