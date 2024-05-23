using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Auteur du code : Antoine Lachance
/// Commetaires ajoutés par : Antoine Lachance
/// Classe pour le déplacement et les fonctionnalités de l'ennemiA
/// </summary>
public class EnnemiA : Ennemi
{
    [SerializeField] float vitesse = 6f; // Vitesse de déplacement de l'ennemi
    [SerializeField] float detectionDistance = 1f; // Distance de détection des obstacles et du sol
    [SerializeField] LayerMask obstacleLayer; // Layer des obstacles à détecter
    [SerializeField] LayerMask groundLayer; // Layer du sol à détecter
    
    bool deplacementDroite = true; // Indique si l'ennemi se déplace vers la droite
    bool estEnTrainDeChangerDirection = false; // Indique si l'ennemi est en train de changer de direction
    Rigidbody2D rb; // Référence au composant Rigidbody2D de l'ennemi
    SpriteRenderer sr; // Référence au composant SpriteRenderer de l'ennemi

    // Méthode appelée au démarrage du script
    override protected void Start()
    {
        base.Start(); // Appelle la méthode Start de la classe de base
        rb = GetComponent<Rigidbody2D>(); // Obtient le composant Rigidbody2D attaché à l'ennemi
        sr = GetComponent<SpriteRenderer>(); // Obtient le composant SpriteRenderer attaché à l'ennemi
        sr.flipX = true; // Inverse le sprite de l'ennemi pour qu'il regarde à droite
        _lumiere.GetComponent<Transform>().transform.Rotate(0, 180, 0); // Inversion initiale de la lumière de l'ennemi
    }

    /// <summary>
    /// Cette fonction est appelée à chaque frame avec un framerate fixe, si le MonoBehaviour est activé.
    /// </summary>
    override protected void FixedUpdate()
    {
        base.FixedUpdate(); // Appelle la méthode FixedUpdate de la classe de base
        DeplacerEnnemi(); // Appelle la méthode pour déplacer l'ennemi
    }

    // Méthode pour gérer le déplacement de l'ennemi
    void DeplacerEnnemi()
    {
        Vector2 direction = deplacementDroite ? Vector2.right : Vector2.left; // Détermine la direction du déplacement
        Vector2 raycastOrigin = transform.position + Vector3.down * 0.5f; // Point de départ du raycast, légèrement en dessous de l'ennemi

        // Effectue un raycast pour détecter les obstacles devant l'ennemi
        RaycastHit2D hitWall = Physics2D.Raycast(raycastOrigin, direction, detectionDistance, obstacleLayer);
        // Détermine la direction du raycast pour détecter le sol
        Vector2 raycastDirection = deplacementDroite ? new Vector2(-1,-1) : new Vector2(1,-1);
        // Effectue un raycast pour détecter le sol devant l'ennemi
        RaycastHit2D hitGround = Physics2D.Raycast(raycastOrigin, raycastDirection, detectionDistance, groundLayer);

        // Si un obstacle est détecté ou si le sol n'est pas détecté ou si la vitesse de l'ennemi est nulle
        if (hitWall || !hitGround || rb.velocity == Vector2.zero)
        {
            // Si l'ennemi n'est pas déjà en train de changer de direction
            if (estEnTrainDeChangerDirection == false)
            {
                sr.flipX = !sr.flipX; // Inverse le sprite de l'ennemi
                _lumiere.GetComponent<Transform>().transform.Rotate(0, 180, 0); // Inverse la lumière de l'ennemi
                deplacementDroite = !deplacementDroite; // Change la direction du déplacement
                StartCoroutine(ChangerDirectionCoroutine()); // Lance la coroutine pour gérer le changement de direction
            }
        }

        rb.velocity = direction * vitesse; // Met à jour la vitesse de l'ennemi pour le déplacer
    }

    // Coroutine pour gérer le délai avant que l'ennemi puisse changer de direction à nouveau
    IEnumerator ChangerDirectionCoroutine()
    {
        estEnTrainDeChangerDirection = true; // Indique que l'ennemi est en train de changer de direction
        yield return new WaitForSeconds(2f); // Attend 2 secondes
        estEnTrainDeChangerDirection = false; // Réinitialise la variable
        yield return null; // Termine la coroutine
    }
}
