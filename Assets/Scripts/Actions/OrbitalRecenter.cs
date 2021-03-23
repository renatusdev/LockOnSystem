using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Cinemachine;

public class OrbitalRecenter : AAction
{
    public CinemachineVirtualCamera m_VirtualCamera;

    private CinemachineOrbitalTransposer m_OrbitalTransposer;

    void Start()
    {
        m_OrbitalTransposer = m_VirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public override void Execute(InputAction.CallbackContext context)
    {
        if (context.started) { if(!LockOn.m_isLockedOn) Recenter(); }
    }

    public void Recenter()
    {
        StartCoroutine(CoRecenter());
    }

    public IEnumerator CoRecenter()
    {
        m_OrbitalTransposer.m_RecenterToTargetHeading.m_enabled = true;
        yield return new WaitForSeconds(m_OrbitalTransposer.m_RecenterToTargetHeading.m_RecenteringTime + 0.25f);

        if(!LockOn.m_isLockedOn)
        {
            m_OrbitalTransposer.m_RecenterToTargetHeading.m_enabled = false;
        }
    }
}