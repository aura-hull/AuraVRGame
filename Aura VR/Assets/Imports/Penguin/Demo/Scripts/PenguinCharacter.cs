using UnityEngine;
using System.Collections;

public class PenguinCharacter : MonoBehaviour {
	Animator penguinAnimator;
	public bool jumpUp=false;
	public float groundCheckDistance = 0.1f;
	public float groundCheckOffset=0.01f;
	public bool isSwimming=false;
	public bool isTobogganing=false;
	public float swimSpeed=3f;
	public float tobogganSpeed=1f;

//	public GameObject leftFoot;
//	public bool leftFootIsGrounded;
//	public GameObject rightFoot;
//	public bool rightFootIsGrounded;
	public bool isGrounded;
	
	public float jumpSpeed=1f;
	Rigidbody penguinRigid;
	
	void Start () {
		penguinAnimator = GetComponent<Animator> ();
		penguinRigid=GetComponent<Rigidbody>();
	}
	
	void FixedUpdate(){
		CheckGroundStatus ();
	}
	
	public void Attack(){
		penguinAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		penguinAnimator.SetTrigger("Hit");
	}
	
	public void Eat(){
		penguinAnimator.SetTrigger("Eat");
	}
	
	public void Death(){
		penguinAnimator.SetTrigger("Death");
	}
	
	public void Rebirth(){
		penguinAnimator.SetTrigger("Rebirth");
	}
	
	public void Grooming(){
		penguinAnimator.SetTrigger("Grooming");
	}
	
	public void Flap(){
		penguinAnimator.SetTrigger("Flap");
	}
	
	public void Toboggan(){
		penguinAnimator.SetTrigger("Toboggan");
		isTobogganing = true;
		penguinAnimator.applyRootMotion = false;

	}	
	public void StandUp(){
		penguinAnimator.SetTrigger("StandUp");
		isTobogganing = false;
		penguinAnimator.applyRootMotion = true;

	}
	
	public void CatchTheRock(){
		penguinAnimator.SetTrigger("Catch");
	}
	
	public void ReleaseTheRock(){
		penguinAnimator.SetTrigger("Release");
	}

	public void Dive(){
		penguinAnimator.SetTrigger("Dive");
	}

	public void SwimStart(){
		penguinAnimator.SetBool("IsSwimming",true);
		isSwimming = true;
		penguinRigid.useGravity = false;
		penguinAnimator.applyRootMotion = false;

	}

	public void SwimEnd(){
		penguinAnimator.SetBool("IsSwimming",false);
		isSwimming = false;
		penguinRigid.useGravity = true;
		penguinAnimator.applyRootMotion = true;


	}


	public void Jump(){
		if (isGrounded) {
			penguinAnimator.SetTrigger ("Jump");
			jumpUp = true;
		}
	}
	
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
	//	leftFootIsGrounded=Physics.Raycast(leftFoot.transform.position + (Vector3.up * groundCheckOffset), Vector3.down, out hitInfo, groundCheckDistance);
	//	rightFootIsGrounded=Physics.Raycast(rightFoot.transform.position + (Vector3.up * groundCheckOffset), Vector3.down, out hitInfo, groundCheckDistance);
	//	penguinAnimator.SetBool("LeftFootIsGrounded",leftFootIsGrounded);		
	//	penguinAnimator.SetBool("RightFootIsGrounded",rightFootIsGrounded);	
	//	IsGrounded=leftFootIsGrounded||rightFootIsGrounded;

		isGrounded=Physics.Raycast(transform.position + (Vector3.up * groundCheckOffset), Vector3.down, out hitInfo, groundCheckDistance);


		if(isGrounded){
			if(!jumpUp){
				penguinAnimator.applyRootMotion = true;
				penguinAnimator.SetBool("IsGrounded",true);
			}
		}
		else
		{
			//penguinAnimator.applyRootMotion = false;
			penguinAnimator.SetBool("IsGrounded",false);
		}
		
		if (jumpUp) {
			if(!isGrounded){
				jumpUp=false;
				penguinAnimator.applyRootMotion = false;
				penguinRigid.AddForce((transform.up+transform.forward*penguinRigid.velocity.sqrMagnitude*.1f)*jumpSpeed,ForceMode.Impulse);
			}
		}
	}
	
	public void Move(float v,float h){
		penguinAnimator.SetFloat ("Forward", v);
		penguinAnimator.SetFloat ("Turn", h);

		if (isSwimming) {
			penguinAnimator.SetFloat ("UpDown", v);
			transform.RotateAround(transform.position,transform.up,Time.deltaTime*h*100f);

			penguinRigid.velocity=(transform.up*v+transform.forward)*swimSpeed;	
			//penguinRigid.AddForce((transform.up*v+transform.forward)*swimSpeed);

		}
		if (isTobogganing) {

			transform.RotateAround(transform.position,transform.up,Time.deltaTime*h*100f);
			
			penguinRigid.velocity=transform.forward*tobogganSpeed;	
			//penguinRigid.AddForce((transform.up*v+transform.forward)*swimSpeed);
			
		}


	}
}
