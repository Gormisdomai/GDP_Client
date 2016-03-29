using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	string gameScene = "Test_Scene";
	public void start() {
		SceneManager.LoadScene(gameScene);
		//SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}
}
