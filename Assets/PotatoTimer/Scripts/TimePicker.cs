using System;
using System.Collections;
using System.Linq;
using System.Security.Authentication;
using TMPro;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TimePicker : MonoBehaviour {
    public DrumScrollRect hourDrum, minuteDrum, secondDrum;
    public Action<int, int, int> onChanged;

    [Header("HighlightArea Preferences")]
    public HighlightAreaPreferences option;

    [Serializable]
    public class HighlightAreaPreferences {
        [TooltipAttribute("Adjust highlight area and border")]
        public bool autoHighlightSizeAndPosition;
        [TooltipAttribute("Adjust border")] public bool autoBorderSizeAndPosition;
        public RectTransform borderTop, borderBottom, highlightArea;
    }
    
    //コンストラクタ
    void OnEnable() {
        //Hierarchyに変化があった時にメソッドが呼ばれるように。
        EditorApplication.hierarchyWindowChanged += OnHierarchyChanged;
    }
    //コンストラクタ
    void OnDisable() {
        //Hierarchyに変化があった時にメソッドが呼ばれるように。
        EditorApplication.hierarchyWindowChanged -= OnHierarchyChanged;
    }


    //Hierarchyに変化があった
    private void OnHierarchyChanged() {
        GameObject[] gos = {hourDrum.gameObject, hourDrum.gameObject, secondDrum.gameObject};
        TextMeshProUGUI textMesh = gos.Select(g => g.GetComponentInChildren<TextMeshProUGUI>())
            .FirstOrDefault(tm => (tm != null));
        if (textMesh == null)
            return;

        float rollContentHeight = textMesh.GetComponent<RectTransform>().rect.height;
        if (rollContentHeight <= 0f)
            return;

        if (option.autoHighlightSizeAndPosition) {
            option.highlightArea.sizeDelta = new Vector2(option.highlightArea.sizeDelta.x, rollContentHeight);
        }

        if (option.autoBorderSizeAndPosition) {
            option.borderTop.offsetMin = new Vector2(option.borderTop.offsetMin.x, rollContentHeight / 2f);
            option.borderTop.sizeDelta = new Vector2(option.borderTop.sizeDelta.x, 2);
            option.borderBottom.offsetMax = new Vector2(option.borderBottom.offsetMax.x, -rollContentHeight / 2f);
            option.borderBottom.sizeDelta = new Vector2(option.borderBottom.sizeDelta.x, 2);
        }
    }

    /// <summary>
    /// ロールコンテンツの値が変更した時に呼び出される
    /// </summary>
    public void onChangedValue() {
        onChanged.InvokeSafe(
            int.Parse(hourDrum.SelectedContentText),
            int.Parse(minuteDrum.SelectedContentText),
            int.Parse(secondDrum.SelectedContentText));
    }

// 動的に数値を生成
//    [SerializeField] private GameObject _prefabRollContent;
//
//    public void Awake() {
//        RectTransform hContentRect= hourDrum.GetComponentsInChildren<RectTransform>().First(d => d.name.Equals("Content"));
//        RectTransform mContentRect= minuteDrum.GetComponentsInChildren<RectTransform>().First(d => d.name.Equals("Content"));
//        RectTransform sContentRect= secondDrum.GetComponentsInChildren<RectTransform>().First(d => d.name.Equals("Content"));
//        
//        for(int i = 1; i<=24;i++)
//            createRollContent(hContentRect,Instantiate(_prefabRollContent),i);
//        
//        for(int i = 1; i<=60;i++)
//            createRollContent(mContentRect,Instantiate(_prefabRollContent),i);
//        
//        for(int i = 1; i<=60;i++)
//            createRollContent(sContentRect,Instantiate(_prefabRollContent),i);
//        
//    }
//
//    private void createRollContent(RectTransform parentObject,GameObject rollContent,int num) {
//        rollContent.transform.parent = parentObject.gameObject.transform;
//        rollContent.GetComponent<TextMeshProUGUI>().text = num.ToString();
//    }

    public void setTime(int hour, int minute, int second) {
        StartCoroutine(setTimeAfterInit(hour, minute, second));
    }

    public void setTime(int seconds) {
        int hour = seconds / 3600;
        int minute = (seconds % 3600) / 60;
        int second = seconds % 60;
        setTime(hour, minute, second);
    }

    private IEnumerator setTimeAfterInit(int hour, int minute, int second) {
        while (hourDrum.initializing || minuteDrum.initializing || secondDrum.initializing) {
            yield return null;
        }

        hourDrum.SelectedContentText = hour.ToString();
        minuteDrum.SelectedContentText = minute.ToString();
        secondDrum.SelectedContentText = second.ToString();
    }

    public void setHour(int hour) {
        hourDrum.SelectedContentText = hour.ToString();
    }

    public void setMinute(int minute) {
        minuteDrum.SelectedContentText = minute.ToString();
    }

    public void setSecond(int second) {
        secondDrum.SelectedContentText = second.ToString();
    }
}