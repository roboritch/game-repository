using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Attached to some text componentents to facilitate better maintenance of the defuat text in the text component.
/// </summary>
public class defaultTextHolder : MonoBehaviour{

  private string defautText;
  private Text textComponent;
  private Color defaultColor;
  private Color flashColor;
  private bool flashing;
	
  // Use this for initialization.
  /// <summary>
  /// Start this instance.
  /// </summary>
  void Start(){
    textComponent = transform.GetComponent<Text>();
    defaultColor = textComponent.color;
    defautText = textComponent.text;
  }

  /// <summary>
  /// Will flash the text in the specified color for the flashTime (s) in 0.75 second intervals.
  /// </summary>
  /// <param name="colorToFlash">Color to flash.</param>
  /// <param name="flashTime">Flash time (in seconds).</param>
  public void flashText(Color colorToFlash, float flashTime){
    if (flashing){
      endFlash();
    }
    flashColor = colorToFlash;
    InvokeRepeating("flash", 0f, 0.75f);
    Invoke("endFlash", flashTime);
    flashing = true;
  }

  private void flash(){
    if (textComponent.color == defaultColor){
      textComponent.color = flashColor;
    } else{
      textComponent.color = defaultColor;
    }
  }

  private void endFlash(){
    CancelInvoke("flash");
    CancelInvoke("endFlash");
    textComponent.color = defaultColor;
    flashing = false;
  }

  public void newText(string displayText){
    textComponent.text = displayText;
  }

  /** Display the addedText after the defaultText. */
  public void addNewTextToDefalt(string addedText){
    textComponent.text = defautText + addedText;
  }

  public void setNewDefautText(string newDefaut){
    defautText = newDefaut;
  }

  public void setDefault(){
    textComponent.text = defautText;
  }

  public void setDefault(float timeToReset){
    CancelInvoke("setDefault");
    Invoke("setDefault", timeToReset);
  }
}