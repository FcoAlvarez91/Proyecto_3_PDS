using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDisplay : MonoBehaviour
{

    public Enemy enemy;
    private Sprite body;
    public GameObject weapon1;
    public GameObject weapon2;
    private HealthManager theHM;
    private SpriteRenderer theSR;

    public void assignInfo()
    {
        theHM = GetComponent<HealthManager>();
        theSR = GetComponent<SpriteRenderer>();

        theHM.maxHealth = enemy.maxHP;
        theHM.currentHealth = enemy.maxHP;
        body = enemy.body;
        weapon1.GetComponent<SpriteRenderer>().sprite = enemy.weapon1;
        weapon2.GetComponent<SpriteRenderer>().sprite = enemy.weapon2;
        theSR.sprite = body;
    }
}
