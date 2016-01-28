using System;
using System.Collections.Generic;

public class Bet
{
	public Bet(double bet) {
		this.inCredits = bet;
	}

	public override string ToString ()
	{
//		return string.Format ("[Bet: inCredits={0}, inBetMath={1}, inCreditsNoM={2}]", inCredits, inBetMath, inCreditsNoM);
		return string.Format ("[Bet: {0} credits | {1} math]", this.inCredits, this.inBetMath);
	}

	private void reset() {
		this._inCreditsNoM = 0;
		this._inBetMath = 0;
		this._inCredits = 0;
	}

	private double _inCredits;
	public double inCredits {
		get {
			return this._inCredits;
		}
		set {
			reset ();
			this._inCredits = value;//(double)value;
			if (this._inCredits != 0) {
				this._inCreditsNoM = this._inCredits / Settings.betCreditsMultiplier;
				this._inBetMath = this._inCreditsNoM / Settings.betCurrentMultiplier;
			}
		}
	}
	
	private double _inBetMath;
	public double inBetMath {
		get {
			return _inBetMath;
		}
		set {
			reset ();
			this._inBetMath = value;//(double)value;
			if (this._inBetMath != 0) {
				this._inCreditsNoM = this._inBetMath * Settings.betCurrentMultiplier;
				this._inCredits = this._inCreditsNoM * Settings.betCreditsMultiplier;
			}
		}
	}
	
	private double _inCreditsNoM;
	public double inCreditsNoM {
		get {
			return _inCreditsNoM;
		}
		set {
			reset ();
			this._inCreditsNoM = value;//(double)value;
			if (this._inCreditsNoM != 0) {
				this._inBetMath = this._inCreditsNoM / Settings.betCurrentMultiplier;
				this._inCredits = this._inCreditsNoM * Settings.betCreditsMultiplier;
			}
		}
	}

	public static Bet operator +(Bet a, Bet b) {
		var c = a.inCredits + b.inCredits;
		return new Bet(c);
	}

	public static Bet operator -(Bet a, Bet b) {
		var c = a.inCredits - b.inCredits;
		return new Bet(c);
	}

	public static Bet operator *(Bet a, Bet b) {
		var c = a.inCredits * b.inCredits;
		return new Bet(c);
	}

	public static Bet operator /(Bet a, Bet b) {
		var c = a.inCredits / b.inCredits;
		return new Bet(c);
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
		var c = a.inCredits + b;
		return new Bet(c);
	}
	
	public static Bet operator -(Bet a, double b) {
		var c = a.inCredits - b;
		return new Bet(c);
	}
	
	public static Bet operator *(Bet a, double b) {
		var c = a.inCredits * b;
		return new Bet(c);
	}
	
	public static Bet operator /(Bet a, double b) {
		var c = a.inCredits / b;
		return new Bet(c);
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
