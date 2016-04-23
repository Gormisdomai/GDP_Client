using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpikeSpawner : MonoBehaviour {

	[SerializeField] public GameObject spike;
	//TODO, change this type to support mean and SD
	private Queue<float> spikesToDraw = new Queue<float> ();
	private Queue<GameObject> spikesDrawn = new Queue<GameObject>();

	public float rate = 10;
	public float elapsed = 0;
	public float despawnX = 0;
	public float spawnX = 100;
	public float spawnY = 0;
	public float speed= 3;

	//private int i = 0;

	// Use this for initialization
	void Start () {
		/*spikesToDraw = new double[6][];
		spikesToDraw.Enqueue(new double[2]{1.55324, 0.011111929253036557});
		spikesToDraw[1] = new double[2]{1.556865, 0.011530722814396703};
		spikesToDraw[2] = new double[2]{1.555535, 0.011847792580502569};
		spikesToDraw[3] = new double[2]{1.55717, 0.012294357934866372};
		spikesToDraw[4] = new double[2]{1.553435, 0.012530010741969735};
		spikesToDraw[5] = new double[2]{1.555165, 0.012792830638881347};*/
	}
	/** A temporary method to fill the queue. *
	void fill() {

	}*/

	/** Returns the height of the next spike. */
	float genHeight() {
		//i = (i + 1) % 6;
		double average = spikesToDraw.Dequeue();
		//double sd = spikesToDraw [i][1];
		return (float)(average);
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

			//spikesDrawn.Enqueue (genSpike (spawnY, height));
			//spikesDrawn.Enqueue (genSpike (1 - spawnY, -height));

			if (spikesDrawn.Count > 0 && spikesDrawn.Peek ().transform.position.x < despawnX) {
				Destroy (spikesDrawn.Dequeue ());
				Destroy (spikesDrawn.Dequeue ());
			}
		}
	}

	//TODO change this to support mean and SD
	public void addSpike(float mean) {
		print ("spike queued:" + mean);
		spikesToDraw.Enqueue(mean);
	}
}
