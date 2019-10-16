using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scroll", menuName = "Scroll")]
public class Scroll : ScriptableObject
{
    public int id;
    public new string name;
    public string effect;
    public string energyType;

    public Sprite artwork;
    public Sprite energy;
    public string type;

    public int damage;
    public int heal;
    public List<string> conditions;

}
