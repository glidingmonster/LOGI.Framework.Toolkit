﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using LOGI.Framework.Toolkit.Core.Reflection;
using LOGI.Framework.Toolkit.Core.Threading;
using System.Runtime.Serialization;

namespace LOGI.Framework.Toolkit.Core.Common.Collections
{
    [Serializable]
    [DataContract]
    public class BusinessObjectCollection<T> : IList<T>// where T : BusinessObjectBase
    {
        #region "Member Variables"
        [DataMember]
        protected List<T> _innerArray;  //inner ArrayList object

        [DataMember]
        protected bool _IsReadOnly;       //flag for setting collection to read-only mode (not used in this example)

        #endregion

        #region "Constructors"

        /// <summary>
        /// Default constructor
        /// </summary>
        public BusinessObjectCollection()
        {
            _innerArray =new List<T>();
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(T item)
        {
            return _innerArray.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, T item)
        {
            _innerArray.Insert(index, item);
            if (ItemArrayChanged != null)
            {
                ItemArrayChanged.Invoke();
            }
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            var itemObj = _innerArray[index];
            this.Remove((type) => type.Equals(itemObj));
        }

        /// <summary>
        /// Default accessor for the collection 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual T this[int index]
        {
            get
            {
                return _innerArray[index];
            }
            set
            {
                _innerArray[index] = value;
            }
        }

        /// <summary>
        /// Number of elements in the collection
        /// </summary>
        public virtual int Count
        {
            get
            {
                return _innerArray.Count;
            }
        }

        /// <summary>
        /// Flag sets whether or not this collection is read-only
        /// </summary>
        public virtual bool IsReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Add a business object to the collection
        /// </summary>
        /// <param name="BusinessObject"></param>
        public virtual void Add(T BusinessObject)
        {
            _innerArray.Add(BusinessObject);
            if (ItemArrayChanged != null)
            {
                ItemArrayChanged.Invoke();
            }

        }

        /// <summary>
        /// Add a business object to the collection
        /// </summary>
        /// <param name="BusinessObjects"></param>
        public virtual void AddRange(IList<T> BusinessObjects)
        {
            var result = false;
            foreach (var businessObject in BusinessObjects)
            {
                _innerArray.Add(businessObject);
                result = true;
            }

            if (result)
            {
                if (ItemArrayChanged != null)
                {
                    ItemArrayChanged.Invoke();
                }
            }
        }

        public bool Contains(T item)
        {
            return _innerArray.Contains(item);
        }

        public bool Remove(T item)
        {
            var result = _innerArray.Remove(item);

            if (result)
            {
                if (ItemArrayChanged != null)
                {
                    ItemArrayChanged.Invoke();
                }
            }
            return result;
        }

        /// <summary>
        /// Remove first instance of a business object from the collection
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        public virtual bool Merge(T item,Func<T, bool> predicate)
        {
            bool result = false;

            //loop through the inner array's indices
            for (int i = 0; i < _innerArray.Count; i++)
            {
                //store current index being checked
                T obj = _innerArray[i];

                //compare the BusinessObjectBase UniqueId property
                if (predicate(obj))
                {
                    //remove item from inner ArrayList at index i
                    _innerArray[i] = item;
                    result = true;
                    break;
                }
            }

            if (!result)
            {
                _innerArray.Add(item);
            }

            if (ItemArrayChanged != null)
            {
                ItemArrayChanged.Invoke();
            }

            return result;

        }

        /// <summary>
        /// Remove first instance of a business object from the collection
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        public virtual bool Remove(Func<T, bool> predicate)
        {
            bool result = false;

            //loop through the inner array's indices
            for (int i = 0; i < _innerArray.Count; i++)
            {
                //store current index being checked
                T obj = _innerArray[i];

                //compare the BusinessObjectBase UniqueId property
                if (predicate(obj))
                {
                    //remove item from inner ArrayList at index i
                    _innerArray.RemoveAt(i);

                    result = true;
                    break;
                }
            }

            if (result)
            {
                if (ItemArrayChanged != null)
                {
                    ItemArrayChanged.Invoke();
                }
            }

            return result;

        }

        /// <summary>
        /// Returns true/false based on whether or not it finds the requested object in the collection.
        /// </summary>
        /// <param name="BusinessObject"></param>
        /// <returns></returns>
        public bool Contains(Func<T,bool> predicate)
        {
            //loop through the inner ArrayList
            foreach (T obj in _innerArray)
            {
                //compare the BusinessObjectBase UniqueId property
                if (predicate(obj))
                {
                    //if it matches return true
                    return true;
                }
            }

            //no match
            return false;
        }

        /// <summary>
        /// Copy objects from this collection into another array
        /// Not implemented
        /// </summary>
        /// <param name="BusinessObjectArray"></param>
        /// <param name="index"></param>
        public virtual void CopyTo(T[] BusinessObjectArray, int index)
        {
            _innerArray.CopyTo(BusinessObjectArray, index);
        }

        /// <summary>
        /// Clear the collection of all it's elements
        /// </summary>
        public virtual T[] ToArray()
        {
            var result = _innerArray.ToArray();
            return result;
        }
        
        /// <summary>
        /// Clear the collection of all it's elements
        /// </summary>
        public virtual void Clear()
        {
            _innerArray.Clear();
            if (ItemArrayChanged != null)
            {
                ItemArrayChanged.Invoke();
            }
        }

        /// <summary>
        /// Returns custom generic enumerator for this BusinessObjectCollection
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            //return a custom enumerator object instantiated to use this BusinessObjectCollection 
            return new BusinessObjectEnumerator<T>(this);
        }

        /// <summary>
        /// Explicit non-generic interface implementation for IEnumerable extended and required by ICollection (implemented by ICollection<T>)
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BusinessObjectEnumerator<T>(this);
        }

        public event Action ItemArrayChanged;


        public T GetNewItem(params object[] parameters)
        {
            var returnVal = InstanceCreator.CreateInstance<T>(parameters);
            return returnVal;
        }

        public IList<T> GetNewInstance(params object[] parameters)
        {
            Type type = this.GetType();
            var returnVal = Activator.CreateInstance(type, parameters) as IList<T>;
            return returnVal;
        }

        public void SetReadOnly(bool isReadOnly)
        {
            _IsReadOnly = isReadOnly;
        }
        #endregion
    }
}
