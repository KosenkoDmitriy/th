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
		inCredits = bet;
	}

	private void reset(double bet) {
		if (bet == 0) {
			_inCreditsNoM = 0;
			_inBetMath = 0;
			_inCredits = 0;
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
				_inCreditsNoM = _inCredits / Settings.betCreditsMultiplier;
				_inBetMath = _inCreditsNoM / Settings.betCurrentMultiplier;
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
				_inCreditsNoM = _inBetMath * Settings.betCurrentMultiplier;
				_inCredits = _inCreditsNoM * Settings.betCreditsMultiplier;
			}
		}
	}
	
	private double _inCreditsNoM;
	public double inCreditsNoM {
		get {
			return _inCreditsNoM;
		}
		set {
			reset (value);
			_inCreditsNoM = value;
			if (_inCreditsNoM != 0) {
				_inBetMath = _inCreditsNoM / Settings.betCurrentMultiplier;
				_inCredits = _inCreditsNoM * Settings.betCurrentMultiplier * Settings.betCreditsMultiplier;
			}
		}
	}

	public static Bet operator +(Bet a, Bet b) {
		a.inCredits += b.inCredits;
		return a;
	}

	public static Bet operator -(Bet a, Bet b) {
		a.inCredits -= b.inCredits;
		return a;
	}

	public static Bet operator *(Bet a, Bet b) {
		a.inCredits *= b.inCredits;
		return a;
	}

	public static Bet operator /(Bet a, Bet b) {
		a.inCredits /= b.inCredits;
		return a;
	}

	public static bool operator <=(Bet a, Bet b) {
		return a.inCredits <= b.inCredits;
	}

	public static bool operator >=(Bet a, Bet b) {
		return a.inCredits >= b.inCredits;
	}

	public static bool operator <(Bet a, Bet b) {
		return a.inCredits < b.inCredits;
	}
	
	public static bool operator >(Bet a, Bet b) {
		return a.inCredits > b.inCredits;
	}

	public static bool operator ==(Bet a, Bet b) {
		return a.inCredits == b.inCredits;
	}

	public static bool operator !=(Bet a, Bet b) {
		return a.inCredits != b.inCredits;
	}

	// bet and double
	public static Bet operator +(Bet a, double b) {
		a.inCredits += b;
		return a;
	}
	
	public static Bet operator -(Bet a, double b) {
		a.inCredits -= b;
		return a;
	}
	
	public static Bet operator *(Bet a, double b) {
		a.inCredits *= b;
		return a;
	}
	
	public static Bet operator /(Bet a, double b) {
		a.inCredits /= b;
		return a;
	}


	public static bool operator <=(Bet a, double b) {
		return a.inCredits <= b;
	}
	
	public static bool operator >=(Bet a, double b) {
		return a.inCredits >= b;
	}
	
	public static bool operator <(Bet a, double b) {
		return a.inCredits < b;
	}
	
	public static bool operator >(Bet a, double b) {
		return a.inCredits > b;
	}
	
	public static bool operator ==(Bet a, double b) {
		return a.inCredits == b;
	}
	
	public static bool operator !=(Bet a, double b) {
		return a.inCredits != b;
	}

}
