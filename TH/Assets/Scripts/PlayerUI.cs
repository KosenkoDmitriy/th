using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : Player {

	public PlayerUI() {

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
	public Image chip;
	public Image dealer;
	public Text lblCredits;
	public Text lblAction;
	public Text lblName;
}
