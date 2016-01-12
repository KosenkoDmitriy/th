using System;

public static class Extension {
	public static string f(this Credit c, double d) {
		string res = String.Format("{0:N2}", d);
		return res;
	}

	public static string to_s(this double d) {
		string res = String.Format("{0:N2}", d * Settings.betCurrentMultiplier * Settings.betCreditsMultiplier);
		return res;
	}

	public static string to_credits_only(this double d) {
		string res = String.Format("{0:N2}", d * Settings.betCreditsMultiplier);
		return res;
	}

	public static string to_s_only(this double d) {
		string res = String.Format("{0:N2}", d);
		return res;
	}
//	public string FormatCreditsOrDollars(double amount) {
//		string creditAmount = String.Format("{0:N2}", amount);// amount.ToString("#,#", System.Globalization.CultureInfo.CurrentCulture);
//		return creditAmount;
//	}

}
