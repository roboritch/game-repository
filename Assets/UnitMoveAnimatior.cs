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
	#pragma warning disable
	[SerializeField] private GridBlock upBlockDebugHelper;

	void Start(){
		//test code
/*		transform.GetComponentInParent<GridBlock>().setAdj(Direction.UP, upBlockDebugHelper);
		setMovmentDirection(transform.GetComponentInParent<GridBlock>(), Direction.UP);*/

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

	#region move perticals per tick
	#pragma warning disable
	[SerializeField] private int widthOfParticals = 8;
	[SerializeField] private float emissionRate = 40f;
	[SerializeField] private float moveOverTimeMultiplyer = 1f;
	[SerializeField] private float particleMoveAmount = 0.1f;

	/// <summary>
	/// Move all the alive particles to there positions on the unit
	/// </summary>
	public void moveParticls(){
		emissionRate += 0.05f;
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
				Destroy(gameObject, 0.1f);
				/* Debug.Log("Time to compleat is " + time); //uncoment this to find time
				timeHasBeenFound = true; */
			}	
		}

		PS.SetParticles(particles, numberP);
	}
	#endregion

	#region set loctions particals move to
	#pragma warning disable
	private ParticleSystem.Particle[] particles;
	// container for the particles in the system
	private Vector3[,] endLocations;
	// end location of each particle

	[SerializeField] private float offsetAmount = 0.18f;

	private void setParticleStartLocations(){
		endLocations = new Vector3[widthOfParticals, widthOfParticals];
		Vector3 particelStartLocation = gridBlockMovingTo.transform.position;
		//particelStartLocation = new Vector3(particelStartLocation.x, 0.01f, particelStartLocation.y);
		int x, y;
		for(x = 0; x < widthOfParticals; x++){
			for(y = 0; y < widthOfParticals; y++){
				endLocations[x, y] = particelStartLocation;
				float posX, posY;
				posX = x * offsetAmount;
				posX -= ((widthOfParticals - 1f) * offsetAmount) / 2f;
				posY = y * offsetAmount;
				posY -= ((widthOfParticals - 1f) * offsetAmount) / 2f;

				endLocations[x, y] += new Vector3(posX, 0, posY);
			}
		}

		#region semi randomize end locations 
		int n = endLocations.GetLength(0);
		Vector3[,] temp = new Vector3[n, n];
		Vector3 endTemp;
		x = 0;
		y = 0;
		int x1, y1;
	
		if(directionMoving == Direction.UP){
			for(int i = 0; i < endLocations.Length * 3; i++){
				x = Random.Range(0, widthOfParticals);
				y = Random.Range(0, widthOfParticals);
				x1 = Random.Range(0, widthOfParticals);

				endTemp = endLocations[x, y]; // swap 2 x values in a single y column
				endLocations[x, y] = endLocations[x1, y];
				endLocations[x1, y] = endTemp;
			}
		} else if(directionMoving == Direction.RIGHT){
			for(int i = 0; i < n; ++i){
				for(int j = 0; j < n; ++j){
					temp[i, j] = endLocations[j, i];
				}
			}
			endLocations = temp;
			for(int i = 0; i < endLocations.Length * 3; i++){
				x = Random.Range(0, widthOfParticals);
				y = Random.Range(0, widthOfParticals);
				x1 = Random.Range(0, widthOfParticals);

				endTemp = endLocations[x, y]; // swap 2 y values in a single x column
				endLocations[x, y] = endLocations[x1, y];
				endLocations[x1, y] = endTemp;
			}
		} else if(directionMoving == Direction.LEFT){
			for(int i = 0; i < n; ++i){
				for(int j = 0; j < n; ++j){
					temp[i, j] = endLocations[n - j - 1, i];
				}
			}
			endLocations = temp;
			for(int i = 0; i < endLocations.Length * 3; i++){
				x = Random.Range(0, widthOfParticals);
				y = Random.Range(0, widthOfParticals);
				x1 = Random.Range(0, widthOfParticals);

				endTemp = endLocations[x, y]; // swap 2 y values in a single x column
				endLocations[x, y] = endLocations[x1, y];
				endLocations[x1, y] = endTemp;
			}
		} else if(directionMoving == Direction.DOWN){
			for(int i = 0; i < n; ++i){
				for(int j = 0; j < n; ++j){
					temp[i, j] = endLocations[i, n - j - 1];
				}
			}
			endLocations = temp;
			for(int i = 0; i < endLocations.Length * 3; i++){
				x = Random.Range(0, widthOfParticals);
				y = Random.Range(0, widthOfParticals);
				x1 = Random.Range(0, widthOfParticals);

				endTemp = endLocations[x, y]; // swap 2 x values in a single y column
				endLocations[x, y] = endLocations[x1, y];
				endLocations[x1, y] = endTemp;
			}
		}
		#endregion
	}
	#endregion

	#region set movment direction
	private GridBlock currentGridBlock;
	private GridBlock gridBlockMovingTo;
	private Direction directionMoving;

	/// <summary>
	/// called by the unit when it moves
	/// </summary>
	/// <param name="">.</param>
	public void setMovmentDirection(GridBlock currentGridBlockTmp, Direction movmentDirection){
		currentGridBlock = currentGridBlockTmp;
		gridBlockMovingTo = currentGridBlockTmp.getAdj(movmentDirection);
		switch(movmentDirection){
		case Direction.UP:
			transform.Translate(0, 0, 0.7f);
			break;
		case Direction.DOWN:
			transform.Translate(0, 0, -0.7f);
			break;
		case Direction.LEFT:
			transform.Translate(-0.7f, 0, 0);
			break;
		case Direction.RIGHT:
			transform.Translate(0.7f, 0, 0);
			break;
		default:
			break;
		}
		directionMoving = movmentDirection;
	}
	#endregion





}
