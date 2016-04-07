using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Unit acting script.
/// Many of these are controlled by the game GUI script.
/// </summary>
public class UnitActingScript : MonoBehaviour{
	#pragma warning disable
	[SerializeField] private Image unitColor;
	[SerializeField] private Image unitHeadSprite;
	public UnitScript unit;
	public RectTransform location;
	public bool enemy;

	void Awake(){
		location = GetComponent<RectTransform>();
	}

	public bool checkIfUnitIsAlive(){
		if(unit == null){
			Debug.LogWarning("unit destroyed before it could act");
			return false;
		}
		return true;
	}


	/// <summary>
	/// Sets the currently acting.
	/// </summary>
	/// <returns><c>true</c>, if unit is alive and is now acting, <c>false</c> otherwise.</returns>
	public bool setCurrentlyActing(){
		unit.startActing();
		return unit.IsActing;
			
		
		//TODO Add some animation to show that this unit is acting.
	}

	public void setUnit(UnitScript u){
		unit = u;
		unitColor.color = unit.getUnitColor();
		unitHeadSprite.sprite = unit.getUnitHeadSprite();
		//TODO Set enemy using unit params.
	}

	/// <summary>
	/// Enable or disable images.
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
	/// Returns the unit attached to this script.
	/// </summary>
	/// <returns>The unit.</returns>
	public UnitScript getUnit(){
		return unit;
	}
}
