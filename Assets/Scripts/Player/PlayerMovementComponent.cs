using System;
using UnityEngine;
using UniRx;

namespace ARena
{
	public class PlayerMovementComponent : MonoBehaviour, IRequireMovementInput {
		public IMovementInputProxy MovementInputProxy { get; set;}


	}
}

