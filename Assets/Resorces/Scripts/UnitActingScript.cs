using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Unit acting script.
/// Many of these are controled by the game gui script
/// </summary>
public class UnitActingScript : MonoBehaviour {
	#pragma warning disable
	[SerializeField] private Image unitColor;
	[SerializeField] private Image unitHeadSprite;
	private UnitScript unit;
	public RectTransform location;
	public bool enemy;

	void Awake(){
		location = GetComponent<RectTransform>();
	}

	public void setCurrentlyActing() {
		unit.startActing();
		//TODO add some animation to show that this unit is acting 
	}

	public void setUnit(UnitScript u) {
		unit = u;
		unitColor.color = unit.getUnitColor();
		unitHeadSprite.sprite = unit.getUnitHeadSprite();
		//TODO set enamy using unit params
	}

	/// <summary>
	/// enable disable images
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	public void setVisible(bool state){
		unitColor.enabled = state;
		unitHeadSprite.enabled = state;
	}

	/// <summary>
	/// Destroy this object.
	/// </summary>
	public void destroyThis(){
		Destroy(gameObject);
	}

	/// <summary>
	/// returns the unit attached to this script
	/// </summary>
	/// <returns>The unit.</returns>
	public UnitScript getUnit(){
		return unit;
	}
}
