using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ARena
{
	public class PlayerInjector : MonoBehaviour {

		public MovementComponent movementComponent;

		void Awake() {
			movementComponent = gameObject.AddComponent<MouseMovementControlComponent>();

			var components = gameObject.GetComponents<MonoBehaviour> ();
			var dependents = components.Where(c=>c is IRequireMovementInput).Cast<IRequireMovementInput>();
			foreach(var dependent in dependents) {
				dependent.MovementInputProxy = movementComponent;
			}
		}

		public void inject(PlayerActor player) {
			injectControl (player);
		}

		private void injectControl(PlayerActor player) {
			player.gameObject.AddComponent<MouseMovementControlComponent>();
			// player.gameObject.AddComponent<ActionControlComponent> ();
		}
	}
}

