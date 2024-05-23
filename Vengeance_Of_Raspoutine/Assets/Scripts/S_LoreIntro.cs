using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class S_LoreIntro : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private string _fullText;
    [SerializeField] private float _delay;
    private string _currentText = "";
    [SerializeField] private string _sceneToLoad;

    private void Start()
    {
        _fullText = _text.text;
        _text.text = _currentText;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= _fullText.Length; i++)
        {
                _currentText = _fullText.Substring(0, i);
                _text.text = _currentText;
                yield return new WaitForSeconds(_delay);
        }
    }

    public void NextBtn()
    {
        if (_text.text != _fullText)
        {
            StopAllCoroutines();
            _text.text = _fullText;
        }
        else
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
    }
}
