using UnityEngine;
using UnityEngine.InputSystem;

public class LightControl : MonoBehaviour
{
    public Light lightComp;
    public InputActionReference changeColorAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightComp = GetComponent<Light>();

        // Enable input action
        changeColorAction.action.Enable();

        // When button is pressed -> change color
        changeColorAction.action.performed += (ctx) =>
        {
            lightComp.color = Color.red;
        };
    }
}

