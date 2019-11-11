using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Functions;

public class EnemyDisplay : MonoBehaviour
{

    public Enemy enemy;
    private Sprite body;
    public GameObject weapon1;
    public GameObject weapon2;
    private HealthManager theHM;
    private SpriteRenderer theSR;
    private TargetManager theTM;

    // Start is called before the first frame update
    void Start()
    {

    }


    public void assignInfo(string enemyName)
    {
        theHM = GetComponent<HealthManager>();
        theSR = GetComponent<SpriteRenderer>();
        theTM = GetComponent<TargetManager>();

        theHM.maxHealth = enemy.maxHP;
        body = enemy.body;
        weapon1.GetComponent<SpriteRenderer>().sprite = enemy.weapon1;
        weapon2.GetComponent<SpriteRenderer>().sprite = enemy.weapon2;
        theSR.sprite = body;

        getEnemyInfo(enemyName);
    }

    public void getEnemyInfo(string enemyName)
    {
        Debug.Log("HP assigned.");
        theHM.currentHealth = int.Parse(TurnManager.data[theTM.targetName].ToString());
    }
}
