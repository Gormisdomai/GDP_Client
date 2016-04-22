using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;




public class SpikeSpawner : MonoBehaviour {

	[SerializeField] public GameObject spike;
	public Queue<float> spikesToDraw;
	private Queue<GameObject> spikesDrawn = new Queue<GameObject>();
	private float elapsed = 0;
	public float rate = 10;
	public float despawnX = 0;
	public float spawnX = 100;
	public float spawnY = 0;
	public float speed= 3;

	// Use this for initialization
	void Start () {
		spikesToDraw = new Queue<float>();
		/*spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (2);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (3);
		spikesToDraw.Enqueue (1);
		spikesToDraw.Enqueue (2);)*/
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject s in spikesDrawn) {
			s.transform.position -= new Vector3(Time.deltaTime * speed, 0);

		}

		elapsed += Time.deltaTime;
		if (elapsed > rate) {
			elapsed = 0;
			if (spikesToDraw.Count > 0) {
				GameObject s = Instantiate (spike, new Vector3 (spawnX, spawnY), Quaternion.identity) as GameObject;
				s.GetComponent<SpriteRenderer> ().transform.localScale = new Vector3 (s.GetComponent<SpriteRenderer> ().transform.localScale.x, (spikesToDraw.Dequeue ()-1)*6);
				spikesDrawn.Enqueue (s);
			}

			if (spikesDrawn.Count > 0 && spikesDrawn.Peek ().transform.position.x < despawnX) {
				Destroy (spikesDrawn.Dequeue ());
			}
		}
	}
}
