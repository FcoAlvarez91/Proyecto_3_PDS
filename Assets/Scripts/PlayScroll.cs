using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScroll : MonoBehaviour
{

    private Animator animator1;
    private Animator animator2;
    private Animator animator3;
    public GameObject scroll1;
    public GameObject scroll2;
    public GameObject scroll3;
    private ScrollDisplay scrollDisplay1;
    private ScrollDisplay scrollDisplay2;
    private ScrollDisplay scrollDisplay3;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    // Start is called before the first frame update
    void Start()
    {
        animator1 = scroll1.GetComponent<Animator>();
        animator2 = scroll2.GetComponent<Animator>();
        animator3 = scroll3.GetComponent<Animator>();
        scrollDisplay1 = scroll1.GetComponent<ScrollDisplay>();
        scrollDisplay2 = scroll2.GetComponent<ScrollDisplay>();
        scrollDisplay3 = scroll3.GetComponent<ScrollDisplay>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runScroll()
    {
        //scroll1.SetActive(false);
        //scroll2.SetActive(false);
        //scroll3.SetActive(false);
        if (animator1.GetBool("isActive"))
        {
            scrollRead(scrollDisplay1.scroll);
        }
        else if (animator2.GetBool("isActive"))
        {
            scrollRead(scrollDisplay2.scroll);
        }
        else if (animator3.GetBool("isActive"))
        {
            scrollRead(scrollDisplay3.scroll);
        }
        animator1.SetBool("isActive", false);
        animator2.SetBool("isActive", false);
        animator3.SetBool("isActive", false);
    }

    public void scrollRead(Scroll scroll)
    {
        if(scroll.id == 0)
        {

        }
        else if (scroll.id == 1)
        {

        }
        else if (scroll.id == 2)
        {

        }
    }
}
