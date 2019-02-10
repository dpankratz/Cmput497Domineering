using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 1f;
		
	private void Update () {
	    transform.LookAt(_target);
	    transform.Translate(_speed * Vector3.right * Time.deltaTime);
    }
}
