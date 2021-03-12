using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routing
{
    public class Reporter : IEnumerable<KeyValuePair<int, List<int>>>
    {
        private readonly Dictionary<int, List<int>> untraced; // Key: номер цепи, Value: список цепей, которые не удалось соединить
        public Reporter()
        {
            untraced = new Dictionary<int, List<int>>();
        }

        public void AddNode(int traceId, int nodeId)
        {
            if (!untraced.ContainsKey(traceId))
                untraced.Add(traceId, new List<int>());
            untraced[traceId].Add(nodeId);
        }
        public void Clear()
        {
            untraced.Clear();
        }


        IEnumerator<KeyValuePair<int, List<int>>> IEnumerable<KeyValuePair<int, List<int>>>.GetEnumerator()
        {
            foreach (var pair in untraced)
                yield return pair;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var pair in untraced)
                yield return pair;
        }

        IEnumerable<int> GetTraceIds()
        {
            foreach (int k in untraced.Keys)
                yield return k;
        }

        List<int> GetNodeIds(int traceId)
        {
            if (untraced.ContainsKey(traceId))
                return untraced[traceId];
            return null;
        }
    }
}

