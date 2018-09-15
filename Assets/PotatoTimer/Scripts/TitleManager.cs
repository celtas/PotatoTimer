using UnityEngine;

public class TitleManager : MonoBehaviour {
	public GameObject[] sprites;

	void Start () {
		//入力イベントの登録
		InputManager.inputEvent += new InputEventHandler(onInput);
	}
	
	void Update () {
	}

	void onInput(object sender, InputEventArgs e) {
		if (e.State != InputState.SINGLE_TOUCH)
			return;

		foreach (GameObject sprite in sprites) {
			sprite.SetActive(false);
		}
		FrechflySpawner.instance.gameObject.SetActive(true);
	}
}
