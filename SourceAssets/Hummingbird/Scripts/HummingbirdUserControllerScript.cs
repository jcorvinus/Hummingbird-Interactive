using UnityEngine;
using System.Collections;

public class HummingbirdUserControllerScript : MonoBehaviour {

	public HummingbirdCharacterScript hummingbirdCharacter;

	void Start () {
		hummingbirdCharacter = GetComponent<HummingbirdCharacterScript> ();	
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.A)) {
			if (!hummingbirdCharacter.isFlying){
				hummingbirdCharacter.HopLeft();
			}
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			if (!hummingbirdCharacter.isFlying){
				hummingbirdCharacter.HopForward();
			}
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			if (!hummingbirdCharacter.isFlying){
				hummingbirdCharacter.HopRight();
			}
		}



		if (Input.GetKeyDown(KeyCode.N)) {
			if(hummingbirdCharacter.isFlying){
				hummingbirdCharacter.Nectar();
			}
		}
		if (Input.GetKeyDown(KeyCode.H)) {
			if(hummingbirdCharacter.isFlying){
				hummingbirdCharacter.Hit();
			}
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			if(!hummingbirdCharacter.isFlying){
				hummingbirdCharacter.LookAround();
			}
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			if(!hummingbirdCharacter.isFlying){
				hummingbirdCharacter.StandFlap();
			}
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			if(!hummingbirdCharacter.isFlying){
				hummingbirdCharacter.StandUp();
			}
		}
		if (Input.GetKeyDown(KeyCode.B)) {
			if(!hummingbirdCharacter.isFlying){
				hummingbirdCharacter.SitDown();
			}
		}


		if (Input.GetKeyDown(KeyCode.L)) {
			if(hummingbirdCharacter.isFlying){
				hummingbirdCharacter.Landing();
			}
		}

		if (Input.GetButtonDown ("Jump")) {
			if (!hummingbirdCharacter.isFlying){
				hummingbirdCharacter.Soar ();
			}
		}

		if (Input.GetButtonDown ("Fire1")) {
			if(hummingbirdCharacter.isFlying){
				hummingbirdCharacter.Attack ();
			}
		}
	}
	
	void FixedUpdate(){
		float v = Input.GetAxis ("Vertical");
		float h = Input.GetAxis ("Horizontal");
		hummingbirdCharacter.Move (v,h);		
	}
}
