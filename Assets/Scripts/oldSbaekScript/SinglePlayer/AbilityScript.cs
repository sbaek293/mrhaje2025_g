//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//using System.Net;


//using UnityEngine.UI;
//public class AbilityScript : MonoBehaviour
//{
//    public GameObject moritz;
//    public GameObject pyro;
//    public Ability[] loadout;
//    public float currentCooldown;
//    public AudioSource sfx;
//    public int currentIndex = 0;
//    private float sprint;
//    public Player playerScript;
//    public Weapon weaponScript;
//    private float slow;
//    private float zoom;
//    private float range;
//    public bool abiIsActiveBool;
//    private bool audiobool = true;
//    private float fireRate;
//    private int damage;
//    private float healing;
//    private ParticleSystem sprintParticle;
//    private Vector3 orgSize;
//    private Vector3 cameraPos;
//    private GameObject error;
//    public GameObject errorGO;
//    public void Start()
//    {

//        /*if (Select.whichRole == 3 || Select.whichRole == 4)
//        {
//            if (photonView.IsMine)
//            {
//                sprintParticle = GameObject.FindGameObjectWithTag("Sprint").GetComponent<ParticleSystem>();

//            }

//        }*/
//    }
//    public void Update()
//    {

//        if (Input.GetKeyDown("q") && currentCooldown <= 0)
//        {
//            Ability();
//        }


//    }
//    public void FixedUpdate()
//    {
//        //cooldown
//        if (currentCooldown > 0 && Select.whichRole != 12)
//        {
//            currentCooldown -= Time.fixedDeltaTime;
//        }
//        else if (currentCooldown > 0 && !abiIsActiveBool)
//        {
//            currentCooldown -= Time.fixedDeltaTime;
//        }
//    }

//    public void Ability()
//    {

//        //cooldown
//        currentCooldown = loadout[currentIndex].coolDown;

//        if (Select.whichRole == 3)
//        {
//            //Axis
//            if (!Ninja())
//            {
//                currentCooldown = 0f;
//                audiobool = false;
//                if (error == null)
//                {
//                    error = Instantiate(errorGO, Player.HUD.transform);
//                    error.GetComponent<Text>().text = "You can't teleport through walls!";


//                    Invoke("disableError", 2f);
//                }
//            }
//            else
//            {

//                float t_hmove = Input.GetAxisRaw("Horizontal");
//                float t_vmove = Input.GetAxisRaw("Vertical");
//                if (t_vmove != 0)
//                {
//                    Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
//                    t_direction.Normalize();
//                    transform.position = transform.position + transform.TransformDirection(t_direction) * 5;
//                    //sprintParticle.Play();

//                    //Invoke("stopParticle", 1f);
//                }
//                else
//                {
//                    currentCooldown = 0f;
//                    audiobool = false;

//                    if (error == null)
//                    {
//                        error = Instantiate(errorGO, Player.HUD.transform);
//                        error.GetComponent<Text>().text = "You have to walk in a direction to telport!";

//                        Invoke("disableError", 2f);
//                    }
//                }

//            }

//            //use for ant ability;
//            //orgSize = gameObject.transform.localScale;
//            //gameObject.transform.localScale = Vector3.one * Time.deltaTime;
//            //Invoke("Ant", 3f);
//        }
//        else if (Select.whichRole == 2)
//        {
//            if (playerScript.current_health < playerScript.max_health)
//            {
//                playerScript.current_health += 300f;
//                if (playerScript.current_health >= playerScript.max_health)
//                {
//                    playerScript.current_health = playerScript.max_health;
//                }
//                audiobool = true;
//            }
//            else
//            {
//                currentCooldown = 0f;
//                audiobool = false;

//                if (error == null)
//                {
//                    error = Instantiate(errorGO, Player.HUD.transform);
//                    error.GetComponent<Text>().text = "You have already max health!";

//                    Invoke("disableError", 2f);
//                }
//            }

//        }
//        else if (Select.whichRole == 4)
//        {


//            playerScript.sprintModifier = 3f;
//            abiIsActiveBool = true;
//            //sprintParticle.Play();
//            Invoke("Assasine", 1f);

//        }
//        else if (Select.whichRole == 6)
//        {
//            slow = weaponScript.slowValue;
//            weaponScript.slowValue = 0f;
//            abiIsActiveBool = true;
//            Invoke("Mage", 3f);
//        }
//        else if (Select.whichRole == 1)
//        {
//            if (weaponScript.currentCooldown > 0)
//            {

