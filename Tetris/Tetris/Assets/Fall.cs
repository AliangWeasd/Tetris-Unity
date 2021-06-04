using UnityEngine;
using System.Collections;

public class Fall : MonoBehaviour {
	public GameObject score;
	public GameObject lines;
	public Sprite type;
	public Sprite empty;

	string blockName = "";

	//The positions for all 4 blocks in the tetronimo.
	public int x1 = 5;
	public int y1 = 16;
	public int x2 = 6;
	public int y2 = 16;
	public int x3 = 7;
	public int y3 = 16;
	public int x4 = 7;
	public int y4 = 17;

	public int dropScore = 0;
	public int totalScore = 0;
	public bool resetable = true;
	public bool end = false;
	bool active = true;
	int turns = 0;
	int count = 0;
	float timex = 0f;
	float timey = 0f;
	float drop = 0f;
	public float rate = 1f;
	float rx = 0.5f;
	float ry = 0.04f;
	bool go = false;
	bool goY = false;
	bool autoY = true;
	public bool turnable = true;
		
	void Update () {
		//"Go" controls when the block moves.
		if (Input.GetButtonDown ("Horizontal"))
			go = true;

		//Movement along the x-axis
		if (Input.GetButton("Horizontal")){ 
			timex += Time.deltaTime;

			//Movement on a graph.
			if (resetable && go && (CheckX(x1,y1) && CheckX(x2,y2) && CheckX(x3,y3)) && CheckX(x4,y4)) {
				ChangeHortPos (ref x1, y1);
				ChangeHortPos (ref x2, y2);
				ChangeHortPos (ref x3, y3);
				ChangeHortPos (ref x4, y4);
				Ichange ();
				go = false;
			}

			//Controls the timing when the blocks automoves.
			if (timex >= rx) {
				go = true;
				rx = 0.1f;
			}
			timex = timex % rx;
		}

		//Automove is disabled when the button is released.
		if (Input.GetButtonUp ("Horizontal")) {
			rx = 0.5f;
			go = false;
			timex = 0;
		}

		//goY controls y-axis automove. When the block resets to the top, automove is cancelled.
		if (Input.GetButtonUp ("Vertical")) {
			autoY = true;
		}

		if (Input.GetButtonDown ("Vertical") || timey >= ry) {
			goY = true;
			timey = 0;
		}

		// Have decided to allow dropping while making horizontal movement to prevent "stickiness".
		if (Input.GetAxisRaw ("Vertical") < 0 /*&& !Input.GetButton("Horizontal")*/ && autoY) {
			timey += Time.deltaTime;
			drop -= Time.deltaTime;
		}
		//Movement along the y-axis. X-axis movement overrides vertical movement.
		if (goY){
			if (resetable) {
				if ((CheckY (x1, y1) || CheckY (x2, y2) || CheckY (x3, y3) || CheckY (x4, y4))) {
					lines.GetComponent<lineCheck> ().ReCheck (y1, y2, y3, y4);
					EndMove ();
					resetable = false;
					autoY = false;
					goY = false;
					drop = 0;
					turns = 0;
					lines.GetComponent<Score> ().Redigit (dropScore);
					dropScore = 0;
					StartCoroutine (lines.GetComponent<lineCheck> ().Check ());
				} else {
					ChangeVertPos (ref x1, ref y1);
					ChangeVertPos (ref x2, ref y2);
					ChangeVertPos (ref x3, ref y3);
					ChangeVertPos (ref x4, ref y4);
					Ichange ();
					goY = false;
					dropScore++;
				}
			} else if(count < 2 && !active){
				if(count != 1)
					EndReset ();
				drop = 0;
				goY = false;
				count++;
			}
		}

		//If any moving tile is blocked, the move ends.
		if (drop >= rate) {
			if (resetable) {
				if (CheckY (x1, y1) || CheckY (x2, y2) || CheckY (x3, y3) || CheckY (x4, y4)) {
					lines.GetComponent<lineCheck> ().ReCheck (y1, y2, y3, y4);
					EndMove ();
					resetable = false;
					autoY = false;
					drop = 0;
					turns = 0;
					lines.GetComponent<Score> ().Redigit (dropScore);
					dropScore = 0;
					StartCoroutine (lines.GetComponent<lineCheck> ().Check ());
				} else {
					AutoVertPos (ref x1, ref y1);
					AutoVertPos (ref x2, ref y2);
					AutoVertPos (ref x3, ref y3);
					AutoVertPos (ref x4, ref y4);
					Ichange ();
					drop = 0;
				}
			}
			else if(count < 2 && !active){
				if(count != 1)
					EndReset ();
				drop = 0;
				count++;
			}
		}

		if(resetable || end)
			drop += Time.deltaTime;

		if (Input.GetButtonDown ("Jump")) {
			Turn (GetComponent<nextTetronimo>().prev_rand);
		}

		if (count >= 2) {
			StartCoroutine (lines.GetComponent<lineCheck> ().End ());
			this.GetComponent<SpriteRenderer> ().sprite = empty;
			this.GetComponent<Fall> ().enabled = false;
		}
	}


