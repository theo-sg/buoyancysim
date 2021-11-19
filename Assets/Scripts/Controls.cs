// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Camera"",
            ""id"": ""1c94a705-bf4b-442b-92d0-4b24865bf73c"",
            ""actions"": [
                {
                    ""name"": ""LMB"",
                    ""type"": ""Button"",
                    ""id"": ""04e7ccfe-5e57-4a18-9462-3c02b1a93df4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MMB"",
                    ""type"": ""Button"",
                    ""id"": ""771b7db5-b0de-4779-b655-b37406f9bfc4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scroll"",
                    ""type"": ""Value"",
                    ""id"": ""bdc71f4f-03ed-4e2a-b74e-18c29addfe18"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""F"",
                    ""type"": ""Button"",
                    ""id"": ""00a94ae2-38c2-4caf-b0ee-4cd9b3af0dee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5a12c033-865d-4cbc-ab99-373dcff153a7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""503b3fa8-f1bc-4102-8e17-044c2aefa9ab"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MMB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""205665a4-eea5-4288-879f-61be4e216b8e"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d19f0d80-4169-47de-b8b7-f04534d60942"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""F"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_LMB = m_Camera.FindAction("LMB", throwIfNotFound: true);
        m_Camera_MMB = m_Camera.FindAction("MMB", throwIfNotFound: true);
        m_Camera_Scroll = m_Camera.FindAction("Scroll", throwIfNotFound: true);
        m_Camera_F = m_Camera.FindAction("F", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_LMB;
    private readonly InputAction m_Camera_MMB;
    private readonly InputAction m_Camera_Scroll;
    private readonly InputAction m_Camera_F;
    public struct CameraActions
    {
        private @Controls m_Wrapper;
        public CameraActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LMB => m_Wrapper.m_Camera_LMB;
        public InputAction @MMB => m_Wrapper.m_Camera_MMB;
        public InputAction @Scroll => m_Wrapper.m_Camera_Scroll;
        public InputAction @F => m_Wrapper.m_Camera_F;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @LMB.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnLMB;
                @LMB.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnLMB;
                @LMB.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnLMB;
                @MMB.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMMB;
                @MMB.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMMB;
                @MMB.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMMB;
                @Scroll.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnScroll;
                @Scroll.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnScroll;
                @Scroll.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnScroll;
                @F.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnF;
                @F.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnF;
                @F.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnF;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LMB.started += instance.OnLMB;
                @LMB.performed += instance.OnLMB;
                @LMB.canceled += instance.OnLMB;
                @MMB.started += instance.OnMMB;
                @MMB.performed += instance.OnMMB;
                @MMB.canceled += instance.OnMMB;
                @Scroll.started += instance.OnScroll;
                @Scroll.performed += instance.OnScroll;
                @Scroll.canceled += instance.OnScroll;
                @F.started += instance.OnF;
                @F.performed += instance.OnF;
                @F.canceled += instance.OnF;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);
    public interface ICameraActions
    {
        void OnLMB(InputAction.CallbackContext context);
        void OnMMB(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnF(InputAction.CallbackContext context);
    }
}
