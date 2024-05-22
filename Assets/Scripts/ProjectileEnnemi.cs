using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;


/// <summary>
/// #synthese
/// Classe qui gère les projectiles des ennemis
/// Auteur : Leon Yu
/// Commentaires : Leon Yu
/// </summary>
public class ProjectileEnnemi : MonoBehaviour
{
    Transform _cible; // La cible du projectile
    public Transform cible { get => _cible; set => _cible = value; } // Propriété pour obtenir et définir la cible du projectile
    [SerializeField] float _vitesse = 5f; // La vitesse du projectile
    [SerializeField] int _degats = 30; // Les dégâts du projectile
    [SerializeField] float _dureeVie = 5f; // La durée de vie du projectile
    [SerializeField] ParticleSystem _particules; // Les particules du projectile
    [SerializeField] ParticleSystem _particulesImpact; // Les particules d'impact du projectile
    ParticleSystem.ShapeModule _shape; // Le module d'émission des particules

    void Awake()
    {
        _shape = _particules.shape;
    }

    void Start()
    {
        StartCoroutine(CoroutineDestruction());
    }
    void FixedUpdate()
    {
        if (_cible != null)
        {
            // On déplace le projectile vers la cible
            transform.position = Vector2.MoveTowards(transform.position, _cible.position, _vitesse * Time.deltaTime);

            //Change la rotation de l'emmision de particules pour qu'elle soit dans à l'opposé de la direction du projectile
            Vector2 direction = _cible.position - transform.position;
            direction.y = -direction.y;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += 180;
            _shape.rotation = new Vector3(0, angle, 0); ;

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.CompareTag("Sol"));
        if (collision.GetComponent<Perso>() != null) // Si le projectile touche le joueur, on le détruit
        {
            LayerMask maskInvincible = LayerMask.NameToLayer("JoueurInvincible"); 
            Debug.Log(collision.gameObject.layer + " " + maskInvincible.value);
            if (collision.gameObject.layer == maskInvincible.value) // Si le joueur est invincible, on ne lui inflige pas de dégâts
            {
                Detruire();
                return;
            }

            Perso perso = collision.GetComponent<Perso>();
            perso.SubirDegats(_degats);
            Detruire();
            // Si le projectile touche le joueur, on le détruit
        }

        // Initalement, on voulait que le projectile soit détruit lorsqu'il touche le sol
        // Cependant, la collision avec le sol ne fonctionnait pas
        // else if (collision.CompareTag("Sol"))
        // { 
        //     Debug.Log("Collision avec le sol");
        //     // Si le projectile touche une tuile, on le détruit
        //     Detruire();
        // }
    }

    /// <summary>
    /// Coroutine qui détruit le projectile après un certain temps
    /// </summary>
    IEnumerator CoroutineDestruction()
    {
        yield return new WaitForSeconds(_dureeVie);
        Detruire();
    }

    /// <summary>
    /// Méthode qui détruit le projectile et instancie les particules d'impact
    /// </summary>
    void Detruire()
    {
        Instantiate(_particulesImpact, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
