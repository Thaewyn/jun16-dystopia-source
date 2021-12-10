using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {

	//control object for 'highlight' sprites, allowing for easy finding and SpriteRenderer.Color access.

	public void SetHighlightColor (float red, float green, float blue, float alpha) {
		transform.GetComponent<SpriteRenderer> ().color = new Color (red, green, blue, alpha);
	}
}
