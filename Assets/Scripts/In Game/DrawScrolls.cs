using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawScrolls : MonoBehaviour
{

    public GameObject scroll1;
    public GameObject scroll2;
    public GameObject scroll3;
    private ScrollDisplay scrollDisplay1;
    private ScrollDisplay scrollDisplay2;
    private ScrollDisplay scrollDisplay3;
    private Animator scrollAnimator1;
    private Animator scrollAnimator2;
    private Animator scrollAnimator3;

    // Start is called before the first frame update
    void Start()
    {   
        scrollDisplay1 = scroll1.GetComponent<ScrollDisplay>();
        scrollDisplay2 = scroll2.GetComponent<ScrollDisplay>();
        scrollDisplay3 = scroll3.GetComponent<ScrollDisplay>();

        scrollAnimator1 = scroll1.GetComponent<Animator>();
        scrollAnimator2 = scroll2.GetComponent<Animator>();
        scrollAnimator3 = scroll3.GetComponent<Animator>();

    }

    public void pullFromDeck()
    {
        // ----- Change this for db scroll pick ----- //
        int IDscroll1 = (int)Random.Range(0, GameManager.deck.Count);
        int IDscroll2 = (int)Random.Range(0, GameManager.deck.Count);
        int IDscroll3 = (int)Random.Range(0, GameManager.deck.Count);
        // ----------------------------------------- //

        scroll1.SetActive(true);
        scroll2.SetActive(true);
        scroll3.SetActive(true);

        scrollAnimator1.SetBool("notTurn", false);
        scrollAnimator2.SetBool("notTurn", false);
        scrollAnimator3.SetBool("notTurn", false);

        Dictionary<string, object> scrollInfo1 = (Dictionary<string, object>)GameManager.deck["scroll" + IDscroll1];
        Scroll sc1 = GameManager.allScrolls[int.Parse(scrollInfo1["id"].ToString())];

        Dictionary<string, object> scrollInfo2 = (Dictionary<string, object>)GameManager.deck["scroll" + IDscroll2];
        Scroll sc2 = GameManager.allScrolls[int.Parse(scrollInfo2["id"].ToString())];

        Dictionary<string, object> scrollInfo3 = (Dictionary<string, object>)GameManager.deck["scroll" + IDscroll3];
        Scroll sc3 = GameManager.allScrolls[int.Parse(scrollInfo3["id"].ToString())];

        assignScroll(sc1, sc2, sc3);

        scrollAnimator1.SetTrigger("draw");
        scrollAnimator2.SetTrigger("draw");
        scrollAnimator3.SetTrigger("draw");
    }

    public void notTurn()
    {

        scrollAnimator1 = scroll1.GetComponent<Animator>();
        scrollAnimator2 = scroll2.GetComponent<Animator>();
        scrollAnimator3 = scroll3.GetComponent<Animator>();

        scrollAnimator1.SetBool("notTurn", true);
        scrollAnimator2.SetBool("notTurn", true);
        scrollAnimator3.SetBool("notTurn", true);
    }

    public void assignScroll(Scroll deckScroll1, Scroll deckScroll2, Scroll deckScroll3)
    {

        scrollDisplay1 = scroll1.GetComponent<ScrollDisplay>();
        scrollDisplay2 = scroll2.GetComponent<ScrollDisplay>();
        scrollDisplay3 = scroll3.GetComponent<ScrollDisplay>();

        scrollDisplay1.scroll = deckScroll1;
        scrollDisplay2.scroll = deckScroll2;
        scrollDisplay3.scroll = deckScroll3;

        scrollDisplay1.setInfo();
        scrollDisplay2.setInfo();
        scrollDisplay3.setInfo();
    }
}
