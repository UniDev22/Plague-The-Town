using UnityEngine;
using System.Collections;

public class Crosshairs : MonoBehaviour {

	public LayerMask targetMask;
	public SpriteRenderer dot;
	public Color dotHighlightColor;
	Color originalDotColor;

	void Start() {
		originalDotColor = dot.color;
	}
	
	void Update () {
		transform.Rotate (Vector3.forward * 40 * Time.deltaTime);
	}

	public void DetectTargets(Ray ray) {
		if (Physics.Raycast (ray, 100, targetMask)) {
			dot.color = dotHighlightColor;
            transform.Rotate(Vector3.forward * 160 * Time.deltaTime);
		} else {
			dot.color = originalDotColor;
		}
	}
}