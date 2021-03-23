using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LockOnUI : MonoBehaviour
{
    public SpriteRenderer m_SpriteRenderer;
    
    private Animator m_Animator;

    private Tween m_TweenRotation;

    private void Start() {
        m_SpriteRenderer.enabled = false;
        m_TweenRotation = transform.DOLocalRotate(Vector3.forward * 240, 0.20f, RotateMode.FastBeyond360).SetAutoKill(false).Pause();
    }


    public void Lock(Transform target)
    {
        m_SpriteRenderer.enabled = true;
        this.gameObject.SetActive(true);
        this.transform.SetParent(target);

        transform.DOLocalMove(Vector3.zero, 0.25f).Play();
        m_TweenRotation.Restart();
    }

    public void Unlock()
    {
        this.transform.SetParent(null);
        this.gameObject.SetActive(false);
    }

    public void TurnOff()
    {
        transform.DOPunchRotation(Vector3.forward * 50, 0.5f, 10, 1).Play().OnComplete(() => 
        {
            m_SpriteRenderer.enabled = false;
            transform.SetParent(null);
            gameObject.SetActive(false);
        });
    }

    private void LateUpdate() {
        if(!m_TweenRotation.IsPlaying()){ transform.LookAt(Camera.main.transform.position); }
    }
}
