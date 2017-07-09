using UnityEngine;
using System.Collections;


/// <summary>
/// a display that represents the amount of time till the unit can act again
/// using an old school loading bar that updates in fixed increments based on the max amount of time till ready
/// </summary>
public class TimerDisplay : MonoBehaviour{
	private UnitScript unitUsingTimer;

	//TODO simplify the setting of which unit is currently using the timer (too much is controlled by the main GUIScript)
	public void setUnitUsingTimer(UnitScript unit){
		unitUsingTimer = unit;
	}
	
	/// <summary>
	/// show the timer to the user
	/// </summary>
	public void show(){
		GetComponent<SpriteRenderer>().enabled = true;
		foreach( var item in GetComponentsInChildren<SpriteRenderer>(true) ){
			item.enabled = true;
		}
	}

	/// <summary>
	/// hide the timer from user
	/// </summary>
	public void hide(){
		unitUsingTimer = null;
		GetComponent<SpriteRenderer>().enabled = false;
		foreach( var item in GetComponentsInChildren<SpriteRenderer>(true) ){
			item.enabled = false;
		}
	}

	#pragma warning disable
	[Range(0, 30)] [SerializeField] private int maxBars;
	[SerializeField] private GameObject bar;
	private float barXScale;
	[SerializeField] private Transform barStart;

	[SerializeField] private float loadingBarLength = 10f;
	[SerializeField] private float loadingBitSize = 0.4f;
	[SerializeField] private float barWhitespace = 0.2f;
	private int numberOfTimeSlices;

	void Start(){
		numberOfTimeSlices = Mathf.CeilToInt(loadingBarLength / (loadingBitSize + barWhitespace));
		barXScale = bar.transform.localScale.x;
	}

	[SerializeField] Vector3 timerLocationOffset = new Vector3(-1f, -1f);

	/// <summary>
	/// set position of the timer relative to the currently selected unit
	/// </summary>
	public void setTimer(){
		if(unitUsingTimer != null){
			if(unitUsingTimer.getCurrentBlockHeadLocation() != null){
				transform.position = unitUsingTimer.getCurrentBlockHeadLocation().transform.position - timerLocationOffset;
				timerToBars(unitUsingTimer.unitTimer.time, unitUsingTimer.unitTimer.maxTime);
			}
		}
	}

	private float storedTimeSliceLength;
	private float storedLastMaxTime;

	/// <summary>
	/// set what the timer bars should be based on the current time and the max time
	/// </summary>
	/// <param name="currentTime"></param>
	/// <param name="maxTime"></param>
	private void timerToBars(float currentTime, float maxTime){
		if(storedLastMaxTime != maxTime){
			storedLastMaxTime = maxTime;
			storedTimeSliceLength = maxTime / (float)numberOfTimeSlices;
		}
		float timeRepresentedByBars = currentNumberOfBars * storedTimeSliceLength;
		while(timeRepresentedByBars + storedTimeSliceLength < currentTime){
			addBar();
			timeRepresentedByBars += storedTimeSliceLength;
		}
		while(timeRepresentedByBars > currentTime){
			removeBar();
			timeRepresentedByBars -= storedTimeSliceLength;
		}
	}

	///<summary>the current number of timer bars</summary>
	private int currentNumberOfBars = 0;
	/// <summary>
	/// a stack cantoning timber bars max 40
	/// </summary>
	private GameObject[] bars = new GameObject[40];
	/// <summary>
	/// add a timer bar to the stack
	/// </summary>
	private void addBar(){
		bars[currentNumberOfBars] = Instantiate(bar) as GameObject;
		bars[currentNumberOfBars].transform.SetParent(transform);
		bars[currentNumberOfBars].transform.localPosition = new Vector3(barStart.localPosition.x + (barWhitespace + loadingBitSize) * currentNumberOfBars, 0f, -0.1f);
		bars[currentNumberOfBars].transform.localScale = new Vector3(barXScale * loadingBitSize, 1f, 1f);
		bars[currentNumberOfBars].transform.localRotation = new Quaternion();
		currentNumberOfBars++;
	}
	
	/// <summary>
	/// remove the first timer bar in the stack
	/// </summary>
	private void removeBar(){
		Destroy(bars[currentNumberOfBars - 1]);
		currentNumberOfBars--;
	}

	/// <summary>
	/// remove all timer bars from the timer
	/// </summary>
	private void clearBars(){
		for(int i = 0; i < currentNumberOfBars; i++){
			Destroy(bars[i].gameObject);
		}
		currentNumberOfBars = 0;
	}
}