	/* ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! FUNCTIONS DOWN HERE ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! */

	//Manually moves the block horizontally.
	void ChangeHortPos(ref int x,int y){
		if (Input.GetAxisRaw ("Horizontal") > 0) {
			if(Share(ref x, ref y))
				Move (x + 1, y);
			else {
				Clear (x, y);
				Move (x + 1, y);
			}
			x++;
		} 
		if (Input.GetAxisRaw ("Horizontal") < 0) {
			if (Share(ref x, ref y))
				Move (x - 1, y);
			else {
				Clear (x, y);
				Move (x - 1, y);
			}
			x--;
		}
	}

	//Manually moves the block down.
	void ChangeVertPos(ref int x, ref int y){
		if (Input.GetAxisRaw ("Vertical") < 0 && y != 1){
			if (Share (ref x, ref y))
				Move (x, y - 1);
			else {
				Clear (x, y);
				Move (x, y - 1);
			}
			y--;
		}
	}

	//Automatically moves the block down when 'drop' occurs.
	void AutoVertPos(ref int x, ref int y){
		if (Share (ref x, ref y))
			Move (x, y - 1);
		else {
			Clear (x, y);
			Move (x, y - 1);
		}
		y--;
	}

	//Renders the current tile to be white.
	//blockName finds the corresponding tile.
	public void Clear(int a, int b){
		blockName = a + "," + b + "(Clone)";

		GameObject.Find (blockName).GetComponent<SpriteRenderer> ().sprite = empty;
		GameObject.Find (blockName).tag = "Empty";
	}

	//Renders the next tile position.
	//Checks if the tile is then next to a static tile.
	public void Move(int a, int b){
		blockName = a + "," + b + "(Clone)";

		GameObject.Find (blockName).GetComponent<SpriteRenderer> ().sprite = type;

		if (a != 1 || a != 10 || GetBlockState(a,b) == "Empty" || GetBlockState(a,b) == "Empty")
			GameObject.Find (blockName).tag = "Moving";
		else
			GameObject.Find (blockName).tag = "Filled";
	}

	//Finds out if the named tile is occupied.
	string GetBlockState(int a, int b){
		blockName = a + "," + b + "(Clone)";
		return GameObject.Find (blockName).tag;
	}

	//Checks whether the block can horizontally move to a tile or not.
	bool CheckX(int x, int y){
		if((GetBlockState (x - 1, y) == "Filled") && Input.GetAxisRaw ("Horizontal") < 0|| (GetBlockState (x + 1, y) == "Filled") && Input.GetAxisRaw ("Horizontal") > 0)
			return false;
		else
			return true;
	}

	//Checks if the block has touched the bottom or has touched a static tile.
	bool CheckY(int x, int y){
		if(y == 1 || GetBlockState (x, y - 1) == "Filled")
			return true;
		return false;
	}

	//Checks if there are moving tiles already in the position. 
	bool Share(ref int x, ref int y){
		string block1 = x + "," + y + "(Clone)";
		string block2;
		string block3;
		string block4;

		if (x == x1 && y == y1) {
			block2 = x2 + "," + y2 + "(Clone)";
			block3 = x3 + "," + y3 + "(Clone)";
			block4 = x4 + "," + y4 + "(Clone)";

			if (block1 == block2 || block1 == block3 || block1 == block4)
				return true;
			return false;
		}
		if (x == x2 && y == y2) {
			block2 = x1 + "," + y1 + "(Clone)";
			block3 = x3 + "," + y3 + "(Clone)";
			block4 = x4 + "," + y4 + "(Clone)";

			if (block1 == block2 || block1 == block3 || block1 == block4)
				return true;
			return false;
		}
		if (x == x3 && y == y3) {
			block2 = x1 + "," + y1 + "(Clone)";
			block3 = x2 + "," + y2 + "(Clone)";
			block4 = x4 + "," + y4 + "(Clone)";

			if (block1 == block2 || block1 == block3 || block1 == block4)
				return true;
			return false;
		}
		if (x == x4 && y == y4) {
			block2 = x1 + "," + y1 + "(Clone)";
			block3 = x2 + "," + y2 + "(Clone)";
			block4 = x3 + "," + y3 + "(Clone)";

			if (block1 == block2 || block1 == block3 || block1 == block4)
				return true;
			return false;
		}
		return false;
	}

