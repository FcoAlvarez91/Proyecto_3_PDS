using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagerSingle : MonoBehaviour
{
    public string mageElement;

    public GameObject fireMage;
    public GameObject waterMage;
    public GameObject earthMage;
    public GameObject windMage;

    public GameObject endHP;

    public EnemyManagerSingle theEM;

    public GameObject currentMage;

    public List<Scroll> deck;

    // Start is called before the first frame update
    void Start()
    {
        setElement(GameManager.singleMage); 
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMage.GetComponent<HealthManagerSingle>().currentHealth <= 0)
        {
            //endHP.SetActive(true);
            GetComponent<EndControllerSingle>().goGameOver(true);
        }
    }

    public void setElement(string element)
    {
        mageElement = element;
        if (mageElement == "fire")
        {
            currentMage = fireMage;
            fireMage.SetActive(true);
            fireMage.GetComponent<Animator>().SetBool("isSelected", true);
            waterMage.SetActive(false);
            earthMage.SetActive(false);
            windMage.SetActive(false);
            theEM.setMage(fireMage);
        }
        else if (mageElement == "water")
        {
            currentMage = waterMage;
            fireMage.SetActive(false);
            waterMage.SetActive(true);
            waterMage.GetComponent<Animator>().SetBool("isSelected", true);
            earthMage.SetActive(false);
            windMage.SetActive(false);
            theEM.setMage(waterMage);
        }
        else if (mageElement == "earth")
        {
            currentMage = earthMage;
            fireMage.SetActive(false);
            waterMage.SetActive(false);
            earthMage.SetActive(true);
            earthMage.GetComponent<Animator>().SetBool("isSelected", true);
            windMage.SetActive(false);
            theEM.setMage(earthMage);
        }
        else if (mageElement == "wind")
        {
            currentMage = windMage;
            fireMage.SetActive(false);
            waterMage.SetActive(false);
            earthMage.SetActive(false);
            windMage.SetActive(true);
            windMage.GetComponent<Animator>().SetBool("isSelected", true);
            theEM.setMage(windMage);
        }
    }
}
