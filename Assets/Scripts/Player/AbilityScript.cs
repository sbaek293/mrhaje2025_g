using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class AbilityScript : MonoBehaviour
{
    public PropertyManager propertyManager;
    public Player playerScript;
    public Weapon weaponScript;

    private float org_jump;
    private float org_speed;
    public Image abilityImage;

    public PhysicsMaterial newMaterial;
    public PhysicsMaterial oldMaterial;

    [Header("Objects")]
    public GameObject shieldPrefab;
    public AudioClip hornSound;
    public GameObject playerDecoyPrefab;
    public GameObject marionettePrefab;


    public void Start()
    {

    }
    public void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            Ability();
        }
    }
    public void FixedUpdate()
    {
    }

    public void Ability()
    {
        if (propertyManager.properties.Count == 0) return;
        if (propertyManager.properties[0].name == "Jump")
        {
            org_jump = playerScript.jumpForce;
            abilityImage.sprite = propertyManager.properties[0].icon;   
            playerScript.jumpForce += 700f;
            Invoke("resetJumpForce", 3f);
        }
        else if (propertyManager.properties[0].name == "Blue")
        {
            org_speed = playerScript.originalSpeed;
            abilityImage.sprite = propertyManager.properties[0].icon;
            playerScript.originalSpeed += 100;
            Invoke("resetSpeed", 3f);
        }
        else if (propertyManager.properties[0].name == "Red")
        {
            playerScript.col.sharedMaterial = newMaterial;
            abilityImage.sprite = propertyManager.properties[0].icon;
            Invoke("resetRed", 3f);
        }
        else if (propertyManager.properties[0].name == "Dash")
        {
            playerScript.movementType = "truck";
            playerScript.normalCam.fieldOfView = playerScript.baseFOV * 1.3f;
            abilityImage.sprite = propertyManager.properties[0].icon;
            Invoke("resetDash", 10f);
        }
        else if (propertyManager.properties[0].name == "Heavy")
        {
            org_speed = playerScript.originalSpeed;
            playerScript.rig.mass = playerScript.originalWeight * 5;
            playerScript.originalSpeed *= 0.5f;
            abilityImage.sprite = propertyManager.properties[0].icon;
            Invoke("resetHeavy", 10f);
        }
        else if (propertyManager.properties[0].name == "Horn")
        {
            AudioManager.PlaySound(gameObject, hornSound, false, 10, 0.1f);
            abilityImage.sprite = propertyManager.properties[0].icon;
            Invoke("resetHorn", 0.25f);
        }
        else if (propertyManager.properties[0].name == "Shield")
        {
            GameObject shield = Instantiate(shieldPrefab, transform);
            abilityImage.sprite = propertyManager.properties[0].icon;
            Invoke("resetShield", 20f);
        }
        else if (propertyManager.properties[0].name == "Automatic")
        {
            weaponScript.Equip(1);
            abilityImage.sprite = propertyManager.properties[0].icon;
            Invoke("resetAuto", 7f);
        }
        else if (propertyManager.properties[0].name == "Decoy")
        {
            transform.GetComponent<Player>().playerDecoy = Instantiate(playerDecoyPrefab, transform.position, transform.rotation);
            abilityImage.sprite = propertyManager.properties[0].icon;
            Invoke("resetDecoy", 10f);
        }
        else if (propertyManager.properties[0].name == "Marionette")
        {
            weaponScript.Equip(2);
            abilityImage.sprite = propertyManager.properties[0].icon;
            Invoke("resetMarionette", 120f);
        }
    }

    void resetJumpForce()
    {
        playerScript.jumpForce = org_jump;
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetSpeed()
    {
        playerScript.originalSpeed = org_speed;
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetRed()
    {
        playerScript.col.sharedMaterial = oldMaterial;
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetDash()
    {
        playerScript.movementType = "normal";
        playerScript.normalCam.fieldOfView = playerScript.baseFOV;

        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetHeavy()
    {
        playerScript.rig.mass = playerScript.originalWeight;
        playerScript.originalSpeed = org_speed;
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetHorn()
    {
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetShield()
    {
        Transform shield = transform.Find("shieldPlayer");
        if (shield != null)
        {
            Destroy(shield.gameObject);
        }
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetAuto()
    {
        weaponScript.Equip(0);
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetDecoy()
    {
        Destroy(transform.GetComponent<Player>().playerDecoy);
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetMarionette()
    {
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }
}

