using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera
    {
        get;
        set;
    }
    public Vector2 MousePosition
    {
        get;
        private set;
    }
    public bool ToSpawn
    {
        get;set;
    } 

    public bool ToReset { get; set; }

    public void OnSpawn(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1 && !ToReset)
        {
            ToSpawn = true;
        }
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        MousePosition = MainCamera.ScreenToWorldPoint(context.ReadValue<Vector2>());

    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1 && !ToSpawn)
        {
            ToReset = true;
        }
    }
}
