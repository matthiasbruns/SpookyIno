using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARena {
	public class DebugLookDirection : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			if (RuntimeConfig.DEBUG)
			{
				Debug.DrawRay (transform.position, gameObject.transform.forward * 5.0f, Color.red);
			}
		}
	}
}