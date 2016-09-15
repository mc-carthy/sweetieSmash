using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {

	public BonusType bonus { get; set; }

	public int row { get; set; }
	public int column { get; set; }

	public string type { get; set; }

	public Candy() {
		bonus = BonusType.None; 
	}

	public bool IsSameType (Candy otherCandy) {
		return (string.Compare (this.type, otherCandy.type) == 0);
	}

	public void Initialize (string type, int row, int column) {
		this.type = type;
		this.row = row;
		this.column = column;
	}

	public static void SwapRowColumn (Candy c0, Candy c1) {
		int temp = c0.row;
		c0.row = c1.row;
		c1.row = temp;

		temp = c0.column;
		c0.column = c1.column;
		c1.column = temp;
	}
}
