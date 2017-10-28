using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailgunAction : BaseAction {

	public GameObject bulletPrefab;

    public override void execute(GameObject executer) {
        var actor = executer.GetComponent<IActor>();
        var bullet = GameObject.Instantiate(bulletPrefab, executer.transform.position, executer.transform.rotation);
		var rbody = bullet.GetOrCreateComponent<Rigidbody2D>();
		rbody.velocity = actor.LookAngle * 10f;
        bullet.GetComponent<Bullet>().ignoreTags.Add(executer.gameObject.tag);
    }
}
