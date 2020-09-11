using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Runtime.CompilerServices;

namespace FiniteTopologicalSpaceBase.Topology
{
    /// <summary>
    /// 自己関係（始域と終域の一致する二項関係）を表すクラス。
    /// 正確には自己関係におけるグラフGのことであり、対象Tの範囲はSortedSet<T> Universeで指定する。
    /// </summary>
    /// <typeparam name="T">対象の型。enum,FiniteSet<TEnum>,FamilyOfSubsets<TEnum>が入ることを想定</typeparam>
    public class EndoRelation<T> : SortedSet<ValueTuple<T, T>>, IFormattable
    {

        #region メンバ変数
        /// <summary>
        /// 対象Tの範囲
        /// </summary>
        public SortedSet<T> Universe 
        { 
            get;
            private set;
        }

        #endregion
        #region コンストラクタ
        public EndoRelation() : base()
        {
            this.Update();
        }

        public EndoRelation(IComparer<ValueTuple<T, T>> comparer) : base(comparer)
        {
            this.Update();
        }

        public EndoRelation(IEnumerable<ValueTuple<T, T>> collection) : base(collection)
        {
            this.Update();
        }

        public EndoRelation(IEnumerable<ValueTuple<T, T>> collection, IComparer<ValueTuple<T, T>> comparer) : base(collection, comparer)
        {
            this.Update();
        }

        public EndoRelation(SortedSet<T> inputUniverse) : this()
        {
            this.SetUniverse(inputUniverse);
        }

        public EndoRelation(IComparer<ValueTuple<T, T>> comparer, SortedSet<T> inputUniverse) : this(comparer)
        {
            this.SetUniverse(inputUniverse);
        }

        public EndoRelation(IEnumerable<ValueTuple<T, T>> collection, SortedSet<T> inputUniverse) : this(collection)
        {
            this.SetUniverse(inputUniverse);
        }

        public EndoRelation(IEnumerable<ValueTuple<T, T>> collection, IComparer<ValueTuple<T, T>> comparer, SortedSet<T> inputUniverse) : this(collection, comparer)
        {
            this.SetUniverse(inputUniverse);
        }

        protected EndoRelation(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


        #endregion

        #region インターフェースのメソッド
        public string ToString(string format = "D", IFormatProvider formatProvider = null)
        {
            string ret = "{";

            foreach (ValueTuple<T, T> pair in this)
            {
                (T item1, T item2) = pair;
                string setString1;
                string setString2;
                string setStringAll;
                Type type = typeof(T);
                Type[] interfaces = type.GetInterfaces();
                MethodInfo method = type.GetMethod("ToString", new Type[] { typeof(string), typeof(IFormatProvider) });


                if (method != null)
                {
                    setString1 = (string)method.Invoke(item1, new Object[] { format, formatProvider });
                    setString2 = (string)method.Invoke(item2, new Object[] { format, formatProvider });

                }
                else
                {
                    setString1 = item1.ToString();
                    setString2 = item2.ToString();

                }

                if (setString1 == null) throw new NullReferenceException();
                if (setString2 == null) throw new NullReferenceException();

                setStringAll = "(" + setString1 + "," + setString2 + ")";

                //メンバの名前を表示する
                Debug.WriteLine(setStringAll);

                //末尾カンマを削除するようにカンマ追加
                if (ret != "{") ret += ",";
                ret += setStringAll;
            }


            ret += "}";
            return ret;
        }
        #endregion

        #region メソッド
        
        /// <summary>
        /// 全体集合Uを入れるメソッド
        /// </summary>
        /// <param name="inputUniverse">入れる全体集合</param>
        /// <returns>成功</returns>
        public bool SetUniverse(SortedSet<T> inputUniverse)
        {
            if (!isOKUniverse(inputUniverse))
            {
                throw new ArgumentException("inputUniverseがグラフGの各要素を含んでおりません。");
                //return false;
            }
            this.Universe = inputUniverse;
            return true;

        }

        /// <summary>
        /// 全体集合Uを生成してセットするソッド
        /// </summary>
        /// <returns>成功</returns>
        public bool UniverseSetter()
        {
            return this.SetUniverse(CreateUniverse());
        }


        /// <summary>
        /// グラフGが、直積S×Sの部分集合かどうか
        /// </summary>
        /// <returns></returns>
        public bool isOKUniverse(SortedSet<T> inputSet)
        {
            if (inputSet is null)
            {
                throw new ArgumentNullException(nameof(inputSet));
            }

            foreach (var pair in this)
            {
                var (item1, item2) = pair;
                if (!inputSet.Contains(item1)) return false;
                if (!inputSet.Contains(item2)) return false;

            }

            return true;
        }


        /// <summary>
        /// グラフG(this)から最小のUniverseを作成するメソッド
        /// </summary>
        public SortedSet<T> CreateUniverse()
        {
            SortedSet<T> retUniverse = new SortedSet<T>();
            foreach (var pair in this)
            {
                var (item1, item2) = pair;
                retUniverse.Add(item1);
                retUniverse.Add(item2);
            }
            return retUniverse;
        }


        public virtual void Update()
        {
            if (this is null) return;
            if (Universe is null) this.UniverseSetter();

            if (!isOKUniverse(Universe)) throw new Exception("Domainが不正です");

        }

        #endregion




    }
}
