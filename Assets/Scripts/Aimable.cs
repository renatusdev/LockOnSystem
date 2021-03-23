using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Aimable : MonoBehaviour
{
    public Transform m_uiAnchor;
    public static List<Aimable> aimableObjects;
    
    public bool m_IsLockedOn;

    private void Awake()
    {
        aimableObjects = new List<Aimable>();
    }

    void OnBecameInvisible()
    {
        if(aimableObjects.Contains(this))
        {
            if(LockOn.m_isLockedOn) { return; }

            aimableObjects.Remove(this);
        }
    }
    
    void OnBecameVisible()
    {
        if(!aimableObjects.Contains(this))
        {
            aimableObjects.Add(this);

            if(LockOn.m_isLockedOn & aimableObjects.Count != (CameraTargets.m_CameraFollowTargets.m_Targets.Length-1))
            {
                CameraTargets.AddMember(this.transform, CameraTargets.s_NonLockedTargetWeight);
            }
        }
    }
}