using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class OrbitalLook : AAction
{
    private readonly static float orbitalSpeedMultipler = 0.75f;

    public LockOn m_LockOn;
    public CinemachineVirtualCamera m_VirtualCamera;
    
    float m_OrbitalDirection;
    CinemachineOrbitalTransposer m_OrbitalTransposer;

    void Start()
    {
        m_OrbitalTransposer = m_VirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public override void Execute(InputAction.CallbackContext context)
    {
        m_OrbitalDirection = context.ReadValue<float>();
    }

    void LateUpdate()
    {
        if(!LockOn.m_isLockedOn)
        {
            m_OrbitalTransposer.m_XAxis.Value += m_OrbitalDirection * orbitalSpeedMultipler;
        }
    }
}