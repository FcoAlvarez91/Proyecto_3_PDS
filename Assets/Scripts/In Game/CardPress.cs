using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPress : MonoBehaviour
{
    private Animator animator;
    private Animator animator2;
    private Animator animator3;
    public GameObject useScroll;
    public GameObject activeScroll;
    public GameObject otherScroll1;
    public GameObject otherScroll2;
    public ScrollHolder scrollHolder;
    public ToggleButtons toggleButtons;
    private ScrollDisplay scrollDisplay;
    private ScrollDisplay scrollDisplay2;
    private ScrollDisplay scrollDisplay3;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animator2 = otherScroll1.GetComponent<Animator>();
        animator3 = otherScroll2.GetComponent<Animator>();
        scrollDisplay = GetComponent<ScrollDisplay>();
        scrollDisplay2 = otherScroll1.GetComponent<ScrollDisplay>();
        scrollDisplay3 = otherScroll2.GetComponent<ScrollDisplay>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (scrollDisplay.isActive)
        {
            scrollHolder.played = true;
            toggleButtons.activateButtons();

            animator.ResetTrigger("deactivate");
            animator2.ResetTrigger("deactivate");
            animator3.ResetTrigger("deactivate");

            animator.ResetTrigger("active");
            animator2.ResetTrigger("active");
            animator3.ResetTrigger("active");

            animator.SetTrigger("deactivate");
            animator2.SetBool("notTurn", true);
            animator3.SetBool("notTurn", true);

            scrollDisplay.isActive = false;
            scrollDisplay2.isActive = false;
            scrollDisplay3.isActive = false;
        }

        else if (!activeScroll.GetComponent<ScrollHolder>().played)
        {
            scrollDisplay.isActive = true;
            scrollDisplay2.isActive = false;
            scrollDisplay3.isActive = false;

            animator.ResetTrigger("deactivate");
            animator2.ResetTrigger("deactivate");
            animator3.ResetTrigger("deactivate");

            animator.ResetTrigger("active");
            animator2.ResetTrigger("active");
            animator3.ResetTrigger("active");

            animator.SetTrigger("active");
            animator2.SetTrigger("deactivate");
            animator3.SetTrigger("deactivate");

            activeScroll.GetComponent<ScrollHolder>().setScroll(scrollDisplay.scroll);
        }
    }
}
