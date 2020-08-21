using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace FiniteTopologicalSpaceBase.Topology
{
    public class FiniteSet<TEnum> : SortedSet<TEnum>,  IFormattable, IComparable<FiniteSet<TEnum>>
        where TEnum : Enum
    {
        #region メンバ変数

        #endregion

        #region コンストラクタ

        public FiniteSet() : base()
        {
        }
        public FiniteSet(IEnumerable<TEnum> collection) : base(collection)
        {
        }

        public FiniteSet([Nullable] IComparer<TEnum> comparer) : base(comparer)
        {
        }

        public FiniteSet(IEnumerable<TEnum> collection, [NullableAttribute] IComparer<TEnum> comparer) : base(collection, comparer)
        {
        }

        protected FiniteSet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }



        #endregion

        #region インターフェースのメソッド


        public int CompareTo([AllowNull] FiniteSet<TEnum> other)
        {
            //値参照して同じなら0
            if (this.SequenceEqual(other)) return 0;

            //要素数で比較
            if (this.Count > other.Count) return 1;
            else if (this.Count < other.Count) return -1;

            //要素数が同じ場合、番号が若いenumを持っているほうが先に表示
            foreach(var ele1 in this)
            {
                foreach(var ele2 in other)
                {
                    //同じ要素を比較した場合、次のele1を比較
                    if (ele1.Equals(ele2)) break;
                    else if (ele1.CompareTo(ele2) == 1) return 1;
                    else if (ele1.CompareTo(ele2) == -1) return -1;
                }
            }

            //ここまで来ると同じの可能性があるが、そもそも同じであることは冒頭で比較したのでここまでくるとおかしい
            throw new NotImplementedException();
        }


        /// <summary>
        /// 有限集合を文字列として出力するメソッド
        /// </summary>
        /// <returns>有限集合を表す文字列</returns>
        public string ToString(string format = "D", IFormatProvider formatProvider = null)
        {
            if (format is null) format = "D";

            string ret = "{";

            switch (format)
            {
                case "N":
                case "n":
                    foreach (var element in this)
                    {
                        string numeric = (Convert.ToInt32(element)).ToString();
                        //メンバの名前を表示する
                        Debug.WriteLine(numeric);
                        //末尾カンマ削除
                        if (ret != "{") ret += ",";
                        ret += numeric;
                    }
                    break;

                case "S":
                case "s":
                    foreach (var element in this)
                    {
                        string name = Enum.GetName(typeof(TEnum), element);
                        //メンバの名前を表示する
                        Debug.WriteLine(name);
                        //末尾カンマ削除
                        if (ret != "{") ret += ",";
                        ret += name;
                    }
                    break;
                case null:
                default:
                    //enumのフォーマットとしてはG,F,D,Xが標準対応されている
                    foreach (var element in this)
                    {
                        string name = element.ToString(format);
                        //メンバの名前を表示する
                        Debug.WriteLine(name);
                        //末尾カンマ削除
                        if (ret != "{") ret += ",";
                        ret += name;
                    }
                    break;
            }


            ret += "}";
            return ret;
        }


        #endregion

        #region メソッド


        /// <summary>
        /// 予約してみたけどアップデートするもんないわ
        /// </summary>
        public void Update()
        {
        }


        #endregion
    }
}
