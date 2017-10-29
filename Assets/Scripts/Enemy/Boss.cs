using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, DeathHandler
{
	public string gameFlag;

    public void HandleDeath() {
        GameManager.Instance.gameState.AddFlag(gameFlag);
    }
}
