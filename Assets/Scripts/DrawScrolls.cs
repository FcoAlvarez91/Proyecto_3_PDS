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
    public Scroll[] deckOptions;

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
        int IDscroll1 = (int)Random.Range(0, 4);
        int IDscroll2 = (int)Random.Range(0, 4);
        int IDscroll3 = (int)Random.Range(0, 4);
        // ----------------------------------------- //

        scrollAnimator1.SetBool("notTurn", false);
        scrollAnimator2.SetBool("notTurn", false);
        scrollAnimator3.SetBool("notTurn", false);

        Scroll sc1 = deckOptions[IDscroll1];
        Scroll sc2 = deckOptions[IDscroll2];
        Scroll sc3 = deckOptions[IDscroll3];

        assignScroll(sc1, sc2, sc3);

        scrollAnimator1.SetTrigger("draw");
        scrollAnimator2.SetTrigger("draw");
        scrollAnimator3.SetTrigger("draw");
    }

    public void notTurn()
    {
        scrollAnimator1.SetBool("notTurn", true);
        scrollAnimator2.SetBool("notTurn", true);
        scrollAnimator3.SetBool("notTurn", true);
    }

    public void assignScroll(Scroll deckScroll1, Scroll deckScroll2, Scroll deckScroll3)
    {
        scrollDisplay1.scroll = deckScroll1;
        scrollDisplay2.scroll = deckScroll2;
        scrollDisplay3.scroll = deckScroll3;

        scrollDisplay1.setInfo();
        scrollDisplay2.setInfo();
        scrollDisplay3.setInfo();
    }
}
