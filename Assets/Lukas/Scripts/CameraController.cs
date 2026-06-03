using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float offsetX = 5f;

    // Update is called once per frame
    void LateUpdate()
    {
        // On modifie la position de la caméra
        // Elle prend le "X" (gauche/droite) du joueur + le décalage
        // Elle garde son propre "Y" (haut/bas) et son "Z" (profondeur)
        transform.position = new Vector3(player.position.x + offsetX, transform.position.y, transform.position.z);
    }
}
