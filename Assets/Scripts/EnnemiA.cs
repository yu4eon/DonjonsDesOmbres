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
    SpriteRenderer sr;

    new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = true;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        DeplacerEnnemi();
    }

    void DeplacerEnnemi()
    {
        GestAudio.instance.JouerEffetSonore(sonEnnemi[0]);
        Vector2 direction = deplacementDroite ? Vector2.right : Vector2.left;
        Vector2 raycastOrigin = transform.position + Vector3.down * 0.5f;

        RaycastHit2D hitWall = Physics2D.Raycast(raycastOrigin, direction, detectionDistance, obstacleLayer);
        RaycastHit2D hitGround = Physics2D.Raycast(raycastOrigin, new Vector2(1,-1), detectionDistance, groundLayer);
        if (hitWall || !hitGround || rb.velocity == Vector2.zero)
        {
            if (estEnTrainDeChangerDirection == false)
            {
                sr.flipX =!sr.flipX;
                deplacementDroite =!deplacementDroite;
                StartCoroutine(ChangerDirectionCoroutine());   
            }
        }
        rb.velocity = direction * vitesse;
    }

    IEnumerator ChangerDirectionCoroutine()
    {
        estEnTrainDeChangerDirection = true;
        yield return new WaitForSeconds(2f); // Attend un court instant avant de réinitialiser la variable
        estEnTrainDeChangerDirection = false;
        yield return null;
    }
}
