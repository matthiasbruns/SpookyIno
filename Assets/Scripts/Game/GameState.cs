using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	private SortedSet<string> flags = new SortedSet<string>();
	
	public void AddFlag(string flag) => flags.Add(flag);

	public void RemoveFlag(string flag) => flags.Remove(flag);

	public bool HasFlag(string flag) => flags.Contains(flag);
}
