using UnityEngine;
using System.Collections.Generic;

public class CheckArea : MonoBehaviour
{
    private List<Collider> insideObjects = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!insideObjects.Contains(other))
            insideObjects.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (insideObjects.Contains(other))
            insideObjects.Remove(other);
    }

    public List<Collider> GetInsideObjects()
    {
        return insideObjects;
    }
}
