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
	public float SFadd;
	public float SFmult;
	public float limit; // highest absolute value to draw walls at
	public float serverRate; // data points sent per second
	public float barWidth;
	float dist; // x distance between points (= speed/serverRate)

	public GameObject square;
	public GameObject triangle;
	public GameObject squareBlack;

	// generates triangle with vertices (x1,y1) and (x2,y2)
	GameObject genTriTop(float x1, float y1, float x2, float y2) {
		GameObject tri = (GameObject) Instantiate(triangle, new Vector2 ((x1+x2)/2,(y1+y2)/2), Quaternion.identity);
		tri.transform.localScale = new Vector2((x1-x2)/2, (y2-y1)/2);
		if (y2 > y1) {
			tri.GetComponent<SpriteRenderer>().flipX = true;
			tri.GetComponent<SpriteRenderer>().flipY = true;
		}
		tri.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,0);
	 	return tri;
	}

	// generates triangle with vertices (x1,y1) and (x2,y2)
	GameObject genTriBottom(float x1, float y1, float x2, float y2) {
		GameObject tri = (GameObject) Instantiate(triangle, new Vector2 ((x1+x2)/2,(y1+y2)/2), Quaternion.identity);
		tri.transform.localScale = new Vector2((x1-x2)/2, (y2-y1)/2);
		if (y2 < y1) {
			tri.GetComponent<SpriteRenderer>().flipX = true;
			tri.GetComponent<SpriteRenderer>().flipY = true;
		}
		tri.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,0);
	 	return tri;
	}

	GameObject genBar(float x) {
		GameObject bar = (GameObject) Instantiate(squareBlack, new Vector2(x,0), Quaternion.identity);
		bar.transform.localScale = new Vector2(barWidth/2, 6);
		bar.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,0);
		return bar;
	}

	// generates rectangle with bottom corners (l,y) and (r,y)
	GameObject genRectTop(float l, float r, float y) {
		GameObject rect = (GameObject) Instantiate(square, new Vector2 ((l+r)/2,(y+6)/2), Quaternion.identity);
		rect.transform.localScale = new Vector2((r-l)/2,(6-y)/2);
		rect.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,0);
		return rect;
	}

	// generates rectangle with top corners (l,y) and (r,y)
	GameObject genRectBottom(float l, float r, float y) {
		GameObject rect = (GameObject) Instantiate(square, new Vector2 ((l+r)/2,(y-6)/2), Quaternion.identity);
		rect.transform.localScale = new Vector2((r-l)/2,(6+y)/2);
		rect.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,0);
		return rect;
	}

	GameObject lastTop;
	void genSpikeTop(float upper) {
		float y = upper;
		Vector2 last = getCornerTop(lastTop);
		genBar(last.x + dist);
		GameObject tri = genTriTop(last.x, last.y, last.x+dist, upper);
		spikesDrawnTop.Enqueue(tri);
		spikesDrawnTop.Enqueue(genRectTop(last.x,last.x + dist, Math.Max(y,last.y)));
		lastTop = tri;
	}

	GameObject lastBottom;
	void genSpikeBottom(float lower) { //add triangle
		float y = lower;
		Vector2 last = getCornerBottom(lastBottom);
		GameObject tri = genTriBottom(last.x, last.y, last.x+dist, lower);
		spikesDrawnBottom.Enqueue(tri);
		spikesDrawnBottom.Enqueue(genRectBottom(last.x,last.x + dist, Math.Min(y,last.y)));
		lastBottom = tri;
	}

	bool firstTop = true;
	Vector2 getCornerTop(GameObject obj) { //assume triangle
		Vector2 scale = obj.transform.localScale;
		Vector2 pos = obj.transform.localPosition;
		if (firstTop) {
			firstTop = false;
			return new Vector2(scale.x + pos.x, 2*pos.y - 6);
		} else {
			return new Vector2(pos.x - scale.x, pos.y + scale.y);
		}
	}

	bool firstBottom = true;
	Vector2 getCornerBottom(GameObject obj) { //assume triangle
		Vector2 scale = obj.transform.localScale;
		Vector2 pos = obj.transform.localPosition;
		if (firstBottom) {
			firstBottom = false;
			return new Vector2(scale.x + pos.x, 2*pos.y + 6);
		} else {
			return new Vector2(pos.x - scale.x, pos.y + scale.y);
		}
	}

	// deletes rectangles which have disappeared off the screen
	void deleteOldObjectsTop() {
		while (spikesDrawnTop.Count > 100/dist && spikesDrawnTop.Peek().transform.localPosition.x < despawnX) {//100 is too high, 5 screens of objects
			Destroy (spikesDrawnTop.Dequeue());
			Destroy (spikesDrawnTop.Dequeue());
		}
	}

	// deletes rectangles which have disappeared off the screen
	void deleteOldObjectsBottom() {
		while (spikesDrawnBottom.Count > 100/dist && spikesDrawnBottom.Peek().transform.localPosition.x < despawnX) {
			Destroy (spikesDrawnBottom.Dequeue());
			Destroy (spikesDrawnBottom.Dequeue());
		}
	}

	// Update is called once per frame
	void Update () {
		if (spikesToDraw.Count > 0) {
			float[] data = spikesToDraw.Dequeue();
			print ("Spike drawn");
			float q = data[0]/data[1]; // (tick-mean)/sd
			float upper = q + SFadd;
			upper = 5 - (5-upper)/SFmult;
			upper = Math.Min(limit,upper);
			float lower = q - SFadd;
			lower = (lower+5)/SFmult - 5;
			lower = Math.Max(-limit,lower);
			genSpikeTop(upper); // upper, lower expect values between -5 (very bottom of screen) and 5 (top)
			genSpikeBottom(lower);
			deleteOldObjectsTop();
			deleteOldObjectsBottom();
		}
	}

	void Start() {
		dist = speed/serverRate;
		float y = SFadd; // may lead to impossible situations? probably not
		lastTop = genRectTop(despawnX,2*spawnX,y);
		lastBottom = genRectBottom(despawnX,2*spawnX,-y);
		spikesDrawnTop.Enqueue(lastTop);
		spikesDrawnBottom.Enqueue(lastBottom);
		for (int i = (int) despawnX; i <= (int) despawnX; i++) {
			genBar(i);
		}
		for (int i = -3; i <= 3; i+= 3) {
			GameObject bar = (GameObject) Instantiate(squareBlack, new Vector2(0,i), Quaternion.identity);
			bar.transform.localScale = new Vector2(10, barWidth/2);
		}
	}

	public void addSpike(float[] data) {
		print ("spike queued: (" + data[0] + ", " + data[1] + ")");
		spikesToDraw.Enqueue(data);
	}
}
