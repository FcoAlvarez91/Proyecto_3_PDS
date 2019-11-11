using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHolder : MonoBehaviour
{

    public Scroll scroll;
    public GameObject scrollGO;
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public bool played;

    public void setScroll(Scroll scrollInput)
    {
        scroll = scrollInput;
    }

    public void stopScrolls()
    {
        animator1.SetBool("notTurn", true);
        animator2.SetBool("notTurn", true);
        animator3.SetBool("notTurn", true);
        played = true;
    }
}
