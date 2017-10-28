using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour {

	public int seconds = 10;

	void Start(){
		StartCoroutine(Die());
	}

	IEnumerator Die(){
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
		yield return null;
	}	
}