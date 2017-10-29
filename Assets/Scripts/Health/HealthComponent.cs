using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class HealthComponent : MonoBehaviour, HasHealth {

    Vector3 audioPosition;
    bool isDead = false;
    public bool canBeDamaged = true;
    public int currentHealth;
    public int currentArmor;
    public int startHealth = 100;
    public int startArmor = 0;
    public int maxHealth = 100;
    public int maxArmor = 100;
    public bool damaged = false;
    public AudioClip gainHealthClip;
    public AudioClip gainArmorClip;
    public AudioClip hurtClip;
    public AudioClip armorHurtClip;
    public AudioClip deathClip;

    public bool CanBeDamaged => canBeDamaged;
    SoundList database;

    void Awake ()
    {
        currentHealth = startHealth;
        currentArmor = startArmor;
        audioPosition = gameObject.transform.position;
        database = GameManager.Instance.audioDatabase;
    }

    void Update() {
        audioPosition = gameObject.transform.position;
    }

    public void ApplyDamage(int amount)
    {
        damaged = true;
        currentArmor -= amount;
        if (currentArmor<0)
        {
            currentHealth += currentArmor;
            currentArmor = 0;
            AudioSource.PlayClipAtPoint(hurtClip, audioPosition);
        }
        else
        {
            AudioSource.PlayClipAtPoint(armorHurtClip, audioPosition);
        }

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    public int Health => currentHealth;

    void Death()
    {
        isDead = true;
        AudioSource.PlayClipAtPoint(deathClip, audioPosition);
        // Broadcast 
        List<DeathHandler> handlers;
        gameObject.GetInterfaces<DeathHandler>(out handlers);

        if(handlers != null && handlers.Count > 0){
            foreach(DeathHandler handler in handlers) {
                handler.HandleDeath();
            }
        }

        Destroy(gameObject);
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        AudioSource.PlayClipAtPoint(gainHealthClip, audioPosition);
    }

    public void IncreaseArmor(int amount)
    {
        currentArmor = Mathf.Clamp(currentArmor + amount, 0, maxArmor);
        AudioSource.PlayClipAtPoint(gainArmorClip, audioPosition);
    }
}