	//Turns the moving block into a static tile.
	public void EndMove (){
		GameObject.Find (x1 + "," + y1 + "(Clone)").tag = "Filled";
		GameObject.Find (x2 + "," + y2 + "(Clone)").tag = "Filled";
		GameObject.Find (x3 + "," + y3 + "(Clone)").tag = "Filled";
		GameObject.Find (x4 + "," + y4 + "(Clone)").tag = "Filled";
	}

	//Sets the initial sprite and block location.
	public void Reset () {
		GetComponent<nextTetronimo>().Shuffle();
		ResetMove (x1, y1);
		ResetMove (x2, y2);
		ResetMove (x3, y3);
		ResetMove (x4, y4);
		Ichange ();
		drop = 0;
	}

	public void ResetMove (int a, int b) {
		blockName = a + "," + b + "(Clone)";

		if (GameObject.Find (blockName).tag != "Empty") {
			active = false;
			resetable = false;
			end = true;
		}
		
		GameObject.Find (blockName).GetComponent<SpriteRenderer> ().sprite = type;
		GameObject.Find (blockName).tag = "Moving";
	}

	public void EndReset () {
		GetComponent<nextTetronimo>().Shuffle();
		Move (x1, y1);
		Move (x2, y2);
		Move (x3, y3);
		Move (x4, y4);
		Ichange ();
	}

