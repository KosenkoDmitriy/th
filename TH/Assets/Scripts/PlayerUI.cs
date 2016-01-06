using System;
using System.Collections.Generic;
using System.Collections;

public class PlayerUI : Player {
	
}

/// <summary>
/// The 'Aggregate' interface
/// </summary>
interface IAbstractCollection
{
	PlayerIterator CreateIterator();
}

/// <summary>
/// The 'ConcreteAggregate' class
/// </summary>
public class PlayerCollection : IAbstractCollection
{
	private ArrayList _items = new ArrayList();
	
	public PlayerIterator CreateIterator()
	{
		return new PlayerIterator(this);
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
interface IAbstractPlayerIterator
{
	Player First();
	Player Next();
	bool IsDone { get; }
	Player CurrentItem { get; }
}

/// <summary>
/// The 'ConcreteIterator' class
/// </summary>
public class PlayerIterator : IAbstractPlayerIterator
{
	private PlayerCollection _collection;
	private int _current = 0;
	private int _step = 1;
	
	// Constructor
	public PlayerIterator(PlayerCollection collection)
	{
		this._collection = collection;
	}
	
	// Gets first item
	public Player First()
	{
		_current = 0;
		return _collection[_current] as Player;
	}
	
	// Gets next item
	public Player Next()
	{
		_current += _step;
		if (!IsDone)
			return _collection[_current] as Player;
		else
			return null;
	}

	public Player NextLoop() {
		if (_current >= _collection.Count)
			_current = 0;
		Player player = _collection[_current] as Player;
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
	public Player CurrentItem
	{
		get { return _collection[_current] as Player; }
	}
	
	// Gets whether iteration is complete
	public bool IsDone
	{
		get { return _current >= _collection.Count; }
	}
}
