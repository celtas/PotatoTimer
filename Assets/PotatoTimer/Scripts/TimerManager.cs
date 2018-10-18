using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour {
    public TimePicker timePicker;

    [SerializeField] private float _initTime, _elapsedTime;
    [SerializeField] private float[] _separeteCountdown = new float[3];
    [SerializeField] private float[] _separeteTime = new float[3];
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private ProgressRing[] _progressRings;
    [SerializeField] private ImageButton _startButton, _pauseButton, _resumeButton, _cancelButton;
    [SerializeField] private GameObject _timeDisplay, _timePicker, _cancelButtons, _playButtons;
    private Action _showTimerAction,_showPickerAction;

    private bool _enable;
    [SerializeField] private AudioClip _soundPotato;
    [SerializeField] private AudioSource _audioSource;

    IEnumerator Start () {
        yield return new WaitForEndOfFrame();
        registerEventListener();
        _cancelButton.DisableButton();
        setTimer(2, 2, 2);
    }

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

    // ゲームオブジェクトの状態を操作するアクションを登録
    private void registerEventListener() {
        _startButton.eventClicked.AddListener(() => {
            _startButton.gameObject.SetActive(false);
            _resumeButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            _cancelButton.EnableButton();

            _audioSource.Stop();
            _enable = true;
        });

        _pauseButton.eventClicked.AddListener(() => {
            _startButton.gameObject.SetActive(false);
            _resumeButton.gameObject.SetActive(true);
            _pauseButton.gameObject.SetActive(false);
            _cancelButton.EnableButton();

            _enable = false;
        });

        _resumeButton.eventClicked.AddListener(() => {
            _startButton.gameObject.SetActive(true);
            _resumeButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(false);
            _cancelButton.DisableButton();

            _startButton.eventClicked.InvokeSafe();
        });

        _cancelButton.eventClicked.AddListener(() => {
            _startButton.gameObject.SetActive(true);
            _resumeButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(false);
            _cancelButton.DisableButton();

            setTimer(200, 200, 200);
        });

        _showTimerAction = () => {
            _cancelButtons.SetActive(true);
            _playButtons.SetActive(true);

            _timePicker.SetActive(false);
            _timeDisplay.SetActive(true);
        };
        _showPickerAction = () => {
            _cancelButtons.SetActive(false);
            _playButtons.SetActive(false);

            _timePicker.SetActive(true);
            _timeDisplay.SetActive(false);
        };
    }

    // フッターメニュー
    public void clickFooterMenu(int index) {
        setTimer(2, 2, 2);
        Debug.Log(index);
    }

    public void setTimer(int second1, int second2, int second3) {
        _separeteCountdown[0] = second1;
        _separeteCountdown[1] = second2;
        _separeteCountdown[2] = second3;
        for (int i = 0; i < _separeteCountdown.Length; i++)
            _separeteTime[i] = _separeteCountdown[i];
        _enable = false;
        _initTime = Countdown;
        updateTimerDisplay();

        timePicker.setTime(second1);
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

    // Update is called once per frame
    void Update() {
        //タイマーが無効の場合
        if (!_enable)
            return;

        updateTime();

        if (Countdown <= 0)
            complate();

        updateTimerDisplay();
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

    public float Countdown {
        get { return _separeteCountdown.Sum(); }
    }
}