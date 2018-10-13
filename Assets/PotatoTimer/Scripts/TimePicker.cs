using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class TimePicker : MonoBehaviour {
    public DrumScrollRect hourDrum, minuteDrum, secondDrum;
    
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
        hourDrum.SelectedContentText = hour.ToString();
        minuteDrum.SelectedContentText = minute.ToString();
        secondDrum.SelectedContentText = second.ToString();
    }

    public void setTime(int seconds) {
        int hour = seconds / 3600;
        int minute = (seconds / 3600) / 60;
        int second = seconds % 60;
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

    public String getHour() {
        return hourDrum.SelectedContentText;
    }

    public String getMinute() {
        return minuteDrum.SelectedContentText;
    }

    public String getSecond() {
        return minuteDrum.SelectedContentText;
    }
}