using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Gets all avalible units
/// Do not create or destroy this object, move it of and on screen insted
/// </summary>
public class UnitSelectionScript : MonoBehaviour {
	public UnitInformationStruct[] unitInfo;
	public Button[] unitSelections;
	/// <summary>
	/// if it is not posible to set a unit disable it's selection with this
	/// </summary>
	public bool[] disabledUnits;

	public GameObject unitDisplayPrefab;

	public GridBlock currentGridblock;

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
				float size = temp.GetComponent<RectTransform>().sizeDelta.x; //width = hight
				temp.transform.localPosition.Set(5f + (size + 5f) * x, -5f - (size - 5f) * y, 0);
				int i = y * 4 + x; //index number read left to right
				unitSelections[i] = temp.GetComponent<Button>();
				unitSelections[i].onClick.AddListener(() => {
					createThisUnit(i);
				});

			}
		}
	}

	public void createThisUnit(int unitNumberFromSelection) {
		GameObject unit = Instantiate(unitInfo[unitNumberFromSelection].unit) as GameObject;
		currentGridblock.spawnUnit(unit.GetComponent<UnitScript>()); // send to gridBlockforCreation
	}

}
