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
	private Queue<GameObject> barsDrawn = new Queue<GameObject>();
	private List<GameObject> initObjects = new List<GameObject>();

	public float despawnX;
	public float spawnX;
	public float speed;
	public float SFadd;
	public float SFmult;
	public float limit; // highest absolute value to draw walls at
	public float serverRate; // data points sent per second
	public float barWidth;
	float dist; // x distance between points (= speed/serverRate)
	Color wallColor = new Color(85f/255f,86f/255f,96f/255f,1);
	Color hazardColor = new Color(216f/255f,71f/255f,71f/255f,1);

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
		tri.tag = "Hazard Top";
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
		tri.tag = "Hazard Bottom";
	 	return tri;
	}

	//generates horizontal line at given x coordinate
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
		rect.tag = "Hazard Top";
		return rect;
	}

	// generates rectangle with top corners (l,y) and (r,y)
	GameObject genRectBottom(float l, float r, float y) {
		GameObject rect = (GameObject) Instantiate(square, new Vector2 ((l+r)/2,(y-6)/2), Quaternion.identity);
		rect.transform.localScale = new Vector2((r-l)/2,(6+y)/2);
		rect.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed,0);
		rect.tag = "Hazard Bottom";
		return rect;
	}

	GameObject lastTop;
	bool firstTop = true;
	void genSpikeTop(float upper) {
		float y = upper;
		Vector2 last;
		if (firstTop) {
			firstTop = false;
			GameObject obj = lastTop;
			Vector2 scale = obj.transform.localScale;
			Vector2 pos = obj.transform.localPosition;
			last = new Vector2(scale.x + pos.x, 2*pos.y - 6);
		} else {
			last = getCornerTop(lastTop);
		}
		barsDrawn.Enqueue(genBar(last.x + dist));
		GameObject tri = genTriTop(last.x, last.y, last.x+dist, upper);
		spikesDrawnTop.Enqueue(tri);
		spikesDrawnTop.Enqueue(genRectTop(last.x,last.x + dist, Math.Max(y,last.y)));
		lastTop = tri;
	}

	GameObject lastBottom;
	bool firstBottom = true;
	void genSpikeBottom(float lower) { //add triangle
		float y = lower;
		Vector2 last;
		if (firstBottom) {
			firstBottom = false;
			GameObject obj = lastBottom;
			Vector2 scale = obj.transform.localScale;
			Vector2 pos = obj.transform.localPosition;
			last = new Vector2(scale.x + pos.x, 2*pos.y + 6);
		} else {
			last = getCornerBottom(lastBottom);
		}
		GameObject tri = genTriBottom(last.x, last.y, last.x+dist, lower);
		spikesDrawnBottom.Enqueue(tri);
		spikesDrawnBottom.Enqueue(genRectBottom(last.x,last.x + dist, Math.Min(y,last.y)));
		lastBottom = tri;
	}

	Vector2 getCornerTop(GameObject obj) { //assume triangle
		Vector2 scale = obj.transform.localScale;
		Vector2 pos = obj.transform.localPosition;
		return new Vector2(pos.x - scale.x, pos.y + scale.y);
	}

	Vector2 getCornerBottom(GameObject obj) { //assume triangle
		Vector2 scale = obj.transform.localScale;
		Vector2 pos = obj.transform.localPosition;
		return new Vector2(pos.x - scale.x, pos.y + scale.y);
	}

	// deletes rectangles which have disappeared off the screen
	bool firstDestroy = true;
	void deleteOldObjects() {
		while (spikesDrawnTop.Peek().transform.localPosition.x < despawnX) {//100 is too high, 5 screens of objects
			Destroy (spikesDrawnTop.Dequeue());
			Destroy (spikesDrawnTop.Dequeue());
			Destroy (spikesDrawnBottom.Dequeue());
			Destroy (spikesDrawnBottom.Dequeue());
			Destroy (barsDrawn.Dequeue());
			if (firstDestroy) {
				firstDestroy = false;
				foreach (GameObject obj in initObjects) {
					Destroy (obj);
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		while (spikesToDraw.Count > 0 && lastTop.transform.localPosition.x < spawnX) {
			float[] data = spikesToDraw.Dequeue();
			//print ("Spike drawn");
			float q = data[0]/data[1]; // (tick-mean)/sd
			float upper = q + SFadd;
			upper = 5 - (5-upper)/SFmult;
			upper = Math.Min(limit,upper);
			float lower = q - SFadd;
			lower = (lower+5)/SFmult - 5;
			lower = Math.Max(-limit,lower);
			genSpikeTop(upper); // upper, lower expect values between -5 (very bottom of screen) and 5 (top)
			genSpikeBottom(lower);
			deleteOldObjects();
		}
	}

	void Start() {
		dist = speed/serverRate;
		float y = SFadd; // may lead to impossible situations? probably not
		lastTop = genRectTop(despawnX,2*spawnX,y);
		initObjects.Add(lastTop);
		lastBottom = genRectBottom(despawnX,2*spawnX,-y);
		initObjects.Add(lastBottom);
		for (int i = (int) despawnX; i <= (int) 2*spawnX; i+= (int) dist) {
			initObjects.Add(genBar(i));
		}
		for (int i = -3; i <= 3; i+= 3) {
			GameObject bar = (GameObject) Instantiate(squareBlack, new Vector2(0,i), Quaternion.identity);
			bar.transform.localScale = new Vector2(10, barWidth/2);
		}
	}

	void colorChange(Queue<GameObject> q, Color color) {
		foreach (GameObject obj in q) {
			obj.GetComponent<SpriteRenderer>().color = color;
		}
		if (firstDestroy) {
			String tag = "Hazard " + ((q == spikesDrawnTop) ? "Top" : "Bottom");
			foreach (GameObject obj in initObjects) {
				if (obj.tag == tag) {
					obj.GetComponent<SpriteRenderer>().color = color;
				}
			}
		}
	}
	public void colorTop() {
		colorChange(spikesDrawnTop, hazardColor);
	}
	public void decolorTop() {
		colorChange(spikesDrawnTop, wallColor);
	}
	public void colorBottom() {
		colorChange(spikesDrawnBottom, hazardColor);
	}
	public void decolorBottom() {
		colorChange(spikesDrawnBottom, wallColor);
	}

	public void addSpike(float[] data) {
		//print ("spike queued: (" + data[0] + ", " + data[1] + ")");
		spikesToDraw.Enqueue(data);
	}
}
