using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System;

// TODO: Optimize by saving tweens on start up,
// then when we remove them all, we can CANCEL any tweens that were 
[RequireComponent(typeof(CinemachineTargetGroup))]
public class CameraTargets : MonoBehaviour
{
    public readonly static float s_LockedTargetWeight = 10;
    public readonly static float s_NonLockedTargetWeight = 1;
    public readonly static float s_TweenWeightDuration = 0.55f;

    public static CinemachineTargetGroup m_CameraFollowTargets;

    static int targetCounter;

    private void Awake()
    {
        m_CameraFollowTargets = GetComponent<CinemachineTargetGroup>();
        targetCounter = 0;
    }

    public static void AddMember(Transform t, float weight)
    {
        m_CameraFollowTargets.AddMember(t, 0, 1);
        targetCounter++;

        int i = targetCounter;
        ChangeWeight(i, weight);
    }

    public static void RemoveMember(int index)
    {      
        ChangeWeight(index, 0, () => { targetCounter--; m_CameraFollowTargets.RemoveMember(CameraTargets.m_CameraFollowTargets.m_Targets[index].target); });
    }

    public static void RemoveAll(bool skipFirstTarget)
    {
        int skip = skipFirstTarget ? 1 : 0;
        int length = m_CameraFollowTargets.m_Targets.Length; 

        // Reduce weight of all targets except the last and the first (if !skipFirstTarget). 
        for(int i = 0 + skip; i < length-1; i++)
        {
            ChangeWeight(i, 0);
        }
        
        // Reduce weight of last target, and then remove all targets except the first (if !skipFirstTarget).
        ChangeWeight(m_CameraFollowTargets.m_Targets.Length-1, 0, () =>
        {
            for(int i = 0 + skip; i < length; i++)
            {
                m_CameraFollowTargets.RemoveMember(CameraTargets.m_CameraFollowTargets.m_Targets[m_CameraFollowTargets.m_Targets.Length-1].target);
            }
            targetCounter = 0;
        });
    }

    public static void ChangeWeight(int index, float weight)
    {    
        DOTween.To(() => CameraTargets.m_CameraFollowTargets.m_Targets[index].weight, x => CameraTargets.m_CameraFollowTargets.m_Targets[index].weight = x, weight, s_TweenWeightDuration).
        Play();
    }

    public static void ChangeWeight(int index, float weight, Action onComplete)
    {    
        DOTween.To(() => CameraTargets.m_CameraFollowTargets.m_Targets[index].weight, x => CameraTargets.m_CameraFollowTargets.m_Targets[index].weight = x, weight, s_TweenWeightDuration).
        OnComplete(() => onComplete.Invoke()).Play();
    }
}