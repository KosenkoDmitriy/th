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

namespace Iterator
{
	/// <summary>
	/// MainApp startup class for Real-World 
	/// Iterator Design Pattern.
	/// </summary>
	class MainApp
	{
		/// <summary>
		/// Entry point into console application.
		/// </summary>
		static void Main()
		{
			// Build a collection
			Collection collection = new Collection();
			collection[0] = new Item("Item 0");
			collection[1] = new Item("Item 1");
			collection[2] = new Item("Item 2");
			collection[3] = new Item("Item 3");
			collection[4] = new Item("Item 4");
			collection[5] = new Item("Item 5");
			collection[6] = new Item("Item 6");
			collection[7] = new Item("Item 7");
			collection[8] = new Item("Item 8");
			
			// Create iterator
			Iterator iterator = new Iterator(collection);
			
			// Skip every other item
			iterator.Step = 2;
			
			Console.WriteLine("Iterating over collection:");
			
			for (Item item = iterator.First();
			     !iterator.IsDone; item = iterator.Next())
			{
//				Console.WriteLine(item.Name);
			}
			
			// Wait for user
//			Console.ReadKey();
		}
	}
	
	/// <summary>
	/// A collection item
	/// </summary>
	class Item
	{
		private string _name;
		
		// Constructor
		public Item(string name)
		{
			this._name = name;
		}
		
		// Gets name
		public string Name
		{
			get { return _name; }
		}
	}
	
	/// <summary>
	/// The 'Aggregate' interface
	/// </summary>
	interface IAbstractCollection
	{
		Iterator CreateIterator();
	}
	
	/// <summary>
	/// The 'ConcreteAggregate' class
	/// </summary>
	class Collection : IAbstractCollection
	{
		private ArrayList _items = new ArrayList();
		
		public Iterator CreateIterator()
		{
			return new Iterator(this);
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
	interface IAbstractIterator
	{
		Item First();
		Item Next();
		bool IsDone { get; }
		Item CurrentItem { get; }
	}
	
	/// <summary>
	/// The 'ConcreteIterator' class
	/// </summary>
	class Iterator : IAbstractIterator
	{
		private Collection _collection;
		private int _current = 0;
		private int _step = 1;
		
		// Constructor
		public Iterator(Collection collection)
		{
			this._collection = collection;
		}
		
		// Gets first item
		public Item First()
		{
			_current = 0;
			return _collection[_current] as Item;
		}
		
		// Gets next item
		public Item Next()
		{
			_current += _step;
			if (!IsDone)
				return _collection[_current] as Item;
			else
				return null;
		}
		
		// Gets or sets stepsize
		public int Step
		{
			get { return _step; }
			set { _step = value; }
		}
		
		// Gets current iterator item
		public Item CurrentItem
		{
			get { return _collection[_current] as Item; }
		}
		
		// Gets whether iteration is complete
		public bool IsDone
		{
			get { return _current >= _collection.Count; }
		}
	}
}
