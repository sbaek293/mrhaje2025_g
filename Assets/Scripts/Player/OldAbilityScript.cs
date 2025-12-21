using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OldAbilityScript : MonoBehaviour
{
    [Header("References")]
    public PropertyManager propertyManager;
    public Player playerScript;
    public Weapon weaponScript;
    public Transform enemyContainer;

    private float org_jump;
    private float org_speed;
    public Image abilityImage;

    public PhysicsMaterial newMaterial;
    public PhysicsMaterial oldMaterial;

    public float blinkDistance=10;

    public PropertyDatas currentUsingProperty;
    public GameObject currentTimer;

    [Header("Objects")]
    public GameObject shieldPrefab;
    public AudioClip hornSound;
    public GameObject playerDecoyPrefab;
    public GameObject marionettePrefab;
    public PhysicsMaterial playerMaterial;
    public PhysicsMaterial bouncyMaterial;


    public void Ability()
    {
        if (propertyManager.properties.Count == 0) return;
        if (propertyManager.properties[0].name == "Jump")
        {
            org_jump = playerScript.jumpForce;
            abilityImage.sprite = propertyManager.properties[0].icon;
            playerScript.jumpForce += 700f;
        }
        else if (propertyManager.properties[0].name == "Blue")
        {
            org_speed = playerScript.originalSpeed;
            abilityImage.sprite = propertyManager.properties[0].icon;
            playerScript.originalSpeed += 100;
        }
        else if (propertyManager.properties[0].name == "Red")
        {
            playerScript.col.sharedMaterial = newMaterial;
            abilityImage.sprite = propertyManager.properties[0].icon;
        }
        else if (propertyManager.properties[0].name == "Dash")
        {
            playerScript.movementType = "truck";
            playerScript.normalCam.fieldOfView = playerScript.baseFOV * 1.3f;
            abilityImage.sprite = propertyManager.properties[0].icon;
        }
        else if (propertyManager.properties[0].name == "Heavy")
        {
            org_speed = playerScript.originalSpeed;
            playerScript.rig.mass = playerScript.originalWeight * 5;
            playerScript.originalSpeed *= 0.5f;
            abilityImage.sprite = propertyManager.properties[0].icon;
        }
        else if (propertyManager.properties[0].name == "Horn")
        {
            AudioManager.PlaySound(gameObject, hornSound, false, 10, 0.1f);
            abilityImage.sprite = propertyManager.properties[0].icon;
        }
        else if (propertyManager.properties[0].name == "Shield")
        {
            GameObject shield = Instantiate(shieldPrefab, transform);
            abilityImage.sprite = propertyManager.properties[0].icon;
        }
        else if (propertyManager.properties[0].name == "Automatic")
        {
            weaponScript.Equip(1);
            abilityImage.sprite = propertyManager.properties[0].icon;
        }
        else if (propertyManager.properties[0].name == "Blink")
        {
            abilityImage.sprite = propertyManager.properties[0].icon;
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");
            Vector3 dir = new Vector3(t_hmove, 0, t_vmove).normalized;
            if (dir.magnitude == 0) dir = new Vector3(0, 0, 1);
            Vector3 diff = dir * blinkDistance;
            transform.position = transform.position + transform.TransformDirection(diff);
        }
        else if (propertyManager.properties[0].name == "Hardening")
        {
            playerScript.hardening = true;
            org_speed = playerScript.originalSpeed;
            playerScript.originalSpeed *= 0.2f;
        }
        else if (propertyManager.properties[0].name == "Decoy")
        {
            GameObject temp_decop = Instantiate(playerDecoyPrefab, enemyContainer);
            temp_decop.transform.position = transform.position;
            abilityImage.sprite = propertyManager.properties[0].icon;
            propertyManager.RemoveProperty(propertyManager.properties[0]);
        }
        else if (propertyManager.properties[0].name == "Marionette")
        {
            weaponScript.Equip(2);
            abilityImage.sprite = propertyManager.properties[0].icon;
            propertyManager.RemoveProperty(propertyManager.properties[0]);
        }
        else if (propertyManager.properties[0].name == "Copy")
        {
            weaponScript.Equip(3);
            abilityImage.sprite = propertyManager.properties[0].icon;
            propertyManager.RemoveProperty(propertyManager.properties[0]);
        }
        else if (propertyManager.properties[0].name == "Bouncy")
        {
            GetComponent<CapsuleCollider>().material = bouncyMaterial;
            playerScript.rig.mass = playerScript.originalWeight * 0.75f;
            abilityImage.sprite = propertyManager.properties[0].icon;
        }
        else if (propertyManager.properties[0].name == "Lavitating")
        {
            playerScript.lavitating = true;
            abilityImage.sprite = propertyManager.properties[0].icon;
        }
        else if (propertyManager.properties[0].name == "Shoot")
        {
            weaponScript.Equip(4);
            abilityImage.sprite = propertyManager.properties[0].icon;
            propertyManager.RemoveProperty(propertyManager.properties[0]);
        }
        else if (propertyManager.properties[0].name == "Fire")
        {
            weaponScript.Equip(5);
            abilityImage.sprite = propertyManager.properties[0].icon;
            propertyManager.RemoveProperty(propertyManager.properties[0]);
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
    void resetBlink()
    {
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetDecoy()
    {
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetHardening()
    {
        playerScript.hardening = false;
        playerScript.originalSpeed = org_speed;
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }
    void resetMarionette()
    {
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetCopy()
    {
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetBouncy()
    {
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        GetComponent<CapsuleCollider>().material = playerMaterial;
        playerScript.rig.mass = playerScript.originalWeight;
        abilityImage.sprite = null;
    }

    void resetLavitating()
    {
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        playerScript.lavitating = false;
        abilityImage.sprite = null;
    }

    void resetShoot()
    {
        weaponScript.Equip(0);
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }

    void resetFire()
    {
        weaponScript.Equip(0);
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }
}

