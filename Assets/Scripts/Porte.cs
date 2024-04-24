using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #Tp3
/// Auteur du code : Antoine Lachance
/// Commentaires ajoutés par : Antoine Lachance
/// Classe qui gère la porte
/// </summary>
public class Porte : MonoBehaviour
{
    [SerializeField] SONavigation _navigation; // Référence au script de navigation entre les scènes
    [SerializeField] Sprite[] _sprites; // Tableau des sprites de la porte (ouverte et fermée)
    [SerializeField] SOPerso _donneesPerso; // Données du personnage (ScriptableObject)
    [SerializeField] AudioClip _sonPorte;

    SpriteRenderer _sr; // Référence au composant SpriteRenderer de la porte
    static public bool aCle = false; // Booléen indiquant si la clé a été trouvée

    /// <summary>
    /// Start est appelée lors du premier frame où le script est activé, avant n'importe quel Update.
    /// </summary>
    void Start()
    {
        aCle = false; // Réinitialisation de la variable aCle
        _sr = GetComponent<SpriteRenderer>(); // Obtention du composant SpriteRenderer attaché à cet objet
        _sr.sprite = _sprites[0]; // Définition du sprite initial de la porte (fermée)
    }

    /// <summary>
    /// OnTriggerEnter2D est appelée lorsque quelque chose entre en collision avec le collider de cet objet.
    /// </summary>
    /// <param name="other">Le Collider2D entrant en collision avec cet objet.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (aCle && other.CompareTag("Player")) // Si la clé a été trouvée et le joueur entre en collision avec la porte
        {
            _sr.sprite = _sprites[1]; // Changement du sprite de la porte (ouverte)
            SoundManager.PlaySound(_sonPorte); // Joue le son de la porte qui s'ouvre
            StartCoroutine(ChangerScene()); // Appel de la coroutine pour changer de scène après un délai
        }
    }

    /// <summary>
    /// Coroutine qui change de scène après 2 secondes et vide l'inventaire du joueur.
    /// </summary>
    IEnumerator ChangerScene()
    {
        yield return new WaitForSeconds(2f); // Attente de 2 secondes
        _donneesPerso.ViderInventaire(); // Vidage de l'inventaire du joueur
        _navigation.AllerSceneSuivante(); // Appel de la fonction pour aller à la scène suivante dans le script de navigation
    }
}
