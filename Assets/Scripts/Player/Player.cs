using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Player : MonoBehaviour
{

    //Possible use in future
    //public Ability[] loadout2;
    //private float slowValue;
    //private GameObject abiIsActive;
    //private bool mage;
    //private Text AbiCDText;
    //private Image AbiCDImage;
    //public static int deaths;
    //public PlayerData playerData = new PlayerData();
    //public GameObject icePanel;
    //public GameObject ice;
    //public AbilityScript abi;
    //private bool hitByMage;
    //public bool respawning;
    //public GameObject respawnMsg;


    [Header("Player Stats")]
    Vector3 curPos;
    private float t_adjustedSpeed;
    public float originalSpeed;
    public float speed;
    public float sprintModifier;
    public float jumpForce;
    public int max_health;
    public float current_health;
    public float regen;
    public float originalWeight = 3;
    public String movementType = "normal";
    public bool stunned = false;
    public bool hardening = false;
    public bool lavitating = false;
    public float maxLavitateSpeed = 10f;
    public float lavitatingForce = 18f;


    [Header("UI")]
    public static GameObject HUD;
    public GameObject dmgTakenPanel;
    private GameObject dmgT;
    public Text CDText;
    public Image CDImage;
    public Image ui_healthbar;
    public Text ui_health;

    [Header("Combat")]
    public Weapon weapon;
    private bool damaged;
    private Vector3 targetWeaponBobPosition;
    private Vector3 weaponParentOrigin;
    public Transform weaponParent;

    [Header("Camera")]
    public Camera normalCam;
    public float zoom;
    public float baseFOV;
    public float sprintFOVModifier;
    //public GameObject cameraParent;

    [Header("Misc")]
    public Transform groundDetector;
    public LayerMask ground;
    public Rigidbody rig;
    public CapsuleCollider col;
    public GameObject environment;
    private float movementCounter;
    private float idleCounter;
    public PauseScript pauseScript;
    public bool touchingWall = false;
    public Transform spawnPoint;

    [Header("SFX")]
    public AudioClip jump;
    public AudioClip land;
    public AudioClip dmgTaken;
    private float landTimer = 0;
    public List<AudioClip> defaultFootSteps;
    public float footStepMultiplier = 2f;
    private float footStepTimer = 0;




    private void Start()
    {
        current_health = max_health;
        baseFOV = normalCam.fieldOfView;
        speed = originalSpeed;

        //if (Camera.main) Camera.main.enabled = false;

        weaponParentOrigin = weaponParent.localPosition;

        ui_health = GameObject.FindWithTag("Text").GetComponent<Text>();
        ui_healthbar = GameObject.FindWithTag("Healthbar").GetComponent<Image>();
        CDImage = GameObject.FindWithTag("CD").GetComponent<Image>();
        CDText = GameObject.FindWithTag("CDText").GetComponent<Text>();

        RefreshHealthBar();
        RefreshCD();

        HUD = GameObject.Find("HUD").gameObject;
        dmgT = Instantiate(dmgTakenPanel, HUD.transform);
        dmgT.SetActive(false);
    }


    private void Update()
    {

        if (PauseScript.paused) return;
        //Axis
        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");


        //Controls
        bool sprint = Input.GetKey(KeyCode.LeftControl);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool pause = Input.GetKeyDown("j");
        



        //States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;


        //footStepFX
        if (isGrounded && defaultFootSteps.Count > 0 && rig.linearVelocity.magnitude > 0.5f)
        {

            if (footStepTimer > footStepMultiplier / rig.linearVelocity.magnitude)
            {
                footStepTimer = 0;
                AudioManager.PlaySound(gameObject, defaultFootSteps[UnityEngine.Random.Range(0, defaultFootSteps.Count)], false, 10, 0.1f);
            }
            else
            {
                footStepTimer += Time.deltaTime;
            }
        }
        //land FX

        if (land)
        {
            if (landTimer > 0.5f && isGrounded)
            {
                landTimer = 0;
                AudioManager.PlaySound(gameObject, land, false, 10, 0.2f);
            }
            else if (!isGrounded)
            {
                landTimer += Time.deltaTime;
            }
            else
            {
                landTimer = 0;
            }

        }

        //Jumping
        if (isJumping)
        {
            rig.AddForce(Vector3.up * jumpForce);



        }

        //Headbob
        if (t_hmove == 0 && t_vmove == 0)
        {
            HeadBob(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.fixedDeltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.fixedDeltaTime * 2f);
        }
        else if (!isSprinting)
        {
            HeadBob(movementCounter, 0.035f, 0.035f);
            movementCounter += Time.fixedDeltaTime * 3f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.fixedDeltaTime * 6f);
        }
        else
        {
            HeadBob(movementCounter, 0.15f, 0.075f);
            movementCounter += Time.fixedDeltaTime * 7f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.fixedDeltaTime * 10f);
        }

        if (Input.GetKeyDown("h"))
        {
            rig.AddForce(Vector3.up * jumpForce);
        }

    }



    void FixedUpdate()
    {
        if (PauseScript.paused) return;

        //Axis

        float t_hmove = Input.GetAxisRaw("Horizontal");
        float t_vmove = Input.GetAxisRaw("Vertical");

        //Controls
        bool sprint = Input.GetKey(KeyCode.LeftControl);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        bool aim = Input.GetMouseButton(1);

        //States
        bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        bool isJumping = jump && isGrounded;
        bool isSprinting = sprint && t_vmove > 0;
        bool isAiming = aim;


        //Movement
        if ((isGrounded || lavitating) && !touchingWall && !stunned)
        {
            if (movementType == "normal")
            {
                Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
                t_direction.Normalize();

                t_adjustedSpeed = speed;
                if (isSprinting) t_adjustedSpeed *= sprintModifier;

                Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustedSpeed * Time.fixedDeltaTime;
                t_targetVelocity.y = rig.linearVelocity.y;
                rig.linearVelocity = t_targetVelocity;
            }
            else if (movementType == "truck")
            {
                t_adjustedSpeed += speed*1.5f * Time.fixedDeltaTime;
                t_adjustedSpeed = Mathf.Clamp(t_adjustedSpeed, 0, speed*6);

                Vector3 forwardVelocity = transform.forward * t_adjustedSpeed * Time.fixedDeltaTime;
                forwardVelocity.y = rig.linearVelocity.y;

                rig.linearVelocity = forwardVelocity;
            }
            
        }

        //Lavitating
        if (lavitating)
        {
            if (rig.linearVelocity.y < maxLavitateSpeed)
            {
                rig.AddForce(Vector3.up * lavitatingForce, ForceMode.Acceleration);
            }
        }

        //FOV
        if (isSprinting) { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.fixedDeltaTime * 8f); }
        else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.fixedDeltaTime * 8f); }

        if (isAiming) { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * zoom, Time.fixedDeltaTime * 8f); }
        else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.fixedDeltaTime * 8f); }

        //Regeneration
        if (current_health + regen <= max_health && damaged == false)
        {
            current_health += regen * Time.fixedDeltaTime;
        }

        RefreshHealthBar();
        RefreshCD();
    }

    void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
    {
        targetWeaponBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(p_z) * p_x_intensity, Mathf.Sin(p_z * 2) * p_y_intensity, 0);
    }

    void RefreshHealthBar()
    {
        float t_health_ratio = current_health / (float)max_health;

        ui_healthbar.fillAmount = Mathf.Lerp(ui_healthbar.fillAmount, t_health_ratio, Time.fixedDeltaTime * 8f);
        ui_health.text = "" + Mathf.Round(current_health / 10);
    }

    void RefreshCD()
    {
        float cd_ratio = weapon.currentCooldown / weapon.loadout[weapon.currentIndex].firerate;

        CDImage.fillAmount = Mathf.Lerp(CDImage.fillAmount, cd_ratio, Time.fixedDeltaTime * 8f);

        CDText.text = "" + System.Math.Round(weapon.currentCooldown, 1);
        if (System.Math.Round(weapon.currentCooldown, 1) == 0)
        {
            CDText.text = "";
        }
    }
    public void TakeDamage(int damage, bool slow, float slowValue)
    {
        //Stamina.Instance.Recover(Stamina.StaminaEventType.TakeDamage);

        if (hardening) current_health -= (int)(damage * 0.2f);
        else current_health -= damage;

        RefreshHealthBar();
        //PlaySound("DMG");
        damaged = true;
        Invoke("notDamaged", 4f);


        if (current_health <= 0)
        {
            transform.position = spawnPoint.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            touchingWall = true;
        } else if (collision.gameObject.layer == 6)
        {
            if (collision.gameObject.GetComponent<Truck>() != null)
            {
                if (collision.gameObject.GetComponent<EnemyPropertyManager>().HasPropertyName("Heavy") && collision.gameObject.GetComponent<EnemyPropertyManager>().HasPropertyName("Dash"))
                {
                    Vector3 forcedir = (transform.position - collision.gameObject.transform.position).normalized;
                    forcedir += Vector3.up;
                    stunned = true;
                    rig.AddForce(forcedir * 10000, ForceMode.Force);
                }
            }
        }
    }



    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {

            touchingWall = false;
        }
    }
    public void notDamaged()
    {
        damaged = false;
    }
    public void DmgTakenPanelOn()
    {

        dmgT.SetActive(true);
        Invoke("DmgTakenPanelOff", 0.25f);

    }
    public void DmgTakenPanelOff()
    {
        dmgT.SetActive(false);



    }
}
