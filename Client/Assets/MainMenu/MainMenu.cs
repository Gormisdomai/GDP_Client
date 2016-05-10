using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	string gameScene = "Test_Scene";

	private void Awake()
	{
		GameObject.Find("InputField").GetComponent<InputField>().text = PlayerPrefs.GetString ("name", "anon");

	}

	public void start() {
		SceneManager.LoadScene(gameScene);
		PlayerPrefs.SetString ("name", GameObject.Find("InputField").GetComponent<InputField>().text);
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
