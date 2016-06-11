using UnityEngine;
using System.Collections;

public class HummingbirdCharacterScript : MonoBehaviour {
	public Animator hummingbirdAnimator;
	public float animSpeed=5f;
	Rigidbody hummingbirdRigid;
	public bool isFlying=false;

	public float forwardSpeed=3f;
	public float hopSpeed=2f;
	public float rotateSpeed=.2f;


	void Start () {
		hummingbirdAnimator = GetComponent<Animator> ();
		hummingbirdAnimator.speed = animSpeed;
		hummingbirdRigid = GetComponent<Rigidbody> ();
	}	

	public void AnimSpeedSet(float speed){
		animSpeed = speed;
		hummingbirdAnimator.speed = animSpeed;
	}

	public void ForwardSpeedSet(float speed){
		forwardSpeed = speed;
	}

	public void Landing(){
		hummingbirdAnimator.SetTrigger ("Landing");
		hummingbirdRigid.useGravity = true;
		isFlying = false;
	}
	
	public void Soar(){
		hummingbirdAnimator.SetTrigger ("Soar");
		hummingbirdRigid.useGravity = false;
		isFlying = true;
	}

	public void HopForward(){
		hummingbirdAnimator.SetTrigger ("HopForward");
		hummingbirdRigid.AddForce ((transform.forward +transform.up)* hopSpeed,ForceMode.Impulse);	
	}
	
	public void HopRight(){
		hummingbirdAnimator.SetTrigger ("HopRight");
		hummingbirdRigid.AddForce ((transform.forward +transform.up)* hopSpeed,ForceMode.Impulse);
		hummingbirdRigid.AddTorque (transform.up * rotateSpeed,ForceMode.Impulse);
	}
	
	public void HopLeft(){
		hummingbirdAnimator.SetTrigger ("HopLeft");

		hummingbirdRigid.AddForce ((transform.forward +transform.up)* hopSpeed,ForceMode.Impulse);
		hummingbirdRigid.AddTorque (-transform.up * rotateSpeed,ForceMode.Impulse);
	}


	public void Attack(){
		hummingbirdAnimator.SetTrigger ("Attack");
	}

	public void LookAround(){
		hummingbirdAnimator.SetTrigger ("LookAround");
	}

	public void SitDown(){
		hummingbirdAnimator.SetTrigger ("SitDown");
	}

	public void StandUp(){
		hummingbirdAnimator.SetTrigger ("StandUp");
	}

	public void StandFlap(){
		hummingbirdAnimator.SetTrigger ("StandFlap");
	}

	public void Nectar(){
		hummingbirdAnimator.SetTrigger ("Nectar");
	}

	public void Hit(){
		hummingbirdAnimator.SetTrigger ("Hit");
	}

	public void Move(float v,float h){

		if (isFlying) {
			if (v > 0.1f || v < -0.1f) {
				hummingbirdRigid.AddForce ((transform.forward * forwardSpeed - transform.up * .05f) * -v);
			} else {
				hummingbirdRigid.AddForce (transform.forward * forwardSpeed);
			}
			hummingbirdRigid.AddTorque (transform.up * h * rotateSpeed);
		}


		hummingbirdAnimator.SetFloat ("Forward",v);
		hummingbirdAnimator.SetFloat ("Turn",h);
	}
}
