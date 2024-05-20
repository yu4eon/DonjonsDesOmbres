using System.Collections;
using UnityEngine;

public class EnnemiA : Ennemi
{
    [SerializeField] float vitesse = 3f;
    [SerializeField] float detectionDistance = 1f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask groundLayer;

    bool deplacementDroite = true;
    bool estEnTrainDeChangerDirection = false;
    Rigidbody2D rb;

    override protected void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DeplacerEnnemiRoutine());
    }

    IEnumerator DeplacerEnnemiRoutine()
    {
        while (true)
        {
            DeplacerEnnemi();
            yield return null;
        }
    }

    void DeplacerEnnemi()
    {
        if (estEnTrainDeChangerDirection)
        {
            return; // Si en train de changer de direction, ne fait rien
        }

        Vector2 direction = deplacementDroite ? Vector2.right : Vector2.left;
        Vector2 raycastOrigin = transform.position + Vector3.down * 0.5f;

        RaycastHit2D hitWall = Physics2D.Raycast(raycastOrigin, direction, detectionDistance, obstacleLayer);
        RaycastHit2D hitGround = Physics2D.Raycast(raycastOrigin, new Vector2(1,1), detectionDistance, groundLayer);

        if (hitWall || !hitGround)
        {
            rb.velocity = Vector2.zero;
            if (!estEnTrainDeChangerDirection)
            {
                StartCoroutine(ChangerDirectionCoroutine());
            }
        }

        rb.velocity = direction * vitesse;
    }

    IEnumerator ChangerDirectionCoroutine()
    {
        deplacementDroite = !deplacementDroite;
        Vector3 nouvelleEchelle = transform.localScale;
        nouvelleEchelle.x *= -1;
        transform.localScale = nouvelleEchelle;
        estEnTrainDeChangerDirection = true;

        yield return new WaitForSeconds(4f); // Attend un court instant avant de réinitialiser la variable
        estEnTrainDeChangerDirection = false;
    }
}
