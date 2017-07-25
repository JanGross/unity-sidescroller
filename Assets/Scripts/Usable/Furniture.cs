using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour {
    [SerializeField]
    private string f_Name = "Unnamed Furniture";
    [SerializeField]
    private string f_Desc = "Some furniture";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void interact()
    {
        Debug.Log(string.Format("You interacted with {0}", f_Name));
    }
}
