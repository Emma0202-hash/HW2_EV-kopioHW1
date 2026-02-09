using UnityEngine;

public class LensPlaneBillboard : MonoBehaviour
{
    public Transform lensCenter; // LensCenter tyhj‰
    public Transform headCamera; // XR Main Camera

    private void LateUpdate()
    {
        if (!lensCenter || !headCamera) return;

        // Pid‰ plane linssin keskell‰
        transform.position = lensCenter.position;

        // Suunta p‰‰n kamerasta linssin keskelle
        Vector3 dir = (lensCenter.position - headCamera.position).normalized;

        // K‰‰nn‰ plane katsomaan pelaajaa (stabiloidaan "rolling")
        transform.rotation = Quaternion.LookRotation(dir, headCamera.up);        
    }
}
