using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    /// <summary>
    /// 兩個Key的Dictionary
    /// </summary>
    /// <typeparam name="K1"></typeparam>
    /// <typeparam name="K2"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MultiKeyDictionary<K1, K2, V>
    {
        private Dictionary<K1, Dictionary<K2, V>> baseDictionary = new Dictionary<K1, Dictionary<K2, V>>();

        public IEnumerable<K1> Key1s { get { return baseDictionary.Keys; } }

        public IEnumerable<V> Values { get { return baseDictionary.Values.SelectMany(e => e.Values); } }

        public V this[K1 key1, K2 key2]
        {
            get
            {
                return baseDictionary[key1][key2];
            }
            set
            {
                baseDictionary[key1][key2] = value;
            }
        }
        public Dictionary<K2, V> this[K1 key1]
        {
            get
            {
                return baseDictionary[key1];
            }
            set
            {
                baseDictionary[key1] = value;
            }
        }
        public void Add(K1 key1, K2 key2, V value)
        {
            if (!baseDictionary.ContainsKey(key1))
            {
                baseDictionary.Add(key1, new Dictionary<K2, V>());
            }
            baseDictionary[key1].Add(key2, value);
        }
        public bool ContainsKey(K1 key1)
        {
            if (key1 == null)
            {
                return false;
            }
            if (!baseDictionary.ContainsKey(key1))
            {
                return false;
            }
            return true;
        }
        public bool ContainsKey(K1 key1, K2 key2)
        {
            if (key1 == null || key2 == null)
            {
                return false;
            }
            if (!baseDictionary.ContainsKey(key1))
            {
                return false;
            }
            if (!baseDictionary[key1].ContainsKey(key2))
            {
                return false;
            }
            return true;
        }
        public bool Remove(K1 key1, K2 key2)
        {
            if (!baseDictionary.ContainsKey(key1))
            {
                return false;
            }
            if (!baseDictionary[key1].Remove(key2))
            {
                return false;
            }
            if (baseDictionary[key1].Count == 0)
            {
                return baseDictionary.Remove(key1);
            }
            return true;
        }
    }
}