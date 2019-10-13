using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public PlayerManager player;
    private HealthManager theHM;

    // Start is called before the first frame update
    void Start()
    {
        theHM = GetComponent<HealthManager>();
    }

    public void scrollEffect(GameObject scrollObject)
    {
        Scroll scroll = scrollObject.GetComponent<ScrollHolder>().scroll;
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
            theHM.setCondition("Vulnerable");
        }
        theHM.HurtEntity(scroll.damage + extraDamage);
        theHM.HealEntity(scroll.heal + extraHeal);
        foreach(string cond in scroll.conditions)
        {
            theHM.setCondition(cond);
        }
    }
}
