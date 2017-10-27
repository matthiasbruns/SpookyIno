using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace ARena
{
	public class MouseMovementControlComponent: MovementComponent
	{
		private IObservable<Vector3> mouseControlObservable;

		void Awake(){
			mouseControlObservable = this.FixedUpdateAsObservable()
				.Where(_ => Camera.main != null)
				.Where(_ => Input.GetMouseButtonDown(0))
				.Select(_ => {
					return ShootRay();
				});
		}

		public override IObservable<Vector3> DestinationObservable() {
			return mouseControlObservable;
		}

		private Vector3 ShootRay(){
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 100f)) {
				return hit.point;
			}

			return Vector3.zero;
		}
	}
}

