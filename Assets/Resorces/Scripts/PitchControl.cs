using UnityEngine;
using System.Collections;

public class PitchControl : MonoBehaviour{

  private AudioSource AS;
  [SerializeField] private float pitchChangeRate = 0.01f;
  private float wantedPitch = 0.85f;

  void Start(){
    AS = GetComponent<AudioSource>(); 
    InvokeRepeating("updatePitch", 0.0f, 0.01f);
  }

  public void setPitch(float pitchValue){
    wantedPitch = pitchValue;
  }

  void updatePitch(){
    AS.pitch = Mathf.MoveTowards(AS.pitch, wantedPitch, pitchChangeRate);
  }
}