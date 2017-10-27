using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ARena
{
	//[RequireComponent(typeof(HelperComponent))]
	[DisallowMultipleComponent]
	[SelectionBase]
	public abstract class Actor : NetworkBehaviour
	{
	}
}

