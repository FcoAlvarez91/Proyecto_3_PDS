using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Functions;

public class EnemyManager : MonoBehaviour
{

    public LedgerManager ledgerManager;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public HealthManager health1;
    public HealthManager health2;
    public HealthManager health3;
    public EnemyDisplay disp1;
    public EnemyDisplay disp2;
    public EnemyDisplay disp3;
    public Animator anim1;
    public Animator anim2;
    public Animator anim3;

    public GameObject grid0;
    public GameObject grid1;
    public GameObject grid2;
    public GameObject grid3;

    public HealthManager fireMage;
    public HealthManager waterMage;
    public HealthManager earthMage;
    public HealthManager windMage;

    private int level;

    public Enemy[] enemyDC0;
    public Enemy[] enemyDC1;
    public Enemy[] enemyDC2;
    public Enemy[] enemyDC3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void getEnemies()
    {
        Enemy en1 = null;
        Enemy en2 = null;
        Enemy en3 = null;
        level = int.Parse(TurnManager.data["level"].ToString());

        if (level == 0)
        {
            en1 = enemyDC0[1];
            en2 = enemyDC0[2];
            en3 = enemyDC0[3];
            grid0.SetActive(true);
            grid1.SetActive(false);
            grid2.SetActive(false);
            grid3.SetActive(false);
        }
        else if (level <= 2)
        {
            en1 = enemyDC0[0];
            en2 = enemyDC1[0];
            en3 = enemyDC0[2];
            grid0.SetActive(true);
            grid1.SetActive(false);
            grid2.SetActive(false);
            grid3.SetActive(false);
        }
        else if (level <= 4)
        {
            en1 = enemyDC1[1];
            en2 = enemyDC1[2];
            en3 = enemyDC0[3];
            grid0.SetActive(false);
            grid1.SetActive(true);
            grid2.SetActive(false);
            grid3.SetActive(false);
        }
        else if (level <= 6)
        {
            en1 = enemyDC0[1];
            en2 = enemyDC2[0];
            en3 = enemyDC1[3];
            grid0.SetActive(false);
            grid1.SetActive(true);
            grid2.SetActive(false);
            grid3.SetActive(false);
        }
        else if (level <= 8)
        {
            en1 = enemyDC1[1];
            en2 = enemyDC2[1];
            en3 = enemyDC1[3];
            grid0.SetActive(false);
            grid1.SetActive(false);
            grid2.SetActive(true);
            grid3.SetActive(false);
        }
        else if(level == 10)
        {
            en1 = enemyDC3[0];
            en2 = enemyDC3[1];
            en3 = enemyDC3[2];
            grid0.SetActive(false);
            grid1.SetActive(false);
            grid2.SetActive(false);
            grid3.SetActive(true);
        }
        else
        {
            en1 = enemyDC2[1];
            en2 = enemyDC2[2];
            en3 = enemyDC2[3];
            grid0.SetActive(false);
            grid1.SetActive(false);
            grid2.SetActive(true);
            grid3.SetActive(false);
        }
        assignEnemies(en1, en2, en3);
    }


    public void setEnemiesLevel()
    {
        Enemy en1 = null;
        Enemy en2 = null;
        Enemy en3 = null;
        level = int.Parse(TurnManager.data["level"].ToString());

        if (level == 0)
        {
            en1 = enemyDC0[1];
            en2 = enemyDC0[2];
            en3 = enemyDC0[3];
        }
        else if (level <= 2)
        {
            en1 = enemyDC0[0];
            en2 = enemyDC1[0];
            en3 = enemyDC0[2];
        }
        else if (level <= 4)
        {
            en1 = enemyDC1[1];
            en2 = enemyDC1[2];
            en3 = enemyDC0[3];
        }
        else if (level <= 6)
        {
            en1 = enemyDC0[1];
            en2 = enemyDC2[0];
            en3 = enemyDC1[3];
        }
        else if (level <= 8)
        {
            en1 = enemyDC1[1];
            en2 = enemyDC2[1];
            en3 = enemyDC1[3];
        }
        else if (level == 10)
        {
            en1 = enemyDC3[0];
            en2 = enemyDC3[1];
            en3 = enemyDC3[2];
        }
        else
        {
            en1 = enemyDC2[1];
            en2 = enemyDC2[2];
            en3 = enemyDC2[3];
        }

        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("enemy1").SetValueAsync(en1.maxHP);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("enemy2").SetValueAsync(en2.maxHP);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("enemy3").SetValueAsync(en3.maxHP);
    }

    public void assignEnemies(Enemy en1, Enemy en2, Enemy en3)
    {
        disp1.enemy = en1;
        disp2.enemy = en2;
        disp3.enemy = en3;

        disp1.assignInfo("enemy1");
        disp2.assignInfo("enemy2");
        disp3.assignInfo("enemy3");
    }

