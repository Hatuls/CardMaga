using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
namespace ReiTools.TokenMachine
{
    [Serializable]
    public class SortedQueue<TClass> : IEnumerable
        where TClass : class, IComparable<TClass>
    {
        TClass[] _items;
        public SortedQueue()
        {

            Reset();
        }
        public TClass this[int i]
        {
            set => _items[i] = value;
            get => _items[i];
        }

        #region Public 
        /// <summary>
        /// Returns the amount of items is the queue
        /// </summary>
        public int Count { get => _items.Length; }
        /// <summary>
        /// check if the queue is empty
        /// </summary>
        public bool IsEmpty => Count == 0;
        /// <summary>
        /// Assign a new sorting method
        /// </summary>
        /// <param name="sortMethod"></param>

        /// <summary>
        /// Add Item to the queue
        /// </summary>
        /// <param name="item"></param>
        public void Add(TClass item)
        {
            int index = Count;

            Array.Resize(ref _items, index + 1);
            _items[index] = item;
            Sort();
        }

        /// <summary>
        /// Add Item to the queue
        /// </summary>
        /// <param name="item"></param>
        public void AddAt(TClass item, int indexRequired)
        {
            if (indexRequired >= Count || indexRequired < 0)
                return;


            int index = Count;

            Array.Resize(ref _items, index + 1);
            if (indexRequired != Count - 1)
            {
                for (int i = Count - 1; i > indexRequired; i--)
                {
                    _items[i] = _items[i - 1];
                }
            }
            _items[indexRequired] = item;
//Sort();
        }

        /// <summary>
        /// sort by func
        /// </summary>
        /// <param name="compareMethod"></param>
        public void SortByMethod(Func<TClass, TClass, bool> compareMethod)
        {
            int count = Count;
            TClass t1, t2;
            for (int i = 0; i < count - 1; i++)
            {
                t1 = _items[i];

                for (int j = i + 1; j < count; j++)
                {
                    t2 = _items[j];
                    if (t2.IsNull())
                        return;
                    if (compareMethod.Invoke(t1, t2))
                    {
                        _items[i] = t2;
                        _items[j] = t1;
                        break;
                    }
                }

            }
        }
        /// <summary>
        /// sort by ICompareable
        /// </summary>
        /// <param name="compareMethod"></param>
        public void SortByMethod(IComparer<TClass> compareMethod)
            => Array.Sort(_items, compareMethod);
        public TClass Peek()
        {
            if (!IsEmpty)
                return _items[0];


            if (typeof(TClass).IsValueType)
            {
                throw new Exception($"PriorityQueue of type {typeof(TClass)} is empty!\ntry to check if its empty before");
            }
            return default;

        }
        public bool Contains(TClass item)
        {
            if (!IsEmpty)
            {
                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    if (item.IsEquals(_items[i]))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Remove all the elements by a condition
        /// </summary>
        /// <param name="condition"></param>
        public void RemoveAllBy(Predicate<TClass> condition)
        {
            if (!condition.IsNull() && !IsEmpty)
            {
                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    if (condition.Invoke(_items[i]))
                        _items[i] = null;

                }
                MoveNullsToTheBack();
                ShrinkNulls();
            }
        }
        /// <summary>
        /// Remove an element from the queue
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveFromQueue(TClass item)
        {
            if (!IsEmpty)
            {
                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    if (item.IsEquals(_items[i]))
                    {
                        _items[i] = null;
                        Sort();
                        return true;
                    }
                }
            }

            return false;
        }
        public bool Pop(ref TClass outCome)
        {
            if (IsEmpty)
                return false;

            outCome = Pop();
            return true;
        }
        public TClass Pop()
        {
            TClass outCome = _items[0];
            _items[0] = null;
            Sort();
            return outCome;
        }
        public void Reset() => _items = new TClass[0];
        #endregion

        #region Private
        /// <summary>
        /// Default sort
        /// will remove the nulls and also sort it by func method if assigned
        /// </summary>

        public void Sort()
        {
            if (IsEmpty)
                return;
            int count = Count;

            MoveNullsToTheBack();
            ShrinkNulls();
            _items.Sort();
            //for (int i = 0; i < count - 1; i++)
            //{
            //    if (_items[i].CompareTo(_items[i + 1]) < 0)
            //    {
            //        SwitchPlaces(i, i + 1);
            //        i = -1;
            //    }
            //}

            void SwitchPlaces(int A, int B)
            {
                var cache = _items[B];
                _items[B] = _items[A];
                _items[A] = cache;
            }
        }

        /// <summary>
        /// Move all the nulls to the back of the array
        /// </summary>
        private void MoveNullsToTheBack()
        {
            int count = Count;
            for (int i = 0; i < count - 1; i++)
            {
                if (_items[i] != null)
                    continue;
                for (int j = count - 1; j >= i; j--)
                {
                    if (_items[j] != null)
                    {
                        _items[i] = _items[j];
                        _items[j] = null;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Resize the array to its length without nulls
        /// </summary>
        private void ShrinkNulls()
        {
            int count = Count;
            int amountOfNulls = 0;
            for (int i = count - 1; i >= 0; i--)
            {
                if (_items[i] == null) amountOfNulls++;
                else break;
            }
            if (amountOfNulls > 0)
                Array.Resize(ref _items, Math.Max(0, count - amountOfNulls));

        }

        public IEnumerator GetEnumerator()
        => _items.GetEnumerator();



        #endregion


    }


    public static class ReferenceExtentions
    {
        public static bool IsNull(this object reference) => reference?.IsEquals(null) ?? true;
        public static bool IsEquals(this object obj1, object obj2) => ReferenceEquals(obj1, obj2);

    }
}