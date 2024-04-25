using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// #tp4
/// Auteur du code: Leon Yu
/// Ajout des commentaires: Leon Yu
/// Classe pour la ligne du tableau d'honneur, qui affiche les informations d'un joueur
/// </summary>

public class RowUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nom; // Nom du joueur
    [SerializeField] TextMeshProUGUI _rang; // Rang du joueur
    [SerializeField] TextMeshProUGUI _score; // Score du joueur
    [SerializeField] TableauHonneur _tableauHonneur; // Tableau d'honneur
    [SerializeField] TMP_InputField _champNom; // Champ pour entrer le nom du joueur
    [SerializeField] Button _boutonValider; // Bouton pour valider le nom du joueur

    /// <summary>
    /// Affiche les informations du joueur
    /// </summary>
    /// <param name="joueurScore">Joueur et son score</param>
    /// <param name="index">Index du joueur</param>
    public void AfficherInfo(JoueurScore joueurScore, int index) 
    {
        _rang.text = index.ToString();

        // Si le joueur est le joueur actuel, on affiche le champ pour entrer le nom
        if(joueurScore.estJoueurActuelle)
        {
            _nom.gameObject.SetActive(false);
            _champNom.gameObject.SetActive(true);
            _champNom.text = joueurScore.joueur;
            _boutonValider.gameObject.SetActive(true);
            _score.text = joueurScore.score.ToString();
        }

        // Sinon, on affiche le nom et le score du joueur
        else
        {
            _champNom.gameObject.SetActive(false);
            _boutonValider.gameObject.SetActive(false);
            _nom.text = joueurScore.joueur;
            _score.text = joueurScore.score.ToString();
        }
        
        // Si le rang est 6, on met en gris le nom et le score du joueur et on désactive le champ pour entrer le nom et le bouton pour valider
        if(index == 6)
        {
            _nom.color = Color.gray;
            _score.color = Color.gray;
            _champNom.interactable = false;
            _boutonValider.interactable = false;
            _tableauHonneur.ActiverBoutonMenu();
        }
    }

    /// <summary>
    /// On envoie le nom du joueur pour le sauvegarder
    /// </summary>
    public void ConfirmerNom()
    {
        _tableauHonneur.SauvegarderScore(_champNom.text);
        _nom.gameObject.SetActive(true);
        _nom.text = _champNom.text;
        _champNom.gameObject.SetActive(false);
        _boutonValider.gameObject.SetActive(false);
    }

}
