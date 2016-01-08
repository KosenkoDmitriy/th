using System;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// The 'Aggregate' interface
/// </summary>
public interface IAbstractStateCollection
{
	StateIterator CreateIterator();
}

/// <summary>
/// The 'ConcreteAggregate' class
/// </summary>
public class StateCollection : IAbstractStateCollection
{
	private ArrayList _items = new ArrayList();
	
	public StateIterator CreateIterator()
	{
		return new StateIterator(this);
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
interface IAbstractStateIterator
{
	BetRound First();
	BetRound Next();
	bool IsDone { get; }
	BetRound CurrentItem { get; }
}

/// <summary>
/// The 'ConcreteIterator' class
/// </summary>
public class StateIterator : IAbstractStateIterator
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
