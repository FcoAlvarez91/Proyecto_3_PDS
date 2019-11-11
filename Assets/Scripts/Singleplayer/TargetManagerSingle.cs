using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManagerSingle : MonoBehaviour
{
    public PlayerManagerSingle player;
    private HealthManagerSingle theHM;

    // Start is called before the first frame update
    void Start()
    {
        theHM = GetComponent<HealthManagerSingle>();
    }

    public void scrollEffect(GameObject scrollObject)
    {
        Scroll scroll = scrollObject.GetComponent<ScrollHolderSingle>().scroll;
        Debug.Log("Scroll Effect Run: " + scroll.name);
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

        if (scroll.type.Contains("attack"))
        {
            if (theHM.conditions.Contains("Vulnerable"))
            {
                extraDamage = extraDamage + 2;
            }
            theHM.HurtEntity(scroll.damage + extraDamage);
        }
        if (scroll.type.Contains("heal"))
        {
            if (theHM.conditions.Contains("Wounds"))
            {
                extraHeal = extraHeal - 2;
            }
            theHM.HealEntity(scroll.heal + extraHeal);
        }

        foreach(string cond in scroll.conditions)
        {
            theHM.setCondition(cond);
        }
    }
}
