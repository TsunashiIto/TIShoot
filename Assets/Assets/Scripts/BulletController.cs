using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletController : MonoBehaviour {

	public int damage = 1;
	public float speed = 1600;
	public static UnityAction<int> Hit;

	void Awake(){
		Transform p = GameObject.Find("SceneController").transform;
		transform.parent = p;
	}

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().AddForce(0.0f,0.0f,speed);
		StartCoroutine("lifeTime");
	}

	IEnumerator displife(){
		while(true){
			Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
			if(pos.z >  Screen.height/2){
				Destroy(gameObject);
			}				
			yield return 0;
		}
	}

	IEnumerator lifeTime(){
		yield return new WaitForSeconds(1.5f);
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision collision){
		//Debug.Log( collision.gameObject );
		collision.gameObject.SendMessage("Hit",damage,SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
