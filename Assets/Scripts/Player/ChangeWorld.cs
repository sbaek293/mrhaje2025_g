using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using URPGlitch;

public class ChangeWorld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Transition")]
    public float lensStrength;
    public float jitterStrength;
    public float colorStrength;
    public float digitalGlitchStrength;

    public float transitionTime = 1f;
    public Volume v;
    public LensDistortion l;

    public ShaderTest shaderTest;

    private DigitalGlitchVolume digitalGlitchVolume;
    private AnalogGlitchVolume analogGlitchVolume;
    private bool isTransitioning = false;
    private int currentWorld = 0;
    public static bool isInMatrix;

    public bool inCodeWorld()
    {
        return currentWorld == 1;
    }


    public static event Action<int> OnChangeWorld;
    void Start()
    {
        v.profile.TryGet<AnalogGlitchVolume>(out analogGlitchVolume);
        v.profile.TryGet<DigitalGlitchVolume>(out digitalGlitchVolume);
        v.profile.TryGet(out l);
        l.intensity.overrideState = true;
        l.intensity.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        bool changeWorld = Input.GetKeyDown(KeyCode.Q);

        if (changeWorld)
        {
            changeWorldFunc();
        }
    }

    public void changeWorldFunc()
    {
        if (!isTransitioning)
        {
            if (!inCodeWorld() && GetComponent<Stamina>().TrySkill())
            {
                StartCoroutine(WarpTransition(1));
                shaderTest.Matrixmode = true;
                currentWorld = 1;
                Player player = GetComponent<Player>();
                player.speed = player.originalSpeed * 0.05f;
            }
            else if (currentWorld == 1)
            {

                StartCoroutine(WarpTransition(0));
                shaderTest.Matrixmode = false;
                currentWorld = 0;
                Player player = GetComponent<Player>();
                player.speed = player.originalSpeed;
            }

            OnChangeWorld?.Invoke(currentWorld); //Invoke event
        }
    }

    IEnumerator WarpTransition(int world)
    {
        isTransitioning = true;

        float elapsed = 0f;

        // PHASE 1: Distort in (camera warps)
        while (elapsed < transitionTime / 2f)
        {
            elapsed += Time.deltaTime;
            l.intensity.value = Mathf.Lerp(l.intensity.value, lensStrength, elapsed / (transitionTime / 2f));
            analogGlitchVolume.scanLineJitter.value = Mathf.Lerp(analogGlitchVolume.scanLineJitter.value, jitterStrength, elapsed / (transitionTime / 2f));
            analogGlitchVolume.colorDrift.value = Mathf.Lerp(analogGlitchVolume.colorDrift.value, colorStrength, elapsed / (transitionTime / 2f));
            digitalGlitchVolume.intensity.value = Mathf.Lerp(digitalGlitchVolume.intensity.value, digitalGlitchStrength, elapsed / (transitionTime / 2f));
            yield return null;
        }

        //for (int i = 0; i < environment.transform.childCount; i++)
        //{
        //    environment.transform.GetChild(i).GetComponent<ChangeMaterial>().changeMaterial(world);
        //}

        elapsed = 0f;

        // PHASE 2: Distort out (camera returns to normal)
        while (elapsed < transitionTime / 2f)
        {
            elapsed += Time.deltaTime;
            l.intensity.value = Mathf.Lerp(lensStrength, 0, elapsed / (transitionTime / 2f));
            analogGlitchVolume.scanLineJitter.value = Mathf.Lerp(jitterStrength, 0, elapsed / (transitionTime / 2f));
            analogGlitchVolume.colorDrift.value = Mathf.Lerp(colorStrength, 0, elapsed / (transitionTime / 2f));
            digitalGlitchVolume.intensity.value = Mathf.Lerp(digitalGlitchStrength, 0, elapsed / (transitionTime / 2f));
            yield return null;
        }


        l.intensity.value = 0f;
        isTransitioning = false;
    }
}
