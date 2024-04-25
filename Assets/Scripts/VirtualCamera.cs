using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MonScript : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        TrouverJoueur();
    }

    void TrouverJoueur()
    {
        // Recherche un GameObject avec le tag "Player" dans la scène
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Définit la cible de la caméra virtuelle sur le GameObject "Player"
            virtualCamera.Follow = player.transform;
        }
        else
        {
            Debug.LogError("Le GameObject avec le tag 'Player' n'a pas été trouvé dans la scène !");
        }
    }
}
