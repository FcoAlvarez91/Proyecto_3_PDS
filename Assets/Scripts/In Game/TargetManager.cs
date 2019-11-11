using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Functions;

public class TargetManager : MonoBehaviour
{
    public PlayerManager player;
    private HealthManager theHM;
    public string targetName;
    public TextMeshProUGUI targetText;

    // Start is called before the first frame update
    void Start()
    {
        theHM = GetComponent<HealthManager>();
    }

    public string[] scrollEffect(Scroll scroll)
    {
        string[] scrollData = new string[3];
        int extraDamage = 0;
        int extraHeal = 0;
        scrollData[0] = "0";
        scrollData[1] = "0";
        scrollData[2] = "";
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
            scrollData[2] += " Vulnerable,";
        }
        if (scroll.energyType == "wind" && player.mageElement == "wind")
        {
            scrollData[2] += " Wounds,";
        }
        if (scroll.type.Contains("attack"))
        {
            if (theHM.conditions.Contains("Vulnerable"))
            {
                extraDamage = extraDamage + 2;
            }
            scrollData[0] = (scroll.damage + extraDamage).ToString();
        }
        if (scroll.type.Contains("Heal"))
        {
            if (theHM.conditions.Contains("Wounds"))
            {
                extraHeal = extraHeal - 2;
            }
            scrollData[1] = (scroll.heal + extraHeal).ToString();
        }

        foreach(string cond in scroll.conditions)
        {
            scrollData[2] += " " + cond + ",";
        }
        
        return scrollData;
    }

    public void setTurnEffect(GameObject scrollObject)
    {
        Scroll scroll = scrollObject.GetComponent<ScrollHolder>().scroll;
        string[] scrollRun = scrollEffect(scroll);
        string added = "effect_" + TurnManager.auth.CurrentUser.UserId;
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("turn").Child(added).Child("scroll").SetValueAsync(scroll.id);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("turn").Child(added).Child("target").SetValueAsync(targetName);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("turn").Child(added).Child("attack").SetValueAsync(scrollRun[0]);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("turn").Child(added).Child("heal").SetValueAsync(scrollRun[1]);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("turn").Child(added).Child("conditions").SetValueAsync(scrollRun[2]);
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("starts").Child("start_" + player.mageElement).SetValueAsync(0);
        targetText.text = "SCROLL PLAYED";
    }
}