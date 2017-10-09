using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	public GUI.WindowFunction InfoWindow;
	public float speed;
	public GUIText countText;
	public GUIText winText;
	public GUIText infoText;	
	public Text distanceText;
	public GUIText effectText;
	private int count;
	private float moveHorizontal;
	private float moveVertical;
	private bool reverse;
	private bool viewChanged;	
	private bool viewChanged2;
	private int nbReverse;
	private int nbEffectText;
	public GameObject Camera;
	public GameObject Finish;
	public Slider progressBarSlider;
	public AudioSource audioFall;
	public AudioSource audioCollision;
	private bool tombe;



	void Start()
	{
		count = 0;
		countText.text = "Nb bananas : " + count.ToString()+"/10";
		winText.text = "";
		infoText.text = "";
		distanceText.text = "";
		effectText.text = "";
		reverse = false;
		viewChanged = false;
		viewChanged2 = false;
		nbReverse = 0;
		nbEffectText = 0;
		Finish = GameObject.Find ("Finish");
		tombe = false;
	}

	void FixedUpdate()
	{

		if (reverse) {	
			moveHorizontal = -Input.GetAxis ("Horizontal");
			moveVertical = Input.GetAxis ("Vertical");

		} else {
			moveHorizontal = Input.GetAxis ("Horizontal");
			moveVertical = Input.GetAxis ("Vertical");

		}
		if (viewChanged) {
			Camera.transform.position = transform.position + new Vector3 (0, 20, 0);
			Camera.transform.rotation = Quaternion.Euler (90, 0, 0);
		}else if (viewChanged2) {
			Camera.transform.position = transform.position + new Vector3(0,1,-3);;
			Camera.transform.rotation = new Quaternion(0, 0, 1, 0);
		} else {
			Camera.transform.rotation = new Quaternion(0,0,0,0);
			Camera.transform.position = transform.position + new Vector3(0,1,-3);
		}
		if (nbEffectText > 0 && (effectText.fontSize<140)) {
			effectText.fontSize = (int)(effectText.fontSize*1.075f);
		}
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		GetComponent<Rigidbody>().AddForce (movement * speed * Time.deltaTime);


		if(Input.GetKeyDown(KeyCode.Space)){
			Vector3 down = Vector3.down;
			if (Physics.Raycast (transform.position, down, 0.55f)) {
				GetComponent<Rigidbody>().AddForce (new Vector3(0,200,0));
			}

		}


		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}

		if(Input.GetKeyDown(KeyCode.R)){
			GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

			transform.position=new Vector3(0,5,0);		
		}
		if (transform.position.y < -5) {

			SetEffectText("Press R to respawn!");

		} 




		distanceText.text = ((int)this.gameObject.transform.position.z )+" m / 1000m";
		progressBarSlider.value = (this.gameObject.transform.position.z*100) / Finish.transform.position.z;

		if (this.gameObject.transform.position.y < 0 && !audioFall.isPlaying && !tombe) {
			tombe=true;
			audioFall.Play();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.transform.parent!=null){
			if(other.transform.parent.gameObject.tag.Equals("PickUp")){
				other.gameObject.SetActive(false);
				if (other.gameObject.tag.Equals ("Boost")) 
				{
					SpeedUp();
				}
				else if (other.gameObject.tag.Equals ("Penality")) 
				{
					SpeedDown();
				}
				else if (other.gameObject.tag.Equals ("Reverse Control")) 
				{
					ReverseControl();
				}
				else if (other.gameObject.tag.Equals ("Change View")) 
				{
					ChangeView();
				}			
				else if (other.gameObject.tag.Equals ("Change View 2")) 
				{
					ChangeView2();
				}
				else if(other.gameObject.tag.Equals ("Banana")){
					count++;
					countText.text = "Nb bananas : " + count.ToString() + "/10";
				}
			}
		}else if (other.gameObject.tag.Equals ("Finish")) {
			if(count>=10)
			{				
				infoText.text = "";
				winText.text = "You win";
				transform.position=new Vector3(0,5,0);
			}
			else{
				SetEffectText("Il vous manque des bananes !!!");
				transform.position=new Vector3(0,5,0);
			}

		}
	}



	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag.Equals("Wall")){
			audioCollision.Play();
		}
	}

	void SetEffectText(string s){
		nbEffectText++;
		effectText.fontSize = 30;
		effectText.text = s;
		StartCoroutine(WaitAndDo(0.75f, StopSetEffectText));
	}

	public void StopSetEffectText()
	{
		nbEffectText--;
		if (nbEffectText == 0) {
			effectText.fontSize = 30;
			effectText.text = "";
		}
	}

	void SpeedUp(){
		SetEffectText("Speed up!");
		speed = speed * 5;
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().AddForce (movement * speed * Time.deltaTime);
		StartCoroutine(WaitAndDo(1, StopSpeedUp));
	}

	public void StopSpeedUp()
	{
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		speed = speed / 5;
		GetComponent<Rigidbody>().AddForce (movement * speed * Time.deltaTime);
	}
	
	void SpeedDown(){
		SetEffectText("Speed down!");
		speed = speed / 5;
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().AddForce (movement * speed * Time.deltaTime);
		StartCoroutine(WaitAndDo(2, StopSpeedDown));
	}

	public void StopSpeedDown()
	{
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		speed = speed * 5;
		GetComponent<Rigidbody>().AddForce (movement * speed * Time.deltaTime);
	}

	void ReverseControl(){
		SetEffectText("reverse control!");
		nbReverse++;
		reverse = true;		
		StartCoroutine(WaitAndDo(2, StopReverseControl));
	}

	public void StopReverseControl()
	{
		nbReverse--;
		if (nbReverse == 0) {
			reverse = false;
		}
	}

	void ChangeView(){
		SetEffectText("Vertical view!");
		viewChanged = true;
		StartCoroutine(WaitAndDo(5, StopChangeView));
	}
	
	public void StopChangeView()
	{
		viewChanged = false;
	}

	void ChangeView2(){
		if (viewChanged == false) {
			SetEffectText("reverse view!");
			viewChanged2 = true;
			StartCoroutine (WaitAndDo (5, StopChangeView2));
		}
	}
	
	public void StopChangeView2()
	{
		viewChanged2 = false;
	}

	delegate void DelayedMethod();
	
	IEnumerator WaitAndDo(float time, DelayedMethod method)
	{
		yield return new WaitForSeconds(time);
		method();
	}
}
