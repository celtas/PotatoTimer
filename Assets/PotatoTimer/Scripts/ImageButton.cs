using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ImageButton : MonoBehaviour {
    [SerializeField] private Color _normalColor,
        _highlightColor,
        _pressColor,
        _releaseColor,
        _disableColor,
        _textColor,
        _disableTextColor,
        _highlightTextColor;

    public UnityEvent pressEvent, clickEvent, enterEvent, eventExited, disableEvent, enableEvent;

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
        registerAction();
    }

    void registerAction() {
        _images = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<TextMeshProUGUI>();
        
        disableEvent.AddListener(() => enable = false);
        enableEvent.AddListener(() => enable = true);
        
        foreach (Image image in _images) {
            pressEvent.AddListener(() => image.color = _pressColor);
            clickEvent.AddListener(() => image.color = _releaseColor);
            enterEvent.AddListener(() => image.color = _highlightColor);
            eventExited.AddListener(() => image.color = _normalColor);
            disableEvent.AddListener(() => image.color = _disableColor);
            enableEvent.AddListener(() => image.color = _normalColor);
        }
        foreach (TextMeshProUGUI text in _texts) {
            enterEvent.AddListener(() => text.color = _highlightTextColor);
            eventExited.AddListener(() => text.color = _textColor);
            // マウスオーバー時のテキスト色変更
            disableEvent.AddListener(() => text.color = _disableTextColor);
            enableEvent.AddListener(() => text.color = _textColor);
        }
    }

    public void DisableButton() {
        Debug.Log(gameObject.name+":"+"disable");
        disableEvent.InvokeSafe();
    }

    public void EnableButton() {
        Debug.Log(gameObject.name+":"+"enable");
        enableEvent.InvokeSafe();
    }

    public void OnMouseEnter() {
        if (!enable)
            return;

        enterEvent.InvokeSafe();
    }

    public void OnMouseDown() {
        if (!enable)
            return;

        pressEvent.InvokeSafe();
    }

    public void OnMouseUp() {
        if (!enable)
            return;

        clickEvent.InvokeSafe();
    }

    public void OnMouseExit() {
        if (!enable)
            return;

        eventExited.InvokeSafe();
    }
}