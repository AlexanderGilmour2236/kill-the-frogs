using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FrogsCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _frogsCountText;
    private Sequence _negativeEffectSequence;

    public void setFrogsCount(int frogsCount)
    {
        _frogsCountText.text = $"X{frogsCount}";
    }

    public void doNegativeAnimation()
    {
        if (_negativeEffectSequence != null)
        {
            _negativeEffectSequence.Complete();
            _negativeEffectSequence = null;
        }
        
        _frogsCountText.transform.localScale = new Vector3(3, 3);
        _frogsCountText.color = Color.red;
        
        _negativeEffectSequence = DOTween.Sequence();
        float effectDuration = 0.5f;

        _negativeEffectSequence.Append(_frogsCountText.transform
                .DOScale(Vector3.one, effectDuration))
            .Join(_frogsCountText
                .DOColor(Color.white, effectDuration))
            .OnComplete(() => _negativeEffectSequence = null);
    }
}
