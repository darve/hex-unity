using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTools : MonoBehaviour {

	int EVEN = 1;
	int ODD = -1;

	public Hex[] hex_diagonals = new Hex[6] {
		new Hex(1, 0, -1), 
		new Hex(1, -1, 0), 
		new Hex(0, -1, 1), 
		new Hex(-1, 0, 1), 
		new Hex(-1, 1, 0), 
		new Hex(0, 1, -1)
	};

	/**
	 * Add two hexes together
	 */
	public Hex hex_add (Hex a, Hex b) {

		return new Hex(a.q + b.q, a.r + b.r, a.s + b.s);

	}

	/**
	 * Subtract Hex B from Hex A
	 */
	public Hex hex_subtract (Hex a, Hex b) {

		return new Hex(a.q - b.q, a.r - b.r, a.s - b.s);

	}

	/**
	 * Multiplies all of the Hex properties by scalar K
	 */
	public Hex hex_scale( Hex a, double k) {
	
		return new Hex(a.q * k, a.r * k, a.s * k);

	}

	/**
	 * Returns the neighbouring Hex for the value provided
	 */
	public Hex hex_diagonal_neighbour( Hex hex, int direction ) {
	
		return hex_add (hex, hex_diagonals[direction]);

	}

	/**
	 * Calculate the magnitude of the Hex
	 */
	public float hex_length ( Hex hex ) {

		return (System.Math.Abs (hex.q) + System.Math.Abs (hex.r) + System.Math.Abs (hex.s)) / 2;

	}

	/**
	 * Calculate the distance between Hexes
	 */
	public float hex_distance( Hex a, Hex b) {
	
		return hex_length (hex_subtract (a, b));

	}

	/**
	 * Get the nearest hex to a set of coordinates
	 */
	public Hex hex_round(Hex h) {
	
		double q = h.q;
		double r = h.r;
		double s = h.s;
 
		double q_diff = System.Math.Abs(q - h.q);
		double r_diff = System.Math.Abs(r - h.r);
		double s_diff = System.Math.Abs(s - h.s);

		if (q_diff > r_diff && q_diff > s_diff) {
			q = -r - s;
		} else {
			if (r_diff > s_diff) {
				r = -q - s;
			} else {
				s = -q - r;
			}
		}

		return new Hex (q, r, s);

	}

	/**
	 * Linear interpolation between two Hex's
	 */
	public Hex hex_lerp( Hex a, Hex b, double t ) {

		return new Hex (a.q + (b.q - a.q) * t, a.r + (b.r - a.r) * t, a.s + (b.s - a.s) * t);

	}
		
	public HexCoord qoffset_from_cube( double offset, Hex h) {

		double col = h.q;
		double row = h.r + ((h.q + offset * (h.q & 1)) / 2);

		return new HexCoord (col, row);

	}

	public Hex qoffset_to_cube (double offset, HexCoord h) {
	
		double q = h.col;
		double r = h.row - ((h.col + offset * h.col & 1)) / 2;
		double s = -q -r;

		return new Hex(q, r, s);

	}

	public HexCoord roffset_from_cube (double offset, Hex h) {

		double col = h.q + ((h.r + offset * (h.r & 1)) / 2);
		double row = h.r;	

		return new HexCoord(col, row);

	}

	public Hex roffset_to_cube (double offset, HexCoord h) {

		double q = h.col - (double)((h.row + offset * (h.row & 1)) / 2);
		double r = h.row;
		double s = -q - r;

		return new Hex (q, r, s);

	}

	public Vector2 hex_to_pixel( HexLayout layout, Hex h) {

		HexOrientation M = layout.orientation;
		Vector2 size = layout.size;
		Vector2 origin = layout.origin;

		double x = (M.f0 * h.q + M.f1 * h.r) * size.x;
		double y = (M.f2 * h.q + M.f3 * h.r) * size.y;

		return new Vector2 ((x + origin.x), (y + origin.y));
	
	}

	public Hex pixel_to_hex (HexLayout layout, Vector2 p) {

		HexOrientation M = layout.orientation;

		Vector2 size = layout.size;
		Vector2 origin = layout.origin;
		Vector2 pt = new Vector2 ((p.x - origin.x) / size.x, (p.y - origin.y) / size.y);

		double q = M.b0 * pt.x + M.b1 * pt.y;
		double r = M.b2 * pt.x + M.b3 * pt.y;

		return new Hex ( q, r, -q - r);
		
	}

	public Vector2 hex_corner_offset (HexLayout layout, int corner) {

		HexOrientation M = layout.orientation;
		Vector2 size = layout.size;

		double angle = 2 * System.Math.PI * (corner + M.start_angle) / 6;

		return new Vector2 (size.x * (float)System.Math.Cos (angle), size.y * (float)System.Math.Sin (angle));

	}

	public Vector2[] polygon_corners (HexLayout layout, Hex h) {

		Vector2[] corners = new Vector2[6];
		Vector2 center = hex_to_pixel (layout, h);

		for ( int i = 0; i < 6; i++ ) {
			Vector2 offset = hex_corner_offset (layout, i);
			corners[i] = new Vector2 (center.x + offset.x, center.y + offset.y);
		}

		return corners;

	}

	public string hex_to_string (Hex h) {

		return string.Format ("{0},{1},{2}", h.q, h.r, h.s);

	}

	public Hex string_to_hex (string str) {

		string[] q = str.Split (',');
		return new Hex( int.Parse(q[0]), int.Parse(q[2]), int.Parse(q[4]) );

	}
		
}

public class HexCoord : MonoBehaviour {

	public double col;
	public double row;

	public HexCoord (double col, double row) {

		this.col = col;
		this.row = row;

	}

}

public class HexOrientation : MonoBehaviour {

	public double f0;
	public double f1;
	public double f2;
	public double f3;

	public double b0;
	public double b1;
	public double b2;
	public double b3;

	public double start_angle;

	public HexOrientation (double f0, double f1, double f2, double f3, double b0, double b1, double b2, double b3, double start_angle ) {

		this.f0 = f0;
		this.f1 = f1;
		this.f2 = f2;
		this.f3 = f3;

		this.b0 = b0;
		this.b1 = b1;
		this.b2 = b2;
		this.b3 = b3;

		this.start_angle = start_angle;

	}

}

public class HexLayout : MonoBehaviour {	

	public HexOrientation orientation;
	public Vector2 size;
	public Vector2 origin;

	public HexLayout ( HexOrientation orientation, Vector2 size, Vector2 origin ) {
		
		this.orientation = orientation;
		this.size = size;
		this.origin = origin;

	}

}
	