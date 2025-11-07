using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    [CreateAssetMenu(fileName = "New Character", menuName = "Character")]
    public class Character : ScriptableObject
    {
        public string name;
        public float damage;
        public float reload;
        public float health;
        public float mobility;
        public float range;
        public Color color;
        public Text info;
        public int cost;
        public string rarity;



}