//                weaponScript.currentCooldown -= 8f;
//                if (weaponScript.currentCooldown < 0)
//                {
//                    weaponScript.currentCooldown = 0f;
//                }
//            }
//        }
//        else if (Select.whichRole == 0)
//        {
//            fireRate = weaponScript.fireRate;
//            damage = weaponScript.damage;
//            weaponScript.fireRate = 0.3f;
//            weaponScript.damage = 130;
//            abiIsActiveBool = true;
//            Invoke("Basic", 3f);
//        }
//        else if (Select.whichRole == 5)
//        {
//            Hide();
//            Invoke("Show", 6f);

//        }
//        else if (Select.whichRole == 7)
//        {
//            if (Abi() == false)
//            {
//                currentCooldown = 0f;
//                audiobool = false;

//                if (error == null)
//                {
//                    error = Instantiate(errorGO, Player.HUD.transform);
//                    error.GetComponent<Text>().text = "Enemy not in range!";

//                    Invoke("disableError", 2f);
//                }
//            }


//        }
//        else if (Select.whichRole == 8)
//        {
//            healing = weaponScript.healing;
//            weaponScript.healing = 75f;
//            abiIsActiveBool = true;
//            Invoke("Bene", 4f);

//        }
//        else if (Select.whichRole == 9)
//        {
//            playerScript.dmgModi = 0;
//            abiIsActiveBool = true;
//            Invoke("Daniel", 2f);

//        }
//        else if (Select.whichRole == 12)
//        {
//            if (photonView.IsMine)
//            {

//                if (!abiIsActiveBool)
//                {

//                    Vector3 playerPos = transform.position;
//                    Vector3 playerDirection = transform.forward;
//                    Quaternion playerRotation = new Quaternion(0, 90f, 0, 0);
//                    float spawnDistance = 1f;

//                    Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

//                    playerScript.sprintModifier = 2.5f;
//                    fireRate = weaponScript.fireRate;

//                    weaponScript.fireRate = 0.3f;
//                    moritz = PhotonNetwork.Instantiate("MoritzAbi", spawnPos, playerRotation);
//                    currentCooldown = 0;
//                    abiIsActiveBool = true;
//                    playerScript.moritz();
//                    Invoke("Moritz", 2f);
//                }
//                else
//                {
//                    playerScript.current_health += 200f;
//                    if (playerScript.current_health >= playerScript.max_health)
//                    {
//                        playerScript.current_health = playerScript.max_health;
//                    }
//                    playerScript.moritz();
//                    transform.position = moritz.transform.position;
//                    PhotonNetwork.Destroy(moritz);
//                    abiIsActiveBool = false;
//                }
//            }

//        }
//        else if (Select.whichRole == 11)
//        {
//            if (photonView.IsMine)
//            {


//                Vector3 playerPos = transform.position;
//                Vector3 playerDirection = transform.forward;
//                Quaternion playerRotation = new Quaternion(0, 90f, 0, 0);
//                float spawnDistance = 1.2f;

//                Vector3 spawnPos = playerPos + playerDirection * spawnDistance - new Vector3(0, 0.45f, 0);


//                pyro = PhotonNetwork.Instantiate("PyroAbi", spawnPos, playerRotation);
//                pyro.GetComponent<PyroAbi>().ownerID = PhotonNetwork.LocalPlayer.ActorNumber;
//                pyro.GetComponent<PyroAbi>().profileID = ProfileChange.whichProfile;
//                pyro.GetComponent<PyroAbi>().borderID = ProfileChange.whichBorder;
//                pyro.GetComponent<PyroAbi>().nickname = playerScript.playerData.UserName;

//            }

//        }
//        else if (Select.whichRole == 10)
//        {
//            if (photonView.IsMine)
//            {


//                Vector3 playerPos = transform.position;
//                Vector3 playerDirection = transform.forward;
//                Quaternion playerRotation = new Quaternion(0, 90f, 0, 0);
//                float spawnDistance = 1.2f;

//                Vector3 spawnPos = playerPos + playerDirection * spawnDistance - new Vector3(0, 0.45f, 0);


//                GameObject oli = PhotonNetwork.Instantiate("OliAbi", spawnPos, playerRotation);
//                HideOli();
//                Invoke("ShowOli", 5f);
//            }

//        }



