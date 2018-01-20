using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {

	public double q;
	public double r;
	public double s;

	public bool rounded = true;

	public Hex ( int q, int r, int s ) {
		this.q = (double) q;
		this.r = (double) r;
		this.s = (double) s;
	}

	public Hex ( float q, float r, float s ) {
		this.q = (double) q;
		this.r = (double) r;
		this.s = (double) s;
	}

	public Hex ( double q, double r, double s ) {
		this.q = q;
		this.r = r;
		this.s = s;
	}
	
}
