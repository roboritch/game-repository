using UnityEngine;
using System.Collections;

public class SquareParticleFill : MonoBehaviour {
	/// <summary>
	/// The Particle system, some peramiters are already setup in the prefab.
	/// </summary>
	private ParticleSystem PS;

	#pragma warning disable
	/// <summary>
	/// 1D number of particals
	/// </summary>
	[SerializeField] private int widthOfParticals;
	/// <summary>
	/// The number of Particles that are moving to make the unit
	/// This should be a squire number ie x^2
	/// </summary>
	private int numberOfParticles;


	void Awake(){
		PS = GetComponent<ParticleSystem>();
	}

	void Start(){
		numberOfParticles = widthOfParticals*widthOfParticals;
		PS.maxParticles = numberOfParticles;
		setParticleStartLocations();
	}

	void Update(){
		moveParticls();
	}

	/// <summary>
	/// Move all the alive particles to there positions on the unit
	/// </summary>
	public void moveParticls(){
		particles = new ParticleSystem.Particle[PS.particleCount];
		int numberP = PS.GetParticles(particles);
		int x = 0;
		int y = 0;
		for (int i = 0; i < numberP; i++) {
			particles[i].position = Vector3.MoveTowards(particles[i].position,endLocations[x,y],particleMoveAmount);
			x++;
			if(x >= widthOfParticals){
				x = 0;
				y++;
			}
		}
		PS.SetParticles(particles,numberP);
	}

	#pragma warning disable
	private ParticleSystem.Particle[] particles; // container for the particles in the system
	private Vector3[,] endLocations; // end location of each particle
	public float offsetAmount; // this will need to be tweeked in editor
	public float particleMoveAmount;

	private void setParticleStartLocations(){
		endLocations = new Vector3[widthOfParticals,widthOfParticals];
		Vector3 midpoint = transform.localPosition;
		Vector3 offset = new Vector3(1f,0,1f);
		Vector3 particelStartLocation = midpoint - offset; // bottem left partical of the unit
		int x,y;
		for (x = 0; x < widthOfParticals; x++) {
			for (y = 0; y < widthOfParticals; y++) {
				endLocations[x,y] = particelStartLocation;
				endLocations[x,y] += new Vector3(x*offsetAmount,0,y*offsetAmount);
			}
		}
		//randomize the end locations
		Vector3 endTemp;
		x = 0; y = 0;
		int x1 ,y1;
		for (int i = 0; i < numberOfParticles*10; i++) {
			x = Random.Range(0,widthOfParticals);
			y = Random.Range(0,widthOfParticals);
			x1 = Random.Range(0,widthOfParticals);
			y1 = Random.Range(0,widthOfParticals);
			endTemp = endLocations[x,y];
			endLocations[x,y] = endLocations[x1,y1];
			endLocations[x1,y1] = endTemp;
		}


	}



}
