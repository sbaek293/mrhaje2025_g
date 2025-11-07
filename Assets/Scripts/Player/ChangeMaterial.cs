using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Material[] materials;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeMaterial(int i)
    {
        GetComponent<MeshRenderer>().material = materials[i];
    }
}
