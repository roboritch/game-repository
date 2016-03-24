using UnityEngine;
using System.Collections;

public class TooltipScript : MonoBehaviour{

  public RectTransform guiObjectToToolTip;
  public defaultTextHolder toolTipText;

  public void setupToolTip(RectTransform obj, string text){
    guiObjectToToolTip = obj;
    toolTipText.addNewTextToDefalt(text);
  }

  // Use this for initialization.
  /// <summary>
  /// Start this instance.
  /// </summary>
  void Start(){
  }
	
  // Update is called once per frame.
  /// <summary>
  /// Update this instance.
  /// </summary>
  void Update(){
  }
}
