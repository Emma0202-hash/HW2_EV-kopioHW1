using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)

    CustomGrab otherHand = null; // Tallennetaan toisen ohjaimen skripti, jotta kädet voivat "jakaa" objektin
    public List<Transform> nearObjects = new List<Transform>(); // Lista lähellä olevista objekteista
    public Transform grabbedObject = null; // Tällä hetkellä kiinni oleva objekti, jos on kädessä
    public InputActionReference action; // Unityn Input System-toiminto
    bool grabbing = false; // Tarkistaa onko nappi pohjassa

    // Edellisen framen sijainti ja rotaatio
    Vector3 lastPos;
    Quaternion lastRot;

    // Apumuuttuja, tarkistaa onko tämä käsi tarttumassa (kahden käden logiikkaa varten)
    public bool IsGrabbing => grabbedObject != null;

    private void Start()
    {
        action.action.Enable(); // Aktivoi napin, jotta sitä voidaan lukea

        // Etsitään toinen käsi -> etsii kaikki CustomGrab-skriptit parentin alta
        foreach (CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }

        // Tallennetaan käden alkuasento. Voidaan verrata nykyistä sijaintia tähän
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    void Update()
    {
        // Lasketaan delta-liikkeet
        Vector3 deltaPos = transform.position - lastPos;
        Quaternion deltaRot = transform.rotation * Quaternion.Inverse(lastRot);

        // Tarkistetaan onko nappi pohjassa
        grabbing = action.action.IsPressed();

        if (grabbing)
        {
            // Jos toinen käsi pitää jo objektia -> tartu samaan objektiin
            if (!grabbedObject)
            {
             // grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;
             if (otherHand.IsGrabbing)
             {
                 grabbedObject = otherHand.grabbedObject;
             }
             // Muuten tartu lähimpään objektiin
             else if (nearObjects.Count > 0)
             {
                 grabbedObject = nearObjects[0];
             }
          }
            // Jos jokin objekti on valittu
            if (grabbedObject)
            {
                // --- YHDEN KÄDEN LOGIIKKA ---
                if (!otherHand.IsGrabbing || otherHand.grabbedObject != grabbedObject)
                {
                    ApplyOneHand(deltaPos, deltaRot);
                }
                // --- KAHDEN KÄDEN LOGIIKKA ---
                else
                {
                    ApplyTwoHands(deltaPos, deltaRot);
                }
            }
        }
        // Jos nappi vapautetaan → päästetään irti
        else if (grabbedObject)
        {
            grabbedObject = null;
        }

        // Tallennetaan nykyinen sijainti ja rotaatio seuraavaa framea varten
        lastPos = transform.position;
        lastRot = transform.rotation;
    }

    // YHDEN KÄDEN DELTA-LIIKE
    void ApplyOneHand(Vector3 deltaPos, Quaternion deltaRot)
    {
        // 1. Objekti liikkuu saman verran kuin käsi liikkuu (translation)
        grabbedObject.position += deltaPos;

        // 2. Objekti pyörii saman verran kuin käsi pyörii (rotation causes translation)
        Vector3 offset = grabbedObject.position - transform.position;
        offset = deltaRot * offset;
        grabbedObject.position = transform.position + offset;

        // 3. Objekti kiertää kättä, jos käsi pyörii = orbit-liike
        grabbedObject.rotation = deltaRot * grabbedObject.rotation;
    }

    // KAHDEN KÄDEN DELTA-LIIKE
    void ApplyTwoHands(Vector3 myDeltaPos, Quaternion myDeltaRot)
    {
        // Toisen käden delta-liike
        Vector3 otherDeltaPos = otherHand.transform.position - otherHand.lastPos;
        Quaternion otherDeltaRot = otherHand.transform.rotation * Quaternion.Inverse(otherHand.lastRot);

        // Yhdistetään liikkeet
        Vector3 combinedPos = myDeltaPos + otherDeltaPos;
        Quaternion combinedRot = myDeltaRot * otherDeltaRot;

        // 1. Yhdistetty translation
        grabbedObject.position += combinedPos;

        // 2. Orbit-liike (rotaatio aiheuttaa liikettä)
        Vector3 offset = grabbedObject.position - transform.position;
        offset = combinedRot * offset;
        grabbedObject.position = transform.position + offset;

        // 3. Yhdistetty rotaatio
        grabbedObject.rotation = combinedRot * grabbedObject.rotation;
    }

    // Kun ohjain koskettaa jotain
    private void OnTriggerEnter(Collider other)
    {
        // TEHTY Make sure to tag grabbable objects with the "grabbable" tag
        // TEHTY You also need to make sure to have colliders for the grabbable objects and the controllers
            // Magnifier Box Collider
            // Ohjaimet Sphere Collider
        // TEHTY Make sure to set the controller colliders as triggers or they will get misplaced
        // TEHTY You also need to add Rigidbody to the controllers for these functions to be triggered
        // TEHTY Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Debug.Log("Trigger entered: " + other.name);
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
