using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpikeSpawner : MonoBehaviour {

	[SerializeField] public GameObject spike;
	//TODO, change this type to support mean and SD
	private Queue<float[]> spikesToDraw = new Queue<float[]> ();
	private Queue<GameObject> spikesDrawnTop = new Queue<GameObject>();
	private Queue<GameObject> spikesDrawnBottom = new Queue<GameObject>();

	public float despawnX;
	public float spawnX;
	public float speed;

	public GameObject square;
	public GameObject triangle;

	// generates rectangle with bottom corners (l,y) and (r,y)
	GameObject genRectTop(float l, float r, float y) {
		GameObject rect = (GameObject) Instantiate(square, new Vector2 ((l+r)/2,(y+6)/2), Quaternion.identity);
		rect.transform.localScale = new Vector2((r-l)/2,(6-y)/2);
		rect.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,0);
		return rect;
	}

	GameObject lastTop;
	GameObject genSpikeTop(float upper) { //add triangle
		float y = upper * 5;
		if (lastTop != null) {
			Vector2 last = getCornerTop(lastTop);
			lastTop = genRectTop(last.x,spawnX,Math.Max(y,last.y));
		}
		else {
			lastTop = genRectTop(despawnX,spawnX,y);
		}
		return lastTop;
	}

	Vector2 getCornerTop(GameObject obj) { //assume rect for now
		Vector2 scale = obj.transform.localScale;
		Vector2 pos = obj.transform.localPosition;
		return new Vector2(scale.x + pos.x, 2*pos.y - 6);
	}

	// deletes rectangles which have disappeared off the screen
	void deleteOldObjectsTop() {
		while (getCornerTop(spikesDrawnTop.Peek()).x < despawnX) {
			Destroy (spikesDrawnTop.Dequeue());
		}
	}

	// generates rectangle with top corners (l,y) and (r,y)
	public void f() {
		genRectBottom(2,4,-2.5f);
	}
	GameObject genRectBottom(float l, float r, float y) {
		GameObject rect = (GameObject) Instantiate(square, new Vector2 ((l+r)/2,(y-6)/2), Quaternion.identity);
		rect.transform.localScale = new Vector2((r-l)/2,(6+y)/2);
		rect.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,0);
		return rect;
	}

	GameObject lastBottom;
	GameObject genSpikeBottom(float lower) { //add triangle
		float y = lower * 5;
		if (lastBottom != null) {
			Vector2 last = getCornerBottom(lastBottom);
			lastBottom = genRectBottom(last.x,spawnX,Math.Min(y,last.y));
		}
		else {
			lastBottom = genRectBottom(despawnX,spawnX,y);
		}
		return lastBottom;
	}

	Vector2 getCornerBottom(GameObject obj) { //assume rect for now
		Vector2 scale = obj.transform.localScale;
		Vector2 pos = obj.transform.localPosition;
		return new Vector2(scale.x + pos.x, 2*pos.y + 6);
	}

	// deletes rectangles which have disappeared off the screen
	void deleteOldObjectsBottom() {
		while (getCornerBottom(spikesDrawnBottom.Peek()).x < despawnX) {
			Destroy (spikesDrawnBottom.Dequeue());
		}
	}

	// Update is called once per frame
	void Update () {
		if (spikesToDraw.Count > 0) { // possible issues if no spikes to draw, but doesnt seem to happen
			print ("Spike drawn");
			float[] data = spikesToDraw.Dequeue();
			float diff = data [0]; // tick - mean
			float sd = data[1];
			float upper = (diff/sd +1)/5;
			float lower = (diff/sd -1)/5;
			spikesDrawnTop.Enqueue(genSpikeTop(upper)); // upper, lower expect values between -1 (very bottom of screen) and 1 (top)
			spikesDrawnBottom.Enqueue(genSpikeBottom(lower));
			deleteOldObjectsTop(); deleteOldObjectsBottom();
		}
	}

	//TODO change this to support mean and SD
	public void addSpike(float[] data) {
		print ("spike queued: (" + data[0] + ", " + data[1] + ")");
		spikesToDraw.Enqueue(data);
	}
}

// Very early implementation of solid walls, will look a lot better when texture changed, smoother and perhaps server sending data more frequently.
// Very much work in progress, but decided to push before going to bed so people can work with it.
// For Ryan/Vasil, your interaction with the rest of the program should now (probably) be entirely through setting the correct values for upper and lower in lines 67-68, based on the data in the spikesToDraw queue
// I shall get straight back to work on this in the morning.
