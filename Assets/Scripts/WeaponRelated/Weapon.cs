using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Weapon : MonoBehaviour
{

    public Animator animator;
 
    public Camera cam;

    public WeaponStats[] loadout;
    public Transform weaponParent;
   
    public LayerMask canBeShot;
    
    public AudioSource sfx;

    public float currentCooldown;

    public int currentIndex;
    private GameObject currentWeapon;
    private float orgBloom;

    private float slowValue;
   
    public Player playerScript;

    private int damage;
    private float fireRate;
    private float range;

    [SerializeField]
    private ParticleSystem muzzleFlash;
    
    private GameObject HUD;
    //private bool sfxPlaying = false;

    public void Start()
    {
        HUD = GameObject.Find("HUD").gameObject;
        Equip(0);
        
    }
    void Update()
    {
        if (PauseScript.paused) return;

        if (currentWeapon != null)
        {
           
                if (currentIndex == 0)
                {
                    Aim(Input.GetMouseButton(1));
                    
                   
                        if (Input.GetMouseButtonDown(0) && currentCooldown <= 0 && loadout[currentIndex].burst != 1)
                        {
                            Shoot();
                        }
                         
                   
                }else
                {
                    Aim(Input.GetMouseButton(1));
                        if (Input.GetMouseButton(0) && currentCooldown <= 0 )
                        {
                            Shoot();
                        }
                }
        }

            //weapon position
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.fixedDeltaTime * 4f);


        }

    

    void FixedUpdate()
    {
   
        //cooldown
        if (currentCooldown > 0) currentCooldown -= Time.fixedDeltaTime;
       
    
    }

    public void Equip(int p_ind)
    {
        if (currentWeapon != null) Destroy(currentWeapon);

        currentIndex = p_ind;

        fireRate = loadout[currentIndex].firerate;
        damage = loadout[currentIndex].damage;
        range = loadout[currentIndex].range;
        orgBloom = loadout[currentIndex].bloom;

        GameObject t_newWeapon = Instantiate(loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        t_newWeapon.transform.localPosition = Vector3.zero;
        t_newWeapon.transform.localEulerAngles = Vector3.zero;

        currentWeapon = t_newWeapon;
    }

    void Aim(bool p_isAiming)
    {
        Transform t_anchor = currentWeapon.transform.Find("Anchor");
        Transform t_state_ads = currentWeapon.transform.Find("States/ADS");
        Transform t_state_hip = currentWeapon.transform.Find("States/Hip");

        if (p_isAiming)
        {
            //aim
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_ads.position, Time.fixedDeltaTime * loadout[currentIndex].aimSpeed);
        }
        else
        {
            //hip
            t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.fixedDeltaTime * loadout[currentIndex].aimSpeed);
        }
    }


    void Shoot()
    {
        
            if (muzzleFlash == null)
                muzzleFlash = gameObject.transform.Find("Weapon/" + currentWeapon.name + "/Anchor/Design/Barrel/Particle System").GetComponent<ParticleSystem>();
            muzzleFlash.Play();

            Transform t_spawn = transform.Find("Main Camera");

            // bloom
            Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;

            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.up;
            t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.right;
            t_bloom -= t_spawn.position;
            t_bloom.Normalize();

            //cooldown

            currentCooldown = fireRate;


            //raycast
            RaycastHit t_hit = new RaycastHit();

            if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, range, canBeShot))
            {
                GameObject t_newHole = Instantiate(loadout[currentIndex].bulletHole, t_hit.point + t_hit.normal * 0.001f, Quaternion.identity) as GameObject;
                t_newHole.transform.LookAt(t_hit.point + t_hit.normal);

                Destroy(t_newHole, 2.5f);

                if (t_hit.transform.TryGetComponent<EnemyHealth>(out EnemyHealth T))
                {
                        T.TakeDamage(damage);
                        t_newHole.transform.parent = t_hit.collider.gameObject.transform;
                        //Stamina.Instance.Recover(Stamina.StaminaEventType.AttackHit);
                        Destroy(t_newHole, 0.5f);
                }
            }

            //sound
            //sfx.Stop();
            //sfx.clip = loadout[currentIndex].gunshotSound;
            //sfx.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
            //sfx.Play();
            
            //gun fx
            currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
        
    }
    //public void Attack()
    //{
    //    animator.SetBool("Attack", true);
    //}

   
    //void cancelAnimation()
    //{
    //    animator.SetBool("Attack", false);
    //}
   


}
