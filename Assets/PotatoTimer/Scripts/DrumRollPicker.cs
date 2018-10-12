using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DrumRollPicker : MonoBehaviour {
    [SerializeField] 
    private GameObject[] _hours = new GameObject[5];
    private RectTransform[] _hours_rect = new RectTransform[5];
    private TextMeshProUGUI[] _hours_text = new TextMeshProUGUI[5];

    // 数字ロール1つのアンカーの高さ
    public static float ROLL_HEIGHT_ANCHOR = 0.2f;

    // Use this for initialization
    void Start() {
        for (int i = 0; i < _hours.Length; i++) {
            _hours_rect[i] = _hours[i].GetComponent<RectTransform>();
            _hours_text[i] = _hours[i].GetComponent<TextMeshProUGUI>();
        }

        foreach (RectTransform rectTransform in _hours_rect) {
            adjustScale(rectTransform);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            up();
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            down();
        }
    }

    void down() {
        float anchorIncrement = -0.02f;
        foreach (RectTransform rectTransform in _hours_rect.Reverse()) {
            if (rectTransform.anchorMax.y + anchorIncrement < 0f)
                return;
            
            rectTransform.anchorMin = rectTransform.anchorMin + new Vector2(0, anchorIncrement);
            rectTransform.anchorMax = rectTransform.anchorMax + new Vector2(0, anchorIncrement);

            adjustScale(rectTransform);
        }
    }

    void up() {
        float anchorIncrement = 0.02f;
        foreach (RectTransform rectTransform in _hours_rect) {
            if (rectTransform.anchorMin.y + anchorIncrement > 1f)
                return;
            
            rectTransform.anchorMin = rectTransform.anchorMin + new Vector2(0, anchorIncrement);
            rectTransform.anchorMax = rectTransform.anchorMax + new Vector2(0, anchorIncrement);

            adjustScale(rectTransform);
        }
    }

    // ドラムロールの再現
    void adjustScale(RectTransform rectTransform) {
        // ロールの場所によりscale値を変える
        if (rectTransform.anchorMin.y >= 1 - ROLL_HEIGHT_ANCHOR) {
            rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 1f);
            if (rectTransform.anchorMin.y > 1f) // 文字の反転を防ぐ
                rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 1f);

            float diff = rectTransform.anchorMin.y - (1 - ROLL_HEIGHT_ANCHOR);
            float scale_y = (ROLL_HEIGHT_ANCHOR - diff) / ROLL_HEIGHT_ANCHOR * 0.4f;
            rectTransform.localScale = new Vector3(rectTransform.localScale.x, scale_y, rectTransform.localScale.z);
        }
        else if (rectTransform.anchorMin.y >= 1 - ROLL_HEIGHT_ANCHOR * 2) {
            float diff = rectTransform.anchorMin.y - (1 - ROLL_HEIGHT_ANCHOR * 2);
            float scale_y = (ROLL_HEIGHT_ANCHOR - diff) / ROLL_HEIGHT_ANCHOR * 0.6f + 0.4f;
            rectTransform.localScale = new Vector3(rectTransform.localScale.x, scale_y, rectTransform.localScale.z);
        }
        else if (rectTransform.anchorMax.y <= ROLL_HEIGHT_ANCHOR) {
            rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0f);
            if (rectTransform.anchorMax.y < 0f) // 文字の反転を防ぐ
                rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 0f);
            
            float diff = rectTransform.anchorMax.y;
            float scale_y = diff / ROLL_HEIGHT_ANCHOR * 0.4f;
            rectTransform.localScale = new Vector3(rectTransform.localScale.x, scale_y, rectTransform.localScale.z);
        }
        else if (rectTransform.anchorMax.y <= ROLL_HEIGHT_ANCHOR * 2) {
            float diff = rectTransform.anchorMax.y - ROLL_HEIGHT_ANCHOR;
            float scale_y = diff / ROLL_HEIGHT_ANCHOR * 0.6f  + 0.4f;
            rectTransform.localScale = new Vector3(rectTransform.localScale.x, scale_y, rectTransform.localScale.z);
        }
    }

    void OnMouseDrag() {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        float z = 100.0f;
        //位置を変換してオブジェクトの座標に指定
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(x, y, z));
        Debug.Log(position.y);
    }
}