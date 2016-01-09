using System;
using System.Collections.Generic;
using System.Collections;

/*
class RowBase
{
	//some properties and abstract methods
}
//And I have two specific row classes that are derived from the row base class

class SpecificRow1 : RowBase
{
	//some extra properties and overrides
}

class SpecificRow2 : RowBase
{
	//some extra properties and overrides
}
//Then I have a second base class that is a generic class which contains a collection of derived classes from RowBase


interface ISomeRow<out T> where T : RowBase
{
	
}

class SomeBase<T> : ISomeRow<T> where T : RowBase
//class SomeBase<T> where T : RowBase
{
	ICollection<T> Collection { get; set; }
	//some other properties and abstract methods
}
//Then I have two classes that derive from SomeBase but are using different specific row class


class SomeClass1 : SomeBase<SpecificRow1>
{
	//some properties and overrides
}

class SomeClass2 : SomeBase<SpecificRow2>
{
	//some properties and overrides
}



/// <summary>
/// I some row.
/// </summary>


//List<ISomeRow<RowBase>> myList = new List<ISomeRow<RowBase>>();
//myList.Add(new SomeClass1());
//myList.Add(new SomeClass2());

*/

/*
/// <summary>
/// The 'Aggregate' interface
/// </summary>
public interface IAbstractStateCollection<T>
{
	StateIterator<T> CreateIterator();
}

/// <summary>
/// The 'ConcreteAggregate' class
/// </summary>
public class StateCollection<T> : IAbstractStateCollection<T> where T: BetRound
{
	private ArrayList _items = new ArrayList();
	
	public StateIterator<T> CreateIterator()
	{
		return new StateIterator<T>(this);
	}
	
	// Gets item count
	public int Count
	{
		get { return _items.Count; }
	}
	
	// Indexer
	public object this[int index]
	{
		get { return _items[index]; }
		set { _items.Add(value); }
	}
}

/// <summary>
/// The 'Iterator' interface
/// </summary>
interface IAbstractStateIterator<out T> where T : BetRound
{
	T First();
	T Next();
	bool IsDone { get; }
	T CurrentItem { get; }
}

/// <summary>
/// The 'ConcreteIterator' class
/// </summary>
public class StateIterator<T> : IAbstractStateIterator<T> where T : BetRound
{
	private StateCollection _collection;
	private int _current = 0;
	private int _step = 1;
	private int _foldedCount;

	// Constructor
	public StateIterator(StateCollection collection)
	{
		this._collection = collection;
	}
	
	// Gets first item
	public BetRound First()
	{
		_current = 0;
		return _collection[_current] as BetRound;
	}
	
	// Gets next item
	public BetRound Next()
	{
		_current += _step;
		if (!IsDone)
			return _collection[_current] as BetRound;
		else
			return null;
	}
	
	public BetRound PrevLoop() {
		int prevIndex = _current - _step;
		if (prevIndex < 0) {
			prevIndex = _collection.Count - _step;
		}
		BetRound player = _collection[prevIndex] as BetRound;
		return player;
	}

	public BetRound NextLoop() {
		if (_current >= _collection.Count)
			_current = 0;
		BetRound player = _collection[_current] as BetRound;
		_current += _step;
		return player;
	}

	// Gets or sets stepsize
	public int Step
	{
		get { return _step; }
		set { _step = value; }
	}
	
	// Gets current iterator item
	public BetRound CurrentItem
	{
		get { return _collection[_current] as BetRound; }
	}
	
	// Gets whether iteration is complete
	public bool IsDone
	{
		get { return _current >= _collection.Count; }
	}
}
*/