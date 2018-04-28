using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract jiki controll.
/// 自機コントロールの抽象クラス
/// initialMove スポーン・リスポン時の動作
/// InitialChar スポーン時無敵処理
/// MoveChar 自機のコントロール
/// </summary>
public abstract class AbstractJikiControll : MonoBehaviour {

	protected Camera cam;
	protected bool damageEnabled = false;//eventmanager
	private int weapon = 0;
	protected bool bombEnabled = false;

	public Transform bullet;
	public int HP = 1;
	public Transform cutin;
	public Transform bombObj;

	//配信イベント
	public static UnityAction Death;

	void Awake(){
		//set parent
		Transform p = GameObject.Find("SceneController").transform;
		transform.parent = p;		
	}

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		StartCoroutine ("InitialChar");
	}

	IEnumerator InitialChar(){
		Debug.Log ("EnterGame");

		yield return new WaitForFixedUpdate();
	}

	/// <summary>
	/// スポーン時の動き。終わったらmovecharに移行.
	/// </summary>
	/// <returns>The move.</returns>
	public virtual IEnumerator InitialMove(){

		Vector3 outPos = new Vector3(0.0f , 0.0f , -20.0f);
		Vector3 initPos = new Vector3(0.0f , 0.0f , -2.0f);
		transform.position = outPos;

		int count = 0;

		while(true){
			transform.position = Vector3.Slerp(transform.position , initPos ,Time.deltaTime*3.0f);
			count++;
			if(count>60){break;}
			yield return new WaitForFixedUpdate();	
		}
		Debug.Log("break");
		//bombEnabled = true;
		StartCoroutine("MoveChar");
	}



	public virtual IEnumerator MoveChar(){
		Vector3 prevPos = new Vector3(0,0,0);
		int counter = 0;//frame count
		//move
		while(true){
			Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,25.0f));
			Vector3 mPos = transform.position + pos - prevPos;
			Vector3 sPos = Camera.main.WorldToScreenPoint(mPos);			
			if(Input.GetMouseButton(0)){
				//transform.position = Vector3.Lerp(transform.position , pos ,0.25f);
				if(counter != 0){
					if(sPos.x <= Screen.width &&  sPos.x >= 0.0 && sPos.y <= Screen.height && sPos.y >= 0.0){
						transform.position = mPos ;
					}

				}
				counter++;
				prevPos = pos;
			}else{
				counter=0;
			}
			if(Input.GetMouseButton(1)){
				StartCoroutine("bomb");
			}

			yield return new WaitForFixedUpdate();
		}		
	}


	/// <summary>
	/// ショット時の挙動　オーバーライドする
	/// </summary>
	public virtual IEnumerator Shoot(){
		float temps=0.0f;
		while(true){
			if(Input.GetButton("Fire1")){
				temps = Time.time;
			}

			if(Input.GetButton("Fire1") && (Time.time - temps)<0.2){
				//short click effect
			}

			yield return new WaitForSeconds(0.07f);
		}
	}

	/// <summary>
	/// 自機や敵の弾と接触時にsendmessageで呼び出される。
	/// </summary>
	/// <param name="damage">Damage.</param>
	public virtual void Hit(int damage){
		HP-= damage;
		if(HP<0){
			//イベント配信
			Death ();
			Destroy(gameObject);
		}
	}

	//sendmessage
	/// <summary>
	/// Spawn this instance.
	/// スポーン時・ステージ遷移時等外部から初期動作をトリガーするときに使用
	/// sendmessageで受け取る
	/// </summary>
	public void SpawnJiki(){
		StartCoroutine("InitialMove");
	}

}
