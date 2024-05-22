using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Runtime.InteropServices;


/// <summary>
/// #tp4 Léon
/// Classe qui gère les informations des joueurs et les sauvegarde dans un fichier.
/// auteur : Léon Yu
/// commentaires : Léon Yu
/// </summary>
[CreateAssetMenu(menuName = "Sauvegarde", fileName = "Sauvegarde")]
public class SOSauvegarde : ScriptableObject
{
    [SerializeField] List<JoueurScore> _lesJoueursScores = new List<JoueurScore>();
    public List<JoueurScore> lesJoueursScores
    {
        get => _lesJoueursScores;
        set => _lesJoueursScores = value;
    }

    [DllImport("__Internal")]
    static extern void SynchroniserWebGL();

    [SerializeField] string _fichier = "sauvegarde.TIM";

    /// <summary>
    /// Lit les informations des joueurs dans un fichier
    /// </summary>
    public string LireFichier()
    {
        string cheminEtFichier = Application.persistentDataPath + "/" + _fichier;
        Debug.Log(cheminEtFichier);
        if (File.Exists(cheminEtFichier))
        {
            string contenue = File.ReadAllText(cheminEtFichier);
            JsonUtility.FromJsonOverwrite(contenue, this);
            Debug.Log(contenue);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
            return contenue;
        }
        else
        {
            Debug.LogWarning("Le fichier n'existe pas");
            return null;
        }
    }


    /// <summary>
    /// Écrit les informations des joueurs dans un fichier
    /// </summary>
    public void EcrireFichier()
    {
        // foreach (JoueurScore joueurScore in lesJoueursScores)
        // {
        //     Debug.Log(string.Join(" ", joueurScore.joueur, joueurScore.score));
        // }

        string cheminEtFichier = Application.persistentDataPath + "/" + _fichier;
        string contenue = JsonUtility.ToJson(this);
        Debug.Log(contenue);
        File.WriteAllText(cheminEtFichier, contenue);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SynchroniserWebGL();
            Debug.Log("Coucou WebGL");
        }
    }

    /// <summary>
    /// Méthode qui permet de remettre à zéro les informations des joueurs
    /// et de créer des nouveaux joueurs de test
    /// </summary>
    public void Reinitialiser()
    {
        _lesJoueursScores.Clear(); // Supprime tous les joueurs
        _lesJoueursScores.Add(new JoueurScore { joueur = "Bob", score = 7000, estJoueurActuelle = false }); // Crée un joueur de test
        _lesJoueursScores.Add(new JoueurScore { joueur = "Alice", score = 5000, estJoueurActuelle = false }); // Crée un joueur de test
        EcrireFichier(); // Ecrit les informations dans le fichier
    }

}