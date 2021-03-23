using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Variables

    public AAction m_ActionMovement;
    public AAction m_ActionLockOn;
    public AAction m_ActionLockOnScroll;
    public AAction m_ActionOrbitalRecenter;
    public AAction m_ActionOrbitalLook;
    
    PlayerInput m_PlayerInput;
    Rigidbody m_Rigidbody;
    Animator m_Animator;

    #endregion

    #region Unity Functions

    void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
        m_PlayerInput.onActionTriggered += HandleActions;
    }

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
    }

    #endregion

    #region Private Functions

    private void HandleActions(InputAction.CallbackContext context)
    {
        if(context.action.name.Equals("Move"))
        {
            m_ActionMovement.Execute(context);
        }
        else if (context.action.name.Equals("OrbitalLook"))
        {
            m_ActionOrbitalLook.Execute(context);
        }
        else if(context.action.name.Equals("OrbitalRecenter"))
        {
            m_ActionOrbitalRecenter.Execute(context);
        }
        else if(context.action.name.Equals("LockOn"))
        {
            m_ActionLockOn.Execute(context);
        }
        else if(context.action.name.Equals("LockOnScroll"))
        {
            m_ActionLockOnScroll.Execute(context);
        }
    }

    #endregion
}