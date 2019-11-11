using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
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
        yield return new WaitForSeconds(2);
        goGameOver();
    }

    public void goGameOver()
    {
        StartCoroutine(WaitFor2());
    }

    public void endGame()
    {
        SceneManager.LoadScene("Game Over");
    }

    public void goInOver()
    {

        SceneManager.LoadScene("In Game");
    }

    public void goMenu()
    {

        SceneManager.LoadScene("Menu");
    }
}
