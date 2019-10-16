using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Functions;

public class TargetManager : MonoBehaviour
{
    public PlayerManager player;
    private HealthManager theHM;
    public string targetName;

    // Start is called before the first frame update
    void Start()
    {
        theHM = GetComponent<HealthManager>();
    }

    public void scrollEffect(Scroll scroll)
    {
        Debug.Log("Running scroll effect: " + scroll.name);
        int extraDamage = 0;
        int extraHeal = 0;
        if (scroll.energyType == "fire" && player.mageElement == "fire")
        {
            extraDamage = extraDamage + 2;
        }
        if (scroll.energyType == "water" && player.mageElement == "water")
        {
            extraHeal = extraHeal + 2;
        }
        if (scroll.energyType == "earth" && player.mageElement == "earth")
        {
            theHM.setCondition("Vulnerable");
        }
        if (scroll.energyType == "wind" && player.mageElement == "wind")
        {
            theHM.setCondition("Wounds");
        }
        theHM.HurtEntity(scroll.damage + extraDamage);
        theHM.HealEntity(scroll.heal + extraHeal);
        foreach(string cond in scroll.conditions)
        {
            theHM.setCondition(cond);
            Debug.Log(cond + " added.");
        }
        Debug.Log(scroll.name + " effect run.");
    }

    public void setTurnEffect(GameObject scrollObject)
    {
        long mil = System.DateTime.Now.Ticks;
        Scroll scroll = scrollObject.GetComponent<ScrollHolder>().scroll;
        string added = "effect_" + TurnManager.auth.CurrentUser.Email.ToString().Replace('.',',');
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("turn").Child(added).Child("scroll").SetValueAsync(scroll.id);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("turn").Child(added).Child("target").SetValueAsync(targetName);
        TurnManager.change = true;
    }
}