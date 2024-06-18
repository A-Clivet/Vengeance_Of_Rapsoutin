using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_UnitSelectorMenu : MonoBehaviour
{
    [Serializable]
    public class PlayersSelectedUnit
    {
        public GameObject selectedUnit;
        [Range(0, 2)] public int unitColor;
    }

    [Serializable]
    public class PlayersUnitSprites
    {
        public List<Sprite> unitSprites = new(3);
    }

    public static S_UnitSelectorMenu Instance;

    [Header("Highlight references :")]
    [SerializeField] GameObject _player1Highlight;
    [SerializeField] GameObject _player2Highlight;

    [Header("Selected units references :")]
    [SerializeField] List<GameObject> _player1SelectedUnitGameObject = new(3) { null, null, null };
    [SerializeField] List<GameObject> _player2SelectedUnitGameObject = new(3) { null, null, null };

    [SerializeField] List<Image> _player1SelectedUnitImage = new(3) { null, null, null };
    [SerializeField] List<Image> _player2SelectedUnitImage = new(3) { null, null, null };

    [Header("Units references :")]
    [SerializeField] List<GameObject> _unitsPrefab = new();

    [Header("Units sprites references :")]
    [SerializeField] List<PlayersUnitSprites> _player1UnitSprites = new(3);
    [SerializeField] List<PlayersUnitSprites> _player2UnitSprites = new(3);

    [HideInInspector] public List<PlayersSelectedUnit> player1SelectedUnits = new(3);
    [HideInInspector] public List<PlayersSelectedUnit> player2SelectedUnits = new(3);

    int _player1PositionInList;
    int _player2PositionInList;

    int _intSelectedUnit;
    int _unitListIndex = 0;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);

        _unitListIndex = _unitsPrefab.Count;
    }

    public void SelectedUnit(int p_index)
    {
        _intSelectedUnit = p_index;

        if (_intSelectedUnit > -1 && _intSelectedUnit <= _unitListIndex / 2 - 1)
        {
            //between 0 and 2 are Ussr values

            // We save in the player1SelectedUnits variable the Unit type, and the unit color
            player1SelectedUnits[_player1PositionInList].selectedUnit = _unitsPrefab[_intSelectedUnit];
            player1SelectedUnits[_player1PositionInList].unitColor = _player1PositionInList;

            _player1SelectedUnitImage[_player1PositionInList].sprite = _player1UnitSprites[_intSelectedUnit].unitSprites[_player1PositionInList];

            // The first time the player 1 choose a unit, then we show the player 1 highlight
            if (!_player1Highlight.activeSelf)
                _player1Highlight.SetActive(true);

            _player1Highlight.transform.position = _player1SelectedUnitGameObject[_player1PositionInList].transform.position;

            _player1PositionInList++;

            if (_player1PositionInList >= 3)
            {
                _player1PositionInList = 0;
            }
        }
        else if (_intSelectedUnit > _unitListIndex / 2 - 1)
        {
            //between 3 and 5 are Rasputin values
            player2SelectedUnits[_player2PositionInList].selectedUnit = _unitsPrefab[_intSelectedUnit];
            player2SelectedUnits[_player2PositionInList].unitColor = _player2PositionInList;

            _player2SelectedUnitImage[_player2PositionInList].sprite = _player2UnitSprites[_intSelectedUnit - 3].unitSprites[_player2PositionInList];

            // The first time the player 2 choose a unit, then we show the player 2 highlight
            if (!_player2Highlight.activeSelf)
                _player2Highlight.SetActive(true);

            _player2Highlight.transform.position = _player2SelectedUnitGameObject[_player2PositionInList].transform.position;

            _player2PositionInList++;

            if (_player2PositionInList >= 3)
            {
                _player2PositionInList = 0;
            }
        }
        else
        {
            return;
        }
    }

    public void LaunchGame()
    {
        if (IsPlayerReady())
        {
            // Save the data
            S_CrossSceneDataManager.Instance.player1SelectedUnits = player1SelectedUnits;
            S_CrossSceneDataManager.Instance.player2SelectedUnits = player2SelectedUnits;

            // Load the MainGame Scene
            SceneManager.LoadScene("MainGame");
        }
    }

    bool IsPlayerReady()
    {
        foreach (PlayersSelectedUnit playersSelectedUnit in player1SelectedUnits.Where(
            playersSelectedUnit => playersSelectedUnit.selectedUnit == null))
        {
            return false;
        }

        foreach (PlayersSelectedUnit playersSelectedUnit in player2SelectedUnits.Where(
            playersSelectedUnit => playersSelectedUnit.selectedUnit == null))
        {
            return false;
        }

        return true;
    }
}