using UnityEngine;
using System.Collections;

public class ShaderTest : MonoBehaviour
{
    public bool Matrixmode = false;
    public float TransitionTime = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        y = Matrixmode;
    }

    IEnumerator lol(bool enter)
    {
        float timer = 0;
        while (true)
        {
            //print(timer / TransitionTime);
            timer += Time.deltaTime;
            float a = 0;
            if (timer < TransitionTime)
            {
                a = Mathf.Lerp(10, -10, timer / TransitionTime);
                Shader.SetGlobalFloat("_MatrixMode", a);
            }
            else
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        
        Shader.SetGlobalFloat("_MatrixActive", enter?1:0);
        timer = 0;
        while (true)
        {
         //   print(timer / TransitionTime);
            timer += Time.deltaTime;
            float a = 0;
            if (timer < TransitionTime)
            {
                a = Mathf.Lerp(-10, 10, timer / TransitionTime);
                Shader.SetGlobalFloat("_MatrixMode", a);
            }
            else
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

    }
    private bool y = true;
    // Update is called once per frame

    void Update()
    {
        if(y!= Matrixmode)
        {
            StartCoroutine(lol(Matrixmode));
        }
        y = Matrixmode;
    }

}
