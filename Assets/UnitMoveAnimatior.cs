using UnityEngine;
using System.Collections;

public class UnitMoveAnimatior : MonoBehaviour{
	

	void Start(){
		
	}


	/// <summary>
	/// 
	/// </summary>
	/// <param name="">.</param>
	public void setMovmentDirection(Direction movmentDirection){
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
