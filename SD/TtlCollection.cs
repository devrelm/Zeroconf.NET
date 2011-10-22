using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Network.ZeroConf
{
    public class TtlCollection<T> : ICollection<T>
        where T : IExpirable
    {
        SortedList<DateTime, IList<T>> expirables = new SortedList<DateTime, IList<T>>();

        public event Action<T> ItemExpired;

        Timer timerBeforeNextExpiration;

        public TtlCollection()
        {
            timerBeforeNextExpiration = new Timer(Tick);
        }

        private void Tick(object state)
        {
            List<T> removedItems = EnsureUpToDate();
            if (removedItems.Count > 0 && ItemExpired != null)
            {
                foreach (T item in removedItems)
                    ItemExpired(item);
            }
        }

        #region ICollection<T> Members

        public void Add(T item)
        {
            DateTime expiration = DateTime.Now.AddSeconds(item.Ttl);
            if (expirables.ContainsKey(expiration))
                expirables[expiration].Add(item);
            else
                expirables.Add(expiration, new List<T>() { item });
            if (NextExpiration > expiration)
                NextExpiration = expiration;
        }

        public void Clear()
        {
            expirables.Clear();
        }

        public bool Contains(T item)
        {
            DateTime expiration = DateTime.Now.AddSeconds(item.Ttl);
            if (item.Ttl > 0)
                return expirables[expiration].Contains(item);
            expirables.Remove(expiration);
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {

                return (from expiration in expirables
                        from expirable in expiration.Value
                        select expirable).Count();
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            DateTime expiration = DateTime.Now.AddSeconds(item.Ttl);
            if (expirables.ContainsKey(expiration))
            {
                if (item.Ttl > 0)
                    return expirables[expiration].Remove(item);
                else
                    expirables.Remove(expiration);
            }
            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            foreach (IList<T> expirables in this.expirables.Values)
            {
                foreach (T expirable in expirables)
                    yield return expirable;
            }
        }

        private List<T> EnsureUpToDate()
        {
            List<T> removedItems = new List<T>();
            while (expirables.First().Key < DateTime.Now)
            {
                removedItems.AddRange(expirables.First().Value);
                expirables.Remove(expirables.First().Key);
            }
            foreach (T item in removedItems)
            {
                if (!item.IsOutDated)
                    Add(item);
            }
            return removedItems;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        private DateTime nextExpiration;

        public DateTime NextExpiration
        {
            get { return nextExpiration; }
            set
            {
                nextExpiration = value;
                timerBeforeNextExpiration.Change((DateTime.Now - nextExpiration), new TimeSpan(0, 0, 0, 0, -1));
            }
        }

    }
}
