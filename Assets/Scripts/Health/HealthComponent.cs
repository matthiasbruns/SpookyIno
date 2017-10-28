using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour, HasHealth {

    AudioSource audioSource;
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

    void Awake ()
    {
        audioSource = gameObject.GetOrCreateComponent<AudioSource>();
        currentHealth = startHealth;
        currentArmor = startArmor;
    }

    public void ApplyDamage(int amount)
    {
        damaged = true;
        currentArmor -= amount;
        if (currentArmor<0)
        {
            currentHealth += currentArmor;
            currentArmor = 0;
            audioSource.clip = hurtClip;
        }
        else
        {
            audioSource.clip = armorHurtClip;
        }

        audioSource.Play();

        if (currentHealth < 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        audioSource.clip = deathClip;
        audioSource.Play();
        Destroy(gameObject);
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        audioSource.clip = gainHealthClip;
        audioSource.Play();
    }

    public void IncreaseArmor(int amount)
    {
        currentArmor = Mathf.Clamp(currentArmor + amount, 0, maxArmor);
        audioSource.clip = gainArmorClip;
        audioSource.Play();
    }
}
