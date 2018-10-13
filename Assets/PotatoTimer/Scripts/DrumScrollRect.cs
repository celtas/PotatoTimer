using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CustomEditor(typeof(DrumScrollRect))]
public class DrumScrollRectEditor : Editor {
    public void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}

public class DrumScrollRect : ScrollRect {
    [SerializeField] private String _selectedContentText;
    [SerializeField] private RectTransform _centerRect;
    
    private RectTransform[] _contents;
    
    // m_Draggingがprivateでアクセスできない
    private bool _dragging;

    public override void OnEndDrag(PointerEventData eventData) {
        base.OnEndDrag(eventData);
        _dragging = false;
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        _dragging = true;
    }

    public void Start() {
        _contents = content.gameObject.GetComponentsInChildrenWithoutSelf<RectTransform>();
    }

    public void LateUpdate() {
        base.LateUpdate();

        if (!EditorApplication.isPlaying)
            return;
        if (_dragging)
            return;
        
        float speed = velocity.y;
        Vector2 position = content.position;
        // 中心にもっとも近い要素を取得
        RectTransform rectNearest = _contents.NearestY(_centerRect.position.y);
        
        // 選択されている要素のテキストを取得
        _selectedContentText = rectNearest.gameObject.GetComponent<TextMeshProUGUI>().text;

        // 一定の加速度以下になったら要素間の移動を補完する
        if (Mathf.Abs(speed) < 200f) {
            // Scroll View側の処理での移動を無効化
            velocity = Vector2.zero;
            float delta = _centerRect.position.y - rectNearest.position.y;
                
            content.position = new Vector2(position.x,
                Mathf.SmoothDamp(position.y, position.y + delta, ref speed, elasticity, 50f, 0.05f));
        }
    }

    public override void OnScroll(PointerEventData eventData) {
        base.OnScroll(eventData);
    }

    public string SelectedContentText {
        get {
            return _selectedContentText;
        }
        set {
            RectTransform targetRect = _contents.FirstOrDefault(c => c.GetComponent<TextMeshProUGUI>().text.Equals(value));
            
            float delta = _centerRect.position.y - targetRect.position.y;
            content.position = content.position + new Vector3(0, delta,0);
            _selectedContentText = value;
        }
    }
}