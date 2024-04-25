using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Classe responsable de la configuration de la caméra virtuelle pour suivre le joueur.
public class MonScript : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; // Référence à la caméra virtuelle Cinemachine.

    // Fonction appelée au démarrage de l'objet.
    void Start()
    {
        // Récupère le composant CinemachineVirtualCamera attaché à cet objet.
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        // Cherche le joueur dans la scène et configure la caméra pour le suivre.
        TrouverJoueur();
    }

    // Fonction pour trouver le joueur dans la scène et configurer la caméra pour le suivre.
    void TrouverJoueur()
    {
        // Recherche un GameObject avec le tag "Player" dans la scène.
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Définit la cible de la caméra virtuelle sur le GameObject "Player".
            virtualCamera.Follow = player.transform;
        }
        else
        {
            // Affiche un message d'erreur si aucun GameObject avec le tag "Player" n'est trouvé dans la scène.
            Debug.LogError("Le GameObject avec le tag 'Player' n'a pas été trouvé dans la scène !");
        }
    }
}
