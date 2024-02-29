using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe qui controle les déplacements du personnage
/// Auteurs du code: Léon Yu et Antoine Lachance
/// Auteur des commentaires: Léon Yu
/// </summary>
public class Perso : DetecteurSol
{
    [SerializeField] float _vitesse = 10f; //Vitesse à laquelle le personnage se déplace
    [SerializeField] float _forceSaut = 120f; //L'amplitude du saut
    [SerializeField] int _nbFramesMax = 10; //Nombre de frames maximum pendant lesquelles le joueur peut sauter
    [SerializeField] bool _possedeDoublesSauts = false; //Si le personnage possède le pouvoir de double saut
    float _axeHorizontal; //Axe horizontal du personnage
    int _nbFramesRestants = 0; //Nombre de frames restantes pendant lesquelles le joueur peut sauter
    bool _veutSauter; //Si le joueur veut sauter
    bool _peutDoubleSauter = false; //Si le joueur peut faire un double saut
    bool _auDeuxiemeSaut; //Si le joueur est au deuxième saut
    Rigidbody2D _rb; //Rigidbody du personnage
    SpriteRenderer _sr; //SpriteRenderer du personnage
    
    /// <summary>
    /// Méthode qui est appelée lorsque le script est chargé
    /// </summary>
    void Awake() 
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Méthode qui est appelée à chaque frame à un rhythme fixe
    /// </summary>
    override protected void FixedUpdate()
    {
        
        base.FixedUpdate(); //Appelle la méthode FixedUpdate de la classe mère
        _rb.velocity = new Vector2(_axeHorizontal * _vitesse, _rb.velocity.y);
        Debug.Log(_rb.velocity.y);
        if(_veutSauter) //Si le joueur veut sauter
        {
            if(_peutDoubleSauter && _auDeuxiemeSaut) //Si le joueur peut faire un double saut
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _nbFramesRestants = _nbFramesMax;
                _peutDoubleSauter = false;
            }
            Sauter();
        }
        
        else if(_estAuSol) //Si le joueur est au sol
        {
            _auDeuxiemeSaut = false;
            _nbFramesRestants = _nbFramesMax;
            if(!_possedeDoublesSauts) return; //Si le joueur ne possède pas le pouvoir de double saut
            _peutDoubleSauter = true;
        }
        else //Si le joueur n'est pas au sol et ne maintient pas le bouton de saut
        {
            _auDeuxiemeSaut = true;
            _nbFramesRestants = 0;
        }
    }
    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur les touches de déplacement
    /// </summary>
    /// <param name="value">La valeur retourné par le Input system</param>
    void OnMove(InputValue value)
    {
        _axeHorizontal = value.Get<Vector2>().x;
        if (_axeHorizontal < 0) //Si le joueur se déplace vers la gauche
        {
            _sr.flipX = true;
        }
        else if (_axeHorizontal > 0) //Si le joueur se déplace vers la droite
        {
            _sr.flipX = false;
        }
    }
    /// <summary>
    /// Méthode qui est appelée lorsque le joueur appuie sur le bouton de saut
    /// </summary>
    /// <param name="value">La valeur retourné par le Input system</param>
    void OnJump(InputValue value) //Si le joueur appuie sur le bouton de saut
    {
        _veutSauter = value.isPressed;
    }
    /// <summary>
    /// Méthode qui permet au joueur de sauter
    /// </summary>
    void Sauter()
    {
        float fractionForce = (float)_nbFramesRestants / _nbFramesMax;
        Vector2 vecteurForce = Vector2.up * _forceSaut * fractionForce;
        _rb.AddForce(vecteurForce);
        if(_nbFramesRestants > 0) //S'il reste des frames pendant lesquelles le joueur peut sauter
        {
            _nbFramesRestants--;
        }
        else if(_auDeuxiemeSaut && _nbFramesRestants == 0) //Si le joueur est au deuxième saut et qu'il ne peut plus sauter
        {
            _peutDoubleSauter = false;
        } 
    }
}