using UnityEngine;
using System.Collections;

/// <summary>
/// pulse the sprite by changing the the size and 
/// ulternating sprite Alpha
/// </summary>
public class haloControler : MonoBehaviour {
	private SpriteRenderer SR;

	void Start () {
		SR = GetComponent<SpriteRenderer>();
		pulseOnce();
	}

	[SerializeField] float updateTime = 0.05f;
	public void pulseOnce(){
		InvokeRepeating("pulseSprite",0f,updateTime);
		InvokeRepeating("checkToDestroy",0.0001f,updateTime);
	}

	#pragma warning disable
	[SerializeField]private int maxPulseNumber;
	private void checkToDestroy(){
		if(numberPulsesSinceStart > maxPulseNumber){
			Destroy(gameObject);
		}
	}


	private int numberPulsesSinceStart = 0;
	private void pulseTick(){
		numberPulsesSinceStart++;
	}


	#region flicker
	#pragma warning disable
	[SerializeField] private FlickerPerams flickerPerams;
	private bool flickerMinDirection = false;
	private void flickerSprite(){
		Color tempColor = SR.color;
		if(tempColor.a < flickerPerams.flickerMin || tempColor.a > flickerPerams.flickerMax)
			flickerMinDirection = !flickerMinDirection;

		if(flickerMinDirection == false)
			tempColor.a -= flickerPerams.flickerAmount;
		else
			tempColor.a += flickerPerams.flickerAmount;
		SR.color = tempColor;

	}
	#endregion

	#region pulse
	#pragma warning disable
	[SerializeField] private PulsePerams pulsePerams;
	private bool pulseDirection = false;
	private void pulseSprite(){
		if(transform.localScale.x < pulsePerams.pulseMin || transform.localScale.x > pulsePerams.pulseMax){
			pulseDirection = !pulseDirection;
			if(pulseDirection == false){
				pulseTick();
			}
		}

		if(pulseDirection == false)
			transform.localScale -= new Vector3(pulsePerams.pulseAmount,pulsePerams.pulseAmount);
		else
			transform.localScale += new Vector3(pulsePerams.pulseAmount,pulsePerams.pulseAmount);
	}
	#endregion

	// Update is called once per frame
	void Update () {
		
	}
}
[System.Serializable]
struct FlickerPerams{
	public float flickerAmount;
	public float flickerMin,flickerMax;
}
[System.Serializable]
struct PulsePerams{
	public float pulseAmount;
	public float pulseMin,pulseMax;
}
