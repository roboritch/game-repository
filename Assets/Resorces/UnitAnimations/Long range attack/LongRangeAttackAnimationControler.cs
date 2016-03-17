using UnityEngine;
using System.Collections;


public class LongRangeAttackAnimationControler : MonoBehaviour ,IGetAnimationTimeToFin{
	ParticleSystem PS;

	void Awake(){
		PS = GetComponent<ParticleSystem>();
	}

	void Start(){
		particleSize = new float[PS.maxParticles];
		InvokeRepeating("increaseVortexStrength",0f,0.03f);
	}

	#region pulse
	[SerializeField] private float particleSpeedup;
	private void increaseVortexStrength(){
		int pNumb;
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[PS.maxParticles];
		pNumb = PS.GetParticles(particles);
		if(checkVortexDone(pNumb, particles)){
			CancelInvoke("increaseVortexStrength");
			fire();
			return;
		}
		for (int i = 0; i < pNumb; i++) {
			particles[i].velocity *= particleSpeedup;
		}
		PS.SetParticles(particles,pNumb);
	}

	private bool checkVortexDone(int pNumb,ParticleSystem.Particle[] particles){
		bool allParticlesStopped = true;
		for (int i = 0; i < pNumb; i++) {
			if(particles[i].velocity.sqrMagnitude != 0f){
				allParticlesStopped = false;
			}
		}
		return allParticlesStopped;
	}

	private void destroyPulseCollider(){
		Transform pCollider = transform.GetChild(0);
		pCollider.GetComponent<Collider>().enabled = false;
		Destroy(pCollider.gameObject);
	}
	#endregion

	#region fire
	private void fire(){
		destroyPulseCollider();

		int pNumb;
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[PS.maxParticles];
		pNumb = PS.GetParticles(particles);

		for (int i = 0; i < pNumb; i++) {
			particles[i].velocity = new Vector3(0,0.2f,0);
		}

		getPartcleSizes(pNumb,particles);
		orderParticlesByCurrentDistance();
		InvokeRepeating("increaseSizeByDistance",0,0.03f);

	}

	private Vector3 fireEndpoint = new Vector3(0,10f,0);
	private float[] particleSize;
	private void getPartcleSizes(int pNumb, ParticleSystem.Particle[] particles){
		for (int i = 0; i < pNumb; i++) {
			particleSize[i] = particles[i].startSize;
		}
	}

	//ordered from least to greatest
	private void orderParticlesByCurrentDistance(){
		int pNumb;
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[PS.maxParticles];
		pNumb = PS.GetParticles(particles);
		orderOfParticlesByDistance = new int[pNumb];

		float[] particleDistance = new float[pNumb];
		for (int i = 0; i < pNumb; i++) { // simple sort
			orderOfParticlesByDistance[i] = i;
			particleDistance[i] = particles[i].position.x;
			int offset = 0;
			if(i>0)
				while(particleDistance[i-offset] < particleDistance[i-offset-1]){ //move current particle down the list
					int temp1 = orderOfParticlesByDistance[i-offset-1];
					float temp2 = particleDistance[i-offset-1];

					orderOfParticlesByDistance[i-offset-1] = orderOfParticlesByDistance[i-offset];
					particleDistance[i-offset-1] = particleDistance[i-offset];

					orderOfParticlesByDistance[i-offset] = temp1;
					particleDistance[i-offset] = temp2;

					offset++;
					if(offset == i){
						break;
					}
				}
		}
	}


	private int[] orderOfParticlesByDistance;
	private float currentSizeIncrease = 0;

	private int currentSizeIndex = 0;
	private void increaseSizeByDistance(){
		bool needAnotherIteration = false;
		currentSizeIncrease += 0.01f;
		int pNumb;
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[PS.maxParticles];
		pNumb = PS.GetParticles(particles);
		particles[orderOfParticlesByDistance[currentSizeIndex]].startSize = particleSize[i] + currentSizeIncrease; //TODO change i to CSI of particle order
		

		if(!needAnotherIteration){
			CancelInvoke("increaseSizeByDistance");
		}

		PS.SetParticles(particles, pNumb);
	}
			
	#endregion


	private void hit(){	

	}


	private float foundAnimationTime = 0.0f; //UNDONE this must be found and set
	public float getAnimationTime (){
		return foundAnimationTime;
	}
		

}
