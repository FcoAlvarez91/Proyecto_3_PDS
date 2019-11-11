using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerSingle : MonoBehaviour
{

    public LedgerManagerSingle ledgerManager;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject grid0;
    public GameObject grid1;
    public GameObject grid2;
    public GameObject grid3;

    public GameObject mage;

    public int level;
    private int bossRun;
    public Enemy[] enemyDC0;
    public Enemy[] enemyDC1;
    public Enemy[] enemyDC2;
    public Enemy[] enemyDC3;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        bossRun = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMage(GameObject inputMage)
    {
        mage = inputMage;
    }

    public void getEnemies()
    {
        int IDenemy1 = (int)Random.Range(0, enemyDC0.Length);
        int IDenemy2 = (int)Random.Range(0, enemyDC0.Length);
        int IDenemy3 = (int)Random.Range(0, enemyDC0.Length);

        Enemy en1 = null;
        Enemy en2 = null;
        Enemy en3 = null;

        if (level <= 0)
        {
            en1 = enemyDC0[IDenemy1];
            en2 = enemyDC0[IDenemy2];
            en3 = enemyDC0[IDenemy3];
            grid0.SetActive(true);
            grid1.SetActive(false);
            grid2.SetActive(false);
            grid3.SetActive(false);
        }
        else if (level <= 2)
        {
            en1 = enemyDC0[IDenemy1];
            en2 = enemyDC1[IDenemy2];
            en3 = enemyDC0[IDenemy3];
            grid0.SetActive(true);
            grid1.SetActive(false);
            grid2.SetActive(false);
            grid3.SetActive(false);
        }
        else if (level <= 4)
        {
            en1 = enemyDC1[IDenemy1];
            en2 = enemyDC1[IDenemy2];
            en3 = enemyDC0[IDenemy3];
            grid0.SetActive(false);
            grid1.SetActive(true);
            grid2.SetActive(false);
            grid3.SetActive(false);
        }
        else if (level <= 6)
        {
            en1 = enemyDC0[IDenemy1];
            en2 = enemyDC2[IDenemy2];
            en3 = enemyDC1[IDenemy3];
            grid0.SetActive(false);
            grid1.SetActive(true);
            grid2.SetActive(false);
            grid3.SetActive(false);
        }
        else if (level <= 8)
        {
            en1 = enemyDC1[IDenemy1];
            en2 = enemyDC2[IDenemy2];
            en3 = enemyDC1[IDenemy3];
            grid0.SetActive(false);
            grid1.SetActive(false);
            grid2.SetActive(true);
            grid3.SetActive(false);
        }
        else if(level/10 == bossRun)
        {
            en1 = enemyDC3[0];
            en2 = enemyDC3[1];
            en3 = enemyDC3[2];
            grid0.SetActive(false);
            grid1.SetActive(false);
            grid2.SetActive(false);
            grid3.SetActive(true);
            bossRun++;
        }
        else
        {
            en1 = enemyDC2[IDenemy1];
            en2 = enemyDC2[IDenemy2];
            en3 = enemyDC2[IDenemy3];
            grid0.SetActive(false);
            grid1.SetActive(false);
            grid2.SetActive(true);
            grid3.SetActive(false);
        }
        Debug.Log("Level: " + level);
        GameManager.goldGained = level;
        level++;

        assignEnemies(en1, en2, en3);
    }

    public void assignEnemies(Enemy en1, Enemy en2, Enemy en3)
    {
        enemy1.GetComponent<EnemyDisplaySingle>().enemy = en1;
        enemy2.GetComponent<EnemyDisplaySingle>().enemy = en2;
        enemy3.GetComponent<EnemyDisplaySingle>().enemy = en3;

        enemy1.GetComponent<EnemyDisplaySingle>().assignInfo();
        enemy2.GetComponent<EnemyDisplaySingle>().assignInfo();
        enemy3.GetComponent<EnemyDisplaySingle>().assignInfo();
    }

    public void enemyTurn()
    {

        ledgerManager.resetLedger();

        if (enemy1.GetComponent<HealthManagerSingle>().currentHealth <= 0 &&
            enemy2.GetComponent<HealthManagerSingle>().currentHealth <= 0 &&
            enemy3.GetComponent<HealthManagerSingle>().currentHealth <= 0)
        {
            getEnemies();
        }
        else
        {
            if (enemy1.GetComponent<HealthManagerSingle>().currentHealth > 0)
            {
                if (!enemy1.GetComponent<HealthManagerSingle>().conditions.Contains("Stunned"))
                {
                    attack(enemy1);
                }
                else
                {
                    enemy1.GetComponent<Animator>().SetBool("isStunned", true);
                }
            }
            if (enemy2.GetComponent<HealthManagerSingle>().currentHealth > 0)
            {
                if (!enemy2.GetComponent<HealthManagerSingle>().conditions.Contains("Stunned"))
                {
                    attack(enemy2);
                }
                else
                {
                    enemy1.GetComponent<Animator>().SetBool("isStunned", true);
                }
            }
            if (enemy3.GetComponent<HealthManagerSingle>().currentHealth > 0)
            {
                if (!enemy3.GetComponent<HealthManagerSingle>().conditions.Contains("Stunned"))
                {
                    attack(enemy3);
                }
                else
                {
                    enemy1.GetComponent<Animator>().SetBool("isStunned", true);
                }
            }
        }
    }

    public void attack(GameObject enemy)
    {
        int action = Random.Range(0, 2);
        if(action == 0)
        {
            mage.GetComponent<HealthManagerSingle>().HurtEntity(enemy.GetComponent<EnemyDisplaySingle>().enemy.damage);
            enemy.GetComponent<Animator>().SetTrigger("attack");
            ledgerManager.write(enemy.name + " attacked the Fire Mage for " + enemy.GetComponent<EnemyDisplaySingle>().enemy.damage + " Damage." + "\n");
        }
        else if (action == 1)
        {
            enemy.GetComponent<HealthManagerSingle>().setCondition("Defend");
            enemy.GetComponent<Animator>().SetTrigger("defend");
            ledgerManager.write(enemy.name + " protected itself." + "\n");
        }
    }

    public void resetTurn()
    {
        enemy1.GetComponent<Animator>().SetBool("isStunned", false);
        enemy2.GetComponent<Animator>().SetBool("isStunned", false);
        enemy2.GetComponent<Animator>().SetBool("isStunned", false);
        enemy1.GetComponent<HealthManagerSingle>().resetConditions();
        enemy2.GetComponent<HealthManagerSingle>().resetConditions();
        enemy3.GetComponent<HealthManagerSingle>().resetConditions();
    }

    public void startTurn()
    {
        enemy1.GetComponent<HealthManagerSingle>().removeDefend();
        enemy2.GetComponent<HealthManagerSingle>().removeDefend();
        enemy3.GetComponent<HealthManagerSingle>().removeDefend();
    }
}
