using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class CircleButton : MonoBehaviour {
    [SerializeField]
    private Color _normalColor,_hightlightedColor,_pressedColor,_releaseColor,_disableColor;
    [SerializeField]
    private UnityEvent _eventPressed,_eventReleased,_eventOver,_eventEnter,_eventExit;
    
    void Start() {
        foreach (Image image in GetComponentsInChildren<Image>()) {
            _eventPressed.AddListener(() => image.color = _pressedColor);
            _eventReleased.AddListener(() => image.color = _releaseColor);
            _eventEnter.AddListener(() => image.color = _hightlightedColor);
            _eventExit.AddListener(() => image.color = _normalColor);
        }
    }
    
    void OnDisable() {
        foreach (Image image in GetComponentsInChildren<Image>())
            image.color = _disableColor;
    }

    public void OnMouseOver() {
        _eventOver.InvokeSafe();
    }

    public void OnMouseEnter() {
        _eventEnter.InvokeSafe();
    }
    
    public void OnMouseDown() {
        _eventPressed.InvokeSafe();
    }

    public void OnMouseUp() {
        _eventReleased.InvokeSafe();
    }
    
    public void OnMouseExit(){
        _eventExit.InvokeSafe();
    }
}