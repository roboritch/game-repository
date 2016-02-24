using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Gets all avalible units
/// Do not create or destroy this object, move it of and on screen insted
/// </summary>
public class UnitSelectionScript : MonoBehaviour {
	
	[SerializeField]
	#pragma warning disable
	/// <summary>
	/// This is a problem point as the infromation from the unit must be manualy
	/// placed into this from the unit prefab in the inspecter
	/// We could fix this if we move to an XML system, but that has it's own problems
	/// We will work with manual copying.
	/// </summary>
	private UnitInformationStruct[] unitInfo;
	private Button[] unitSelections;
	/// <summary>
	/// if it is not posible to set a unit disable it's selection with this
	/// </summary>
	[SerializeField]
	private bool[] disabledUnits;

	public GameObject unitDisplayPrefab;

	public GridBlock currentGridblock;

	[SerializeField]
	private Sprite defaultUnitSprite;

	/// <summary>
	/// Sets up a 4 by y grid of units so the user can choose which one to place in a spawn spot.
	/// </summary>
	public void setUpUnits() {
		unitSelections = new Button[unitInfo.Length];

		GameObject temp;
		int maxY = (int)Mathf.Ceil((float)unitInfo.Length / 4f);
		for(int y = 0; y < maxY; y++) {
			for(int x = 0; x < 4; x++) {
				if(unitInfo.Length == y * 4 + x) {
					break;
				}
				temp = Instantiate(unitDisplayPrefab) as GameObject;
				temp.transform.SetParent(transform);
				RectTransform rct = temp.GetComponent<RectTransform>(); //transform used in gui
				float size = rct.sizeDelta.x; //width = hight
				rct.anchoredPosition = new Vector2(5f + (size + 5f) * x, -5f - (size - 5f) * y);
				int i = y * 4 + x; //index number read left to right
				unitSelections[i] = temp.GetComponent<Button>();
				Image img = unitSelections[i].GetComponent<Image>();
				img.sprite = defaultUnitSprite;
				img.color = unitInfo[i].unitColor; 
				unitSelections[i].transform.GetChild(0).GetComponent<Image>().sprite = unitInfo[i].unitHeadSprite;
				unitSelections[i].onClick.AddListener(() => {
					this.createThisUnit(i); // call this when button is pressed
				});

			}
		}
	}

	void Start() {
		setUpUnits();
	}

	public void createThisUnit(int unitNumberFromSelection) {
		GameObject unit = Instantiate(unitInfo[unitNumberFromSelection].unit) as GameObject;
		currentGridblock.spawnUnit(unit.GetComponent<UnitScript>()); // send to gridBlockforCreation
	}

}
