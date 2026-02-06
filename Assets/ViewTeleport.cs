using UnityEngine;
using UnityEngine.InputSystem;

public class ViewTeleport : MonoBehaviour
{
    public Transform roomPoint; // Beginning point for player
    public Transform outsidePoint; // External Viewing Point
    public InputActionReference toggleAction; // Button in HDM to change view

    private bool isOutside = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggleAction.action.Enable(); //Enable VR button
        toggleAction.action.performed += OnTogglePerformed;// Listen for button pressed      
    }

    private void OnTogglePerformed(InputAction.CallbackContext ctx)
    {
        if (isOutside)
        {
            transform.position = roomPoint.position;
            transform.rotation = roomPoint.rotation;
        }
        else
        {
            transform.position = outsidePoint.position;
            transform.rotation = outsidePoint.rotation;
        }

        isOutside = !isOutside; // Swtich state
    }
}


