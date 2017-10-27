using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ARena{
	public class PlayerColorComponent : NetworkBehaviour {

		public override void OnStartLocalPlayer()
		{
			(GetComponentsInChildren<MeshRenderer>().GetValue(0) as MeshRenderer).material.color = Color.blue;
		}
	}
}