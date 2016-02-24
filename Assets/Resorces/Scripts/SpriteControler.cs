using UnityEngine;
using System.Collections;

public class SpriteControler : MonoBehaviour {

	public SpriteRenderer SR;


	void Awake() {
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

	public void setColor(Color c) {
		SR.color = c;
	}

	public Color getColor() {
		return SR.color;
	}



	public void removeSprite() {
		SR.sprite = null;
	}

}
