using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Gets all avalible units.
/// Do not create or destroy this object, move it off and on screen insted.
/// </summary>
public class UnitSelectionScript : MonoBehaviour{
	

	#pragma warning disable
	/// <summary>
	/// This is a problem point as the information from the unit must be manually
	/// placed into this from the unit prefab in the inspector.
	/// We could fix this if we move to an XML system, but that has its own problems.
	/// We will work with manual copying.
	/// </summary>
	[SerializeField] private UnitInformationStruct[] unitInfo;
	private Button[] unitSelections;
	/// <summary>
	/// if it is not posible to set a unit disable it's selection with this
	/// </summary>

	[SerializeField] private bool[] disabledUnits;

	public GameObject unitDisplayPrefab;

	public GridBlock currentGridblock;


	[SerializeField] private Sprite defaultUnitSprite;

	/// <summary>
	/// Sets up a 4 by Y grid of units so the user can choose which one to place in a spawn spot.
	/// </summary>
	public void setUpUnits(){
		unitSelections = new Button[unitInfo.Length];

		GameObject temp;
		int maxY = (int)Mathf.Ceil((float)unitInfo.Length / 4f);
		for(int y = 0; y < maxY; y++){
			for(int x = 0; x < 4; x++){
				if(unitInfo.Length == y * 4 + x){
					break;
				}
				temp = Instantiate(unitDisplayPrefab) as GameObject;
				temp.transform.SetParent(transform);
				//Transform used in gui.
				RectTransform rct = temp.GetComponent<RectTransform>();
				//Width = Hight
				float size = rct.sizeDelta.x;
				rct.anchoredPosition = new Vector2(5f + (size + 5f) * x, -5f - (size - 5f) * y);
				//Index number read left to right.
				int i = y * 4 + x;
				unitSelections[i] = temp.GetComponent<Button>();
				Image img = unitSelections[i].GetComponent<Image>();
				img.sprite = defaultUnitSprite;
				img.color = unitInfo[i].unitColor; 
				unitSelections[i].transform.GetChild(0).GetComponent<Image>().sprite = unitInfo[i].unitHeadSprite;
				unitSelections[i].onClick.AddListener(() =>{
					//Call this when button is pressed.
					this.createThisUnit(i);
				});
			}
		}
		gameObject.SetActive(false);
	}

	void Start(){
		setUpUnits();
	}

	public void enableOnGridBlock(GridBlock gb){
		gameObject.SetActive(true);
		currentGridblock = gb;
	}

	/// <summary>
	/// Creates the this unit 
	/// Units created this way are assumed to be allinged with the player.
	/// </summary>
	/// <param name="unitNumberFromSelection">Unit number from selection.</param>
	public void createThisUnit(int unitNumberFromSelection){
		GameObject unit = Instantiate(unitInfo[unitNumberFromSelection].unit) as GameObject;
		UnitScript su = unit.GetComponent<UnitScript>();
		// Send to gridBlockforCreation.
		currentGridblock.spawnUnit(unit.GetComponent<UnitScript>());
		gameObject.SetActive(false);
	}
	 
}
