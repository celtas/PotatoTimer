using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CircleButton : MonoBehaviour {
    [SerializeField]
    private Color _normalColor,_highlightedColor,_pressedColor,_releaseColor,_disableColor,_disableTextColor,_textColor;
    public UnityEvent _eventPressed,_eventReleased,_eventOver,_eventEnter,_eventExit,_eventDisable,_eventEnable;

    public bool enable = true;
    private Image[] _images;
    private TextMeshProUGUI[] _texts;
    
    void Awake() {
        _images = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (Image image in _images) {
            _eventPressed.AddListener(() => image.color = _pressedColor);
            _eventReleased.AddListener(() => image.color = _releaseColor);
            _eventEnter.AddListener(() => image.color = _highlightedColor);
            _eventExit.AddListener(() => image.color = _normalColor);
            _eventDisable.AddListener(() => image.color = _disableColor);
            _eventEnable.AddListener(() => image.color = _normalColor);
        }
        
        _eventDisable.AddListener(() => enable = false);
        _eventEnable.AddListener(() => enable = true);

        foreach (TextMeshProUGUI text in _texts) {
            // マウスオーバー時のテキスト色変更
            _eventEnter.AddListener(() => text.color = _disableTextColor);
            _eventExit.AddListener(() => text.color = _textColor);
            // ボタンの有効、無効化した時のテキスト色
            _eventDisable.AddListener(() => text.color = _disableTextColor);
            _eventEnable.AddListener(() => text.color = _textColor);
        }
    }
    
    public void DisableButton() {
        _eventDisable.InvokeSafe();
    }
    public void EnableButton() {
        _eventEnable.InvokeSafe();
    }

    public void OnMouseOver() {
        if (!enable)
            return;
        
        _eventOver.InvokeSafe();
    }

    public void OnMouseEnter() {
        if (!enable)
            return;

        _eventEnter.InvokeSafe();
    }
    
    public void OnMouseDown() {
        if (!enable)
            return;

        _eventPressed.InvokeSafe();
    }

    public void OnMouseUp() {
        if (!enable)
            return;

        _eventReleased.InvokeSafe();
    }
    
    public void OnMouseExit(){
        if (!enable)
            return;

        _eventExit.InvokeSafe();
    }
}