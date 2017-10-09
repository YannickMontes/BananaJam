using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

	public void LoadScene(){
	
		Application.LoadLevel (1);
	}

	public void QuitGame(){
		
		Application.Quit ();
	}
}
