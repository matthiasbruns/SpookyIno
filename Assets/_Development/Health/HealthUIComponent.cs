using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIComponent : MonoBehaviour {

    public float barSize;
    float flashSpeed = 5f;
    Color baseColor = new Color(1f, 0f, 0f, 0.1f);
    HealthComponent healthComponentRef;
    public Image damageTakenImage;
    RectTransform healthTransform;
    RectTransform armorTransform;

    public Image health;
    public Image armor;
    public Color hurtFlashColor;
    public Color armorFlashColor;

    // Use this for initialization
    void Start () {
        healthComponentRef = gameObject.GetComponent<HealthComponent>();
        //health = GameObject.Find("HUDCanvas/Health");
        //armor = GameObject.Find("HUDCanvas/Armor");
        healthTransform = GameObject.Find("HUDCanvas/Health").GetComponent<RectTransform>();
        armorTransform = GameObject.Find("HUDCanvas/Armor").GetComponent<RectTransform>();
        barSize = healthTransform.rect.x / -100;
        healthTransform.sizeDelta = new Vector2(healthComponentRef.currentHealth * barSize, 50);
        armorTransform.sizeDelta = new Vector2(healthComponentRef.currentArmor * barSize, 50);
        armorTransform.position = new Vector3(healthTransform.position.x + healthTransform.rect.x - 20, healthTransform.position.y, healthTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (healthComponentRef.damaged)
        {
            if(healthComponentRef.currentArmor>0)
            {
                damageTakenImage.color = armorFlashColor;
            }
            else
            {
                damageTakenImage.color = hurtFlashColor;
            }
            healthTransform.sizeDelta = new Vector2(healthComponentRef.currentHealth * barSize, 50);
            armorTransform.sizeDelta = new Vector2(healthComponentRef.currentArmor * barSize, 50);
            armorTransform.position = new Vector3(healthTransform.position.x + healthTransform.rect.x - 20, healthTransform.position.y, healthTransform.position.z);
        }
        else
        {
            damageTakenImage.color = Color.Lerp(damageTakenImage.color, baseColor, flashSpeed * Time.deltaTime);
        }

        healthComponentRef.damaged = false;
    }
}
