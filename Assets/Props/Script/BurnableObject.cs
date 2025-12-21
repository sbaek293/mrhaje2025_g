using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class BurnableObject : MonoBehaviour
{
    public MeshRenderer meshrenderer;
    [SerializeField]private Material BurnMat;


    private void Start()
    {
        gameObject.GetComponent<EnemyHealth>().OnFirstDamage += Burn;
    }
    public void Burn()
    {
        if (!meshrenderer)
        {
            meshrenderer = gameObject.GetComponent<MeshRenderer>();
        }
        if (meshrenderer)
        {

            GameObject effect = Instantiate(Resources.Load<GameObject>("PropertyEffect/Flame"));
            effect.transform.parent = transform;
            effect.transform.localPosition = new Vector3(0, 0, 0);
            effect.GetComponent<ParticleSystem>().Play();
            StartCoroutine(BurnRoutine());
        }
    }
    public IEnumerator BurnRoutine()
    {
        List <Material> temp = meshrenderer.sharedMaterials.ToList();
        Material mat = new Material(BurnMat);
        temp.Add(mat);
        meshrenderer.SetMaterials(temp);


        float initialTime = 3;
        float timer = 0;
        while (timer < initialTime)
        {
            print("Burn");
            mat.SetFloat("_Burn", Mathf.Lerp(2, 0, timer/ initialTime));
            timer += Time.deltaTime;
            yield return null;
        }
        temp.RemoveRange(0, temp.Count - 1);
        meshrenderer.SetMaterials(temp);

        initialTime = 3;
        timer = 0;
        while (timer < initialTime)
        {
            mat.SetFloat("_Burn", Mathf.Lerp(0, 2, timer / initialTime));
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
