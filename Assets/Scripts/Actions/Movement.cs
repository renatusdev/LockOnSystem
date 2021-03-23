using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : AAction
{
    private readonly static float backwardsSpeedMultipler = 0.35f;
    private readonly static int angularSpeedMultipler = 25;
    
    [Range(0,20)] public int m_speed;
    
    public Rigidbody m_Rigidbody;
    public Animator m_Animator;
    public LockOn m_LockOn;
    public Vector3 m_Direction;

    public override void Execute(InputAction.CallbackContext value)
    {
        Vector2 direction = value.ReadValue<Vector2>();
        m_Direction = new Vector3(direction.x, 0, direction.y).normalized;

        // Animation
        m_Animator.SetInteger("zMove", Mathf.RoundToInt(direction.y));
        m_Animator.SetInteger("hMove", Mathf.RoundToInt(direction.x));
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float zMove = m_Direction.z;

        Vector3 move = new Vector3(0, 0, zMove);

        if(LockOn.m_isLockedOn)
        {
            // Rotation looking at target
            var qTo = Quaternion.LookRotation(m_LockOn.m_LockedOnTarget.position - transform.position);
            qTo.x = 0;
            qTo.z = 0;
            qTo = Quaternion.Slerp(transform.rotation, qTo, m_speed * Time.deltaTime);
            m_Rigidbody.MoveRotation(qTo);

            // Movement is left and right
            move.x =  m_Direction.x;
            move *= LockOn.lockOnMultiplier;
            // TODO: Clamp zMove if too close to Target.
        }
        else
        {
            move.z *=  m_Direction.z < 0 ? backwardsSpeedMultipler : 1;  
            
            // Rotation
            float yRotate = m_Direction.x;
            Quaternion rotate = transform.rotation;
            
            yRotate *= m_speed * angularSpeedMultipler * Time.fixedDeltaTime;
            rotate *= Quaternion.AngleAxis(yRotate, Vector2.up);
            
            m_Rigidbody.MoveRotation(rotate);
        }

        move *= m_speed * Time.fixedDeltaTime;
        move = transform.TransformDirection(move);
        move += transform.position;
        
        m_Rigidbody.MovePosition(move);
    }
}