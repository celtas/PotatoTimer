using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void InputEventHandler(object sender, InputEventArgs e);
public enum InputState{SINGLE_TOUCH,DOUBLE_TOUCH,RELEASE,MOVE,HOLD}

public class InputManager : MonoBehaviour {
	public static event InputEventHandler inputEvent;
	
	void Start () {
	}
	
	void Update () {
		if (inputEvent == null)
			return;
		
		UpdateMouseEvent();
		UpdateTouchEvent();
	}

	//マルチタッチ処理
	void UpdateTouchEvent() {
		Touch[] touches = Input.touches;
		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch(i);
			inputEvent(this, new InputEventArgs(InputState.SINGLE_TOUCH, touch.position));
		}
	}

	//マウス処理
	void UpdateMouseEvent() {
		if (Input.GetMouseButtonDown(0))	inputEvent(this, new InputEventArgs(InputState.SINGLE_TOUCH,Input.mousePosition));
		else if (Input.GetMouseButton(0)) 	inputEvent(this, new InputEventArgs(InputState.HOLD,Input.mousePosition));
		
		if (Input.GetMouseButtonDown(1))	inputEvent(this, new InputEventArgs(InputState.DOUBLE_TOUCH,Input.mousePosition));
//		if (Input.GetMouseButtonUp(0))		inputEvent(this, new InputEventArgs("左クリック終わり"));
//		if (Input.GetMouseButtonUp(1))		inputEvent(this, new InputEventArgs("右クリック終わり"));
//		if (Input.GetMouseButtonUp(2))		inputEvent(this, new InputEventArgs("ホイールクリック終わり"));
	}
}

public class InputEventArgs: EventArgs {
	private readonly InputState _state;
	private readonly Vector2 _position;

	public InputEventArgs(InputState state,Vector2 position) {
		this._state = state;
		this._position = position;
	}

	public Vector2 Position {
		get { return _position; }
	} 

	public InputState State {
		get { return _state; }
	}
}