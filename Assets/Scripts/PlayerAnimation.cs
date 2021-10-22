using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

	private Animator _anim;


	void Start()
	{
		_anim = GetComponent<Animator>();
	}

	void Update()
	{
		// if A or LEFT arrow is pressed...
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			_anim.SetBool("Turn_Left", true);
			_anim.SetBool("Turn_Right", false);
		}

		// if A or LEFT arrow is released...
		else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
		{
			_anim.SetBool("Turn_Left", false);
			_anim.SetBool("Turn_Right", false);
		}

		// if D or RIGHT arrow is pressed...
		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			_anim.SetBool("Turn_Right", true);
			_anim.SetBool("Turn_Left", false);
		}

		// if D or RIGHT arrow is released...
		else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
		{
			_anim.SetBool("Turn_Right", false);
			_anim.SetBool("Turn_Left", false);
		}
	}
}