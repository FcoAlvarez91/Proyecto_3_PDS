using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPressSingle : MonoBehaviour
{
    public int num;
    public DrawScrollsSingle drawScrollsSingle;
    private Animator animator;
    private Animator animator2;
    private Animator animator3;
    public GameObject otherScroll1;
    public GameObject otherScroll2;
    public GameObject activeScroll;
    public ToggleButtonsSingle toggleButtonsSingle;
    private ScrollDisplaySingle scrollDisplay;
    private ScrollDisplaySingle scrollDisplay2;
    private ScrollDisplaySingle scrollDisplay3;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animator2 = otherScroll1.GetComponent<Animator>();
        animator3 = otherScroll2.GetComponent<Animator>();
        scrollDisplay = GetComponent<ScrollDisplaySingle>();
        scrollDisplay2 = otherScroll1.GetComponent<ScrollDisplaySingle>();
        scrollDisplay3 = otherScroll2.GetComponent<ScrollDisplaySingle>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        if (scrollDisplay.isActive)
        {
            toggleButtonsSingle.activateButtons();
            activeScroll.GetComponent<ScrollHolderSingle>().played = true;

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

            drawScrollsSingle.lastPlayed = num;
        }

        else if (!activeScroll.GetComponent<ScrollHolderSingle>().played)
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

            activeScroll.GetComponent<ScrollHolderSingle>().setScroll(scrollDisplay.scroll);
        }
    }
}