    public void resetTurn()
    {
        Debug.Log("Reseting enemy turn.");
        anim1.SetBool("isStunned", false);
        anim2.SetBool("isStunned", false);
        anim3.SetBool("isStunned", false);
        health1.resetConditions();
        health2.resetConditions();
        health3.resetConditions();
        Debug.Log("Enemy turn reset.");
    }

    public void enemyTurn()
    {
        Debug.Log("Enemy turn.");
        ledgerManager.resetLedger();
        if (health1.currentHealth > 0)
        {
            if (!health1.conditions.Contains("Stunned"))
            {
                enemyAct(anim1, health1, disp1.enemy.damage, 1);
            }
            else
            {
                anim1.SetBool("isStunned", true);
            }
        }
        if (health2.currentHealth > 0)
        {
            if (!health2.conditions.Contains("Stunned"))
            {
                enemyAct(anim2, health2, disp2.enemy.damage, 2);
            }
            else
            {
                anim2.SetBool("isStunned", true);
            }
        }
        if (health3.currentHealth > 0)
        {
            if (!health3.conditions.Contains("Stunned"))
            {
                enemyAct(anim3, health3, disp3.enemy.damage, 3);
            }
            else
            {
                anim3.SetBool("isStunned", true);
            }
        }
        if (health1.currentHealth <= 0 && health2.currentHealth <= 0 && health3.currentHealth <= 0)
        {
            Debug.Log("Level Up!");
            level++;
            TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("levelUp").SetValueAsync(1);
            TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("level").SetValueAsync(level);
            setEnemiesLevel();
        }
        Debug.Log("Enemy turn ending.");
    }

    public void enemyAct(Animator anim, HealthManager health, int damage, int num)
    {
        int target = int.Parse(TurnManager.data["target" + num].ToString());
        Debug.Log("Enemy acting.");
        if (target == 0)
        {
            if (fireMage.currentHealth > 0)
            {
                fireMage.HurtEntity(damage);
                ledgerManager.write("Enemy" + num + " attacked the Fire Mage for " + damage + " Damage." + "\n", num);
                anim.SetTrigger("attack");
            }
            else
            {
                health.setCondition("Defend");
                anim.SetTrigger("defend");
                ledgerManager.write("Enemy" + num + " protected itself." + "\n", num);
            }
        }
        else if (target == 1)
        {
            if (waterMage.currentHealth > 0) 
            { 
                waterMage.HurtEntity(damage);
                anim.SetTrigger("attack");
                ledgerManager.write("Enemy" + num + " attacked the Water Mage for " + damage + " Damage." + "\n", num);
            }
            else
            {
                health.setCondition("Defend");
                anim.SetTrigger("defend");
                ledgerManager.write("Enemy" + num + " protected itself." + "\n", num);
            }
        }
        else if (target == 2)
        {
            if(earthMage.currentHealth > 0)
            {
                earthMage.HurtEntity(damage);
                anim.SetTrigger("attack");
                ledgerManager.write("Enemy" + num + " attacked the Earth Mage for " + damage + " Damage." + "\n", num);
            }
            else
            {
                health.setCondition("Defend");
                anim.SetTrigger("defend");
                ledgerManager.write("Enemy" + num + " protected itself." + "\n", num);
            }
        }
        else if (target == 3)
        {
            if(windMage.currentHealth > 0)
            {
                windMage.HurtEntity(damage);
                anim.SetTrigger("attack");
                ledgerManager.write("Enemy" + num + " attacked the Wind Mage for " + damage + " Damage." + "\n", num);
            }
            else
            {
                health.setCondition("Defend");
                anim.SetTrigger("defend");
                ledgerManager.write("Enemy" + num + " protected itself." + "\n", num);
            }
        }
        else if (target == 4)
        {
            health.setCondition("Defend");
            anim.SetTrigger("defend");
            ledgerManager.write("Enemy" + num + " protected itself." + "\n", num);
        }
    }

    public void sendEnemyInfo()
    {
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("enemy1").SetValueAsync(health1.currentHealth);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("enemy2").SetValueAsync(health2.currentHealth);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("enemy3").SetValueAsync(health3.currentHealth);
        Debug.LogFormat("Firebase enemies updated");
    }

    public void sendMageInfo()
    {
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("mageFire").SetValueAsync(fireMage.currentHealth);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("mageWater").SetValueAsync(waterMage.currentHealth);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("mageEarth").SetValueAsync(earthMage.currentHealth);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("mageWind").SetValueAsync(windMage.currentHealth);

    }

    public void pullMageInfo()
    {

        fireMage.currentHealth = int.Parse(TurnManager.data["mageFire"].ToString());
        waterMage.currentHealth = int.Parse(TurnManager.data["mageWater"].ToString());
        earthMage.currentHealth = int.Parse(TurnManager.data["mageEarth"].ToString());
        windMage.currentHealth = int.Parse(TurnManager.data["mageWind"].ToString());
    }
}
