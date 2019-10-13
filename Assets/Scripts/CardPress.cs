using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPress : MonoBehaviour
{
    private Animator animator;
    private Animator animator2;
    private Animator animator3;
    public GameObject otherScroll1;
    public GameObject otherScroll2;
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

    public void cardPress()
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

    }
}
