using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機の制御
/// </summary>
public class JikiControll : AbstractJikiControll {

	public GameObject cannonR;
	public GameObject cannonL;

	void Start () {
		cam = Camera.main;
		StartCoroutine ("InitialChar");
	}

	IEnumerator InitialChar(){
		Debug.Log ("EnterGame");

		//テスト時のみ使用する
		//gamemanagerにうつす
		//StartCoroutine ("InitialMove");
		StartCoroutine ("Shoot");

		yield return new WaitForFixedUpdate();
	}

	/// <summary>
	/// 自機の移動
	/// </summary>
	/// <returns>void</returns>
	public override IEnumerator MoveChar(){

		while(true){
			Vector3 oldPos = transform.position;
			transform.Translate(Input.GetAxisRaw("Horizontal") * 1.0f,0f,Input.GetAxisRaw("Vertical")*0.7f);

			Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
			if(!(0 < viewPos.x && viewPos.x < 1 && 0 < viewPos.y && viewPos.y < 1) ){
				transform.position = oldPos;
			}

			if(Input.GetButton("Fire2")){
				//StartCoroutine("bomb");
			}

			yield return new WaitForFixedUpdate();
		}
	}

	/// <summary>
	/// Shoot this instance.
	/// </summary>
	public override IEnumerator Shoot(){

		float temps=0.0f;
		while(true){
			if(Input.GetButtonUp("Fire1")){
				temps = Time.time;
			}

			if(Input.GetButton("Fire1") && (Time.time - temps)>=0.1f){

				oneShot();
				yield return new WaitForSeconds(0.07f);

			}else if(Input.GetButton("Fire1") && (Time.time - temps)<0.1f){
				//short click effect
				oneShot();
			}

			yield return new WaitForFixedUpdate();
		}
	}

	private void oneShot(){
		Instantiate(bullet,cannonL.transform.position,Quaternion.Euler(new Vector3(-90f,0f,180f)));
		Instantiate(bullet,cannonR.transform.position,Quaternion.Euler(new Vector3(-90f,0f,180f)));		
	}



}
