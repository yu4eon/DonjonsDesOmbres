using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Runtime.InteropServices;

[CreateAssetMenu(menuName = "Sauvegarde", fileName = "Sauvegarde")]
public class SOSauvegarde : ScriptableObject
{
    int _nbEntresMax = 7;
    [SerializeField] List<JoueurScore> _lesJoueursScores = new List<JoueurScore>();
    public List<JoueurScore> lesJoueursScores
    {
        get
        {
            _lesJoueursScores.Sort((a, b) => b.score.CompareTo(a.score));
            if(_lesJoueursScores.Count >= _nbEntresMax)
            {
                _lesJoueursScores.RemoveAt(_lesJoueursScores.Count - 1);
            }
            return _lesJoueursScores;
        }
    }

    [DllImport("__Internal")]
    static extern void SynchroniserWebGL();

    [SerializeField] string _fichier = "scores.TIM";
    public void LireFichier(TMP_InputField inputField)
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
        }
        else
        {
            Debug.LogWarning("Le fichier n'existe pas");
        }
    }
    
    public void EcrireFichier(TMP_InputField inputField)
    {
        string cheminEtFichier = Application.persistentDataPath + "/" + _fichier;
        string contenue = JsonUtility.ToJson(this);
        Debug.Log(contenue);
        File.WriteAllText(cheminEtFichier, contenue);
        if(Application.platform == RuntimePlatform.WebGLPlayer) 
        {
            SynchroniserWebGL();
            Debug.Log("Coucou WebGL");
        }
    }
}