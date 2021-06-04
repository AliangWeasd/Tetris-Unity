using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {
	
	public Sprite[] Numbers;
	public GameObject[] amount;
	public int total = 0;

	//Code used for tracking the current game score and modifying the number graphics.
	public void Redigit(int score){
		//"total" = the total score. "tempTotal" = "total" clone. "max" = number of digits in "total".
		total += score;
		int tempTotal = total;
		int max = 0;

		//Calculates how many digits are in "total".
		while (tempTotal > 0) {
			tempTotal = tempTotal / 10;
			max++;
		}

		//"digit" finds the current value in a digit.
		//A switch-case is used to change the score on-screen.
		for (int x = 0; x < amount.Length; x++) {
			int digit = (int)((total / Mathf.Pow (10, (amount.Length - x - 1))) % 10);

			switch (digit) {
				case 0:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [1];
					break;
				case 1:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [2];
					break;
				case 2:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [3];
					break;
				case 3:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [4];
					break;
				case 4:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [5];
					break;
				case 5:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [6];
					break;
				case 6:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [7];
					break;
				case 7:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [8];
					break;
				case 8:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [9];
					break;
				case 9:
					amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [10];
					break;
			}

			//Any Spriteblocks higher than the total digits is rendered empty.
			if (x <= amount.Length - 1 - max) {
				amount[x].GetComponent<SpriteRenderer> ().sprite = Numbers [0];
			}
		}
	}
}
