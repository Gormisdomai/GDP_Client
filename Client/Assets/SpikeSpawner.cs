using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpikeSpawner : MonoBehaviour {

	[SerializeField] public GameObject spike;
<<<<<<< HEAD
	private double[][] spikesToDraw;
	//private Queue<double[]> spikesDrawn = new Queue<double[]> ();
=======
	public Queue<float> spikesToDraw;
>>>>>>> 82b230c091dc09f75c21d1faa25696d4f7642c80
	private Queue<GameObject> spikesDrawn = new Queue<GameObject>();
	private float elapsed = 0;
	public float rate = 10;
	public float despawnX = 0;
	public float spawnX = 100;
	public float spawnY = 0;
	public float speed= 3;

	private int i = 0;

	// Use this for initialization
	void Start () {
<<<<<<< HEAD
		spikesToDraw = new double[6][];
		spikesToDraw[0] = new double[2]{1.55324, 0.011111929253036557};
		spikesToDraw[1] = new double[2]{1.556865, 0.011530722814396703};
		spikesToDraw[2] = new double[2]{1.555535, 0.011847792580502569};
		spikesToDraw[3] = new double[2]{1.55717, 0.012294357934866372};
		spikesToDraw[4] = new double[2]{1.553435, 0.012530010741969735};
		spikesToDraw[5] = new double[2]{1.555165, 0.012792830638881347};
	}
		
	/** Returns the height of the next spike. */
	float genHeight() {
		i = (i + 1) % 6;
		double average = spikesToDraw[i][0];
		double sd = spikesToDraw [i][1];
		return (float)(average - 1*sd);
	}

	/** Generates and returns a new spike object at top of the frame. */
	GameObject genTopSpike() {
		GameObject s = Instantiate (spike, new Vector2 (spawnX, spawnY), Quaternion.identity) as GameObject;
		s.GetComponent<SpriteRenderer> ().transform.localScale = new Vector3 (s.GetComponent<SpriteRenderer> ().transform.localScale.x, genHeight());
		return s;
=======
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
>>>>>>> 82b230c091dc09f75c21d1faa25696d4f7642c80
	}

	/** Generates and returns a new spike object at the bottom of the frame. */
	GameObject genBottomSpike() {
		GameObject s = Instantiate (spike, new Vector2 (spawnX, 1 - spawnY), Quaternion.identity) as GameObject;
		s.GetComponent<SpriteRenderer> ().transform.localScale = new Vector3 (s.GetComponent<SpriteRenderer> ().transform.localScale.x, -genHeight ());
		return s;
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
			elapsed = 0;
<<<<<<< HEAD
			float height = genHeight ();
			/*if (spikesToDraw.Count > 0) {
				spikesDrawn.Enqueue (genTopSpike ());
				spikesDrawn.Enqueue (genBottomSpike ());
			}*/

			spikesDrawn.Enqueue (genSpike (spawnY, height));
			spikesDrawn.Enqueue (genSpike (1 - spawnY, -height));
=======
			if (spikesToDraw.Count > 0) {
				GameObject s = Instantiate (spike, new Vector3 (spawnX, spawnY), Quaternion.identity) as GameObject;
				s.GetComponent<SpriteRenderer> ().transform.localScale = new Vector3 (s.GetComponent<SpriteRenderer> ().transform.localScale.x, (spikesToDraw.Dequeue ()-1)*6);
				spikesDrawn.Enqueue (s);
			}
>>>>>>> 82b230c091dc09f75c21d1faa25696d4f7642c80

			if (spikesDrawn.Count > 0 && spikesDrawn.Peek ().transform.position.x < despawnX) {
				Destroy (spikesDrawn.Dequeue ());
				Destroy (spikesDrawn.Dequeue ());
			}
		}
	}
}
