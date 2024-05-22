/// <summary>
/// #tp4 Léon
/// Classe qui Definit une variable de type JoueurScore
/// qui contient le nom du joueur, son score et si c'est le joueur actuel
/// utilisé pour la sauvegarde des scores
/// </summary>

[System.Serializable]
public class JoueurScore
{
    public string joueur;
    public int score;
    public bool estJoueurActuelle;
}