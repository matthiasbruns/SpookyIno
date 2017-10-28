using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PrimaryAction();
public delegate void SecondaryAction();
public abstract class ActionsComponent : MonoBehaviour {
	public event PrimaryAction OnPrimary;   
	public event SecondaryAction OnSecondary;    

	protected void TriggerPrimary(){
		if(OnPrimary != null){
			OnPrimary();
		}
	}	
	
	protected void TriggerSecondary(){
		if(OnSecondary != null){
			OnSecondary();
		}
	}
}
