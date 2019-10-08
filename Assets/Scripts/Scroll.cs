using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scroll", menuName = "Scroll")]
public class Scroll : ScriptableObject
{
    public int id;
    public new string name;
    public string effect;

    public Sprite artwork;
    public Sprite energy;
    public int type;



}