//        if (audiobool)
//        {
//            //sound
//            sfx.Stop();
//            sfx.clip = loadout[currentIndex].sfx;

//            sfx.Play();
//            Invoke("Audio", 2f);
//        }



//    }
//    private void Ant()
//    {
//        gameObject.transform.localScale = orgSize;
//    }
//    private void Assasine()
//    {
//        playerScript.sprintModifier = 1.5f;
//        abiIsActiveBool = false;
//        //stopParticle();

//    }
//    private void Moritz()
//    {
//        playerScript.sprintModifier = 1.6f;
//        weaponScript.fireRate = fireRate;
//        //stopParticle();

//    }
//    private void Mage()
//    {
//        weaponScript.slowValue = slow;
//        abiIsActiveBool = false;
//    }
//    private void Daniel()
//    {
//        playerScript.dmgModi = 1;
//        abiIsActiveBool = false;
//    }
//    private void Audio()
//    {
//        sfx.Stop();
//    }
//    private void Basic()
//    {
//        weaponScript.fireRate = fireRate;
//        weaponScript.damage = damage;
//        abiIsActiveBool = false;
//    }
//    private void Bene()
//    {
//        weaponScript.healing = healing;
//        abiIsActiveBool = false;
//    }
//    private void Pyro()
//    {
//        PhotonNetwork.Destroy(pyro);
//    }
//    // Turn on the bit using an OR operation:
//    private void Show()
//    {
//        playerScript.normalCam.cullingMask |= 1 << LayerMask.NameToLayer("Wall");

//        playerScript.normalCam.cullingMask |= 1 << LayerMask.NameToLayer("Ground");
//        weaponScript.canBeShot |= 1 << LayerMask.NameToLayer("Ground");
//        weaponScript.canBeShot |= 1 << LayerMask.NameToLayer("Wall");
//        weaponScript.canBeShot |= 1 << LayerMask.NameToLayer("Stairs");
//        playerScript.zoom = zoom;
//        weaponScript.range = range;
//        abiIsActiveBool = false;
//    }
//    private void ShowOli()
//    {
//        weaponScript.canBeShot |= 1 << LayerMask.NameToLayer("OliAbi");
//        abiIsActiveBool = false;
//    }
//    // Turn off the bit using an AND operation with the complement of the shifted int:
//    private void Hide()
//    {
//        playerScript.normalCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Wall"));
//        playerScript.normalCam.cullingMask &= ~(1 << LayerMask.NameToLayer("Ground"));
//        weaponScript.canBeShot &= ~(1 << LayerMask.NameToLayer("Ground"));
//        weaponScript.canBeShot &= ~(1 << LayerMask.NameToLayer("Wall"));
//        weaponScript.canBeShot &= ~(1 << LayerMask.NameToLayer("Stairs"));
//        zoom = playerScript.zoom;
//        playerScript.zoom = -0.5f;
//        range = weaponScript.range;
//        weaponScript.range = 10000;
//        abiIsActiveBool = true;
//    }
//    private void HideOli()
//    {
//        weaponScript.canBeShot &= ~(1 << LayerMask.NameToLayer("OliAbi"));
//        abiIsActiveBool = true;
//    }
//    public bool Abi()
//    {
//        Transform t_spawn = transform.Find("Cameras/Normal Camera");

//        // bloom
//        Vector3 t_bloom = t_spawn.position + t_spawn.forward * 10000f;


//        //raycast
//        RaycastHit t_hit = new RaycastHit();

//        if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 15f, LayerMask.GetMask("Player")))
//        {

//            if (photonView.IsMine)
//            {
//                //shooting other player
//                if (t_hit.collider.gameObject.layer == 11)
//                {
//                    transform.position = t_hit.transform.position;
//                    return true;
//                }

//            }

//        }
//        return false;
//    }

//    public bool Ninja()
//    {
//        Transform t_spawn = transform.Find("Cameras/Normal Camera");

//        // bloom
//        Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;

//        //raycast
//        RaycastHit t_hit = new RaycastHit();

//        if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 5f, LayerMask.GetMask("Wall")))
//        {
//            if (photonView.IsMine)
//            {

//                if (t_hit.collider.gameObject.layer == 9)
//                {
//                    return false;
//                }
//            }
//        }
//        return true;
//    }
//    public void stopParticle()
//    {
//        sprintParticle.Stop();
//    }
//    private void disableError()
//    {

//        error.SetActive(false);
//        Destroy(error);
//    }
//}
