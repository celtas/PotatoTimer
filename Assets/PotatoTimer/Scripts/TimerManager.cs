using System;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour {
	[SerializeField] private float _countdown,_initTime;
	[SerializeField] private TextMeshProUGUI _timeText;
	[SerializeField] private ProgressRing[] _progressRings;
	[SerializeField] private CircleButton _btnStart, _btnPause, _btnResume,_btnCancel;
	
	private bool _enableTimer;
	[SerializeField] private AudioClip _soundPotato;
	[SerializeField] private AudioSource _audioSource;
	
	private void Start() {
		registerListener();
		_btnCancel.DisableButton();
		setTimer(600);
	}
	void registerListener() {
		_btnStart._eventReleased.AddListener(() => {
			_btnStart.gameObject.SetActive(false);
			_btnResume.gameObject.SetActive(false);
			_btnPause.gameObject.SetActive(true);
			_btnCancel.EnableButton();
			
			_audioSource.Stop();
			_enableTimer = true;
		});

		_btnPause._eventReleased.AddListener(() => {
			_btnStart.gameObject.SetActive(false);
			_btnResume.gameObject.SetActive(true);
			_btnPause.gameObject.SetActive(false);
			_btnCancel.EnableButton();
			
			_enableTimer = false;
		});

		_btnResume._eventReleased.AddListener(() => {
			_btnStart.gameObject.SetActive(true);
			_btnResume.gameObject.SetActive(false);
			_btnPause.gameObject.SetActive(false);
			_btnCancel.DisableButton();
			
			_btnStart._eventReleased.InvokeSafe();
		});
		
		_btnCancel._eventReleased.AddListener(() => {
			_btnStart.gameObject.SetActive(true);
			_btnResume.gameObject.SetActive(false);
			_btnPause.gameObject.SetActive(false);
			_btnCancel.DisableButton();
			
			setTimer(600);
		});
	}

	public void setTimer(int second) {
		_enableTimer = false;
		_countdown = second;
		_initTime = second;
		updateTimerDisplay();
	}

	// タイマーの文字更新
	void updateTimerDisplay() {
		//カウンタが0の時
		if (_countdown < 0f) {
			_timeText.text = "00 00";
			_progressRings[0].updateDisplay("0",1f);
			return;
		}
			
		_timeText.text = Convert.ToString(RoundMinutes(_countdown)).PadLeft(2,'0') + " " + 
		                 Convert.ToString(RoundSeconds(_countdown)).PadLeft(2,'0');
		
		_progressRings[0].updateDisplay(Convert.ToString(Math.Round(_countdown)),1 - _countdown / _initTime);
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
		float remainder = _countdown % 60;
		
		if(remainder > 59.5f)
			return (int) (Math.Floor(_countdown / 60)+1);
		
		return (int) Math.Floor(_countdown / 60);
	}
	
	// Update is called once per frame
	void Update () {
		//タイマーが無効の場合
		if (!_enableTimer)
			return;

		_countdown -= Time.deltaTime;
		updateTimerDisplay();
		
		if (_countdown <= 0)
			complate();
	}

	void complate() {
		_enableTimer = false;
		_audioSource.clip = _soundPotato;
		_audioSource.Play ();
	}
}
