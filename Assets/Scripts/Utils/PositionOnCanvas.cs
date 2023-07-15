using UnityEngine;
using UnityEngine.UI;

public class PositionOnCanvas : MonoBehaviour
{
    [SerializeField] private bool _isUpdating;
    public Canvas canvas;
    public RectTransform rectTransform;
    private Transform _target;

    private void Update()
    {
        setTargetPosition();
    }

    private void setTargetPosition()
    {
        if (_target == null || !_isUpdating) return;

        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(target.position);
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, targetScreenPosition,
            canvas.worldCamera, out anchoredPosition);
        rectTransform.anchoredPosition = anchoredPosition;
    }

    public Transform target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
            setTargetPosition();
        }
    }
}