using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using DG.Tweening;

public class LockOn : AAction
{
    private readonly static int s_CamTransYOffset = 4;
    private readonly static int s_CamTransZOffset = 1;
    private readonly static float s_CamTransDuration = 0.5f;

    public readonly static float lockOnMultiplier = 0.15f;
    public static bool m_isLockedOn;

    public Animator m_Animator;
    public CinemachineVirtualCamera m_VirtualCamera;
    public OrbitalRecenter m_OrbitalRecenter;
    public LockOnUI m_ui;

    public Transform m_LockedOnTarget { get; set; }
    public int m_LockOnScrollIndex { get; set; }

    CinemachineOrbitalTransposer m_OrbitalTransposer;
    Sequence m_TweenCameraTransition;

    private void Awake()
    {
        m_OrbitalTransposer = m_VirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        m_LockOnScrollIndex = 0;
        m_isLockedOn = false;

        m_TweenCameraTransition = DOTween.Sequence().Append(    
            DOTween.To(() => m_VirtualCamera.m_Lens.FieldOfView, x => m_VirtualCamera.m_Lens.FieldOfView = x,
            m_VirtualCamera.m_Lens.FieldOfView - Settings.m_LockOnZoomIn, s_CamTransDuration).SetAutoKill(false).Pause()).Join(
            DOTween.To(() => m_OrbitalTransposer.m_FollowOffset.y, x =>  m_OrbitalTransposer.m_FollowOffset.y = x,
            m_OrbitalTransposer.m_FollowOffset.y - s_CamTransYOffset, s_CamTransDuration).SetAutoKill(false).Pause()).SetAutoKill(false).Join(
            DOTween.To(() => m_OrbitalTransposer.m_FollowOffset.z, x =>  m_OrbitalTransposer.m_FollowOffset.z = x,
            m_OrbitalTransposer.m_FollowOffset.z + s_CamTransZOffset, s_CamTransDuration).SetAutoKill(false).Pause()).SetAutoKill(false).Pause();
    }   

    public override void Execute(InputAction.CallbackContext context)
    {
        // Restricted input execution to AFTER the FOV transitions are done.
        // Completed FOV transitions imply completed target refresh.
        if (context.started)   { if(!m_TweenCameraTransition.IsPlaying()) On(); }
    }

    public void On()
    {
        if(Aimable.aimableObjects.Count == 0) { return; }

        m_Animator.SetInteger("zMove", 0);
        m_Animator.SetInteger("hMove", 0);

        m_isLockedOn = !m_isLockedOn;
        m_Animator.SetBool("isLockedOn", m_isLockedOn);
        m_OrbitalTransposer.m_RecenterToTargetHeading.m_enabled = m_isLockedOn;

        if(m_isLockedOn)
        {
            LockTarget(m_LockOnScrollIndex);

            // Focus camera on all aimable objects.
            foreach(Aimable target in Aimable.aimableObjects)
            {
                if(target.m_IsLockedOn) { CameraTargets.AddMember(target.transform, CameraTargets.s_LockedTargetWeight); }    
                else                    { CameraTargets.AddMember(target.transform, CameraTargets.s_NonLockedTargetWeight); }
            }

            m_TweenCameraTransition.Restart();
        }
        else
        {
            UnlockTarget(m_LockOnScrollIndex);
            CameraTargets.RemoveAll(true);
            
            m_ui.TurnOff();
            m_isLockedOn = false;
            m_OrbitalRecenter.Recenter();
            m_TweenCameraTransition.PlayBackwards();
        }
    }

    public void LockTarget(int index)
    {
        index %= Aimable.aimableObjects.Count;
        m_LockedOnTarget = Aimable.aimableObjects[index].transform;
        Aimable.aimableObjects[index].m_IsLockedOn = true;
        CameraTargets.ChangeWeight(index+1, CameraTargets.s_LockedTargetWeight);

        m_ui.Lock(Aimable.aimableObjects[index].m_uiAnchor);
    }

    public void UnlockTarget(int index)
    {
        index %= Aimable.aimableObjects.Count;
        Aimable.aimableObjects[index].m_IsLockedOn = false;
        m_ui.Unlock();
        CameraTargets.ChangeWeight(index+1, CameraTargets.s_NonLockedTargetWeight);    }
}