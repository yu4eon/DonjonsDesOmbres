using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiB : Ennemi
{
    [SerializeField] GameObject _pointAttaque; // Le point d'attaque de l'ennemi
    [SerializeField] ProjectileEnnemi _projectileModele; // Le projectile que l'ennemi tire
    [SerializeField] float _distanceAttaque = 10f; // La distance à laquelle l'ennemi attaque
    [SerializeField] float _delaiAttaque = 2f; // Le délai entre chaque attaque
    bool _peutAttaquer = false; // Si l'ennemi peut attaquer

    protected override void Start()
    {
        base.Start();
        Coroutine coroutine = StartCoroutine(CoroutinePermettreAttaque());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // Si la distance entre le joueur et l'ennemi est inférieure à la distance d'attaque
        if (Vector2.Distance(transform.position, perso.transform.position) < _distanceAttaque)
        {
            if (_peutAttaquer)
            {
                // lance un raycast pour vérifier si le joueur est en ligne de mire

                RaycastHit2D hit = Physics2D.Raycast(transform.position, perso.transform.position - transform.position, _distanceAttaque, LayerMask.GetMask("Tuile"));
                if (hit.collider != null)
                {
                    return;
                }

                Transform cible = perso.transform;
                // On crée un projectile et on change la rotation du projectile pour qu'il est la même direction que le joueur
                ProjectileEnnemi projectile = Instantiate(_projectileModele, transform.position, Quaternion.identity);
                projectile.cible = cible;
                
                _peutAttaquer = false;
                Coroutine coroutine = StartCoroutine(CoroutinePermettreAttaque());

            }
        }
    }

    IEnumerator CoroutinePermettreAttaque()
    {
        yield return new WaitForSeconds(_delaiAttaque);
        _peutAttaquer = true;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distanceAttaque);
    }
}
