using UnityEngine;

public class LensPlaneBillboard : MonoBehaviour
{
    public Transform lensCenter; // LensCenter tyhjä
    public Transform headCamera; // XR Main Camera

    private void Update()

    {
        if (!lensCenter || !headCamera) return;


        // transform.position = lensCenter.position;  <- linssi tippuu

        // Suunta pään kamerasta linssin keskelle
        Vector3 dir = (lensCenter.position - headCamera.position).normalized;

        // Käännä plane katsomaan pelaajaa (stabiloidaan "rolling")
        transform.rotation = Quaternion.LookRotation(dir, headCamera.up);        
    }
}
