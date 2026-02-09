using UnityEngine;

public class MagnifierFollower : MonoBehaviour
{
    public Transform lens; // Lens-objekti (mesh) (ei koko suurennuslasi!)
    public Transform vrCamera; // XR Origin Main Camera

    [Tooltip("Pieni et‰isyys, ettei kamera j‰‰ linssin sis‰lle")]
    public float forwardOffset = 0.02f; // Offset, jotta kamera ei ole linssin sis‰ll‰

    void LateUpdate()
    {
        if (!lens || !vrCamera) return;

        // 1) Kamera linssin kohdalle (hieman siihen suuntaan minne VR-kamera katsoo)
        Vector3 dir = (lens.position - vrCamera.position).normalized;
        // transform.position = lens.position + dir * forwardOffset;

        // 2) Kamera katsoo samaan suuntaan kuin VR-pelaajan p‰‰
        transform.rotation = Quaternion.LookRotation(dir, vrCamera.up);
    }
}
