using System;
using System.Collections.Generic;

//public class Credit
//{
//	double item;
//
//	public Credit() {
//
//	}
//
//	public double Get()
//	{
//		return this.item;
//	}
//	public void Set(double value)
//	{
//		this.item = value;
////		string dollarAmount = FormatCreditsOrDollars(potAmount);
////		lblPot.GetComponent<Text>().text = dollarAmount;
//	}
//	public override string ToString ()
//	{
//		return this.f (this.item);
//	}
//}

public class Bet
{
	public Bet(double bet) {
		reset (bet);
		inBet = bet;
	}
	private void reset(double bet) {
		if (bet == 0) {
			_inBet = 0;
			_inBetMath = 0;
			_inCredits = 0;
		}
	}
	private double _inBet;
	public double inBet {
		get {
			return _inBet;
		}
		set {
			reset (value);
			_inBet = value;
			if (_inBet != 0) {
				_inBetMath = _inBet / Settings.betCurrentMultiplier;
				_inCredits = _inBet * Settings.betCurrentMultiplier * Settings.betCreditsMultiplier;
			}
		}
	}
	
	private double _inBetMath;
	public double inBetMath {
		get {
			return _inBetMath;
		}
		set {
			reset (value);
			_inBetMath = value;
			if (_inBetMath != 0) {
				_inBet = _inBetMath * Settings.betCurrentMultiplier;
				_inCredits = _inBet * Settings.betCreditsMultiplier;
			}
		}
	}
	
	private double _inCredits;
	public double inCredits {
		get {
			return _inCredits;
		}
		set {
			reset (value);
			_inCredits = value;
			if (_inCredits != 0) {
				_inBet = _inCredits / Settings.betCreditsMultiplier;
				_inBetMath = _inBet / Settings.betCurrentMultiplier;
			}
		}
	}

	public static Bet operator +(Bet a, Bet b) {
		a.inBet += b.inBet;
		return a;
	}

	public static Bet operator -(Bet a, Bet b) {
		a.inBet -= b.inBet;
		return a;
	}

	public static Bet operator *(Bet a, Bet b) {
		a.inBet *= b.inBet;
		return a;
	}

	public static Bet operator /(Bet a, Bet b) {
		a.inBet /= b.inBet;
		return a;
	}

	public static bool operator <=(Bet a, Bet b) {
		return a.inBet <= b.inBet;
	}

	public static bool operator >=(Bet a, Bet b) {
		return a.inBet >= b.inBet;
	}

	public static bool operator <(Bet a, Bet b) {
		return a.inBet < b.inBet;
	}
	
	public static bool operator >(Bet a, Bet b) {
		return a.inBet > b.inBet;
	}

	public static bool operator ==(Bet a, Bet b) {
		return a.inBet == b.inBet;
	}

	public static bool operator !=(Bet a, Bet b) {
		return a.inBet != b.inBet;
	}

	// bet and double
	public static Bet operator +(Bet a, double b) {
		a.inBet += b;
		return a;
	}
	
	public static Bet operator -(Bet a, double b) {
		a.inBet -= b;
		return a;
	}
	
	public static Bet operator *(Bet a, double b) {
		a.inBet *= b;
		return a;
	}
	
	public static Bet operator /(Bet a, double b) {
		a.inBet /= b;
		return a;
	}


	public static bool operator <=(Bet a, double b) {
		return a.inBet <= b;
	}
	
	public static bool operator >=(Bet a, double b) {
		return a.inBet >= b;
	}
	
	public static bool operator <(Bet a, double b) {
		return a.inBet < b;
	}
	
	public static bool operator >(Bet a, double b) {
		return a.inBet > b;
	}
	
	public static bool operator ==(Bet a, double b) {
		return a.inBet == b;
	}
	
	public static bool operator !=(Bet a, double b) {
		return a.inBet != b;
	}

}
