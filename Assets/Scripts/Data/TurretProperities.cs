using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret Properity", menuName = "Scriptable Objects/Turret Properity")]
public class TurretProperities : ScriptableObject
{
    public float Damage;
    public float Accuracy;
    public float TurnSpeed;
    public float ReloadTime;
    public float AngleAccuracy;
}
