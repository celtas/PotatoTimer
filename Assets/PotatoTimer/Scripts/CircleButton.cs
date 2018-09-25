using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CircleButton : MonoBehaviour {
    [SerializeField]
    private Color _normalColor,_highlightedColor,_pressedColor,_releaseColor,_disableColor;
    public UnityEvent _eventPressed,_eventReleased,_eventOver,_eventEnter,_eventExit,_eventDisable,_eventEnable;

    public bool enable = true;
    private Image[] _images;
    
    void Awake() {
        _images = GetComponentsInChildren<Image>();
        foreach (Image image in _images) {
            _eventPressed.AddListener(() => image.color = _pressedColor);
            _eventReleased.AddListener(() => image.color = _releaseColor);
            _eventEnter.AddListener(() => image.color = _highlightedColor);
            _eventExit.AddListener(() => image.color = _normalColor);
            
            _eventDisable.AddListener(() => image.color = _disableColor);
            _eventDisable.AddListener(() => enable = false);
            
            _eventEnable.AddListener(() => image.color = _normalColor);
            _eventEnable.AddListener(() => enable = true);
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