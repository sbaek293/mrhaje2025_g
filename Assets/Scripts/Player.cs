using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using URPGlitch;



public class Player : MonoBehaviour
{

  

    public Weapon weapon;
    [Header("Player Stats")]
    Vector3 curPos;
    private float t_adjustedSpeed;
    public float originalSpeed;
    private float speed;
    public float sprintModifier;
    public float jumpForce;
    public int max_health;
    public float current_health;

    [Header("Misc")]
    public Camera normalCam;
    public Transform groundDetector;
    public LayerMask ground;
    public float zoom;
    public Rigidbody rig;
    public GameObject environment;
    private float movementCounter;
    private float idleCounter;
    private float baseFOV;
    public float sprintFOVModifier;
    public GameObject hackUI;
    public Look lookScript;
    public bool paused = false;
    public GameObject pauseMenu;
    [Header("SFX")] 
    public AudioClip jump;



    [Header("Transition")]
    public float lensStrength;
    public float jitterStrength;
    public float colorStrength;
    public float digitalGlitchStrength;

    public float transitionTime = 1f;
    public Volume v;
    public LensDistortion l;

    
    private DigitalGlitchVolume digitalGlitchVolume;
    private AnalogGlitchVolume analogGlitchVolume;
    private bool isTransitioning = false;
    private int currentWorld = 0;
    
    public bool inCodeWorld()
    {
        return currentWorld == 1;
    }


    public static event Action<int> OnChangeWorld;

    private void Start()
    {


        current_health = max_health;

        v.profile.TryGet<AnalogGlitchVolume>(out analogGlitchVolume);
        v.profile.TryGet<DigitalGlitchVolume>(out digitalGlitchVolume);
        v.profile.TryGet(out l);
        l.intensity.overrideState = true;
        l.intensity.value = 0f;

        baseFOV = normalCam.fieldOfView;

        //if (Camera.main) Camera.main.enabled = false;
    }

    public void Awake()
    {




    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            togglePause();
        }
        if(paused) return;
        //Axis
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");


        //Controls
        bool sprint = Input.GetKey(KeyCode.LeftControl);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool pause = Input.GetKeyDown("j");
        bool attack = Input.GetKeyDown(KeyCode.Mouse0);



        //States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;






        //Jumping
        if (isJumping)
        {
            rig.AddForce(Vector3.up * jumpForce);



        }

        if (attack)
        {
            weapon.Attack();
        }

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
                currentWorld = 1;
            }
            else if (currentWorld == 1)
            {

                StartCoroutine(WarpTransition(0));
                currentWorld = 0;
            }

            OnChangeWorld?.Invoke(currentWorld); //Invoke event
        }
    }

    public void togglePause()
    {
        paused = !paused;
        pauseMenu.SetActive(paused);
        lookScript.UpdateCursorLock();
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

        for (int i = 0; i < environment.transform.childCount; i++)
        {
            environment.transform.GetChild(i).GetComponent<ChangeMaterial>().changeMaterial(world);
        }

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





    void FixedUpdate()
    {
        if (paused) return;

        //Axis

        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");




        //Controls
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool aim = Input.GetMouseButton(1);


        //States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0;
        bool isAiming = aim;
        bool hacking = Input.GetKey(KeyCode.Tab);




        //Movement


        Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
        t_direction.Normalize();

        t_adjustedSpeed = originalSpeed;
        if (isSprinting) t_adjustedSpeed *= sprintModifier;



        Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustedSpeed * Time.fixedDeltaTime;
        t_targetVelocity.y = rig.linearVelocity.y;
        rig.linearVelocity = t_targetVelocity;


        //FOV
        if (isSprinting) { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.fixedDeltaTime * 8f); }
        else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.fixedDeltaTime * 8f); }

        if (hacking) { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * zoom, Time.fixedDeltaTime * 8f); }

        else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.fixedDeltaTime * 8f); }


        //UI
        hackUI.SetActive(hacking);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(paused) return;  
        if (collision == null) return;
        if (collision.transform.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);

            current_health -= 1;
            if (current_health <= 0)
            {
                Debug.Log("DEAD");
            }
            Stamina.Instance.Recover(Stamina.StaminaEventType.TakeDamage);
        }
    }

}
