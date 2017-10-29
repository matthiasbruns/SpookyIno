using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class HealthComponent : MonoBehaviour, HasHealth {

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

    private AudioSource audioSource;

    public bool CanBeDamaged => canBeDamaged;
    SoundList database;

    void Awake ()
    {
        currentHealth = startHealth;
        currentArmor = startArmor;
        database = GameManager.Instance.audioDatabase;
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
    }

    public void ApplyDamage(int amount)
    {
        damaged = true;
        currentArmor -= amount;
        if (currentArmor<0)
        {
            currentHealth += currentArmor;
            currentArmor = 0;
            audioSource.PlayOneShot(hurtClip);
        }
        else
        {
            audioSource.PlayOneShot(armorHurtClip);
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
        audioSource.PlayOneShot(deathClip);
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
        audioSource.PlayOneShot(gainHealthClip);
    }

    public void IncreaseArmor(int amount)
    {
        currentArmor = Mathf.Clamp(currentArmor + amount, 0, maxArmor);
        audioSource.PlayOneShot(gainArmorClip);
    }
}
