﻿


// Authors:
//   Michael Eddington (mike@dejavusecurity.com)

// $Id$

using System;
using System.Collections.Generic;
using System.Linq;

namespace Peach.Core.Dom
{
	public delegate void AddEventHandler<TKey, TValue>(OrderedDictionary<TKey, TValue> sender, TKey key, TValue value);

	/// <summary>
	/// Represents a generic collection of key/value pairs that are ordered independently of the key and value.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
	public interface IOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ITryGetValue<TKey, TValue>
	{
		/// <summary>
		/// Inserts a new entry into the IOrderedDictionary&lt;TKey,TValue&gt; collection with the specified key and value at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the element should be inserted.</param>
		/// <param name="key">The key of the entry to add.</param>
		/// <param name="value">The value of the entry to add. The value can be null if the type of the values in the dictionary is a reference type.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.<br/>
		/// -or-<br/>
		/// <paramref name="index"/> is greater than Count.</exception>
		/// <exception cref="ArgumentException">An element with the same key already exists in the IOrderedDictionary&lt;TKey,TValue&gt;.</exception>
		/// <exception cref="NotSupportedException">The IOrderedDictionary&lt;TKey,TValue&gt; is read-only.<br/>
		/// -or-<br/>
		/// The IOrderedDictionary&lt;TKey,TValue&gt; has a fized size.</exception>
		void Insert(int index, TKey key, TValue value);

		/// <summary>
		/// Gets or sets the value at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the value to get or set.</param>
		/// <value>The value of the item at the specified index.</value>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.<br/>
		/// -or-<br/>
		/// <paramref name="index"/> is equal to or greater than Count.</exception>
		TValue this[int index]
		{
			get;
			set;
		}
		
		/// <summary>
		/// Returns the zero-based index of the specified key in the OrderedDictionary&lt;TKey,TValue&gt;
		/// </summary>
		/// <param name="key">The key to locate in the OrderedDictionary&lt;TKey,TValue&gt;</param>
		/// <returns>The zero-based index of <paramref name="key"/>, if <paramref name="key"/> is found in the OrderedDictionary&lt;TKey,TValue&gt;; otherwise, -1</returns>
		/// <remarks>This method performs a linear search; therefore it has a cost of O(n) at worst.</remarks>
		int IndexOfKey(TKey key);
	}

	/// <summary>
	/// Represents a generic collection of key/value pairs that are ordered independently of the key and value.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
	[Serializable]
	public class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
	{
		private const int DefaultInitialCapacity = 0;

		//private static readonly string _keyTypeName = typeof(TKey).FullName;
		//private static readonly string _valueTypeName = typeof(TValue).FullName;
		//private static readonly bool _valueTypeIsReferenceType = !typeof(ValueType).IsAssignableFrom(typeof(TValue));

		private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
		private List<KeyValuePair<TKey, TValue>> _list = new List<KeyValuePair<TKey,TValue>>();
		private IEqualityComparer<TKey> _comparer = null;

		public event AddEventHandler<TKey, TValue> AddEvent;
		protected void OnAdd(TKey key, TValue value)
		{
			if (AddEvent != null)
				AddEvent(this, key, value);
		}

		/// <summary>
		/// Initializes a new instance of the OrderedDictionary&lt;TKey,TValue&gt; class.
		/// </summary>
		public OrderedDictionary()
		{
		}

		#region IOrderedDictionary<TKey,TValue> Members
		/// <summary>
		/// Returns the zero-based index of the specified key in the OrderedDictionary&lt;TKey,TValue&gt;
		/// </summary>
		/// <param name="key">The key to locate in the OrderedDictionary&lt;TKey,TValue&gt;</param>
		/// <returns>The zero-based index of <paramref name="key"/>, if <paramref name="key"/> is found in the OrderedDictionary&lt;TKey,TValue&gt;; otherwise, -1</returns>
		/// <remarks>This method performs a linear search; therefore it has a cost of O(n) at worst.</remarks>
		public int IndexOfKey(TKey key)
		{
			if (null == key)
				throw new ArgumentNullException("key");

			for (int index = 0; index < _list.Count; index++)
			{
				KeyValuePair<TKey, TValue> entry = _list[index];
				TKey next = entry.Key;
				if (null != _comparer)
				{
					if (_comparer.Equals(next, key))
					{
						return index;
					}
				}
				else if (next.Equals(key))
				{
					return index;
				}
			}

			return -1;
		}

		public void Insert(int index, TKey key, TValue value)
		{
			if (index > Count || index < 0)
				throw new ArgumentOutOfRangeException("index");

			_dictionary.Add(key, value);
			_list.Insert(index, new KeyValuePair<TKey, TValue>(key, value));

			OnAdd(key, value);
		}

		public TValue this[int index]
		{
			get
			{
				return _list[index].Value;
			}

			set
			{
				if (index >= Count || index < 0)
					throw new ArgumentOutOfRangeException("index", "'index' must be non-negative and less than the size of the collection");

				TKey key = _list[index].Key;

				_list[index] = new KeyValuePair<TKey, TValue>(key, value);
				_dictionary[key] = value;

				OnAdd(key, value);
			}
		}

		#endregion

		#region IDictionary<TKey,TValue> Members

		public void Add(TKey key, TValue value)
		{
			_dictionary.Add(key, value);
			_list.Add(new KeyValuePair<TKey, TValue>(key, value));
			OnAdd(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public ICollection<TKey> Keys
		{
			get
			{
				return _list.Select(kv => kv.Key).ToList();
			}
		}

		public bool Remove(TKey key)
		{
			if (null == key)
				throw new ArgumentNullException("key");

			int index = IndexOfKey(key);
			if (index >= 0)
			{
				if (_dictionary.Remove(key))
				{
					_list.RemoveAt(index);
					return true;
				}
			}
			return false;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public ICollection<TValue> Values
		{
			get
			{
				return _list.Select(kv => kv.Value).ToList();
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary[key];
			}
			set
			{
				if (_dictionary.ContainsKey(key))
				{
					_dictionary[key] = value;
					_list[IndexOfKey(key)] = new KeyValuePair<TKey, TValue>(key, value);
					OnAdd(key, value);
				}
				else
				{
					Add(key, value);
				}
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey,TValue>> Members

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
			_list.Clear();
			_dictionary.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			TValue value;
			if (!TryGetValue(item.Key, out value))
				return false;

			return value.Equals(item.Value);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return this._list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return Contains(item) && Remove(item.Key);
		}

		#endregion

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}

}

// end
