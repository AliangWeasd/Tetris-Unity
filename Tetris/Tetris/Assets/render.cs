using UnityEngine;
using System.Collections;

public class render : MonoBehaviour {

	//Makes a Cartesian graph.
	public int [,] blocks = new int[12,20];
	public GameObject [,] oblocks = new GameObject[12,20];
	public GameObject spriteBlock;
	public GameObject blockFall;
	public Sprite Empty;

	int x = 0;
	int y = 0;
	float ratio = 0.08f;

	//Creates all the spaces that the gamespace uses.
	//Also the first action taken in the game.
	void Awake () {
		Vector2 pos;

		//Puts a "Clone" on every graph point.
		//Row
		for (x = 0; x < blocks.GetLength (0); x++) {
			//Column
			for (y = 0; y < blocks.GetLength (1); y++) {
				//0.08f keeps the ratio correct. The 0.2f makes it so the game ignores the first two sections of the screen.
				pos = new Vector2 (x*ratio + .12f, y*ratio - 0.04f);

				if (x == 1)
					spriteBlock.GetComponent<SpriteRenderer> ().sortingLayerName = "1st Blocks";
				else
					spriteBlock.GetComponent<SpriteRenderer> ().sortingLayerName = "Blocks";
				//If the spriteblock is on the edge or on the bottom, the spriteblock is considered the "walls" of the gamespace.
				if (x == 0 || x == 11 || y == 0) {
					spriteBlock.GetComponent<SpriteRenderer> ().enabled = false;
					spriteBlock.tag = "Filled";
				} else {
					spriteBlock.GetComponent<SpriteRenderer> ().enabled = true;
					spriteBlock.tag = "Empty";
				}
				
				//Sets the name to be its position on the array.
				spriteBlock.name = x + "," + y;
				Instantiate (spriteBlock, pos, Quaternion.identity);

				oblocks [x, y] = spriteBlock;
			}
		}
		blockFall.GetComponent<nextTetronimo> ().rand = Random.Range (0, 7);
		blockFall.GetComponent<Fall> ().Reset ();
	}
}
