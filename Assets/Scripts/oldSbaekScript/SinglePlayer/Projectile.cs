using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


    public class Projectile : MonoBehaviour
    {
        
        public float speed;
        public int damage;
        

        // Start is called before the first frame update
        void Start()
        {

        }
        //
        // Update is called once per frame
        void Update()
        {
            if (PauseScript.paused || ChangeWorld.isInMatrix) { return; }
            
            transform.Translate(Vector3.forward*Time.deltaTime*speed);
        }
            
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>().TakeDamage(damage, false, 0);
                Destroy(gameObject);
            }
            else if (other.gameObject.layer == 3 || other.gameObject.layer == 8)
            {
                Destroy(gameObject);
            }
        }

    }
