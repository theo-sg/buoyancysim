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
    float distance = 3f;
    
    bool MMB = false;

    Vector2 turnXY = new Vector2(0, 0);


    void OnEnable()
    {
        currentState = CameraState.Free;
        currentOffset = offset;
        controls = new Controls();
        controls.Enable();
        controls.Camera.LMB.performed += OnLMB;
        controls.Camera.MMB.performed += ctx => { MMB = true; };
        controls.Camera.MMB.canceled += ctx => { MMB = false; };

        controls.Camera.Scroll.performed += ctx => 
        { 
            float target = Mathf.Clamp(distance + (ctx.ReadValue<float>()) / -120f, 2, 6);
            distance = Mathf.Lerp(distance, target, 0.5f); 
        };

        controls.Camera.F.performed += ctx =>
        {
            if (currentState == CameraState.Follow)
            {
                currentState = CameraState.Free;
                target = null;
            }
        };
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
                currentOffset = (3f * Vector3.back);
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
            //if the middle mouse button is being held
            if (MMB)
            {
                Vector2 mouse = Mouse.current.position.ReadValue();
                Vector2 offset = new Vector2(Screen.width / 2, Screen.height / 2);
                mouse = (mouse - offset) * Time.deltaTime * sensitivity;

                //set the mouse rotation
                turnXY.x += mouse.x;
                turnXY.y += mouse.y;
                turnXY.y = Mathf.Clamp(turnXY.y, -10f, 80f);

                //set the offset vector
                

                //Quaternion turnX = Quaternion.AngleAxis(mouse.x, Vector3.up);
                //Quaternion turnY = Quaternion.AngleAxis(mouse.y, transform.right);
                //currentOffset = turnX * turnY * currentOffset;
            }

            currentOffset = Quaternion.Euler(turnXY.y, turnXY.x, 0) * (distance * Vector3.back);
            transform.position = Vector3.Lerp(transform.position, target.position + currentOffset, 0.5f);
            transform.LookAt(target);
        }     
    }

    void ClampRotation()
    {
       
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

