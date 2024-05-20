using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/// <summary>
/// Tp3 Antoine
/// Auteur du code : Antoine Lachance
/// Commentaires ajoutés par : Antoine Lachance
/// Classe ScriptableObject pour la navigation entre les scènes du jeu.
/// </summary>
[CreateAssetMenu(fileName = "Ma navigation", menuName = "Navigation")]
public class SONavigation : ScriptableObject
{
    [SerializeField] SOPerso _donneesPerso; // Référence aux données du personnage
    [SerializeField] AudioClip _sonBouton; // Son joué lorsqu'un bouton est cliqué

    /// <summary>
    /// Tp3 Antoine
    /// Méthode appelée pour démarrer le jeu.
    /// </summary>
    public void Jouer()
    {
        _donneesPerso.Initialiser(); // Initialise les données du personnage
        AllerSceneSuivante(); // Charge la scène suivante
        JouerSon(); // Joue le son du bouton
    }

    /// <summary>
    /// Tp3 Antoine
    /// Méthode appelée pour sortir de la boutique et passer au niveau suivant.
    /// </summary>
    public void SorirBoutique()
    {
        _donneesPerso.niveau++; // Incrémente le niveau du personnage
        SceneManager.LoadScene("niveau" + _donneesPerso.niveau); // Charge la scène du niveau suivant
        JouerSon(); // Joue le son du bouton
    }

    /// <summary>
    /// Tp3 Antoine
    /// Méthode appelée pour charger la scène suivante dans l'ordre de la build.
    /// </summary>
    public void AllerSceneSuivante()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Charge la scène suivante dans l'ordre de la build
        JouerSon(); // Joue le son du bouton
    }

    /// <summary>
    /// #tp4 Leon
    /// Méthode appelée pour retourner au menu.
    /// </summary>
    public void AllerMenu()
    {
        _donneesPerso.Initialiser(); // Initialise les données du personnage
        SceneManager.LoadScene("SceneTitre");
        JouerSon(); // Joue le son du bouton
    }

    /// <summary>
    /// Tp3 Antoine
    /// Méthode appelée pour charger la scène précédente dans l'ordre de la build.
    /// </summary>
    public void AllerScenePrecedente()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Charge la scène précédente dans l'ordre de la build
        JouerSon(); // Joue le son du bouton
    }

    public void AllerSceneTableauHonneur()
    {
        SceneManager.LoadScene("Honneur");
        JouerSon(); // Joue le son du bouton
    }

    public void AllerSceneGenerique()
    {
        SceneManager.LoadScene("Generique");
        JouerSon(); // Joue le son du bouton
    }

    public void AllerSceneExplication()
    {
        SceneManager.LoadScene("InterfaceExplicative");
        JouerSon(); // Joue le son du bouton
    }

    void JouerSon()
    {
        SoundManager.instance.JouerEffetSonore(_sonBouton); // Joue le son du bouton
    }
}
