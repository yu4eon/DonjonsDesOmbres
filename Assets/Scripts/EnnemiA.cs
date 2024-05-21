using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnnemiA : Ennemi
{
    [SerializeField] float vitesse = 3f;
    [SerializeField] float detectionDistance = 1f;
    [SerializeField] float _distanceSon = 20f; // Distance à laquelle le son de déplacement de l'ennemi est joué
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] AudioClip _sonDeplacement; // Son de déplacement de l'ennemi
    bool deplacementDroite = true;
    bool estEnTrainDeChangerDirection = false;
    Rigidbody2D rb;
    SpriteRenderer sr;

    override protected void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = true;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        DeplacerEnnemi();
    }

    void DeplacerEnnemi()
    {
        Vector2 direction = deplacementDroite ? Vector2.right : Vector2.left;
        Vector2 raycastOrigin = transform.position + Vector3.down * 0.5f;

        RaycastHit2D hitWall = Physics2D.Raycast(raycastOrigin, direction, detectionDistance, obstacleLayer);
        Vector2 raycastDirection = deplacementDroite? new Vector2(-1,-1) : new Vector2(1,-1);
        RaycastHit2D hitGround = Physics2D.Raycast(raycastOrigin, raycastDirection, detectionDistance, groundLayer);
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

    public void JouerSonDeplacement()
    {
        // Joue le son de déplacement de l'ennemi si le joueur est à proximité
        if (Vector2.Distance(transform.position, perso.transform.position) < _distanceSon)
        {
            float fractionDistance = Vector2.Distance(transform.position, perso.transform.position) / _distanceSon;
            float volume = Mathf.Clamp(1 - fractionDistance, 0.1f, 1f);
            Debug.Log("Fraction distance : " + fractionDistance);
            GestAudio.instance.JouerEffetSonore(_sonDeplacement, volume);

        }

    }

    IEnumerator ChangerDirectionCoroutine()
    {
        estEnTrainDeChangerDirection = true;
        yield return new WaitForSeconds(2f); // Attend un court instant avant de réinitialiser la variable
        estEnTrainDeChangerDirection = false;
        yield return null;
    }
}
