using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_SettingsMenu : MonoBehaviour
{
    [Header("References :")]
    [SerializeField] private TMP_Dropdown _screenSizeDropdown;
    [SerializeField] private Toggle _fullScreenToggle;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;
    private float _currentRefreshRate;
    private int _currentResolutionIndex = 0;
    private bool _isFullScreen;

    private void Start()
    {

        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();
        _screenSizeDropdown.ClearOptions();
        _currentRefreshRate = Screen.currentResolution.refreshRateRatio.numerator;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resolutions[i].refreshRateRatio.numerator == _currentRefreshRate)
            {
                _filteredResolutions.Add(_resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        for (int i = 0; i < _filteredResolutions.Count; i++)
        {
            string resolutionOption = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height;
            options.Add(resolutionOption);

            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                _currentResolutionIndex = i;
            }
        }

        _screenSizeDropdown.AddOptions(options);

        LoadSavedDatas();

        _screenSizeDropdown.value = _currentResolutionIndex;
        _screenSizeDropdown.RefreshShownValue();
    }

    public void SetResolution()
    {
        _currentResolutionIndex = _screenSizeDropdown.value;
        Resolution resolution = _filteredResolutions[_currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
        PlayerPrefs.SetInt("Resolution", _currentResolutionIndex);
    }

    public void SetFullScreen(bool p_value)
    {
        _isFullScreen = p_value;
        Screen.fullScreen = p_value;

        if (_isFullScreen)
        {
            PlayerPrefs.SetInt("isFullScreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("isFullScreen", 0);
        }
    }

    private void LoadSavedDatas()
    {
        if (PlayerPrefs.GetInt("isFullScreen") == 0)
        {
            _isFullScreen = false;
            Screen.fullScreen = false;
            _fullScreenToggle.isOn = false;
        }
        else
        {
            _isFullScreen = true;
            Screen.fullScreen = true;
            _fullScreenToggle.isOn = true;
        }

        _currentResolutionIndex = PlayerPrefs.GetInt("Resolution");
        _screenSizeDropdown.value = _currentResolutionIndex;
        _screenSizeDropdown.RefreshShownValue();

        SetResolution();
    }
}