using UnityEngine;
using System.Collections;

public class HummingbirdCameraScript : MonoBehaviour {
	public GameObject target;
	public float turnSpeed=.2f;
	public GameObject cameraOrigin;
	float cameraAngleX=150f;
	float cameraAngleY=0f;

	public void Start(){
		Quaternion arotation = Quaternion.identity;
		Vector3 eua = Vector3.zero;
		eua.y = 360f-cameraAngleY;
		eua.z = 0f;
		eua.x = 180f+cameraAngleX;
		arotation.eulerAngles = eua;
		cameraOrigin.transform.localRotation= arotation;
	}
	
	public void CameraRotationX(float x){
		cameraAngleX = x;
		Quaternion arotation = Quaternion.identity;
		Vector3 eua = Vector3.zero;
		eua.y = 360f-cameraAngleY;
		eua.z = 0f;
		eua.x = 180f+cameraAngleX;
		arotation.eulerAngles = eua;
		cameraOrigin.transform.localRotation= arotation;
	}
	public void CameraRotationY(float y){
		cameraAngleY = y;
		Quaternion arotation = Quaternion.identity;
		Vector3 eua = Vector3.zero;
		eua.y = 360f-cameraAngleY;
		eua.z = 0f;
		eua.x = 180f+cameraAngleX;
		arotation.eulerAngles = eua;
		cameraOrigin.transform.localRotation= arotation;
	}
	void FixedUpdate(){
		transform.position = Vector3.Lerp (transform.position,target.transform.position,Time.deltaTime*10f);
		transform.rotation = Quaternion.Lerp (transform.rotation,target.transform.rotation,Time.deltaTime*turnSpeed);
	}
}
