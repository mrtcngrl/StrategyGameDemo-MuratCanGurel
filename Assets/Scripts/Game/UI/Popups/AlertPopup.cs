using DG.Tweening;
using Game.Signals.Core;
using Game.Signals.Utils;
using TMPro;
using UnityEngine;

namespace Game.UI.Popups
{
    public class AlertPopup : MonoBehaviour
    {
        [SerializeField] private RectTransform _popupRect;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _alertText;
        private Sequence _anim;
        private void Start()
        {
            SignalSystem.SignalBus.Subscribe<ShowAlertNotifySignal>(ShowPopup);
        }

        private void ShowPopup(ShowAlertNotifySignal signal)
        {
            _alertText.text = signal.AlertMessage;
            _popupRect.anchoredPosition = new Vector2(0, -100);
            _canvasGroup.alpha = 1;
            _anim?.Kill();
            gameObject.SetActive(true);
            _anim = DOTween.Sequence().SetUpdate(true)
                .Append(_popupRect.DOAnchorPosY(0, 0.4f))
                .AppendInterval(0.5f)
                .Append(_canvasGroup.DOFade(0, 0.4f))
                .OnComplete(() => gameObject.SetActive(false));
            
        }
    }
}