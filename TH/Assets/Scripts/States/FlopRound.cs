using System;

public class FlopRound : BetRound {
	public FlopRound(Game game) {
		this.game = game;
		this.subRoundMaxSize = Settings.betSubRoundMaxSize;

		// flop bet rounds
		var items = game.source.GetFlops ();
		foreach (var player in game.players)
		foreach (var item in items) {
			if (item.position == player.position) {
				if (item.winPercentMin >= player.winPercent && player.winPercent <= item.winPercentMax) {
					
					player.pattern = item.pattern;
					player.alt_patterns = item.alt_patterns;
					
					break;
				} else {
					player.pattern = game.source.GetPatternByName(Settings.defaultPreflopPattern);
				}
			}
		}

		// using in update() of the game loop
		game.playerCollection = new PlayerCollection ();
		foreach (var p in game.players) {
			game.playerCollection[p.position] = p;
		}
		
		game.playerIterator = new PlayerIterator(game.playerCollection);
	}

	public override void FirstAction() {
		game.cards [0].FaceUp = true;
		game.cards [1].FaceUp = true;
		game.cards [2].FaceUp = true;
	}

	public override void LastAction ()
	{
		//		base.LastAction ();
		game.state = new TurnRound (game);
	}

	public override void BetSubRounds ()
	{
		base.BetSubRounds ();
	}
}
