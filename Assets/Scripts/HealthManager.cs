using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public int currentHealth;
    public int maxHealth;
    private Animator anim;
    public Slider healthBar;
    //public SFXManager sfxMan;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void HurtEntity(int damageToGive)
    {
        currentHealth -= damageToGive;
        //sfxMan.playerHurt.Play();
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            anim.SetTrigger("dead");
        }
    }

    public void HealEntity(int healthToGive)
    {
        currentHealth += healthToGive;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