	//Turns the moving piece while checking if the turn is valid.
	void Turn(int rand){
		if (turnable) {
			if (rand == 0) {
				if (turns == 0 && GetBlockState (x1 + 1, y1 - 1) != "Filled" && GetBlockState (x3 - 1, y3 + 1) != "Filled" && GetBlockState (x4 - 2, y4 + 2) != "Filled") {
					TurnCalc (ref x1, ref y1, 1, -1);
					TurnCalc (ref x3, ref y3, -1, 1);
					TurnCalc (ref x4, ref y4, -2, 2);
					GameObject.Find (x1 + "," + y1 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [9];
					GameObject.Find (x2 + "," + y2 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [10];
					GameObject.Find (x3 + "," + y3 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [10];
					GameObject.Find (x4 + "," + y4 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [11];
					turns = 1;
				} else if (turns == 1 && GetBlockState (x1 - 1, y1 + 1) != "Filled" && GetBlockState (x3 + 1, y3 - 1) != "Filled" && GetBlockState (x4 + 2, y4 - 2) != "Filled") {
					TurnCalc (ref x1, ref y1, -1, 1);
					TurnCalc (ref x3, ref y3, 1, -1);
					TurnCalc (ref x4, ref y4, 2, -2);
					GameObject.Find (x1 + "," + y1 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [0];
					GameObject.Find (x2 + "," + y2 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [7];
					GameObject.Find (x3 + "," + y3 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [7];
					GameObject.Find (x4 + "," + y4 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [8];
					turns = 0;
				}
			}
			if (rand == 1) {
				if (turns == 0 && GetBlockState (x1 + 1, y1 + 1) != "Filled") {
					TurnCalc (ref x1, ref y1, 1, 1);
					turns = 1;
				} else if (turns == 1 && GetBlockState (x4 - 1, y4 + 1) != "Filled") {
					TurnCalc (ref x4, ref y4, -1, 1);
					turns = 2;
				} else if (turns == 2 && GetBlockState (x3 - 1, y3 - 1) != "Filled") {
					TurnCalc (ref x3, ref y3, -1, -1);
					turns = 3;
				} else if (turns == 3 && GetBlockState (x3 + 1, y3 + 1) != "Filled" && GetBlockState (x4 + 1, y4 - 1) != "Filled" && GetBlockState (x1 - 1, y1 - 1) != "Filled") {
					TurnCalc (ref x3, ref y3, 1, 1);
					TurnCalc (ref x4, ref y4, 1, -1);
					TurnCalc (ref x1, ref y1, -1, -1);
					turns = 0;
				}
			}
			if (rand == 3) {
				if (turns == 0 && GetBlockState (x1 + 2, y1) != "Filled" && GetBlockState (x2 + 1, y2 - 1) != "Filled" && GetBlockState (x4 - 1, y4 + 1) != "Filled") {
					TurnCalc (ref x1, ref y1, 2, 0);
					TurnCalc (ref x2, ref y2, 1, -1);
					TurnCalc (ref x4, ref y4, -1, 1);
					turns = 1;
				} else if (turns == 1 && GetBlockState (x1, y1 + 2) != "Filled" && GetBlockState (x2 + 1, y2 + 1) != "Filled" && GetBlockState (x4 - 1, y4 - 1) != "Filled") {
					TurnCalc (ref x1, ref y1, 0, 2);
					TurnCalc (ref x2, ref y2, 1, 1);
					TurnCalc (ref x4, ref y4, -1, -1);
					turns = 2;
				} else if (turns == 2 && GetBlockState (x1 - 2, y1) != "Filled" && GetBlockState (x2 - 1, y2 + 1) != "Filled" && GetBlockState (x4 + 1, y4 - 1) != "Filled") {
					TurnCalc (ref x1, ref y1, -2, 0);
					TurnCalc (ref x2, ref y2, -1, 1);
					TurnCalc (ref x4, ref y4, 1, -1);
					turns = 3;
				} else if (turns == 3 && GetBlockState (x1, y1 - 2) != "Filled" && GetBlockState (x2 - 1, y2 - 1) != "Filled" && GetBlockState (x4 + 1, y4 + 1) != "Filled") {
					TurnCalc (ref x1, ref y1, 0, -2);
					TurnCalc (ref x2, ref y2, -1, -1);
					TurnCalc (ref x4, ref y4, 1, 1);
					turns = 0;
				}
			}
			if (rand == 4) {
				if (turns == 0 && GetBlockState (x1 + 1, y1 - 1) != "Filled" && GetBlockState (x3 - 1, y3 + 1) != "Filled" && GetBlockState (x4, y4 + 2) != "Filled") {
					TurnCalc (ref x1, ref y1, 1, -1);
					TurnCalc (ref x3, ref y3, -1, 1);
					TurnCalc (ref x4, ref y4, 0, 2);
					turns = 1;
				} else if (turns == 1 && GetBlockState (x1 + 1, y1 + 1) != "Filled" && GetBlockState (x3 - 1, y3 - 1) != "Filled" && GetBlockState (x4 - 2, y4) != "Filled") {
					TurnCalc (ref x1, ref y1, 1, 1);
					TurnCalc (ref x3, ref y3, -1, -1);
					TurnCalc (ref x4, ref y4, -2, 0);
					turns = 2;
				} else if (turns == 2 && GetBlockState (x1 - 1, y1 + 1) != "Filled" && GetBlockState (x3 + 1, y3 - 1) != "Filled" && GetBlockState (x4, y4 - 2) != "Filled") {
					TurnCalc (ref x1, ref y1, -1, 1);
					TurnCalc (ref x3, ref y3, 1, -1);
					TurnCalc (ref x4, ref y4, 0, -2);
					turns = 3;
				} else if (turns == 3 && GetBlockState (x1 - 1, y1 - 1) != "Filled" && GetBlockState (x3 + 1, y3 + 1) != "Filled" && GetBlockState (x4 + 2, y4) != "Filled") {
					TurnCalc (ref x1, ref y1, -1, -1);
					TurnCalc (ref x3, ref y3, 1, 1);
					TurnCalc (ref x4, ref y4, 2, 0);
					turns = 0;
				}
			}
			if (rand == 5) {
				if (turns == 0 && GetBlockState (x1, y1 + 1) != "Filled" && GetBlockState (x4 - 2, y4 + 1) != "Filled") {
					TurnCalc (ref x1, ref y1, 0, 1);
					TurnCalc (ref x4, ref y4, -2, 1);
					turns = 1;
				} else if (turns == 1 && GetBlockState (x1, y1 - 1) != "Filled" && GetBlockState (x4 + 2, y4 - 1) != "Filled") {
					TurnCalc (ref x1, ref y1, 0, -1);
					TurnCalc (ref x4, ref y4, 2, -1);
					turns = 0;
				}
			}
			if (rand == 6) {
				if (turns == 0 && GetBlockState (x3 - 1, y3) != "Filled" && GetBlockState (x4 - 1, y4 + 2) != "Filled") {
					TurnCalc (ref x3, ref y3, -1, 0);
					TurnCalc (ref x4, ref y4, -1, 2);
					turns = 1;
				} else if (turns == 1 && GetBlockState (x3 + 1, y3) != "Filled" && GetBlockState (x4 + 1, y4 - 2) != "Filled") {
					TurnCalc (ref x3, ref y3, 1, 0);
					TurnCalc (ref x4, ref y4, 1, -2);
					turns = 0;
				}
			}
		}
	}

	//The function used to directly move a turned tile.
	void TurnCalc(ref int x,ref int y, int a, int b){
		Clear (x, y);
		Move (x + a, y + b);
		x += a;
		y += b;
	}

	//Changes the sprite on the I block when it rotates.
	void Ichange(){
		if (type.name == "Hort_I_first") {
			if (turns == 1) {
				GameObject.Find (x1 + "," + y1 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [9];
				GameObject.Find (x2 + "," + y2 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [10];
				GameObject.Find (x3 + "," + y3 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [10];
				GameObject.Find (x4 + "," + y4 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [11];
			} else {
				GameObject.Find (x1 + "," + y1 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [0];
				GameObject.Find (x2 + "," + y2 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [7];
				GameObject.Find (x3 + "," + y3 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [7];
				GameObject.Find (x4 + "," + y4 + "(Clone)").GetComponent<SpriteRenderer> ().sprite = GetComponent<nextTetronimo> ().Blocks [8];
			}
		}
	}
}
