using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Ma navigation", menuName = "Navigation")]
public class SONavigation : ScriptableObject
{
    [SerializeField] SOPerso _donneesPerso;
    public void Jouer()
    {
        _donneesPerso.Initialiser();
        AllerSceneSuivante();
    }

    public void SorirBoutique()
    {
        _donneesPerso.niveau++;
        SceneManager.LoadScene("niveau" + _donneesPerso.niveau);
    }

    public void AllerSceneSuivante()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void AllerScenePrecedente()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
