using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FiniteTopologicalSpaceBase.Topology
{
    /// <summary>
    /// 部分集合族のクラス。TEnum全体の集合が台集合。
    /// </summary>
    public class FamilyOfSubsets<TEnum> : SortedSet<FiniteSet<TEnum>>, IFormattable,
        IComparable<FamilyOfSubsets<TEnum>>
        where TEnum : Enum
    {
        #region メンバ変数

        #endregion

        #region コンストラクタ
        public FamilyOfSubsets() : base()
        {
        }

        public FamilyOfSubsets(IEnumerable<FiniteSet<TEnum>> collection) : base(collection)
        {
        }

        public FamilyOfSubsets([NullableAttribute] IComparer<FiniteSet<TEnum>> comparer) : base(comparer)
        {
        }

        public FamilyOfSubsets(IEnumerable<FiniteSet<TEnum>> collection, [NullableAttribute] IComparer<FiniteSet<TEnum>> comparer) : base(collection, comparer)
        {
        }

        protected FamilyOfSubsets(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


        #endregion

        #region インターフェースのメソッド


        public int CompareTo([AllowNull] FamilyOfSubsets<TEnum> other)
        {
            //値参照して同じなら0
            if (this.SequenceEqual(other)) return 0;

            //要素数で比較
            if (this.Count > other.Count) return 1;
            else if (this.Count < other.Count) return -1;

            //要素数が同じ場合、若いFiniteSet<TEnum>を持っているほうが先に表示
            foreach (var set1 in this)
            {
                foreach (var set2 in other)
                {
                    //同じ要素を比較した場合、次のset1を比較
                    if (set1.SequenceEqual(set2) || set1.CompareTo(set2) == 0) break;
                    else if (set1.CompareTo(set2) == 1) return 1;
                    else if (set1.CompareTo(set2) == -1) return -1;
                }
            }

            //ここまで来ると同じの可能性があるが、そもそも同じであることは冒頭で比較したのでここまでくるとおかしい
            throw new NotImplementedException();
        }


        public string ToString(string format = "D", IFormatProvider formatProvider = null)
        {
            if (format is null)
            {
                format = "D";
            }

            string ret = "{";

            foreach (var set in this)
            {
                string setString = set.ToString(format, formatProvider);
                //メンバの名前を表示する
                Debug.WriteLine(setString);
                //末尾カンマ削除
                if (ret != "{") ret += ",";
                ret += setString;
            }


            ret += "}";
            return ret;
        }

        #endregion


        /// <summary>
        /// 集合族の中に要素である集合が含まれているかどうか
        /// </summary>
        /// <param name="item">有限集合</param>
        /// <returns>含まれている</returns>
        public override bool Contains(FiniteSet<TEnum> item)
        {

            IEqualityComparer<FiniteSet<TEnum>> comparer = new FiniteSetComparer<TEnum>();
            return Enumerable.Contains(this , item, comparer);
        }
    }
}
