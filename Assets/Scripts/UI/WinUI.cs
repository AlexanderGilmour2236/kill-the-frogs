using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _winUICanvasGroup;
    [SerializeField] private TextMeshProUGUI _butYouMissFrogsText;
    [SerializeField] private TextMeshProUGUI _frogsMissedText;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _nextButtonText;

    private const string GOOD_GAME_OVER_TEXT = "WELL DONE!";
    private const string GOOD_GAME_OVER_BUTTON_TEXT = "TRY AGAIN";
    private const string PERFECT_GAME_OVER_TEXT = "PERFECT!";
    private const string PERFECT_GAME_BUTTON_TEXT = "CONTINUE";
    
    [SerializeField] private Button _continueButton;
    private Tween _faderTween;
    private Coroutine _butYouMissedFrogsCoroutine;
    private bool _isFrogsMissed;

    const float FADE_DURATION = 0.5f;

    public event Action<bool> continueClick;

    private void Awake()
    {
        _continueButton.onClick.AddListener(onContinueButtonClick);
    }

    private void onContinueButtonClick()
    {
        continueClick?.Invoke(!_isFrogsMissed);
    }

    public void show(int frogsCrossedTheRoad, bool levelPass)
    {
        gameObject.SetActive(true);
        _isFrogsMissed = frogsCrossedTheRoad > 0;
        _gameOverText.text = _isFrogsMissed ? GOOD_GAME_OVER_TEXT : PERFECT_GAME_OVER_TEXT;
        _nextButtonText.text = levelPass ? PERFECT_GAME_BUTTON_TEXT : GOOD_GAME_OVER_BUTTON_TEXT;

        _butYouMissFrogsText.gameObject.SetActive(_isFrogsMissed);
        _frogsMissedText.gameObject.SetActive(_isFrogsMissed);

        if (_isFrogsMissed)
        {
            if (_butYouMissedFrogsCoroutine != null)
            {
                StopCoroutine(_butYouMissedFrogsCoroutine);
            }
            _butYouMissedFrogsCoroutine = StartCoroutine(butYouMissedFrogsCoroutine(frogsCrossedTheRoad));

        }

        if (_faderTween != null)
        {
            _faderTween.Kill();
            _faderTween = null;
        }
        _faderTween = _winUICanvasGroup.DOFade(1, 0.5f);

    }

    private IEnumerator butYouMissedFrogsCoroutine(int frogsCrossedTheRoad)
    {
        float textOffsetTime = 0.4f;
        int frogsCounted = 0;
        
        while (frogsCounted <= frogsCrossedTheRoad)
        {
            _frogsMissedText.text = frogsCounted.ToString();
            _frogsMissedText.transform.localScale = new Vector3(5.0f,5.0f);
            _frogsMissedText.color = Color.red;
            _frogsMissedText.DOColor(Color.white, textOffsetTime);
            _frogsMissedText.transform.DOScale(Vector3.one, textOffsetTime);
            frogsCounted++;
            yield return new WaitForSeconds(textOffsetTime);
        }
    }

    public void hide()
    {
        if (_butYouMissedFrogsCoroutine != null)
        {
            StopCoroutine(_butYouMissedFrogsCoroutine);
        }
        if (_faderTween != null)
        {
            _faderTween.Kill();
            _faderTween = null;
        }
        _faderTween = _winUICanvasGroup.DOFade(0, FADE_DURATION).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
