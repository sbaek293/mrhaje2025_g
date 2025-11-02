#if UNITY_EDITOR
using UnityEngine;

[ExecuteInEditMode]
public class MatrixEditorScript : MonoBehaviour
{
    public bool Matrix;
    bool t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        t = !Matrix;
    }
    void Update()
    {
        if (t != Matrix)
        {
            Shader.SetGlobalFloat("_MatrixMode", 10);
            Shader.SetGlobalFloat("_MatrixActive", Matrix ? 10 : -10);
        }
        t = Matrix;
    }

}
#endif