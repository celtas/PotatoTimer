using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CustomEditor(typeof(DrumScrollRect))]
public class DrumScrollRectEditor : Editor {
    public void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("contentRect"), true);
        serializedObject.ApplyModifiedProperties();
    }
}

public class DrumScrollRect : ScrollRect {
    private GameObject selectedGameObject;
    private RectTransform[] contents;
    private List<float> contents_anchoredPositionY;

    // m_Draggingがprivateでアクセスできない
    private bool dragging = false;

    [SerializeField] private RectTransform contentRect;

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

        float contentHeight = contentRect.sizeDelta.y;
        Vector2 position = content.anchoredPosition;

        float deltaTime = Time.unscaledDeltaTime;

        float speed = velocity.y;
        if (Mathf.Abs(speed) < 200f) {
            base.velocity = Vector2.zero;
            float nearest = contents_anchoredPositionY.NearestValue(position.y);
            content.anchoredPosition = new Vector2(position.x,
                Mathf.SmoothDamp(position.y, nearest, ref speed, elasticity, 50f, 0.05f));
        }
    }

    public override void OnScroll(PointerEventData eventData) {
        base.OnScroll(eventData);
        Vector2 delta = eventData.scrollDelta;
    }
}