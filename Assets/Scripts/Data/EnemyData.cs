using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string CreeperName;
    public int Speed;
    public int MaxHealth;
}
