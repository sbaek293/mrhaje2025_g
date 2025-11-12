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

    public Image abilityImage;
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
    }

    void resetJumpForce()
    {
        playerScript.jumpForce = org_jump;
        //propertyManager.RemoveProperty(propertyManager.properties[0]);
        abilityImage.sprite = null;
    }
}

