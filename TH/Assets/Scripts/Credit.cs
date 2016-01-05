using System;
using System.Collections.Generic;

public class Credit
{
	double item;

	public Credit() {

	}

	public double Get()
	{
		return this.item;
	}
	public void Set(double value)
	{
		this.item = value;
//		string dollarAmount = FormatCreditsOrDollars(potAmount);
//		lblPot.GetComponent<Text>().text = dollarAmount;
	}
	public override string ToString ()
	{
		return this.f (this.item);
	}
}
