using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpikeSpawner : MonoBehaviour {

	[SerializeField] public GameObject spike;
	//TODO, change this type to support mean and SD
	private Queue<float[]> spikesToDraw = new Queue<float[]> ();
	private Queue<GameObject> spikesDrawn = new Queue<GameObject>();

	public float rate = 10;
	public float elapsed = 0;
	public float despawnX = 0;
	public float spawnX = 100;
	public float spawnY = 0;
	public float speed = 3;

	/** Returns the height of the next spike. */
	float genHeight() {
		float[] data = spikesToDraw.Dequeue();
		float average = data [0];
		float sd = data[1];
		return (average + 50*sd);
	}

	/** Generates and returns a new spike object at the given location in the frame. */
	GameObject genSpike(float new_spawnY, float height) {
		GameObject s = Instantiate (spike, new Vector2 (spawnX, new_spawnY), Quaternion.identity) as GameObject;
		s.GetComponent<SpriteRenderer> ().transform.localScale = new Vector3 (s.GetComponent<SpriteRenderer> ().transform.localScale.x, height);
		return s;
	}
		
	// Update is called once per frame
	void Update () {
		foreach (GameObject s in spikesDrawn) {
			s.transform.position -= new Vector3(Time.deltaTime * speed, 0);

		}

		elapsed += Time.deltaTime;
		if (elapsed > rate) {
			if (spikesToDraw.Count > 0) {
				print ("Spike drawn");
				elapsed = 0;
				float height = genHeight ();
				spikesDrawn.Enqueue (genSpike (spawnY, height));
				spikesDrawn.Enqueue (genSpike (1 - spawnY, -height));
			}

			if (spikesDrawn.Count > 0 && spikesDrawn.Peek ().transform.position.x < despawnX) {
				Destroy (spikesDrawn.Dequeue ());
				Destroy (spikesDrawn.Dequeue ());
			}
		}
	}

	//TODO change this to support mean and SD
	public void addSpike(float[] data) {
		print ("spike queued: (" + data[0] + ", " + data[1] + ")");
		spikesToDraw.Enqueue(data);
	}
}
