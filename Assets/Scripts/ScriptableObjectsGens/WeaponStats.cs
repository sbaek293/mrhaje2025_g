using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
    public class WeaponStats : ScriptableObject
    {
        public string name;
        public int damage;
        public float firerate;
        public float bloom;
        public float recoil;
        public float kickback;
        public float aimSpeed;
        public GameObject prefab;
        public AudioClip gunshotSound;
        public float pitchRandomization;
        public float range;
        public GameObject bulletHole;
        public int burst;


    }

