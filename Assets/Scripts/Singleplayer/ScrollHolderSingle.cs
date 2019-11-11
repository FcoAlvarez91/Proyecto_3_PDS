using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHolderSingle : MonoBehaviour
{

    public Scroll scroll;
    public bool played;

    public void setScroll(Scroll scrollInput)
    {
        scroll = scrollInput;
    }
}
