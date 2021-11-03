using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed;
    private float horizontalInput;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
