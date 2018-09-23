using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProgaressBar : MonoBehaviour {
	private RectTransform _rect;
	
	void Awake () {
		_rect = gameObject.GetComponent<RectTransform>();
	}

	public void start(int countdown) {
//		_rect.DOMoveX (
//			_endPosX,　　//移動後の座標
//			countdown　　　　//時間
//		);
	}
}
