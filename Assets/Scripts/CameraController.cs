using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class CameraController : MonoBehaviour
{
    //### singleton
    public static CameraController Instance;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    //### reference variables
    Controls controls;
    public Transform target;

    //### parameters with default values
    Vector3 defaultOffset = new Vector3(0, 2, -3);
    float sensitivity = 0.25f;

    //### states
    public CameraState currentState;
    bool MMB = false;
    float distance = 3f;
    Vector3 currentOffset;
    Vector2 turnXY = new Vector2(0, 0);
    Vector2 movDir = new Vector2(0, 0);

    void OnEnable()
    {
        //setup controls and state of camera
        currentState = CameraState.Free;
        currentOffset = defaultOffset;
        controls = new Controls();
        controls.Enable();
        controls.Camera.LMB.performed += OnLMB;
        controls.Camera.MMB.performed += ctx => { MMB = true; };
        controls.Camera.MMB.canceled += ctx => { MMB = false; };

        //set scroll wheel behaviour
        //scroll wheel changes distance to targeted object
        controls.Camera.Scroll.performed += ctx => 
        { 
            float target = Mathf.Clamp(distance + (ctx.ReadValue<float>()) / -120f, 2, 12);
            distance = Mathf.Lerp(distance, target, 0.5f); 
        };

        controls.Camera.WASD.performed += ctx => { movDir = ctx.ReadValue<Vector2>().normalized * 2f; };
        controls.Camera.WASD.canceled += ctx => { movDir = Vector2.zero; };

        //set F button behaviour
        //releases camera from followed object
        controls.Camera.F.performed += ctx => UnlinkCamera();
    }

    /// <summary>
    /// method called on left mouse click
    /// </summary>
    public void OnLMB(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); 
        RaycastHit hit;

        //if clicked object is a buoyant object set it as camera target
        if(Physics.Raycast(ray, out hit, 100f))
        {
            if(hit.transform.gameObject.GetComponent<BuoyantObject>() != null)
            {
                target = hit.transform;
                currentState = CameraState.Follow;
                currentOffset = (3f * Vector3.back);
                UIManager.Instance.SetDeleteButton(true);
            }
        }
    }

    /// <summary>
    /// unlinks the camera from the current object
    /// </summary>
    public void UnlinkCamera()
    {
        if (currentState == CameraState.Follow)
        {
            Vector3 f = transform.forward;
            currentState = CameraState.Free;
            target = null;
            transform.rotation = Quaternion.LookRotation(f, transform.up);
        }
    }

    /// <summary>
    /// occurs every frame
    /// </summary>
    void Update()
    {
        HandleMotion();
    }

    /// <summary>
    /// calculates the new position and moves the camera
    /// </summary>
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
                turnXY.y = Mathf.Clamp(turnXY.y, 5f, 80f);
            }

            //set the offset vector 
            currentOffset = Quaternion.Euler(turnXY.y, turnXY.x, 0) * (distance * Vector3.back);
            transform.position = Vector3.Lerp(transform.position, target.position + currentOffset, 0.5f);
            transform.LookAt(target);
        }

        else
        {
            //if there is no target and middle mouse button is held
            if (MMB)
            {
                Vector2 mouse = Mouse.current.position.ReadValue();
                Vector2 offset = new Vector2(Screen.width / 2, Screen.height / 2);
                mouse = (mouse - offset) * Time.deltaTime * sensitivity;

                //set the mouse rotation
                turnXY.x += mouse.x;
                turnXY.y -= mouse.y;
                turnXY.y = Mathf.Clamp(turnXY.y, -30f, 70f);             
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

