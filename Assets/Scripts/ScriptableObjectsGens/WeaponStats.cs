using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType
{
    RayCast,
    Projectile,
    Area
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponStats : ScriptableObject
{
    [Header("Common")]
    public string name;
    public float firerate;
    public float bloom;
    public float recoil;
    public float kickback;
    public float aimSpeed;
    public GameObject prefab;
    public AudioClip gunshotSound;
    public float pitchRandomization;
    public int burst;

    public WeaponType weaponType;


    [Header("RayCast")]
    public int damage;
    public float range;
    public GameObject bulletHole;
    


    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float attackForceForward;
    public float attackForceUp;


    [Header("Area")]
    public int areaDamage;
}

