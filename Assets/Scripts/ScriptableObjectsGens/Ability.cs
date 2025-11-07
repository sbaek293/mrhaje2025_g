using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    public string name;
    public float coolDown;
    public AudioClip sfx;


}