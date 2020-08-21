using System;
using System.Collections.Generic;
using System.Text;

namespace FiniteTopologicalSpaceBase.Topology
{
    /// <summary>
    /// SortedSet<T>の拡張メソッド
    /// </summary>
    public static class SortedSetExtension
    {
        /// <summary>
        /// 集合Sから、直積S×Sとなる自己関係R＝S×Sを作成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalSet">台集合</param>
        /// <returns>自己関係R＝originalSet×originalSet</returns>
        public static EndoRelation<T> MakeSetProductSet<T>(SortedSet<T> originalSet)
        {
            if (originalSet is null)
            {
                throw new ArgumentNullException(nameof(originalSet));
            }

            var retRel = new EndoRelation<T>();
            foreach (var item1 in originalSet)
            {
                foreach (var item2 in originalSet)
                {
                    retRel.Add((item1, item2));
                }
            }
            return retRel;

        }
    }
}
