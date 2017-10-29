using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpriteShadow : MonoBehaviour {

    public ShadowCastingMode ShadowCastingMode = ShadowCastingMode.Off;
    public bool ReceiveShadows = true;

    void Awake() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.shadowCastingMode = ShadowCastingMode;
        renderer.receiveShadows = ReceiveShadows;
    }
	
	void Update () {
		
	}
}
