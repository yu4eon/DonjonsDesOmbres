using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiB : Ennemi
{
    [SerializeField] GameObject _pointAttaque; // Le point d'attaque de l'ennemi
    [SerializeField] ProjectileEnnemi _projectileModele; // Le modèle du projectile que l'ennemi tire
    [SerializeField] float _distanceAttaque = 10f; // La distance à laquelle l'ennemi peut attaquer
    [SerializeField] float _delaiAttaque = 2f; // Le délai entre chaque attaque
    bool _peutAttaquer = false; // Indique si l'ennemi peut attaquer
    Vector3 _scaleLumiere;

    protected override void Start()
    {
        base.Start(); // Appelle la méthode Start de la classe de base
        Coroutine coroutine = StartCoroutine(CoroutinePermettreAttaque()); // Lance la coroutine permettant à l'ennemi d'attaquer après un délai initial
        _scaleLumiere = _lumiere.GetComponent<Transform>().transform.localScale; // Obtient l'échelle de la lumière de l'ennemi
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); // Appelle la méthode FixedUpdate de la classe de base

        // Si la distance entre le joueur et l'ennemi est inférieure à la distance d'attaque
        if (Vector2.Distance(transform.position, perso.transform.position) < _distanceAttaque)
        {
            if (_peutAttaquer) // Si l'ennemi peut attaquer
            {
                // Lance un raycast pour vérifier si le joueur est en ligne de mire
                RaycastHit2D hit = Physics2D.Raycast(transform.position, perso.transform.position - transform.position, _distanceAttaque, LayerMask.GetMask("Tuile"));
                if (hit.collider != null) // Si le raycast touche un obstacle (par exemple, une tuile)
                {
                    return; // Ne pas attaquer si le joueur n'est pas en ligne de mire
                }

                Transform cible = perso.transform; // Référence au transform du joueur

                // Crée un projectile et change la rotation du projectile pour qu'il pointe vers le joueur
                ProjectileEnnemi projectile = Instantiate(_projectileModele, transform.position, Quaternion.identity);
                projectile.cible = cible; // Définit la cible du projectile
                
                _peutAttaquer = false; // Désactive l'attaque jusqu'à la fin du délai
                Coroutine coroutine = StartCoroutine(CoroutinePermettreAttaque()); // Relance la coroutine pour réactiver l'attaque après un délai
            }
        }
        
        Vector3 scaleLumiereInverse = new Vector3(-_scaleLumiere.x, _scaleLumiere.y, _scaleLumiere.z); // Inverse l'échelle de la lumière de l'ennemi
        // Flip l'ennemi pour qu'il regarde vers le joueur
        if (perso.transform.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = false;
            _lumiere.GetComponent<Transform>().transform.localScale = new Vector3(Mathf.Abs(_scaleLumiere.x), _scaleLumiere.y, _scaleLumiere.z); // Inverse la lumière de l'ennemi
        }
        else
        {
            _spriteRenderer.flipX = true;
            _lumiere.GetComponent<Transform>().transform.localScale = scaleLumiereInverse; // Inverse la lumière de l'ennemi
        }
    }

    // Coroutine pour permettre à l'ennemi d'attaquer après un délai
    IEnumerator CoroutinePermettreAttaque()
    {
        yield return new WaitForSeconds(_delaiAttaque); // Attend le délai spécifié avant de réactiver l'attaque
        _peutAttaquer = true; // Réactive l'attaque
    }

    // Méthode pour dessiner des gizmos dans l'éditeur pour visualiser la distance d'attaque de l'ennemi
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Définit la couleur des gizmos en rouge
        Gizmos.DrawWireSphere(transform.position, _distanceAttaque); // Dessine une sphère filaire rouge autour de l'ennemi pour indiquer la portée d'attaque
    }
}
