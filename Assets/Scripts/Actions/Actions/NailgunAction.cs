using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailgunAction : BaseAction {

	public GameObject bulletPrefab;
    public AudioClip shootClip;

    public AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = true;
    }

    public override void execute(GameObject executer) {
        if (Time.timeScale == 0f)
            return;
        var actor = executer.GetComponent<IActor>();
        var bullet = GameObject.Instantiate(bulletPrefab, executer.transform.position, executer.transform.rotation);
		var rbody = bullet.GetOrCreateComponent<Rigidbody2D>();
		rbody.velocity = actor.LookAngle * 10f;
        bullet.GetComponent<Bullet>().ignoreTags.Add(executer.gameObject.tag);

        audioSource.PlayOneShot(shootClip, 0.5f);
    }
}
