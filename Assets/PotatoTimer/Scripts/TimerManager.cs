using System;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEditor;

public class TimerManager : MonoBehaviour {
    #region #Field
    [SerializeField] private float _initTime, _elapsedTime;
    [SerializeField] private float[] _separeteCountdown = new float[3];
    [SerializeField] private int[] _separeteTime = new int[3];
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private ProgressRing[] _progressRings;
    [SerializeField] private ImageButton _startButton, _pauseButton, _resumeButton, _cancelButton, _modifyButton;

    [SerializeField] private GameObject _timerObjects;

    // 各タイマー
    [SerializeField] private RectTransform[] _contents;

    public TimePicker timePicker;

    //　選択しているタイマー番号
    private int _selectContentIndex = -1;

    private bool _enable;
    [SerializeField] private AudioClip _soundPotato;
    [SerializeField] private AudioSource _audioSource;
    
    #endregion

    #region #Property
    /// <summary>
    ///   <para>ボトムエリアの表示状態</para>
    /// </summary>
    [SerializeField] private BottomAreaDisplayType _displayType;
    
    public BottomAreaDisplayType displayType {
        get { return _displayType; }
        set {
            _displayType = value;
            switch (_displayType) {
                case BottomAreaDisplayType.HIDDEN:
                    timerStatus = TimerStatus.STOP_AND_HIDDEN;

                    timePicker.gameObject.SetActive(false);
                    _timerObjects.SetActive(false);
                    break;
                case BottomAreaDisplayType.TIMER:
                    timerStatus = TimerStatus.STOP;

                    timePicker.gameObject.SetActive(false);
                    _timerObjects.gameObject.SetActive(true);
                    break;
                case BottomAreaDisplayType.PICKER:
                    timerStatus = TimerStatus.STOP_AND_HIDDEN;
                    _modifyButton.gameObject.SetActive(true);
                    _cancelButton.gameObject.SetActive(true);
                    _cancelButton.EnableButton();

                    timePicker.gameObject.SetActive(true);
                    _timerObjects.SetActive(false);
                    timePicker.setTime(_separeteTime[_selectContentIndex]);
                    break;
            }
        }
    }

    /// <summary>
    ///   <para>タイマーの状態</para>
    /// </summary>
    [SerializeField] private TimerStatus _timerStatus = TimerStatus.STOP;

