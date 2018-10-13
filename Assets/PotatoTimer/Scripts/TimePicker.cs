using System;
using UnityEngine;

public class TimePicker : MonoBehaviour {
	public DrumScrollRect hourDrum, minuteDrum, secondDrum;

	public void setTime(int hour,int minute,int second) {
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