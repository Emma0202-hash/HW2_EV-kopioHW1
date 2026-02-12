using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // TEHTY This script should be attached to both controller objects in the scene
    // TEHTY Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)

    CustomGrab otherHand = null; // Tallennetaan toisen ohjaimen skripti, jotta kädet voivat "jakaa" objektin
    public List<Transform> nearObjects = new List<Transform>(); // Lista lähellä olevista objekteista
    public Transform grabbedObject = null; // Tällä hetkellä kiinni oleva objekti, jos on kädessä
    public InputActionReference action; // Unityn Input System-toiminto
    bool grabbing = false; // Tarkistaa onko nappi pohjassa

    Vector3 lastPos; // Käden viimeisin sijainti
    Quaternion lastRot; // Käden viimeisin rotaatio

    private void Start()
    {
        action.action.Enable(); // Aktivoi napin, jotta sitä voidaan lukea

        // Find the other hand
        foreach (CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        } 
    }

    void Update()
    {
        grabbing = action.action.IsPressed(); // Tarkistetaan onko nappi pohjassa
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
            
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;

            if (grabbedObject)
            {
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here
                Vector3 posDelta = transform.position - lastPos; // Delta-sijainti
                Quaternion rotDelta = transform.rotation * Quaternion.Inverse(lastRot); // Delta-rotaatio (Mikä rotaatio kumoaa rotaation x?)

                // Discordista
                Vector3 vector = grabbedObject.position - transform.position;
                Vector3 rotVector = rotDelta * vector - vector;

                grabbedObject.position += posDelta + rotVector;
                grabbedObject.rotation = rotDelta * grabbedObject.rotation;
            } 
        }
        // If let go of button, release object
        else if (grabbedObject)
            grabbedObject = null;

        // Should save the current position and rotation here
        lastPos = transform.position;
        lastRot = transform.rotation;

    }
        // Kun ohjain koskettaa jotain
    private void OnTriggerEnter(Collider other)
    {
        // TEHTY Make sure to tag grabbable objects with the "grabbable" tag
        // TEHTY You also need to make sure to have colliders for the grabbable objects and the controllers
        // TEHTY Make sure to set the controller colliders as triggers or they will get misplaced
        // TEHTY You also need to add Rigidbody to the controllers for these functions to be triggered
        // TEHTY Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Add(t);
    }
        // Kun objekti poistuu läheltä
    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Remove(t);
    }
}