using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void go11()
    {
        animator.Play("1-1");
    }

    public void go13()
    {
        animator.Play("1-3");
    }

    public void go15()
    {
        animator.Play("1-5");
    }

    public void go17()
    {
        animator.Play("1-7");
    }
}
