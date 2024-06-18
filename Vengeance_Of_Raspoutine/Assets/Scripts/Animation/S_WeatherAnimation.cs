using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_WeatherAnimation : MonoBehaviour
{
    public static S_WeatherAnimation Instance;

    [SerializeField] private TextMeshProUGUI _playerWeatherText;
    [SerializeField] private S_WeatherEvent _currentEvent;
    [SerializeField] private Animator _animatorWeather;
    [SerializeField] private Animator _animatorPlayerTurn;
    [SerializeField] private GameObject _animPlayerCanvas;

    [HideInInspector] public bool hasPlayedPlayer = false;
    [HideInInspector] public bool hasPlayedWeather = false;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    private void Start()
    {
        _animPlayerCanvas.SetActive(false);
    }

    void Update()
    {
        PlayAnimAfterTheOther();
    }

    public void PlayWeatherAnimation()
    {
        _playerWeatherText.text = "Weather : " + _currentEvent.ManageEvent;
        _animatorWeather.Play(0, 0, 0);
    }

    public void PlayAnimAfterTheOther()
    {
        if (!hasPlayedWeather)
        {
            _animatorWeather.Play("WeatherAnimation");
            hasPlayedWeather = true;
        }

        // Vérifier si la première animation est terminée et jouer la deuxième
        if (hasPlayedWeather && !hasPlayedPlayer && _animatorWeather.GetCurrentAnimatorStateInfo(0).IsName("WeatherAnimation")&& _animatorWeather.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !_animatorWeather.IsInTransition(0))
        {
            _animPlayerCanvas.SetActive(true);
            _animatorPlayerTurn.Play("PlayerTurnAnimation");
            hasPlayedPlayer = true;
        }

        // Vérifier si la deuxième animation est terminée et désactiver l'Animator
        if (hasPlayedPlayer && _animatorPlayerTurn.GetCurrentAnimatorStateInfo(0).IsName("PlayerTurnAnimation") && _animatorPlayerTurn.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !_animatorPlayerTurn.IsInTransition(0))
        {
            _animPlayerCanvas.SetActive(false);
        }
    }
}
