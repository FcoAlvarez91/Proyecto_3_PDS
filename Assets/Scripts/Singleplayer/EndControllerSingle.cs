using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndControllerSingle : MonoBehaviour
{

    public GameObject endPanel;
    public TextMeshProUGUI endText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WaitFor2()
    {
        yield return new WaitForSeconds(1);
        endGame();
    }

    public void goGameOver(bool death)
    {
        endPanel.SetActive(true);
        if (death)
        {
            endText.SetText("You Died!");
        }
        else
        {
            endText.SetText("Out of Scrolls!");
        }
        StartCoroutine(WaitFor2());
    }

    public void endGame()
    {

        SceneManager.LoadScene("Game Over");
    }

    public void goInGame()
    {

        SceneManager.LoadScene("In Game");
    }

    public void goMenu()
    {

        SceneManager.LoadScene("Menu");
    }
}
