using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Runtime.InteropServices;

[CreateAssetMenu(menuName = "Sauvegarde", fileName = "Sauvegarde")]
public class SOSauvegarde : ScriptableObject
{

    
    [SerializeField] int _nb = 10;
    [SerializeField] string[] _tPays = new string[] { "Canada", "Mexique" };

    [DllImport("__Internal")]
    static extern void SynchroniserWebGL();

    [SerializeField] string _fichier = "demo.TIM";
    public void LireFichier(TMP_InputField inputField)
    {
        string cheminEtFichier = Application.persistentDataPath + "/" + _fichier;
        Debug.Log(cheminEtFichier);
        if (File.Exists(cheminEtFichier))
        {
            string contenue = File.ReadAllText(cheminEtFichier);
            JsonUtility.FromJsonOverwrite(contenue, this);
            Debug.Log(contenue);
            inputField.text = _nb + "";
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
        if(int.TryParse(inputField.text, out int nb)) _nb = nb;
        else
        {
            Debug.LogWarning("Le nombre n'est pas valide");
            return;
        }
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