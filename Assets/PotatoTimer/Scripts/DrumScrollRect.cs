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
    public String selectedContentText;
    public RectTransform centerRect;

    private RectTransform[] contents;
    private List<float> contents_anchoredPositionY;

    // m_Draggingがprivateでアクセスできない
    private bool dragging = false;

    public override void OnEndDrag(PointerEventData eventData) {
        base.OnEndDrag(eventData);
        dragging = false;
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        dragging = true;
    }

    public void Start() {
        contents = content.gameObject.GetComponentsInChildrenWithoutSelf<RectTransform>();
        contents_anchoredPositionY = contents.Select(x => -x.anchoredPosition.y).ToList();
    }

    public void LateUpdate() {
        base.LateUpdate();

        if (!EditorApplication.isPlaying)
            return;
        if (dragging)
            return;
        
        float speed = velocity.y;


        if (Mathf.Abs(speed) <= 0f) {
            return;
        }
        
        Vector2 position = content.position;
        // 中心にもっとも近い要素を取得
        RectTransform rectNearest = contents.NearestY(centerRect.position.y);
        
        // 選択されている要素のテキストを取得
        selectedContentText = rectNearest.gameObject.GetComponent<TextMeshProUGUI>().text;

        // 一定の加速度以下になったら要素間の移動を補完する
        if (Mathf.Abs(speed) < 200f) {
            // Scroll View側の処理での移動を無効化
            velocity = Vector2.zero;
            float delta = centerRect.position.y - rectNearest.position.y;
                
            content.position = new Vector2(position.x,
                Mathf.SmoothDamp(position.y, position.y + delta, ref speed, elasticity, 50f, 0.05f));
        }
    }

    public override void OnScroll(PointerEventData eventData) {
        base.OnScroll(eventData);
        Vector2 delta = eventData.scrollDelta;
    }
}