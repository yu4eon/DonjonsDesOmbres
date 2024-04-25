using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Runtime.InteropServices;

// Classe représentant un objet scriptable pour la sauvegarde de données.
[CreateAssetMenu(menuName = "Sauvegarde", fileName = "Sauvegarde")]
public class SOSauvegarde : ScriptableObject
{
    [SerializeField] int _nb = 10; // Nombre par défaut.
    [SerializeField] string[] _tPays = new string[] { "Canada", "Mexique" }; // Tableau de pays.

    // Importe une fonction native pour synchroniser sur WebGL.
    [DllImport("__Internal")]
    static extern void SynchroniserWebGL();

    [SerializeField] string _fichier = "demo.TIM"; // Nom du fichier de sauvegarde.

    // Fonction pour lire le contenu du fichier de sauvegarde et remplir les champs.
    public void LireFichier(TMP_InputField inputField)
    {
        // Chemin et nom complet du fichier.
        string cheminEtFichier = Application.persistentDataPath + "/" + _fichier;
        Debug.Log(cheminEtFichier);
        // Si le fichier existe.
        if (File.Exists(cheminEtFichier))
        {
            // Lit le contenu du fichier.
            string contenu = File.ReadAllText(cheminEtFichier);
            // Désérialise le contenu JSON dans cette instance de scriptable object.
            JsonUtility.FromJsonOverwrite(contenu, this);
            Debug.Log(contenu);
            // Met à jour le champ de l'interface utilisateur avec la valeur du nombre.
            inputField.text = _nb + "";
            
            #if UNITY_EDITOR
            // Marque l'objet comme modifié dans l'éditeur pour sauvegarder les modifications.
            UnityEditor.EditorUtility.SetDirty(this);
            // Sauvegarde les modifications dans les assets.
            UnityEditor.AssetDatabase.SaveAssets();
            #endif
        }
        else
        {
            Debug.LogWarning("Le fichier n'existe pas");
        }
    }
    
    // Fonction pour écrire les données actuelles dans le fichier de sauvegarde.
    public void EcrireFichier(TMP_InputField inputField)
    {
        // Si la conversion du texte en entier réussit, met à jour la valeur du nombre.
        if(int.TryParse(inputField.text, out int nb)) _nb = nb;
        else
        {
            Debug.LogWarning("Le nombre n'est pas valide");
            return;
        }
        // Chemin et nom complet du fichier.
        string cheminEtFichier = Application.persistentDataPath + "/" + _fichier;
        // Convertit cette instance de scriptable object en JSON.
        string contenu = JsonUtility.ToJson(this);
        Debug.Log(contenu);
        // Écrit le contenu JSON dans le fichier.
        File.WriteAllText(cheminEtFichier, contenu);
        // Si l'application s'exécute sur WebGL, synchronise les fichiers.
        if(Application.platform == RuntimePlatform.WebGLPlayer) 
        {
            SynchroniserWebGL();
            Debug.Log("Coucou WebGL");
        }
    }
}
