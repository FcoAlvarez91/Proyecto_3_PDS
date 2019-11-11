using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyScroll : MonoBehaviour
{

    public Scroll scroll;
    public GameObject panelNoGold;

    public void acceptBuyScroll()
    {
        
        if (ItemManager.gold >= scroll.cost)
        {
            string key = UIManager.reference.Child("decks").Child(UIManager.user.UserId).Push().Key;
            UIManager.reference.Child("decks").Child(UIManager.user.UserId).Child(key).Child("id").SetValueAsync(scroll.id);
            UIManager.reference.Child("decks").Child(UIManager.user.UserId).Child(key).Child("inDeck").SetValueAsync(0);
            UIManager.reference.Child("users").Child(UIManager.user.UserId).Child("gold").SetValueAsync(ItemManager.gold - scroll.cost);
            ItemManager.callGold();
        }
        else
        {
            panelNoGold.SetActive(true);
        }

    }


}
