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
    public List<string> conditions;
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
        if(currentHealth > 0)
        {
            anim.SetBool("isDead", false);
        }
    }

    public void HurtEntity(int damageToGive)
    {
        foreach (string cond in conditions)
        {
            if (cond == "Defend")
            {
                damageToGive = damageToGive - 2;
            }
            if (cond == "Vulnerable")
            {
                damageToGive = damageToGive + 1;
            }
        }

        currentHealth -= damageToGive;
        //sfxMan.playerHurt.Play();
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            anim.SetBool("isDead", true);
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

    public void setCondition(string condition)
    {
        conditions.Add(condition);
    }
    
    public void resetConditions()
    {
        conditions.Clear();
    }
}
