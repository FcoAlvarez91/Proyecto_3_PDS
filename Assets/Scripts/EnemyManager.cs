using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public LedgerManager ledgerManager;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject grid0;
    public GameObject grid1;
    public GameObject grid2;
    public GameObject grid3;

    public GameObject fireMage;
    public GameObject waterMage;
    public GameObject earthMage;
    public GameObject windMage;

    private int level;
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
        if(enemy1.GetComponent<HealthManager>().currentHealth <= 0 && 
            enemy2.GetComponent<HealthManager>().currentHealth <= 0 && 
            enemy3.GetComponent<HealthManager>().currentHealth <= 0)
        {
            getEnemies();
        }
    }

    public void getEnemies()
    {

        Enemy en1 = null;
        Enemy en2 = null;
        Enemy en3 = null;

        if (level == 0)
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
        level++;

        assignEnemies(en1, en2, en3);
    }

    public void assignEnemies(Enemy en1, Enemy en2, Enemy en3)
    {
        enemy1.GetComponent<EnemyDisplay>().enemy = en1;
        enemy2.GetComponent<EnemyDisplay>().enemy = en2;
        enemy3.GetComponent<EnemyDisplay>().enemy = en3;

        enemy1.GetComponent<EnemyDisplay>().assignInfo();
        enemy2.GetComponent<EnemyDisplay>().assignInfo();
        enemy3.GetComponent<EnemyDisplay>().assignInfo();
    }

    public void enemyTurn()
    {
        ledgerManager.resetLedger();
        if (enemy1.GetComponent<HealthManager>().currentHealth > 0)
        {
            if (!enemy1.GetComponent<HealthManager>().conditions.Contains("Stunned"))
            {
                enemyAct(enemy1);
            }
            else
            {
                enemy1.GetComponent<Animator>().SetBool("isStunned", true);
            }
        }
        if (enemy2.GetComponent<HealthManager>().currentHealth > 0)
        {
            if (!enemy2.GetComponent<HealthManager>().conditions.Contains("Stunned"))
            {
                enemyAct(enemy2);
            }
            else
            {
                enemy1.GetComponent<Animator>().SetBool("isStunned", true);
            }
        }
        if (enemy3.GetComponent<HealthManager>().currentHealth > 0)
        {
            if (!enemy3.GetComponent<HealthManager>().conditions.Contains("Stunned"))
            {
                enemyAct(enemy3);
            }
            else
            {
                enemy1.GetComponent<Animator>().SetBool("isStunned", true);
            }
        }
    }

    public void enemyAct(GameObject enemy)
    {
        int target = Random.Range(0, 4);
        int action = Random.Range(0, 2);
        if(action == 0)
        {
            if (target == 0)
            {
                if (fireMage.GetComponent<HealthManager>().currentHealth > 0)
                {
                    fireMage.GetComponent<HealthManager>().HurtEntity(enemy.GetComponent<EnemyDisplay>().enemy.damage);
                    enemy.GetComponent<Animator>().SetTrigger("attack");
                    ledgerManager.write(enemy.name + " attacked the Fire Mage for " + enemy.GetComponent<EnemyDisplay>().enemy.damage + " Damage." + "\n");
                }
                else
                {
                    enemyAct(enemy);
                }
            }
            else if (target == 1)
            {
                if (waterMage.GetComponent<HealthManager>().currentHealth > 0) 
                { 
                    waterMage.GetComponent<HealthManager>().HurtEntity(enemy.GetComponent<EnemyDisplay>().enemy.damage);
                    enemy.GetComponent<Animator>().SetTrigger("attack");
                    ledgerManager.write(enemy.name + " attacked the Water Mage for " + enemy.GetComponent<EnemyDisplay>().enemy.damage + " Damage." + "\n");
                }
                else
                {
                    enemyAct(enemy);
                }
            }
            else if (target == 2)
            {
                if(earthMage.GetComponent<HealthManager>().currentHealth > 0)
                {
                    earthMage.GetComponent<HealthManager>().HurtEntity(enemy.GetComponent<EnemyDisplay>().enemy.damage);
                    enemy.GetComponent<Animator>().SetTrigger("attack");
                    ledgerManager.write(enemy.name + " attacked the Earth Mage for " + enemy.GetComponent<EnemyDisplay>().enemy.damage + " Damage." + "\n");
                }
                else
                {
                    enemyAct(enemy);
                }
            }
            else if (target == 3)
            {
                if(windMage.GetComponent<HealthManager>().currentHealth > 0)
                {
                    windMage.GetComponent<HealthManager>().HurtEntity(enemy.GetComponent<EnemyDisplay>().enemy.damage);
                    enemy.GetComponent<Animator>().SetTrigger("attack");
                    ledgerManager.write(enemy.name + " attacked the Wind Mage for " + enemy.GetComponent<EnemyDisplay>().enemy.damage + " Damage." + "\n");
                }
                else
                {
                    enemyAct(enemy);
                }
            }
        }
        else if (action == 1)
        {
            enemy.GetComponent<HealthManager>().setCondition("Defend");
            enemy.GetComponent<Animator>().SetTrigger("defend");
            ledgerManager.write(enemy.name + " protected itself." + "\n");
        }
    }

    public void resetTurn()
    {
        enemy1.GetComponent<Animator>().SetBool("isStunned", false);
        enemy2.GetComponent<Animator>().SetBool("isStunned", false);
        enemy2.GetComponent<Animator>().SetBool("isStunned", false);
        enemy1.GetComponent<HealthManager>().resetConditions();
        enemy2.GetComponent<HealthManager>().resetConditions();
        enemy3.GetComponent<HealthManager>().resetConditions();
    }
}
