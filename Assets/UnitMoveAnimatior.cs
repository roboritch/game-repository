using UnityEngine;
using System.Collections;

public class UnitMoveAnimatior : MonoBehaviour{
	/// <summary>
	/// The Particle system, some peramiters are already setup in the prefab.
	/// </summary>
	private ParticleSystem PS;

	/// <summary>
	/// The number of Particles that are moving to make the unit
	/// This should be a squire number ie x^2
	/// </summary>
	private int numberOfParticles;


	void Awake(){
		PS = GetComponent<ParticleSystem>();
	}

	void Start(){
		//test code
		transform.GetComponentInParent<GridBlock>().setAdj(Direction.UP,)
		setMovmentDirection(Direction.UP);

		numberOfParticles = widthOfParticals * widthOfParticals;
		PS.maxParticles = numberOfParticles;

		Invoke("setParticleStartLocations", 0.1f); // let the Instanteating script do some setup first

		InvokeRepeating("moveParticls", 0.2f, 0.02f);
	}

	public void setParticalColor(Color c){
		PS.startColor = c;
	}


	private bool timeHasBeenFound = false;
	private float time = 0;
	[SerializeField] private float timeFound = 2.6f;
	// this is time for width 8
	void FixedUpdate(){
		time += Time.fixedDeltaTime;
	}

	#region IGetAnimationTimeToFin implementation

	public float getAnimationTime(){
		return timeFound;
	}

	#endregion

	#pragma warning disable
	[SerializeField] private int widthOfParticals = 8;
	[SerializeField] private float emissionRate = 20f;
	[SerializeField] private float moveOverTimeMultiplyer = 1f;
	[SerializeField] private float particleMoveAmount = 0.1f;

	/// <summary>
	/// Move all the alive particles to there positions on the unit
	/// </summary>
	public void moveParticls(){
		emissionRate += 0.2f;
		ParticleSystem.EmissionModule emission = PS.emission;
		ParticleSystem.MinMaxCurve rate = emission.rate;
		rate.constantMax = emissionRate;
		emission.rate = rate;

		moveOverTimeMultiplyer += 0.02f;
		float tempParticalMoveAmount = particleMoveAmount * moveOverTimeMultiplyer;
		particles = new ParticleSystem.Particle[PS.particleCount];
		int numberP = PS.GetParticles(particles);
		int x = 0;
		int y = 0;
		for(int i = 0; i < numberP; i++){
			tempParticalMoveAmount *= particles[i].startLifetime - particles[i].lifetime;
			particles[i].position = Vector3.MoveTowards(particles[i].position, endLocations[x, y], tempParticalMoveAmount);
			x++;
			if(x >= widthOfParticals){
				x = 0;
				y++;
			}
		}

		if(numberP == numberOfParticles){
			if(!timeHasBeenFound && particles[numberOfParticles - 1].position == endLocations[widthOfParticals - 1, widthOfParticals - 1]){
				//Destroy(gameObject);
				/*Debug.Log("Time to compleat is " + time); //uncoment this to find time
				timeHasBeenFound = true;*/
			}	
		}

		PS.SetParticles(particles, numberP);
	}

	#pragma warning disable
	private ParticleSystem.Particle[] particles;
	// container for the particles in the system
	private Vector3[,] endLocations;
	// end location of each particle

	[SerializeField] private float offsetAmount = 0.18f;

	private void setParticleStartLocations(){
		endLocations = new Vector3[widthOfParticals, widthOfParticals];
		Vector3 particelStartLocation = new Vector3(); // bottem left partical of the unit
		int x, y;
		for(x = 0; x < widthOfParticals; x++){
			for(y = 0; y < widthOfParticals; y++){
				endLocations[x, y] = particelStartLocation;
				float posX, posY;
				posX = x * offsetAmount;
				posX -= ((widthOfParticals - 1f) * offsetAmount) / 2f;
				posY = y * offsetAmount;
				posY -= ((widthOfParticals - 1f) * offsetAmount) / 2f;

				endLocations[x, y] += new Vector3(posX, 0, posY) + gridBlockMovingTo.transform.localPosition; 
			}
		}

		//This finds the middle of all the particals so the partical
		//system can be centered over the unit
		/*		Vector3 topRight,bottomRight,topLeft,bottomLeft,top,bottom,midpoint;
		int floor  = Mathf.FloorToInt(widthOfParticals/2f);
		int ceil  = floor+1;
		topLeft = endLocations[floor,ceil];
		topRight = endLocations[ceil,ceil];
		bottomLeft = endLocations[floor,floor];
		bottomRight = endLocations[ceil,floor];

		top = (topLeft + topRight) * 0.5f;
		bottom = (bottomLeft + bottomRight) * 0.5f;

		midpoint = (top + bottom) * 0.5f; 
		Vector3 newMidpoint;
		newMidpoint = new Vector3(-midpoint.x,midpoint.z,0);
*/
		//transform.localPosition = newMidpoint;

		//randomize the end locations for a better effect
		Vector3 endTemp;
		x = 0;
		y = 0;
		int x1, y1;
		for(int i = 0; i < numberOfParticles * 10; i++){
			x = Random.Range(0, widthOfParticals);
			y = Random.Range(0, widthOfParticals);
			x1 = Random.Range(0, widthOfParticals);
			y1 = Random.Range(0, widthOfParticals);
			endTemp = endLocations[x, y];
			endLocations[x, y] = endLocations[x1, y1];
			endLocations[x1, y1] = endTemp;
		}
	}



	public GridBlock gridBlockMovingTo;

	/// <summary>
	/// called by the unit when it moves
	/// </summary>
	/// <param name="">.</param>
	public void setMovmentDirection(Direction movmentDirection){
		gridBlockMovingTo = transform.GetComponentInParent<GridBlock>().getAdj(movmentDirection);
		switch(movmentDirection){
		case Direction.UP:
			break;
		case Direction.DOWN:
			transform.Rotate(new Vector3(0, 180f, 0f));
			break;
		case Direction.LEFT:
			transform.Rotate(new Vector3(0, -90f, 0f));
			break;
		case Direction.RIGHT:
			transform.Rotate(new Vector3(0, 90f, 0f));
			break;
		default:
			break;
		}
	}






}
