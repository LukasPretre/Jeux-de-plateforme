using UnityEngine;

public class BackGround : MonoBehaviour
{
    [Header("Réglages")]
    public Transform cameraTransform;

    [Range(0f, 1f)]
    public float vitesseParallax = 0.9f;

    void LateUpdate()
    {
        transform.position = new Vector3(cameraTransform.position.x * vitesseParallax, transform.position.y, transform.position.z);
    }
}
