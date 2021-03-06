using System;

public static class Extension {

//	public static string f(this Credit c, double d) {
//		string res = String.Format("{0:N2}", d);
//		return res;
//	}

	public static string f(this double d) {
		string res = String.Format("{0:N2}", d);
		return res;
	}

//	public static string to_s(this double d) {
//		string res = String.Format("{0:N2}", d * Settings.betCreditsMultiplier);
////		string res = String.Format("{0:N2}", d * Settings.betCurrentMultiplier * Settings.betCreditsMultiplier);
//		return res;
//	}

	public static string to_b(this double d) {
		string res = String.Format("{0:N2}", d * Settings.betCreditsMultiplier);
		return res;
	}

//	public static string to_s_only(this double d) {
//		string res = String.Format("{0:N2}", d); // amount.ToString("#,#", System.Globalization.CultureInfo.CurrentCulture);
//		return res;
//	}

	
	public static bool isRaise(this string name) {
		if (string.IsNullOrEmpty(name)) return false;
		return name.Contains (Settings.aRaise);
	}
	public static bool isCall(this string name) {
		if (string.IsNullOrEmpty(name)) return false;
		return name.Contains (Settings.aCall);
	}
	public static bool isCheck(this string name) {
		if (string.IsNullOrEmpty(name)) return false;
		return name.Contains (Settings.aCheck);
	}
	public static bool isFold(this string name) {
		if (string.IsNullOrEmpty(name)) return false;
		return name.Contains (Settings.aFold);
	}
	public static bool isAllIn(this string name) {
		if (string.IsNullOrEmpty(name)) return false;
		return name.Contains (Settings.aAllIn);
	}
	public static bool isUnknown(this string name) {
		if (string.IsNullOrEmpty(name)) return false;
		return name.Contains (Settings.aUnknown);
	}
}