    public TimerStatus timerStatus {
        get { return _timerStatus; }
        set {
            _timerStatus = value;
            initButtons();
            switch (_timerStatus) {
                case TimerStatus.PLAY:
                    _pauseButton.gameObject.SetActive(true);
                    _cancelButton.EnableButton();
                    break;
                case TimerStatus.PAUSE:
                    _resumeButton.gameObject.SetActive(true);
                    _cancelButton.EnableButton();
                    break;
                case TimerStatus.STOP:
                    _startButton.gameObject.SetActive(true);
                    break;
                case TimerStatus.STOP_AND_HIDDEN:
                    _cancelButton.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public float Countdown {
        get { return _separeteCountdown.Sum(); }
    }
    
    #endregion
    
    #region #Event

    void OnTimePickerChanged(int h, int m, int s) {
        if (_selectContentIndex < 0)
            return;

        _separeteTime[_selectContentIndex] = h * 3600 + m * 60 + s;
        _separeteCountdown[_selectContentIndex] = _separeteTime[_selectContentIndex];
        _initTime = Countdown;
        updateTimerDisplay();
    }

    void OnValidate() {
        if (EditorApplication.isPlaying)
            return;

        registerEventListener();
        displayType = _displayType;
    }
    
    // コンテンツメニュー
    public void OnClickContentMenu(int clickContentIndex) {
        _selectContentIndex = clickContentIndex;
        switch (clickContentIndex) {
            case -1:
                displayType = BottomAreaDisplayType.TIMER;
                break;
            case 0:
            case 1:
            case 2:
                for (int index = 0; index < _contents.Length; index++) {
                    RectTransform rectTransform = _contents[index];
                    // タッチされた要素
                    if (index == clickContentIndex) {
                        rectTransform.DOLocalMoveY(0, 1.5f).SetEase(Ease.OutQuint);
                    }
                    else {
                        rectTransform.DOLocalMoveY(rectTransform.rect.height, 1.5f).SetEase(Ease.OutQuint);
                    }
                }

                displayType = BottomAreaDisplayType.PICKER;
                break;
        }
    }

    // フッターメニュー
    public void OnClickFooterMenu(int clickContentIndex) {
        switch (clickContentIndex) {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    
    #endregion

    #region #UnityEvent
    
    private void Awake() {
        registerEventListener();
        timePicker.onChanged += OnTimePickerChanged;
    }

    private void Start() {
        setTimer(120, 240, 180);
    }

    void Update() {
        //タイマーが無効の場合
        if (!_enable)
            return;

        updateTime();

        if (Countdown <= 0)
            complate();

        updateTimerDisplay();
    }
    
    #endregion
    
    #region #NativePlugin
    /*
    private void invokeNative(){
        AndroidJavaObject jo = new AndroidJavaObject("java.lang.String", "some string");
        int hash = jo.Call<int>("hashCode");
    
        AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject> ("currentActivity");

        activity.Call ("runOnUiThread", new AndroidJavaRunnable (() => {
            AndroidJavaObject alertDialogBuilder = new AndroidJavaObject ("android.app.AlertDialog$Builder", activity);
            alertDialogBuilder.Call<AndroidJavaObject> ("setMessage", "message");
            alertDialogBuilder.Call<AndroidJavaObject> ("setCancelable", true);
            alertDialogBuilder.Call<AndroidJavaObject> ("setPositiveButton", "OK", new PositiveButtonListner(this));
            AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject> ("create");
            dialog.Call ("show");
        }));
    }
    */
    #endregion

    #region #Initial
    
    private void registerEventListener() {
        // ボタンクリック時
        _startButton.clickEvent.AddListener(() => {
            timerStatus = TimerStatus.PLAY;
            _audioSource.Stop();
            _enable = true;
        });

        _pauseButton.clickEvent.AddListener(() => {
            timerStatus = TimerStatus.PAUSE;
            _enable = false;
        });

        _resumeButton.clickEvent.AddListener(() => {
            timerStatus = TimerStatus.PLAY;
            _startButton.clickEvent.InvokeSafe();
        });

        _cancelButton.clickEvent.AddListener(() => {
            switch (_displayType) {
                case BottomAreaDisplayType.TIMER:
                    timerStatus = TimerStatus.STOP;
                    setTimer(_separeteTime[0], _separeteTime[1], _separeteTime[2]);
                    break;
                case BottomAreaDisplayType.PICKER:
                    // 要素の位置を元に戻す
                    for (int index = 0; index < _contents.Length; index++) {
                        RectTransform rectTransform = _contents[index];
                        rectTransform.DOLocalMoveY(-rectTransform.rect.height * index, 1.5f).SetEase(Ease.OutQuint);
                    }
                    displayType = BottomAreaDisplayType.TIMER;
                    break;
            }
        });

        _modifyButton.clickEvent.AddListener(() => {
            timerStatus = TimerStatus.STOP;
            setTimer(_separeteTime[0], _separeteTime[1], _separeteTime[2]);
        });
    }

    private void initButtons() {
        _startButton.gameObject.SetActive(false);
        _resumeButton.gameObject.SetActive(false);
        _pauseButton.gameObject.SetActive(false);
        _modifyButton.gameObject.SetActive(false);
        _cancelButton.gameObject.SetActive(true);
        _cancelButton.DisableButton();
    }
    
    #endregion
    
    #region #TimerFunction

    public void setTimer(int second1, int second2, int second3) {
        _separeteCountdown[0] = second1;
        _separeteCountdown[1] = second2;
        _separeteCountdown[2] = second3;
        for (int i = 0; i < _separeteCountdown.Length; i++)
            _separeteTime[i] = (int) _separeteCountdown[i];
        _initTime = Countdown;
        updateTimerDisplay();

        _enable = false;
        displayType = BottomAreaDisplayType.TIMER;
    }

    // タイマーの文字更新
    void updateTimerDisplay() {
        _timeText.text = Convert.ToString(RoundMinutes(Countdown)).PadLeft(2, '0') + " " +
                         Convert.ToString(RoundSeconds(Countdown)).PadLeft(2, '0');

        for (int i = 0; i < _separeteCountdown.Length; i++)
            _progressRings[i].updateDisplay(Convert.ToString(Math.Round(_separeteCountdown[i])),
                (_separeteTime[i] - _separeteCountdown[i]) / _separeteTime[i]);
    }

    // 秒を四捨五入するメソッド
    int RoundSeconds(float seconds) {
        seconds = seconds % 60;

        if (seconds > 59.5f || seconds < 0.5f)
            return 0;
        return (int) Math.Round(seconds);
    }

    // 分を四捨五入するメソッド
    int RoundMinutes(float seconds) {
        float remainder = Countdown % 60;

        if (remainder > 59.5f)
            return (int) (Math.Floor(Countdown / 60) + 1);

        return (int) Math.Floor(Countdown / 60);
    }

    // タイマーの更新
    void updateTime() {
        float remainder = Time.deltaTime;
        for (int i = 0; i < _separeteCountdown.Length; i++) {
            if (_separeteCountdown[i] <= 0)
                continue;

            _separeteCountdown[i] -= remainder;
            if (_separeteCountdown[i] >= 0)
                break;

            remainder = -_separeteCountdown[i];
            _separeteCountdown[i] = 0;
        }

        _elapsedTime = _initTime - Countdown;
    }
    
    void complate() {
        _separeteCountdown[0] = 0;
        _separeteCountdown[1] = 0;
        _separeteCountdown[2] = 0;
        _elapsedTime = _initTime;
        _enable = false;
        _audioSource.clip = _soundPotato;
        _audioSource.Play();
    }
    
    #endregion
    
    #region #Enum

    public enum BottomAreaDisplayType {
        /// <summary>
        ///   <para>タイムピッカー、タイマーを非表示にする</para>
        /// </summary>
        HIDDEN,

        /// <summary>
        ///   <para>タイマーを表示する</para>
        /// </summary>
        TIMER,

        /// <summary>
        ///   <para>タイムピッカーを表示する</para>
        /// </summary>
        PICKER,
    }

    public enum TimerStatus {
        PLAY,
        PAUSE,
        STOP,
        STOP_AND_HIDDEN,
    }
    
    #endregion
}