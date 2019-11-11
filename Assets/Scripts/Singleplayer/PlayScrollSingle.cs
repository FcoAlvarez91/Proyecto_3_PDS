using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScrollSingle : MonoBehaviour
{

    public GameObject scroll1;
    public GameObject scroll2;
    public GameObject scroll3;
    private ScrollDisplaySingle scrollDisplay1;
    private ScrollDisplaySingle scrollDisplay2;
    private ScrollDisplaySingle scrollDisplay3;

    public GameObject activeScroll;

    // Start is called before the first frame update
    void Start()
    {
        scrollDisplay1 = scroll1.GetComponent<ScrollDisplaySingle>();
        scrollDisplay2 = scroll2.GetComponent<ScrollDisplaySingle>();
        scrollDisplay3 = scroll3.GetComponent<ScrollDisplaySingle>(); 
    }

    public void setActiveScroll()
    {
        if (scrollDisplay1.isActive)
        {
            activeScroll.GetComponent<ScrollHolderSingle>().setScroll(scrollDisplay1.scroll);
            scroll2.GetComponent<Animator>().SetBool("notTurn", true);
            scroll3.GetComponent<Animator>().SetBool("notTurn", true);
        }
        else if (scrollDisplay2.isActive)
        {
            activeScroll.GetComponent<ScrollHolderSingle>().setScroll(scrollDisplay2.scroll);
            scroll1.GetComponent<Animator>().SetBool("notTurn", true);
            scroll3.GetComponent<Animator>().SetBool("notTurn", true);
        }
        else if (scrollDisplay3.isActive)
        {
            activeScroll.GetComponent<ScrollHolderSingle>().setScroll(scrollDisplay3.scroll);
            scroll1.GetComponent<Animator>().SetBool("notTurn", true);
            scroll2.GetComponent<Animator>().SetBool("notTurn", true);
        }
    }
}
