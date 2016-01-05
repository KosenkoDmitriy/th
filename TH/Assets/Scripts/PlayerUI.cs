using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : Player {

	System.Random rand;

	public PlayerUI() {
		rand = new System.Random ();
	}

	public PlayerUI(Player player) {
		rand = new System.Random ();

		this.name = player.name;
		this.credits = player.credits;
		this.no = player.no;
		this.handPreflopString = player.handPreflopString;
		this.handPreflopStringReversed = player.handPreflopStringReversed;

		this.handPreflop = player.handPreflop;
		this.hand = player.hand;

		this.hands = player.hands;
		this.isFolded = player.isFolded;
		this.actionCurrent = player.actionCurrent;
		this.patternCurrent = player.patternCurrent;

		this.pattern = player.pattern;
		this.alt_patterns = player.alt_patterns;
	}
	
	public Image chip;
	List<Sprite> chipSpriteList;
	public Image dealer;
	public Text lblCredits;
	public Text lblAction;
	public Text lblName;

	public void SetChipRandomly() {
		// start init chips
		if (chipSpriteList == null)
		chipSpriteList = new List<Sprite>() {
			Resources.Load("chips_red", typeof(Sprite)) as Sprite,
			Resources.Load("chips_blue", typeof(Sprite)) as Sprite
		};
		// end init chips
		int index = rand.Next (0, chipSpriteList.Count);
		if (this.chip != null) this.chip.sprite = chipSpriteList [index];
	}
}
