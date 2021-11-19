using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class CameraController : MonoBehaviour
{
    //## reference variables
    Controls controls;
    Transform target;

    //##camera default offset
    public Vector3 offset;
    Vector3 currentOffset;

    //##camera state
    public CameraState currentState;

    float sensitivity = 0.6f;
    bool MMB = false;


    void OnEnable()
    {
        currentState = CameraState.Free;
        currentOffset = offset;
        controls = new Controls();
        controls.Enable();
        controls.Camera.LMB.performed += OnLMB;
        controls.Camera.MMB.performed += ctx => { MMB = true; };
        controls.Camera.MMB.canceled += ctx => { MMB = false; };
    }

    /// <summary>
    /// method called on left mouse click
    /// </summary>
    /// <param name="context">the event details</param>
    public void OnLMB(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); 
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f))
        {
            if(hit.transform.gameObject.GetComponent<BuoyantObject>() != null)
            {
                target = hit.transform;
                currentState = CameraState.Follow;
            }
        }
    }

    /// <summary>
    /// occurs every frame
    /// </summary>
    void Update()
    {
        //if there is a current target
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + currentOffset, 0.5f);
            transform.LookAt(target);

            //if the middle mouse button is being held
            if (MMB)
            {
                Vector2 mouse = Mouse.current.position.ReadValue();
                Vector2 offset = new Vector2(Screen.width / 2, Screen.height / 2);
                mouse = (mouse - offset) * Time.deltaTime * sensitivity;
                Quaternion turnX = Quaternion.AngleAxis(mouse.x, Vector3.up);
                Quaternion turnY = Quaternion.AngleAxis(mouse.y, transform.right);
                currentOffset = turnX * turnY * currentOffset;
            }
        }     
    }

    void OnDisable()
    {
        controls.Disable();
    }
}

public enum CameraState
{
    Free,
    Follow
}

