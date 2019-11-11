using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public new string name;
    public int damage;
    public int maxHP;
    public int difficulty;

    public Sprite body;
    public Sprite weapon1;
    public Sprite weapon2;
}
