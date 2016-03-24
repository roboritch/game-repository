using UnityEngine;
using System.Collections;

public class TimerDisplay : MonoBehaviour {
	private UnitScript unitUsingTimer;
	public void setUnitUsingTimer(UnitScript unit){
		unitUsingTimer = unit;
	}

	public void show(){
		GetComponent<SpriteRenderer>().enabled = true;
		foreach (var item in GetComponentsInChildren<SpriteRenderer>(true)) {
			item.enabled = true;
		}
	}

	public void hide(){
		unitUsingTimer = null;
		GetComponent<SpriteRenderer>().enabled = false;
		foreach (var item in GetComponentsInChildren<SpriteRenderer>(true)) {
			item.enabled = false;
		}
	}

	#pragma warning disable
	[Range(0,30)] [SerializeField] private int maxBars;
	[SerializeField] private GameObject bar;
	private float barXScale;
	[SerializeField] private Transform barStart;

	[SerializeField] private float loadingBarLength = 10f;
	[SerializeField] private float loadingBitSize = 0.4f;
	[SerializeField] private float barWhitespace = 0.2f;
	private int numberOfTimeSlices;

	void Start(){
		numberOfTimeSlices = Mathf.CeilToInt(loadingBarLength/(loadingBitSize+barWhitespace));
		barXScale = bar.transform.localScale.x;
	}

	[SerializeField] Vector3 timerLocationOffset = new Vector3(-1f,-1f);	
	public void setTimer(){
		if(unitUsingTimer != null){
			transform.position = unitUsingTimer.getCurrentBlockHeadLocation().transform.position - timerLocationOffset;
			timerToBars(unitUsingTimer.unitTimer.time ,unitUsingTimer.unitTimer.maxTime);
		}

	}

	private float storedTimeSliceLength;
	private float storedLastMaxTime;
	private void timerToBars(float currentTime,float maxTime){
		if(storedLastMaxTime != maxTime){
			storedLastMaxTime =  maxTime;
			storedTimeSliceLength = maxTime/(float)numberOfTimeSlices;
		}
		float timeRepresentedByBars = currentNumberOfBars * storedTimeSliceLength;
		while(timeRepresentedByBars + storedTimeSliceLength < currentTime ){
			addBar();
			timeRepresentedByBars += storedTimeSliceLength;
		}
		while(timeRepresentedByBars > currentTime){
			removeBar();
			timeRepresentedByBars -= storedTimeSliceLength;
		}
	}

	private int currentNumberOfBars = 0;
	private GameObject[] bars = new GameObject[40];
	private void addBar(){
		bars[currentNumberOfBars] = Instantiate(bar) as GameObject;
		bars[currentNumberOfBars].transform.SetParent(transform);
		bars[currentNumberOfBars].transform.localPosition = new Vector3(barStart.localPosition.x + (barWhitespace + loadingBitSize)*currentNumberOfBars,0f,-0.1f);
		bars[currentNumberOfBars].transform.localScale = new Vector3(barXScale * loadingBitSize,1f,1f);
		bars[currentNumberOfBars].transform.localRotation = new Quaternion();
		currentNumberOfBars++;
	}

	private void removeBar(){
		Destroy(bars[currentNumberOfBars-1]);
		currentNumberOfBars--;
	}

	private void clearBars(){
		for(int i = 0; i < currentNumberOfBars; i++) {
			Destroy(bars[i].gameObject);
		}
		currentNumberOfBars = 0;
	}
}
