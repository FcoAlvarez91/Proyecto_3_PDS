﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LedgerManager : MonoBehaviour
{
    public TextMeshProUGUI ledgerText; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void write(string str, int num)
    {
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("ledger").Child("entry" + num).SetValueAsync(str);
    }

    public void pullLedger()
    {
        Dictionary<string, object> turnDict = (Dictionary<string, object>)TurnManager.data["ledger"];
        foreach (var dict in turnDict)
        {
            ledgerText.text = ledgerText.text.ToString() + "\n" + dict.Value.ToString();
        }
    }

    public void resetLedger()
    {
        ledgerText.text = "Ledger:" + "\n";
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("ledger").Child("entry1").RemoveValueAsync();
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("ledger").Child("entry2").RemoveValueAsync();
        TurnManager.reference.Child("games").Child(GameManager.currentGame).Child("ledger").Child("entry3").RemoveValueAsync();
        Debug.Log("Ledger reset.");
    }

    public void openCloseLedger()
    {
        if (GetComponent<Animator>().GetBool("isOpen"))
        {
            GetComponent<Animator>().SetBool("isOpen", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("isOpen", true);
        }
        
    }
}
