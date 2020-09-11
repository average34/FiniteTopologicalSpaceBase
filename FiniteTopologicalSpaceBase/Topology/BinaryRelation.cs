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
    /// 二項関係を表すクラス。
    /// 正確には二項関係におけるグラフGのことであり、対象T1の範囲である始域はSortedSet<T1> Domainで指定する。
    /// 対象T2の範囲である終域はSortedSet<T2> Codomainで指定する。
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class BinaryRelation<T1,T2> : SortedSet<ValueTuple<T1, T2>>, IFormattable
    {

        #region メンバ変数
        public SortedSet<T1> Domain
        {
            get;
            private set;
        }
        public SortedSet<T2> Codomain
        {
            get;
            private set;
        }

        #endregion
        #region コンストラクタ
        public BinaryRelation() : base()
        {
            this.Update();
        }

        public BinaryRelation(IComparer<ValueTuple<T1, T2>> comparer) : base(comparer)
        {
            this.Update();
        }

        public BinaryRelation(IEnumerable<ValueTuple<T1, T2>> collection) : base(collection)
        {
            this.Update();
        }

        public BinaryRelation(IEnumerable<ValueTuple<T1, T2>> collection,
            IComparer<ValueTuple<T1, T2>> comparer) : base(collection, comparer)
        {
            this.Update();
        }

        public BinaryRelation(SortedSet<T1> inputDomain,SortedSet<T2> inputCodomain) : this()
        {
            this.SetDomain(inputDomain);
            this.SetCodomain(inputCodomain);
        }

        public BinaryRelation(IComparer<ValueTuple<T1, T2>> comparer, 
            SortedSet<T1> inputDomain, SortedSet<T2> inputCodomain) : this(comparer)
        {
            this.SetDomain(inputDomain);
            this.SetCodomain(inputCodomain);
        }

        public BinaryRelation(IEnumerable<ValueTuple<T1, T2>> collection,
            SortedSet<T1> inputDomain, SortedSet<T2> inputCodomain) : this(collection)
        {
            this.SetDomain(inputDomain);
            this.SetCodomain(inputCodomain);
        }

        public BinaryRelation(IEnumerable<ValueTuple<T1, T2>> collection, IComparer<ValueTuple<T1, T2>> comparer,
            SortedSet<T1> inputDomain, SortedSet<T2> inputCodomain) : this(collection, comparer)
        {
            this.SetDomain(inputDomain);
            this.SetCodomain(inputCodomain);
        }

        protected BinaryRelation(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


        #endregion

        #region インターフェースのメソッド
        public string ToString(string format = "D", IFormatProvider formatProvider = null)
        {
            string ret = "{";

            foreach (ValueTuple<T1, T2> pair in this)
            {
                (T1 item1, T2 item2) = pair;
                string setString1;
                string setString2;
                string setStringAll;
                Type type1 = typeof(T1);
                Type type2 = typeof(T2);
                MethodInfo method1= type1.GetMethod("ToString", new Type[] { typeof(string), typeof(IFormatProvider) });
                MethodInfo method2= type2.GetMethod("ToString", new Type[] { typeof(string), typeof(IFormatProvider) });


                if (method1 != null)
                {
                    setString1 = (string)method1.Invoke(item1, new Object[] { format, formatProvider });
                }
                else
                {
                    setString1 = item1.ToString();

                }

                if (method2 != null)
                {
                    setString2 = (string)method2.Invoke(item2, new Object[] { format, formatProvider });

                }
                else
                {
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
        /// 始域Domainを入れるメソッド
        /// </summary>
        /// <param name="inputDomain">入れる全体集合</param>
        /// <returns>成功</returns>
        public bool SetDomain(SortedSet<T1> inputDomain)
        {
            if (!isOKDomain(inputDomain))
            {
                throw new ArgumentException("inputUniverseがグラフGの各要素を含んでおりません。");
                //return false;
            }
            this.Domain = inputDomain;
            return true;

        }

        /// <summary>
        /// 終域Codomainを入れるメソッド
        /// </summary>
        /// <param name="inputCodomain">入れる終域</param>
        /// <returns>成功</returns>
        public bool SetCodomain(SortedSet<T2> inputCodomain)
        {
            if (!isOKCodomain(inputCodomain))
            {
                throw new ArgumentException("inputUniverseがグラフGの各要素を含んでおりません。");
                //return false;
            }
            this.Codomain = inputCodomain;
            return true;

        }

        /// <summary>
        /// 始域Domainを生成してセットするソッド
        /// </summary>
        /// <returns>成功</returns>
        public bool DomainGenerator()
        {
            return this.SetDomain(CreateDomain());
        }
        /// <summary>
        /// 終域Codomainを生成してセットするソッド
        /// </summary>
        /// <returns>成功</returns>
        public bool CodomainGenerator()
        {
            return this.SetCodomain(CreateCodomain());
        }


        /// <summary>
        /// 集合inputSetが、二項関係の始域の条件を満たすかどうか
        /// </summary>
        /// <returns></returns>
        public bool isOKDomain(SortedSet<T1> inputSet)
        {
            if (inputSet is null)
            {
                throw new ArgumentNullException(nameof(inputSet));
            }

            foreach (var pair in this)
            {
                var (item1, item2) = pair;
                if (!inputSet.Contains(item1)) return false;

            }

            return true;
        }

        /// <summary>
        /// 集合inputSetが、二項関係の終域の条件を満たすかどうか
        /// </summary>
        /// <returns></returns>
        public bool isOKCodomain(SortedSet<T2> inputSet)
        {
            if (inputSet is null)
            {
                throw new ArgumentNullException(nameof(inputSet));
            }

            foreach (var pair in this)
            {
                var (item1, item2) = pair;
                if (!inputSet.Contains(item2)) return false;

            }

            return true;
        }


        /// <summary>
        /// グラフG(this)から最小のDomainを作成するメソッド
        /// </summary>
        public SortedSet<T1> CreateDomain()
        {
            SortedSet<T1> retDomain = new SortedSet<T1>();
            foreach (var pair in this)
            {
                var (item1, item2) = pair;
                retDomain.Add(item1);
            }
            return retDomain;
        }

        /// <summary>
        /// グラフG(this)から最小のCodomainを作成するメソッド
        /// </summary>
        public SortedSet<T2> CreateCodomain()
        {
            SortedSet<T2> retCodomain = new SortedSet<T2>();
            foreach (var pair in this)
            {
                var (item1, item2) = pair;
                retCodomain.Add(item2);
            }
            return retCodomain;
        }


        public virtual void Update()
        {
            if (this is null) return;
            if (Domain is null) this.DomainGenerator();
            if (Codomain is null) this.CodomainGenerator();

            if(!isOKDomain(Domain)) throw new Exception("Domainが不正です");
            if(!isOKCodomain(Codomain)) throw new Exception("Codomainが不正です");
            
        }

        #endregion




    }
}
