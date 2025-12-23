using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OutsideTrigger : MonoBehaviour
{
    [Header("Target")]
    public GameObject targetObject;
    public GameObject camera;

    private bool used = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!used && other.GetComponent<Player>() != null)
        {
            Debug.LogWarning("OutsideTrigger : Player enter on trigger area");
            used = true;

            StartTruck();
            camera.GetComponent<AutoTargetHaking>().maxTargetingDistence = 30f;
        }
    }


    private void StartTruck()
    {
        Truck truck = targetObject.GetComponent<Truck>();

        if (truck != null) {
            truck.enabled = true;

            Debug.LogWarning("OutsideTrigger : Truck Start!");
        }
    }
}
