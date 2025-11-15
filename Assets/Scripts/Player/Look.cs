using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;



public class Look : MonoBehaviour
{
    #region Variables

    public  bool cursorLocked = true;

    public Transform player;
    public Transform cams;
    public Transform weapon;

    public float xSensitivity;
    public float ySensitivity;
    public float maxAngle;

    private Quaternion camCenter;



    #endregion

    #region Monobehaviour Callbacks
    void Start()
    {
        camCenter = cams.localRotation;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }
    void Update()
    {
       
        //if (Pause.paused) return;
        SetY();
        SetX();

        //UpdateCursorLock();

    }

    #endregion

    #region Private Methods
    void SetY()
    {
        float t_input = Input.GetAxis("Mouse Y") * ySensitivity * Time.fixedDeltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, -Vector3.right);
        Quaternion t_delta = cams.localRotation * t_adj;

        if (Quaternion.Angle(camCenter, t_delta) < maxAngle)
        {
            cams.localRotation = t_delta;
        }
        if (weapon)
        {
            weapon.rotation = cams.rotation;
        }

    }

    void SetX()
    {
        float t_input = Input.GetAxis("Mouse X") * xSensitivity * Time.fixedDeltaTime;
        Quaternion t_adj = Quaternion.AngleAxis(t_input, Vector3.up);
        Quaternion t_delta = player.localRotation * t_adj;
        player.localRotation = t_delta;
    }

    public void UpdateCursorLock()
    {
        if (!cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

         
            cursorLocked = false;
        }
    }
    #endregion

}

