using System;
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
    private float contentHeightHalf;

    // m_Draggingがprivateでアクセスできない
    private bool _dragging;
    private bool _scrolling;
    // スクロールした後に移動補間を禁止する時間
    private float _forbidedTime;
    
    // Updateした後のcontentの値
    private float _lastUpdatedContentPosY;
    private float _elapsedTime;

    public override void OnEndDrag(PointerEventData eventData) {
        base.OnEndDrag(eventData);
        _dragging = false;
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        _dragging = true;
    }

    public void Awake() {
        _contents = content.gameObject.GetComponentsInChildrenWithoutSelf<RectTransform>();
    }

    public void Start() {
        contentHeightHalf = content.sizeDelta.y / 2f + 0.03f;
    }

    public void LateUpdate() {
        base.LateUpdate();

        if (!EditorApplication.isPlaying)
            return;
        
        // 中心にもっとも近い要素を取得
        RectTransform nearestRect = _contents.NearestY(_centerRect.position.y);
        // 選択されている(ドラムの中心にある)要素を取得
        _selectedContentText = nearestRect.gameObject.GetComponent<TextMeshProUGUI>().text;

        // ドラッグ中やスクロール中、コンテンツが動いていない時、弾性力の影響下にある時
        // 以外であれば、移動補間の処理を行う
        if (isAllowedToMovement())
            interpolateDrumMovement(nearestRect);
    }

    private bool isAllowedToMovement() {
        // 動いていない時にUpdateを呼ばないための処理
        if (_lastUpdatedContentPosY == content.position.y) {
            if (_elapsedTime > 1f)
                return false;
            _elapsedTime += Time.deltaTime;
        }
        else {
            _elapsedTime = 0;
        }

        // ドラッキング中、スクロール中は移動の補間をしない
        if (_forbidedTime > 0f)
            _forbidedTime -= Time.deltaTime;
        if (_forbidedTime < 0f)
            _scrolling = false;
        if (_dragging || _scrolling)
            return false;
        
        // 上下の要素をスクロールしすぎた時に弾性で戻る処理を優先させる
        if (Mathf.Abs(content.localPosition.y) > contentHeightHalf)
            return false;

        return true;
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
        _forbidedTime = 0.1f;
    }

    public string SelectedContentText {
        get { return _selectedContentText; }
        set {
            RectTransform targetRect =　_contents.FirstOrDefault(c => c.GetComponent<TextMeshProUGUI>().text.Equals(value));

            if (targetRect == null)
                return;

            float delta = _centerRect.position.y - targetRect.position.y;
            content.position = content.position + new Vector3(0, delta, 0);
            _selectedContentText = value;
        }
    }
    
    #if UNITY_EDITOR
        // ロールコンテンツが中央に来るように配置
        protected override void OnValidate() {
            content.localPosition = new Vector3(content.localPosition.x,-content.sizeDelta.y/2,content.localPosition.z);
        }
    #endif
}