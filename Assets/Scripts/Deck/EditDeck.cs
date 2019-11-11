using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditDeck : MonoBehaviour
{

    public string scrollID;
    public GameObject scrollShower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addDeck()
    {
        if (!DeckManager.inDeck.Contains(scrollID)) 
        {
            DeckManager.inDeck.Add(scrollID);
            scrollShower.SetActive(true);
        }
        else
        {
            DeckManager.inDeck.Remove(scrollID);
            scrollShower.SetActive(false);
        }
    }
}
