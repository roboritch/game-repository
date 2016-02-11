using UnityEngine;
using System.Collections;

public class SpriteControler : MonoBehaviour {

	SpriteRenderer SR;

	// Use this for initialization
	void Start() {
		SR = transform.GetComponent<SpriteRenderer>();
	}


	/// <summary>
	/// Sets the sprite.
	/// color is reset
	/// </summary>
	/// <param name="sprite">Sprite.</param>
	public void setSprite(Sprite sprite) {
		SR.sprite = sprite;
		SR.color = Color.white;
	}

	/// <summary>
	/// Sets the sprint and color.
	/// </summary>
	/// <param name="sprite">Sprite.</param>
	/// <param name="color">Color.</param>
	public void setSprite(Sprite sprite, Color color) {
		SR.sprite = sprite;
		SR.color = color;
	}
}
