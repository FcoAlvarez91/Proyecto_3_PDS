using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollDisplay : MonoBehaviour
{

    public Scroll scroll;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;

    public Image artworkImage;
    public Image energyImage;
    public Image energySmallImage;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = scroll.name;
        effectText.text = scroll.effect;

        artworkImage.sprite = scroll.artwork;
        energyImage.sprite = scroll.energy;
        energySmallImage.sprite = scroll.energy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
