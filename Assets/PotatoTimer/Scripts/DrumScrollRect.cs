using System;
using System.Collections;
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
    [SerializeField] private TimePicker _timePicker;
    
    // 選択されている文字列
    private String _selectedContentText;
    
    // ロールコンテンツリスト
    private RectTransform[] _contents;
    // ロールサイズ
    private float contentSizeDeltaHalf;
    private float contentHeightHalf;
    // ロールが表示されているエリアの中央のサイズ
    [SerializeField] private RectTransform _centerRect;

    // m_Draggingがprivateでアクセスできない
    private bool _dragging;
    private bool _scrolling;
    // スクロールした後に移動補間を禁止する時間
    private float _forbidScrollingTime;
    // ロールが動いていない時間
    private float _elapsedTime;
    // 初期化されたか
    public bool initializing = true;

    // Updateした後のロールのy値
    private float _lastUpdatedContentPosY;
    
    public override void OnEndDrag(PointerEventData eventData) {
        base.OnEndDrag(eventData);
        _dragging = false;
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        _dragging = true;
    }

    public void Awake() {
        // ロールコンテンツを取得
        _contents = content.gameObject.GetComponentsInChildrenWithoutSelf<RectTransform>();
        _selectedContentText = _contents[0].GetComponent<TextMeshProUGUI>().text;
    }
    
    private void Start() {
        base.Start();
        
        // ContentSizeFilterが実行されるまで待つ
        Canvas.ForceUpdateCanvases();
        
        // ロール全体の高さの半分
        contentSizeDeltaHalf = content.sizeDelta.y / 2f + 0.03f;
        // 表示されているロールの高さの半分
        contentHeightHalf = content.rect.height/2f;
        
        // ロールコンテンツの一番上が表示エリアの中央に来るように合わせる
        content.localPosition = new Vector3(content.localPosition.x, -content.sizeDelta.y / 2, content.localPosition.z);
        
        // 初期化フラグ
        initializing = false;
    }

    private void LateUpdate() {
        base.LateUpdate();
        
        // プレイ中以外
        if (!EditorApplication.isPlaying)
            return;

        if (!isAllowedToScale())
            return;

        // 中心にもっとも近い要素を取得
        RectTransform nearestRect = _contents.NearestY(_centerRect.position.y);

        // 選択されている(ドラムの中心にある)要素を取得
        TextMeshProUGUI textMesh = nearestRect.gameObject.GetComponent<TextMeshProUGUI>();
        if (!_selectedContentText.Equals(textMesh.text)) {
            _selectedContentText = textMesh.text;
            _timePicker.onChangedValue();
        }
        
        // ドラムロールを再現するためにscale値を変更
        List<RectTransform> rollContents = getRollContentsOnDrum(nearestRect,3);
        rollContents.ForEach(scaleRollContent);

        // ドラッグ中やスクロール中、コンテンツが動いていない時、弾性力の影響下にある時は移動補間の処理を中断する
        if (!isAllowedToMovement())
            return;
        
        // コンテンツ間の移動を補間する
        interpolateDrumMovement(nearestRect);
    }

    private bool isAllowedToScale() {
        // ドラムが1秒以上動いていない場合は移動の補間をしない
        if (_lastUpdatedContentPosY == content.position.y) {
            if (_elapsedTime > 1f)
                return false;
            _elapsedTime += Time.deltaTime;
        }
        
        _elapsedTime = 0;

        return true;
    }

    private bool isAllowedToMovement() {
        // ドラッキング中、スクロール中は移動の補間をしない
        if (_forbidScrollingTime > 0f)
            _forbidScrollingTime -= Time.deltaTime;
        if (_forbidScrollingTime < 0f)
            _scrolling = false;
        if (_dragging || _scrolling)
            return false;

        // 上下の要素をスクロールしすぎた時に弾性で戻る処理を優先させる
        if (Mathf.Abs(content.localPosition.y) > contentSizeDeltaHalf)
            return false;

        return true;
    }

    public List<RectTransform> getRollContentsOnDrum(RectTransform rollContent, int searchRange) {
        List<RectTransform> rollContents = new List<RectTransform>();
        
        // ロール中心のコンテンツindexを取得
        int index = Array.IndexOf(_contents, rollContent);
        if (index == -1)
            return rollContents;

        // 中心の要素のscaleを調整
        rollContents.Add(rollContent);
        
        // 上下3つの要素のScaleを調整
        for (int range = 1; range <= searchRange; range++) {
            rollContent = _contents.ElementAtOrDefault(index + range);
            if (rollContent != null)
                rollContents.Add(rollContent);

            rollContent = _contents.ElementAtOrDefault(index - range);
            if (rollContent != null)
                rollContents.Add(rollContent);
        }

        return rollContents;
    }

    public void scaleRollContent(RectTransform rectTransform) {
        float distance = Mathf.Abs(_centerRect.position.y - rectTransform.position.y);
        float axis_x = distance / contentHeightHalf;
        
        if (axis_x > 1f)
            axis_x = 1;

        float axis_y = Mathf.Sqrt(1f - axis_x * axis_x);
        
        rectTransform.localScale = new Vector3(rectTransform.localScale.x, axis_y, rectTransform.localScale.z);
    }

    private void interpolateDrumMovement(RectTransform nearestRect) {
        float speed = velocity.y;
        Vector2 position = content.position;

        // 一定の加速度以下になったら要素間の移動を補間する
        if (Mathf.Abs(speed) < 100f) {
            // Scroll View側の処理での移動を無効化
            velocity = Vector2.zero;
            float delta = _centerRect.position.y - nearestRect.position.y;

            content.position = new Vector2(position.x,
                Mathf.SmoothDamp(position.y, position.y + delta, ref speed, elasticity, 100f, 0.1f));
        }

        // 止まっている時にUpdateを呼ばないための処理
        _lastUpdatedContentPosY = content.position.y;
    }

    public override void OnScroll(PointerEventData eventData) {
        base.OnScroll(eventData);

        _scrolling = true;
        _forbidScrollingTime = 0.25f;
    }
    
    public string SelectedContentText {
        get { return _selectedContentText; }
        set {
            // 同じデータを上書きする場合
            if (_selectedContentText.Equals(value))
                return;

            // 指定された値のロールコンテンツが見つからなかった場合
            RectTransform targetRect = _contents.FirstOrDefault(c => c.GetComponent<TextMeshProUGUI>().text.Equals(value));
            if (targetRect == null)
                return;
            
            // 指定した値のロールコンテンツまでドラムを移動する
            float delta = _centerRect.position.y - targetRect.position.y;
            content.position = content.position + new Vector3(0, delta, 0);
            
            _selectedContentText = value;
            _timePicker.onChangedValue();
        }
    }
}