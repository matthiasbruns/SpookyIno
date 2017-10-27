using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARena {
	
	public class DontDestroy : MonoBehaviour {
		
		void Awake () {
			DontDestroyOnLoad (gameObject);
		}
	}
}