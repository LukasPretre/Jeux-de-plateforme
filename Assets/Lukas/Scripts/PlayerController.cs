using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Déplacements")]
    public float moveSpeed = 8f;
    public float jumpForce = 15f;
    public float rotationSpeed = 450f;

    [Header("Effets Visuels")]
    public ParticleSystem effetSol;

    [Header("Références")]
    public Transform visuelCube;

    [Header("Détection du sol")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        // On récupére le composant physique du joueur au lancement du jeu
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // On vérifie si le joueur touche le sol
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(effetSol != false)
        {
            var emission = effetSol.emission;
            emission.enabled = isGrounded;
        }

        // Le Saut (Appui sur Espace ou Clic gauche de la souris)
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        GestionRotationVisuelle();

    }

    void FixedUpdate()
    {
        // On force le cube à aller vers la droite constamment
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si l'objet avec lequel on vient d'entrer en collision a le tag "Danger"
        if (collision.gameObject.CompareTag("Danger"))
        {
            // On demande à Unity de recharger la scène actuelle
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void GestionRotationVisuelle()
    {
        if (isGrounded)
        {
            // SI ON EST AU SOL : On force le VISUEL à se remettre "à plat".
            float currentAngle = visuelCube.eulerAngles.z;
            float perfectAngle = Mathf.Round(currentAngle / 90) * 90;
            visuelCube.rotation = Quaternion.Euler(0, 0, perfectAngle);
        }
        else
        {
            // SI ON EST EN L'AIR : On fait tourner UNIQUEMENT LE VISUEL.
            visuelCube.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }
    }
}