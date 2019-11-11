using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public string mageElement;

    public GameObject fireMage;
    public GameObject waterMage;
    public GameObject earthMage;
    public GameObject windMage;

    public List<Scroll> deck;

    private GameObject currentMage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void checkGameOver()
    {
        if (currentMage.GetComponent<HealthManager>().currentHealth <= 0)
        {
            SceneManager.LoadScene("Game Over");
        }
    }

    public void setElement(string element)
    {
        mageElement = element;
        if (mageElement == "fire")
        {
            currentMage = fireMage;
            fireMage.GetComponent<Animator>().SetBool("isSelected", true);
            waterMage.GetComponent<Animator>().SetBool("isSelected", false);
            earthMage.GetComponent<Animator>().SetBool("isSelected", false);
            windMage.GetComponent<Animator>().SetBool("isSelected", false);
        }
        else if (mageElement == "water")
        {
            currentMage = waterMage;
            fireMage.GetComponent<Animator>().SetBool("isSelected", false);
            waterMage.GetComponent<Animator>().SetBool("isSelected", true);
            earthMage.GetComponent<Animator>().SetBool("isSelected", false);
            windMage.GetComponent<Animator>().SetBool("isSelected", false);
        }
        else if (mageElement == "earth")
        {
            currentMage = earthMage;
            fireMage.GetComponent<Animator>().SetBool("isSelected", false);
            waterMage.GetComponent<Animator>().SetBool("isSelected", false);
            earthMage.GetComponent<Animator>().SetBool("isSelected", true);
            windMage.GetComponent<Animator>().SetBool("isSelected", false);
        }
        else if (mageElement == "wind")
        {
            currentMage = windMage;
            fireMage.GetComponent<Animator>().SetBool("isSelected", false);
            waterMage.GetComponent<Animator>().SetBool("isSelected", false);
            earthMage.GetComponent<Animator>().SetBool("isSelected", false);
            windMage.GetComponent<Animator>().SetBool("isSelected", true);
        }
    }
}
