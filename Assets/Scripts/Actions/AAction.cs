using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AAction : MonoBehaviour
{
    // A method that initializes all the variables inside an action

    public abstract void Execute(InputAction.CallbackContext context);
}