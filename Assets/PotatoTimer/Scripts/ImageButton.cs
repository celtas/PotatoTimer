using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ImageButton : MonoBehaviour {
    [SerializeField] private Color _normalColor,
        _highlightColor,
        _pressColor,
        _releaseColor,
        _disableColor,
        _textColor,
        _disableTextColor,
        _highlightTextColor;

    public UnityEvent eventPressed, eventClicked, eventEntered, eventExited, eventDisabled, eventEnabled;

    public bool enable = true;
    private Image[] _images;
    private TextMeshProUGUI[] _texts;

    void Start() {
        updateCollider();
    }

    // 画面比率の変更に対応するため、コライダーの当たり判定を調節
    void updateCollider() {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
            return;
        
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();

        // canvasScalerによるcanvasの拡大率を取得
        Vector2 mul = new Vector2(canvasRectTransform.sizeDelta.x / canvasScaler.referenceResolution.x,
            canvasRectTransform.sizeDelta.y / canvasScaler.referenceResolution.y);
        
        // コライダーに適用
        Vector2 rect = collider.size;
        rect.Scale(mul);
        collider.size = rect;
    }

    void Awake() {
        _images = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (Image image in _images) {
            eventPressed.AddListener(() => image.color = _pressColor);
            eventClicked.AddListener(() => image.color = _releaseColor);
            eventEntered.AddListener(() => image.color = _highlightColor);
            eventExited.AddListener(() => image.color = _normalColor);
            eventDisabled.AddListener(() => image.color = _disableColor);
            eventEnabled.AddListener(() => image.color = _normalColor);
        }

        eventDisabled.AddListener(() => enable = false);
        eventEnabled.AddListener(() => enable = true);

        foreach (TextMeshProUGUI text in _texts) {
            eventClicked.AddListener(() => text.color = _releaseColor);
            eventEntered.AddListener(() => text.color = _highlightTextColor);
            eventExited.AddListener(() => text.color = _textColor);
            // マウスオーバー時のテキスト色変更
            eventDisabled.AddListener(() => text.color = _disableTextColor);
            eventEnabled.AddListener(() => text.color = _textColor);
        }
    }

    public void DisableButton() {
        eventDisabled.InvokeSafe();
    }

    public void EnableButton() {
        eventEnabled.InvokeSafe();
    }

    public void OnMouseEnter() {
        if (!enable)
            return;

        eventEntered.InvokeSafe();
    }

    public void OnMouseDown() {
        if (!enable)
            return;

        eventPressed.InvokeSafe();
    }

    public void OnMouseUp() {
        if (!enable)
            return;

        eventClicked.InvokeSafe();
    }

    public void OnMouseExit() {
        if (!enable)
            return;

        eventExited.InvokeSafe();
    }
}