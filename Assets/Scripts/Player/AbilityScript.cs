using System.Collections;
using System.Collections.Generic;
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
}

