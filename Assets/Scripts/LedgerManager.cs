using System.Collections;
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

    public void write(string str)
    {
        ledgerText.text = ledgerText.text.ToString() + "\n" + str;
    }

    public void resetLedger()
    {
        ledgerText.text = "Ledger:" + "\n";
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
