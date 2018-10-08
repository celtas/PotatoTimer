using System;
using System.Linq;
using System.Timers;
using FantomLib;
using PotatoTimer.Scripts;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour {
    [SerializeField] private float _initTime, _elapsedTime;
    [SerializeField] private float[] _separeteCountdown = new float[3];
    [SerializeField] private float[] _separeteTime = new float[3];
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private ProgressRing[] _progressRings;
    [SerializeField] private ImageButton _btnStart, _btnPause, _btnResume, _btnCancel;

    private bool _enableTimer;
    [SerializeField] private AudioClip _soundPotato;
    [SerializeField] private AudioSource _audioSource;

    private void Start() {
        registerEventListener();
        _btnCancel.DisableButton();
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

    // アクションを登録
    private void registerEventListener() {
        _btnStart.eventClicked.AddListener(() => {
            _btnStart.gameObject.SetActive(false);
            _btnResume.gameObject.SetActive(false);
            _btnPause.gameObject.SetActive(true);
            _btnCancel.EnableButton();

            _audioSource.Stop();
            _enableTimer = true;
        });

        _btnPause.eventClicked.AddListener(() => {
            _btnStart.gameObject.SetActive(false);
            _btnResume.gameObject.SetActive(true);
            _btnPause.gameObject.SetActive(false);
            _btnCancel.EnableButton();

            _enableTimer = false;
        });

        _btnResume.eventClicked.AddListener(() => {
            _btnStart.gameObject.SetActive(true);
            _btnResume.gameObject.SetActive(false);
            _btnPause.gameObject.SetActive(false);
            _btnCancel.DisableButton();

            _btnStart.eventClicked.InvokeSafe();
        });

        _btnCancel.eventClicked.AddListener(() => {
            _btnStart.gameObject.SetActive(true);
            _btnResume.gameObject.SetActive(false);
            _btnPause.gameObject.SetActive(false);
            _btnCancel.DisableButton();

            setTimer(200, 200, 200);
        });
    }

    // フッターメニュー
    public void clickFooterMenu(int index) {
        Debug.Log(index);
    }

    public void setTimer(int second1, int second2, int second3) {
        _separeteCountdown[0] = second1;
        _separeteCountdown[1] = second2;
        _separeteCountdown[2] = second3;
        for (int i = 0; i < _separeteCountdown.Length; i++)
            _separeteTime[i] = _separeteCountdown[i];
        _enableTimer = false;
        _initTime = Countdown;
        updateTimerDisplay();
    }

    // タイマーの文字更新
    void updateTimerDisplay() {
        _timeText.text = Convert.ToString(RoundMinutes(Countdown)).PadLeft(2, '0') + " " +
                         Convert.ToString(RoundSeconds(Countdown)).PadLeft(2, '0');

        for(int i = 0; i < _separeteCountdown.Length; i++)
            _progressRings[i].updateDisplay(Convert.ToString(Math.Round(_separeteCountdown[i])), (_separeteTime[i] - _separeteCountdown[i]) / _separeteTime[i]);
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
        if (!_enableTimer)
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
        _enableTimer = false;
        _audioSource.clip = _soundPotato;
        _audioSource.Play();
    }
    
    public float Countdown {
        get {
            return _separeteCountdown.Sum();
        }
    }
}