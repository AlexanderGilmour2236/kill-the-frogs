using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace KillTheFrogs
{
    public class FrogComboIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private PositionOnCanvas _positionOnCanvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        private Sequence _comboSequence;

        private void Awake()
        {
            _positionOnCanvas.rectTransform = (RectTransform)transform;
        }

        public void setCanvas(Canvas canvas)
        {
            _positionOnCanvas.canvas = canvas;
        }

        public void setFrogCombo(int frogsCount, Vector3 frogPosition)
        {
            gameObject.SetActive(true);
            //_positionOnCanvas.target = target;
            RectTransform rectTransform = (RectTransform) transform;
            rectTransform.position = Camera.main.WorldToScreenPoint(frogPosition);
            clampToWindowSize(rectTransform);
            
            _text.text = $"x{frogsCount}";

            if (_comboSequence != null)
            {
                _comboSequence.Kill();
                _comboSequence = null;
            }
            
            _comboSequence = DOTween.Sequence();
            
            transform.localScale = new Vector3(3, 3, 3);
            _canvasGroup.alpha = 1;
            _text.color = Color.red;

            float duration = 0.4f;
            
            _comboSequence.Append(
                    transform.DOScale(Vector3.one, duration))
                .Join(_text.DOColor(Color.white, duration))
                .AppendInterval(1)
                .Append(_canvasGroup.DOFade(0, duration)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    _comboSequence = null;
                }));
        }
        
       
        private void clampToWindowSize(RectTransform rectTransform)
        {
            Vector2 windowSize = new Vector2(Screen.width, Screen.height);
            Vector2 clampedPosition = rectTransform.anchoredPosition;

            Vector2 rectMin = rectTransform.offsetMin;
            Vector2 rectMax = rectTransform.offsetMax;

            float rectWidth = rectMax.x - rectMin.x;
            float rectHeight = rectMax.y - rectMin.y;

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, rectWidth * 0.5f - windowSize.x * 0.5f, windowSize.x * 0.5f - rectWidth * 0.5f);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, rectHeight * 0.5f - windowSize.y * 0.5f, windowSize.y * 0.5f - rectHeight * 0.5f);

            rectTransform.anchoredPosition = clampedPosition;
        }
    }
}