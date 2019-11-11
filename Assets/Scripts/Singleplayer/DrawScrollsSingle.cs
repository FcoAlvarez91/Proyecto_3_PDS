using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawScrollsSingle : MonoBehaviour
{

    public EndControllerSingle endControllerSingle;
    public int lastPlayed;
    public GameObject endDeck;
    public GameObject scroll1;
    public GameObject scroll2;
    public GameObject scroll3;
    private ScrollDisplaySingle scrollDisplay1;
    private ScrollDisplaySingle scrollDisplay2;
    private ScrollDisplaySingle scrollDisplay3;
    private Animator scrollAnimator1;
    private Animator scrollAnimator2;
    private Animator scrollAnimator3;
    public static int scrollsInDeck;

    // Start is called before the first frame update
    void Start()
    {   
        scrollDisplay1 = scroll1.GetComponent<ScrollDisplaySingle>();
        scrollDisplay2 = scroll2.GetComponent<ScrollDisplaySingle>();
        scrollDisplay3 = scroll3.GetComponent<ScrollDisplaySingle>();

        scrollAnimator1 = scroll1.GetComponent<Animator>();
        scrollAnimator2 = scroll2.GetComponent<Animator>();
        scrollAnimator3 = scroll3.GetComponent<Animator>();
    }

    public void pullFromDeck()
    {
        if (TurnManagerSingle.singleDeck.Count > 2)
        {

            List<string> keyList = new List<string>(((Dictionary<string, object>)TurnManagerSingle.singleDeck).Keys);

            string randomKey1 = keyList[(int)Random.Range(0, ((Dictionary<string, object>)TurnManagerSingle.singleDeck).Count)];
            Dictionary<string, object> scrollOBJ1 = (Dictionary<string, object>)TurnManagerSingle.singleDeck[randomKey1];
            Scroll sc1 = GameManager.allScrolls[int.Parse(scrollOBJ1["id"].ToString())];
            TurnManagerSingle.singleDeck.Remove(randomKey1);
            Debug.Log("Scroll key: " + randomKey1);

            keyList = new List<string>(((Dictionary<string, object>)TurnManagerSingle.singleDeck).Keys);

            string randomKey2 = keyList[(int)Random.Range(0, ((Dictionary<string, object>)TurnManagerSingle.singleDeck).Count)];
            Dictionary<string, object> scrollOBJ2 = (Dictionary<string, object>)TurnManagerSingle.singleDeck[randomKey2];
            Scroll sc2 = GameManager.allScrolls[int.Parse(scrollOBJ2["id"].ToString())];
            TurnManagerSingle.singleDeck.Remove(randomKey2);
            Debug.Log("Scroll key: " + randomKey2);

            keyList = new List<string>(((Dictionary<string, object>)TurnManagerSingle.singleDeck).Keys);

            string randomKey3 = keyList[(int)Random.Range(0, ((Dictionary<string, object>)TurnManagerSingle.singleDeck).Count)];
            Dictionary<string, object> scrollOBJ3 = (Dictionary<string, object>)TurnManagerSingle.singleDeck[randomKey3];
            Scroll sc3 = GameManager.allScrolls[int.Parse(scrollOBJ3["id"].ToString())];
            TurnManagerSingle.singleDeck.Remove(randomKey3);
            Debug.Log("Scroll key: " + randomKey3);

            scroll1.GetComponent<Animator>().SetBool("notTurn", false);
            scroll2.GetComponent<Animator>().SetBool("notTurn", false);
            scroll3.GetComponent<Animator>().SetBool("notTurn", false);

            assignScroll(sc1, sc2, sc3);

            scrollAnimator1.SetTrigger("draw");
            scrollAnimator2.SetTrigger("draw");
            scrollAnimator3.SetTrigger("draw");
        }
    }

    public void drawScroll()
    {
        if (TurnManagerSingle.singleDeck.Count >= 1)
        {
            scroll1.GetComponent<Animator>().SetBool("notTurn", false);
            scroll2.GetComponent<Animator>().SetBool("notTurn", false);
            scroll3.GetComponent<Animator>().SetBool("notTurn", false);

            List<string> keyList = new List<string>(((Dictionary<string, object>)TurnManagerSingle.singleDeck).Keys);

            string randomKey = keyList[(int)Random.Range(0, ((Dictionary<string, object>)TurnManagerSingle.singleDeck).Count)];
            Dictionary<string, object> scrollOBJ = (Dictionary<string, object>)TurnManagerSingle.singleDeck[randomKey];
            Scroll sc = GameManager.allScrolls[int.Parse(scrollOBJ["id"].ToString())];
            TurnManagerSingle.singleDeck.Remove(randomKey);
            Debug.Log("Scroll key: " + randomKey);

            if (lastPlayed == 0)
            {
                assignScroll(sc, null, null);
                scrollAnimator1.SetTrigger("draw");
            }
            if (lastPlayed == 1)
            {
                assignScroll(null, sc, null);
                scrollAnimator2.SetTrigger("draw");
            }
            if (lastPlayed == 2)
            {
                assignScroll(null, null, sc);
                scrollAnimator3.SetTrigger("draw");
            }

        }
        else
        {
            //endDeck.SetActive(true);
            endControllerSingle.goGameOver(false);
        }
    }

    public void playScrollNum(int num)
    {
        lastPlayed = num;
    }

    /* public int generateRandom(List<int> range)
    {
        Debug.Log("Scrolls in deck:" + scrollsInDeck);
        int tryNum = -1;
        if(TurnManagerSingle.singleDeck.Count > 0)
        {
            tryNum = (int)Random.Range(0, scrollsInDeck);
            if (!TurnManagerSingle.singleDeck.ContainsKey("scroll" + tryNum))
            {
                tryNum = generateRandom(range);
            }
            if (range.Contains(tryNum))
            {
                tryNum = generateRandom(range);
            }
        }
        return tryNum;
    } */

    public void assignScroll(Scroll deckScroll1, Scroll deckScroll2, Scroll deckScroll3)
    {
        if (deckScroll1)
        {
            scrollDisplay1.scroll = deckScroll1;
            scrollDisplay1.setInfo();
        }
        if (deckScroll2)
        {
            scrollDisplay2.scroll = deckScroll2;
            scrollDisplay2.setInfo();
        }
        if (deckScroll3)
        {
            scrollDisplay3.scroll = deckScroll3;
            scrollDisplay3.setInfo();
        }
    }
}
