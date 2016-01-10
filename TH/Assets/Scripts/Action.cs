using System;

public interface IAction {
	void Do();
}

public class Action : IAction {
//	public Player player;
//	public Action(Player player) {
//		this.player = player;
//	}
	#region IAction implementation

	public virtual void Do ()
	{

	}

	#endregion
}


