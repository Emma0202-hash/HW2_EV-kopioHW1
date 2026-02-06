using UnityEngine;

public class MoonRotation : MonoBehaviour
{   
        public float degreesPerSecond = 2.0f;
    // Update is called once per frame
        void Update()
    {
    // Rotation happens around Y-axis
        transform.Rotate(0, degreesPerSecond * Time.deltaTime, 0);
    }
}

