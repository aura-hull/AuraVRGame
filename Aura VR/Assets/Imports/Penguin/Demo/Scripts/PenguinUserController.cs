using UnityEngine;
using System.Collections;

public class PenguinUserController : MonoBehaviour {
	PenguinCharacter penguinCharacter;
	
	void Start () {
		penguinCharacter = GetComponent <PenguinCharacter> ();
	}
	
	void Update () {	
		if (Input.GetButtonDown ("Fire1")) {
			penguinCharacter.Attack();
		}
		
		if (Input.GetButtonDown ("Jump")) {
			penguinCharacter.Jump();
		}
		
		if (Input.GetKeyDown (KeyCode.H)) {
			penguinCharacter.Hit();
		}
		
		if (Input.GetKeyDown (KeyCode.E)) {
			penguinCharacter.Eat();
		}
		
		if (Input.GetKeyDown (KeyCode.K)) {
			penguinCharacter.Death();
		}
		
		if (Input.GetKeyDown (KeyCode.R)) {
			penguinCharacter.Rebirth();
		}		
		if (Input.GetKeyDown (KeyCode.Y)) {
			penguinCharacter.StandUp();
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			penguinCharacter.Grooming();
		}		
		if (Input.GetKeyDown (KeyCode.F)) {
			penguinCharacter.Flap();
		}

		if (Input.GetKeyDown (KeyCode.T)) {
			penguinCharacter.Toboggan();
		}				
		if (Input.GetKeyDown (KeyCode.V)) {
			penguinCharacter.Dive();
		}	
		if (Input.GetKeyDown (KeyCode.C)) {
			penguinCharacter.CatchTheRock();
		}	
		if (Input.GetKeyDown (KeyCode.R)) {
			penguinCharacter.ReleaseTheRock();
		}	

		if (Input.GetKeyDown (KeyCode.I)) {
			penguinCharacter.SwimStart();
		}	

		if (Input.GetKeyDown (KeyCode.M)) {
			penguinCharacter.SwimEnd();
		}	


	}
	
	private void FixedUpdate()
	{
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		if (Input.GetKey(KeyCode.LeftShift)) v *= 0.5f;
		penguinCharacter.Move (v,h);
	}
}
