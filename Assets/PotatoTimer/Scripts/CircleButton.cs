using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CircleButton : MonoBehaviour {
    [SerializeField]
    private Color _normalColor,_highlightedColor,_pressedColor,_releaseColor,_disableColor,_disableTextColor,_textColor;
    public UnityEvent eventPressed,eventClicked,_eventOver,eventEntered,eventExited,eventDisabled,eventEnabled;

    public bool enable = true;
    private Image[] _images;
    private TextMeshProUGUI[] _texts;
    
    void Awake() {
        _images = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (Image image in _images) {
            eventPressed.AddListener(() => image.color = _pressedColor);
            eventClicked.AddListener(() => image.color = _releaseColor);
            eventEntered.AddListener(() => image.color = _highlightedColor);
            eventExited.AddListener(() => image.color = _normalColor);
            eventDisabled.AddListener(() => image.color = _disableColor);
            eventEnabled.AddListener(() => image.color = _normalColor);
        }
        
        eventDisabled.AddListener(() => enable = false);
        eventEnabled.AddListener(() => enable = true);

        foreach (TextMeshProUGUI text in _texts) {
            // マウスオーバー時のテキスト色変更
            eventEntered.AddListener(() => text.color = _disableTextColor);
            eventExited.AddListener(() => text.color = _textColor);
            // ボタンの有効、無効化した時のテキスト色
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

    public void OnMouseOver() {
        if (!enable)
            return;
        
        _eventOver.InvokeSafe();
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
    
    public void OnMouseExit(){
        if (!enable)
            return;

        eventExited.InvokeSafe();
    }
}