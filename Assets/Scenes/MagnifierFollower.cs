using UnityEngine;

public class MagnifierFollower : MonoBehaviour
{
    public Transform lens; // Lens-objekti (ei koko suurennuslasi!)
    public Transform vrCamera; // XR Origin Main Camera

    [Tooltip("Pieni et‰isyys, ettei kamera j‰‰ linssin sis‰lle")]
    public float forwardOffset = 0.02f;

    void LateUpdate()
    {
        if (!lens || !vrCamera) return;
        
        // 1) Kamera linssin kohdalle (hieman siihen suuntaan minne VR-kamera katsoo)
        transform.position = lens.position + vrCamera.forward * forwardOffset;

        // 2) Kamera katsoo samaan suuntaan kuin VR-pelaajan p‰‰
        transform.rotation = Quaternion.LookRotation(vrCamera.forward, vrCamera.up);
    }
}
