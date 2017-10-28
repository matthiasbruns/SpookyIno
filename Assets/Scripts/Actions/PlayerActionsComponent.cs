using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsComponent : ActionsComponent {
	void Update(){
		if(Input.GetButtonDown(Keys.FIRE_1)){
			TriggerPrimary();
		}
		
		if(Input.GetButtonDown(Keys.FIRE_2)){
			TriggerSecondary();
		}
	}
}
