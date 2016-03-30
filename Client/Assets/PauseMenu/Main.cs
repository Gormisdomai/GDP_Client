using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	string gameScene = "Test_Scene";

	public void Continue() {
		SceneManager.LoadScene(gameScene);
		//SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameScene))
	}

	// Use this for initialization
	void Start () {

	}

	private void Update() {
		if (Input.GetKeyDown("escape")) {
			SceneManager.LoadScene(gameScene);
		}
	}

}
