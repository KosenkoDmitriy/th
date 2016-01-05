﻿using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : Player {

	public PlayerUI() {
		// start init chips
		chipSpriteList = new List<Sprite>() {
			Resources.Load("chips_red", typeof(Sprite)) as Sprite,
			Resources.Load("chips_blue", typeof(Sprite)) as Sprite
		};
		// end init chips
	}

	public PlayerUI(Player player) {
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

//	public override object Clone()
//	{
//		PlayerUI copy = (PlayerUI)base.Clone();
//		copy.chip = this.chip;
//		return copy;
//	}

//	public Player player;
	public Image chip {
		get { return this.chip; }
		set { this.chip = value; SetChipRandomly();	}
	}

	List<Sprite> chipSpriteList;
	public Image dealer;
	public Text lblCredits;
	public Text lblAction;
	public Text lblName;

	public void SetChipRandomly() {
		var rand = new System.Random ();
		this.chip.sprite = chipSpriteList [rand.Next (0, chipSpriteList.Count - 1)];
	}
}
