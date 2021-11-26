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

    float sensitivity = 0.25f;
    float distance = 3f;
    
    bool MMB = false;

    Vector2 turnXY = new Vector2(0, 0);
    Vector2 movDir = new Vector2(0, 0);


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

        controls.Camera.WASD.performed += ctx => { movDir = ctx.ReadValue<Vector2>().normalized; };
        controls.Camera.WASD.canceled += ctx => { movDir = Vector2.zero; };

        controls.Camera.F.performed += ctx =>
        {
            if (currentState == CameraState.Follow)
            {
                Vector3 f = transform.forward;
                currentState = CameraState.Free;
                target = null;
                transform.rotation = Quaternion.LookRotation(f, transform.up);
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
        HandleMotion();
    }

    void HandleMotion()
    {
        //if there is a current target
        if (currentState == CameraState.Follow)
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

            }

            //set the offset vector 
            currentOffset = Quaternion.Euler(turnXY.y, turnXY.x, 0) * (distance * Vector3.back);
            transform.position = Vector3.Lerp(transform.position, target.position + currentOffset, 0.5f);
            transform.LookAt(target);
        }

        else
        {
            if (MMB)
            {
                Vector2 mouse = Mouse.current.position.ReadValue();
                Vector2 offset = new Vector2(Screen.width / 2, Screen.height / 2);
                mouse = (mouse - offset) * Time.deltaTime * sensitivity;

                //set the mouse rotation
                turnXY.x += mouse.x;
                turnXY.y -= mouse.y;
                turnXY.y = Mathf.Clamp(turnXY.y, -70f, 70f);             
            }

            Vector3 pos = (transform.forward * movDir.y + transform.right * movDir.x) * 0.1f ;
            pos += transform.position;

            //set the camera rotation
            Quaternion rot = Quaternion.Euler(turnXY.y, turnXY.x, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 0.4f);
            transform.position = Vector3.Lerp(transform.position, pos, 0.3f);
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

