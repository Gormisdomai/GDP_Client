using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	string gameScene = "Test_Scene";

	public void start() {
		SceneManager.LoadScene(gameScene);
		PlayerPrefs.SetString ("name", "Test_name");
	}

	public void highScores() {
		SceneManager.LoadScene("High_Scores");
	}

	private void Update()
	{
		if (Input.GetKeyDown("escape")) {
    		Application.Quit();
		}
	}
}
