using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollLoader : MonoBehaviour
{

    public List<Scroll> scrolls;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.allScrolls = scrolls;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
