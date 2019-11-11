using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollDisplaySingle : MonoBehaviour
{

    public Scroll scroll;

    public bool isActive;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;

    public Image artworkImage;
    public Image energyImage;
    public Image energySmallImage;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    public void setInfo()
    {

        nameText.text = scroll.name;
        effectText.text = scroll.effect;

        artworkImage.sprite = scroll.artwork;
        energyImage.sprite = scroll.energy;
        energySmallImage.sprite = scroll.energy;
    }
}
