using UnityEngine;
using System.Collections;

public class nextTetronimo : MonoBehaviour {

	public Sprite[] Blocks;
	public Sprite[] Tetris;

	public int prev_rand;
	public int rand;

	//Randomly determines the next tetronimo to use, and changes the sprite accordingly.
	public void Shuffle(){
		prev_rand = rand;

		//The "I" tetronimo.
		if (prev_rand == 0) {
			GetComponent<Fall> ().x1 = 4;
			GetComponent<Fall> ().y1 = 17;
			GetComponent<Fall> ().x2 = 5;
			GetComponent<Fall> ().y2 = 17;
			GetComponent<Fall> ().x3 = 6;
			GetComponent<Fall> ().y3 = 17;
			GetComponent<Fall> ().x4 = 7;
			GetComponent<Fall> ().y4 = 17;
		}
		//The "T" tetronimo.
		if (prev_rand == 1) {
			GetComponent<Fall> ().x1 = 4;
			GetComponent<Fall> ().y1 = 17;
			GetComponent<Fall> ().x2 = 5;
			GetComponent<Fall> ().y2 = 17;
			GetComponent<Fall> ().x3 = 6;
			GetComponent<Fall> ().y3 = 17;
			GetComponent<Fall> ().x4 = 5;
			GetComponent<Fall> ().y4 = 16;
		}
		//The cube tetronimo.
		if (prev_rand == 2) {
			GetComponent<Fall> ().x1 = 5;
			GetComponent<Fall> ().y1 = 16;
			GetComponent<Fall> ().x2 = 6;
			GetComponent<Fall> ().y2 = 16;
			GetComponent<Fall> ().x3 = 5;
			GetComponent<Fall> ().y3 = 17;
			GetComponent<Fall> ().x4 = 6;
			GetComponent<Fall> ().y4 = 17;
		}
		//The "L" tetronimo.
		if (prev_rand == 3) {
			GetComponent<Fall> ().x1 = 4;
			GetComponent<Fall> ().y1 = 16;
			GetComponent<Fall> ().x2 = 4;
			GetComponent<Fall> ().y2 = 17;
			GetComponent<Fall> ().x3 = 5;
			GetComponent<Fall> ().y3 = 17;
			GetComponent<Fall> ().x4 = 6;
			GetComponent<Fall> ().y4 = 17;
		}
		//The "J" tetronimo.
		if (prev_rand == 4) {
			GetComponent<Fall> ().x1 = 4;
			GetComponent<Fall> ().y1 = 17;
			GetComponent<Fall> ().x2 = 5;
			GetComponent<Fall> ().y2 = 17;
			GetComponent<Fall> ().x3 = 6;
			GetComponent<Fall> ().y3 = 17;
			GetComponent<Fall> ().x4 = 6;
			GetComponent<Fall> ().y4 = 16;
		}
		//The "S" tetronimo.
		if (prev_rand == 5) {
			GetComponent<Fall> ().x1 = 4;
			GetComponent<Fall> ().y1 = 16;
			GetComponent<Fall> ().x2 = 5;
			GetComponent<Fall> ().y2 = 16;
			GetComponent<Fall> ().x3 = 5;
			GetComponent<Fall> ().y3 = 17;
			GetComponent<Fall> ().x4 = 6;
			GetComponent<Fall> ().y4 = 17;
		}
		//The "Z" tetronimo.
		if (prev_rand == 6) {
			GetComponent<Fall> ().x1 = 4;
			GetComponent<Fall> ().y1 = 17;
			GetComponent<Fall> ().x2 = 5;
			GetComponent<Fall> ().y2 = 17;
			GetComponent<Fall> ().x3 = 5;
			GetComponent<Fall> ().y3 = 16;
			GetComponent<Fall> ().x4 = 6;
			GetComponent<Fall> ().y4 = 16;
		}
		GetComponent<Fall>().type = Blocks[rand];

		rand = Random.Range (0, 7);
		GetComponent<SpriteRenderer>().sprite = Tetris [rand];
	}
}
