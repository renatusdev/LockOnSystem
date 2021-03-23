using UnityEngine;
using UnityEngine.InputSystem;

public class LockOnScroll : AAction
{
    public LockOn m_LockOn;

    public override void Execute(InputAction.CallbackContext context)
    {
        if(context.performed) { Scroll(); }
    }

    void Scroll()
    {
        if(LockOn.m_isLockedOn)
        {
            // There's only the player and one target, thus nothing to scroll so return.
            if(Aimable.aimableObjects.Count <= 1) { return; }

            m_LockOn.UnlockTarget(m_LockOn.m_LockOnScrollIndex);
            m_LockOn.m_LockOnScrollIndex++;
            m_LockOn.LockTarget(m_LockOn.m_LockOnScrollIndex);
        }
        else
        {
            m_LockOn.On();
        }
    }
}