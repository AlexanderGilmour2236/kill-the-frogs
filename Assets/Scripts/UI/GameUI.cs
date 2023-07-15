using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace KillTheFrogs
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private FrogsCounter _frogsCounter;
        [SerializeField] private WinUI _winUI;
        [SerializeField] private CanvasGroup _nextGameFader;
        [SerializeField] private PositionOnCanvas _handClickPointer;
        [SerializeField] private CanvasGroup _handClickCanvasGroup;
        [SerializeField] private FrogComboIndicator _frogComboIndicatorPrefab;
        [SerializeField] private Canvas _canvas;
        private Dictionary<TrapView, FrogComboIndicator> _trapViewToComboIndicator = new Dictionary<TrapView, FrogComboIndicator>();


        private Tween _nextGameFaderTween;
        private Tween _handFadeTween;

        public event Action newGameClick;

        public void setFrogsLeft(int frogsLeft, bool isNegative = false)
        {
            _frogsCounter.setFrogsCount(frogsLeft);
            if (isNegative)
            {
                _frogsCounter.doNegativeAnimation();
            }
        }

        public void goWinUI(int frogsLeft, int frogsCrossedTheRoad, bool levelPass)
        {
            _winUI.show(frogsCrossedTheRoad, levelPass);
            _winUI.continueClick += onContinueClick;
        }

        private void onContinueClick(bool isNewLevel)
        {
            if (_nextGameFaderTween == null)
            {
                _winUI.hide();
                _nextGameFader.gameObject.SetActive(true);
                _nextGameFader.alpha = 0;
                _nextGameFaderTween = 
                    _nextGameFader.DOFade(1, 0.5f)
                    .OnComplete(() =>
                    {
                        newGameClick?.Invoke();
                        
                        _nextGameFader
                            .DOFade(0, 0.5f)
                            .OnComplete(() =>
                            {
                                _nextGameFader.gameObject.SetActive(false);
                                _nextGameFaderTween = null;
                            });
                    });
            }

        }

        public void showHandOnObject(Transform transform)
        {
            _handClickPointer.target = transform;

            _handClickCanvasGroup.gameObject.SetActive(true);
            _handClickCanvasGroup.alpha = 0;
            _handFadeTween = _handClickCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
            {
                _handFadeTween = null;
            });
        }        
        
        public void hideHand()
        {
            if (_handFadeTween != null)
            {
                _handFadeTween.Kill();
                _handFadeTween = null;
            }
            
            _handFadeTween = _handClickCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                _handClickCanvasGroup.gameObject.SetActive(false);
                _handFadeTween = null;
            });
        }

        public void showFrogsComboIndicator(TrapView trapView, int trapViewToKilledFrog, FrogView frogView)
        {
            if (!_trapViewToComboIndicator.ContainsKey(trapView))
            {
                FrogComboIndicator frogComboIndicator = Instantiate(_frogComboIndicatorPrefab, _canvas.transform, false);
                _trapViewToComboIndicator.Add(trapView, frogComboIndicator);
                frogComboIndicator.setCanvas(_canvas);
            }

            if (trapViewToKilledFrog > 1)
            {
                _trapViewToComboIndicator[trapView].setFrogCombo(trapViewToKilledFrog, frogView.transform.position);
            }
        }

        public void destroyComboIndicator(TrapView trapView)
        {
            if (_trapViewToComboIndicator.ContainsKey(trapView))
            {
                FrogComboIndicator frogComboIndicator = _trapViewToComboIndicator[trapView];
                Destroy(frogComboIndicator.gameObject);
                _trapViewToComboIndicator.Remove(trapView);
            }
        }
    }
}