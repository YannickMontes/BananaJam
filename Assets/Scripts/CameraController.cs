using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GameObject player;
	private Vector3 offset;

	// Use this for initialization
	void Start () 
	{
		offset = transform.position;
		transform.rotation = new Quaternion(0,0,0,0);
		transform.position = player.transform.position + new Vector3(0,1,-3);
		


	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		//if (player.transform.position.y > -1.5)
			//camera vue de haut incline
			//transform.position = player.transform.position + offset;

			//zoom
			//transform.position = player.transform.position +new Vector3(0,3,-3) ;

			//first person
			//transform.position = player.transform.position + new Vector3(0,1,-3);

			//vertical camera above the player
			/*transform.position = player.transform.position + new Vector3 (0, 20, 0);
			transform.rotation = Quaternion.Euler (90, 0, 0);*/

	}
}
