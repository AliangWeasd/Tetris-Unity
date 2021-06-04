using UnityEngine;
using System.Collections;

public class Lines : MonoBehaviour {
	
	public Sprite[] Numbers;
	public GameObject[] amount;

	public GameObject blockManager;
	public int lines = 0;
	public int level = 0;

	//The difficulty set in the menu changes "level".
	//"level" is used to set the default rate of fall in the game.
	void Start () {
		level = GameObject.Find ("Menu").GetComponent<MenuMoveA> ().number * 10;

		blockManager.GetComponent<Fall> ().rate = 0.8833f - (0.07777f * level/10);
	}

	//A clone os component "Score". Changed to track lines made.
	public void Add (int count) {
		lines += count;
		int tempTotal = lines;
		int max = 0;

		while (tempTotal > 0) {
			tempTotal = tempTotal / 10;
			max++;
		}
			
		for (int x = 0; x < amount.Length; x++) {
			int digit = (int)((lines / Mathf.Pow (10, (amount.Length - 1 - x))) % 10);
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

			if (lines < 100) {
				amount[0].GetComponent<SpriteRenderer> ().sprite = Numbers [0];
				if (lines < 10)
					amount[1].GetComponent<SpriteRenderer> ().sprite = Numbers [0];
			}
		}

		//The level is seperated from lines so that I can set the fallrate to a higher number.
		level += count;
		if(level <= 90)
			blockManager.GetComponent<Fall> ().rate = 0.8833f - (0.07777f * level/10);
		if(level >= 100)
			blockManager.GetComponent<Fall> ().rate = 0.1833f - (0.0121f * ((level-90)/10));
	}
}
