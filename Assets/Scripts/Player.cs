﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	
	public Vector3 bounceForce;
	
	Animator _animator;
	
	bool _isFlying;
			
	private Statistics stat;
	private float speed;
	private List<Vector3> prevPos;
	
	private Rigidbody rigidbody;
	
	void Awake() 
	{
		stat = new Statistics();
		rigidbody = GetComponent<Rigidbody>();
	}
	
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_isFlying = false;
		
		prevPos = new List<Vector3>();
		prevPos.Add( transform.position );
		speed = 0;		
	}
	
	void Update()
	{
		Vector3 newPos = transform.position;
		prevPos.Insert( 0, newPos );
		
		const int maxSize = 40;
		speed = 0F;
		for( int i=0; i<prevPos.Count-1; i++ )
		{
			var p1 = prevPos[i];
			var p2 = prevPos[i+1];
			var s = Mathf.Abs( Vector3.Distance( p1, p2 ));
			if( s > speed )
				speed = s;
		}
		
		if( prevPos.Count > maxSize )
		{
			prevPos.RemoveAt( maxSize );
			
//			if( speed < 0.1F )
//				rigidbody.isKinematic = true;
		}
	}
	
	// Update is called once per frame
	float time = 3;
	void FixedUpdate () {		
		
		time -= Time.deltaTime;
		
		if( time <= 0 )
		{
			if( !_animator.IsInTransition( 0 ) )
			{
				if( _animator.GetInteger( "Idle" ) == 0 )
					_animator.SetInteger( "Idle", 2 );
				else 
					_animator.SetInteger( "Idle", 0 );
				
				time = Random.Range( 3, 10 );
			}
		}
	}
	
	void OnCollisionEnter( Collision collision )
	{
		if( collision.gameObject.tag.CompareTo( "Ground" ) == 0 )
		{
			_isFlying = false;
			_animator.SetBool( "Fly", false );	
						
			if( speed > 0.1F )
				this.rigidbody.AddForce( bounceForce );
		}
		else 
		{
			this.stat.Points += 1;
		}
	}
	
	void OnCollisionExit( Collision collision )
	{
		_isFlying = true;
		_animator.SetBool( "Fly", true );
	}	
	
	
	void OnGUI()
	{
		GUI.Label( new Rect( 10.0f, 10.0f, 120.0f, 30.0f ), "Score: " + this.stat.Points.ToString() );
		
		GUI.Label( new Rect(10F, 40F, 120F, 30F), "Speed: " + speed.ToString() );
//		EditorGUILayout.TextField ("File Name:", "test file name");
//		
//		if(GUILayout.Button(recordButton)) {
//			if(recording) { //recording
//				status = "Idle...";
//				recordButton = "Record";				
//				recording = false;
//			} else { // idle
//				capturedFrame = 0;
//				recordButton = "Stop";
//				recording = true;
//			}
//		}
//		EditorGUILayout.LabelField ("Status: ", status);
	}	
}
