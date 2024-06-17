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

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    public void PlayWeatherAnimation()
    {
        _playerWeatherText.text = "Weather : " + _currentEvent.ManageEvent;
        _animatorWeather.Play(0, 0, 0);
    }
}
