using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;


public class States {

	public States(Game game) {
		isDone = false;
		state = new InitGame (game);

		rounds = new List<BetRound> () {
			new AnteRound(game),
			new PreflopRound(game),
			new FlopRound(game),
			new TurnRound(game),
			new RiverRound(game),
			new EndGame(game), // win panel (when close it > InitGame()
		};

		enumerator = rounds.GetEnumerator ();

//		foreach (var round in rounds) {
//			var item = round;
//			var str = item.ToString();
////			round.SubRound();
//		}

//		using (var round = rounds.GetEnumerator())
//		{
//			while (round.MoveNext())
//			{
//				// Do something with round.Current.
////				round.Current.SubRound();
//				var item = round.Current;
//				var str = item.ToString();
//			}
//		}

	}

	
	public void Next() {
		if (enumerator.MoveNext ()) {
			state = enumerator.Current;
			//			state.SubRound ();
			isDone = false;
		} else {
			isDone = true;
		}
	}

	IEnumerable<BetRound> rounds;
	private IEnumerator<BetRound> enumerator;
	public BetRound state;
	public bool isDone;
}
