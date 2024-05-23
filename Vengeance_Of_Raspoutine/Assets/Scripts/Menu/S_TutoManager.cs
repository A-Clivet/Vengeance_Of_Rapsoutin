using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_TutoManager : MonoBehaviour
{

    private int _intTuto;
    private Sprite _currentTuto;

    public List<Sprite> tutoSelection = new(new Sprite[10]);
    [SerializeField] private Image _currentSprite;




    private void Start()
    {
        _intTuto = 0;
        _currentTuto = tutoSelection[_intTuto];  // set the current tuto on start
        _currentSprite.sprite = tutoSelection[_intTuto]; // set the current sprite on start
    }

    public void NextTuto()
    {
        _intTuto += 1;
        _currentTuto = tutoSelection[_intTuto];  
        _currentSprite.sprite = tutoSelection[_intTuto];
    }

    public void PreviousTuto()
    {
        _intTuto -= 1;
        _currentTuto = tutoSelection[_intTuto];  
        _currentSprite.sprite = tutoSelection[_intTuto];
    }
}
