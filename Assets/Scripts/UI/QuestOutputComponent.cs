using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestOutputComponent : MonoBehaviour {

	public float displayDuration = 5f;
	public float displayTimer = 0f;

	private string content;
	public string Content{
		get{
			return content;
		}
		set{
			content = value;
			Show();
		}
	}
	public RectTransform panel;

	private Text output;

	void Awake() {
		output = panel.GetComponentInChildren<Text>();
	}
	void Update(){
		displayTimer -= Time.deltaTime;

		if (content == null || content.Length == 0 || displayTimer <= 0.0f){
			Hide();
		} 
	}

	private void Hide(){
		panel.gameObject.SetActive(false);
		output.text = null;
	}

	private void Show() {
		panel.gameObject.SetActive(true);
		output.text = content;
		displayTimer = displayDuration;
	}
}
