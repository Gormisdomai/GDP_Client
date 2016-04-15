using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HighScores : MonoBehaviour {

	public void backToMain() {
		SceneManager.LoadScene("MainMenu");
		//SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameScene));
		//SceneManager.LoadScene(gameScene);
	}

	// Use this for initialization
	void Start () {

	}

	private void Update() {
	}

}
