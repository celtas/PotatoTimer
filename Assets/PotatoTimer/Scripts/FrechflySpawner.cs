using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrechflySpawner : MonoBehaviour {
	static public FrechflySpawner instance;
	
	public GameObject frenchfly;

	private Vector2 position;
	private int count = 0;
	
	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
			gameObject.SetActive(false);
		} else {
			Destroy (gameObject);
		}
	}

	void Start () {
		position = transform.position;
	}
	
	void Update () {
		if (count > 290)
			return;

		//ポテトを生成
		for (int i = 0; i < 6; i++) {
			float x = position.x + Random.Range(-13, 13);
			Instantiate(frenchfly, new Vector3(x, position.y, 0), Quaternion.identity);
			count++;
		}
	}
}
