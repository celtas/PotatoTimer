using System;
using System.Timers;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour {
	[SerializeField] private float _countdown,_initTime;
	[SerializeField] private TextMeshProUGUI _uiMinutes;
	[SerializeField] private TextMeshProUGUI _uiSeconds;
	[SerializeField] private ProgressRing[] _progressRings;
	
	private bool _enableTimer;
	private AudioClip _soundPotato;
	private AudioSource _audioSource;
	
	private void Awake() {
		_enableTimer = false;
		_audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void StartTimer(int second) {
		_audioSource.Stop();
		_countdown = second;
		_initTime = second;
		_enableTimer = true;
	}

	// タイマーの文字更新
	void updateTimerDisplay() {
		//カウンタが0の時
		if (_countdown < 0f) {
			_uiMinutes.text = "00";
			_uiSeconds.text = "00";
			_progressRings[0].updateDisplay("0",1f);
			return;
		}
			
		_uiMinutes.text = Convert.ToString(RoundMinutes(_countdown)).PadLeft(2,'0');
		_uiSeconds.text = Convert.ToString(RoundSeconds(_countdown)).PadLeft(2,'0');
		
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
