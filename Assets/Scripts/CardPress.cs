using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPress : MonoBehaviour
{
    Animator animator;
    Animator animator2;
    Animator animator3;
    public GameObject otherScroll1;
    public GameObject otherScroll2;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        animator2 = otherScroll1.GetComponent<Animator>();
        animator3 = otherScroll2.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void cardPress()
    {
        animator.SetBool("isActive", true);
        animator2.SetBool("isActive", false);
        animator3.SetBool("isActive", false);
    }
}
