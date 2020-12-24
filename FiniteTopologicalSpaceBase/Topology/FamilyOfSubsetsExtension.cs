using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Linq;

namespace FiniteTopologicalSpaceBase.Topology
{
    public static class FamilyOfSubsetsExtension
    {

        

        #region 基礎的な演算



        /// <summary>
        /// 集合の濃度を返す
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="set">集合</param>
        /// <returns>濃度</returns>
        public static int Cardinality<TEnum>(this FamilyOfSubsets<TEnum> subsets)
            where TEnum : Enum
        {
            return subsets.Count;
        }

        /// <summary>
        /// 部分集合族の総和を取る
        /// </summary>
        /// <param name="subsets">部分集合族</param>
        /// <returns>総和</returns>
        public static FiniteSet<TEnum> SumOfSeq<TEnum>(this FamilyOfSubsets<TEnum> subsets)
            where TEnum : Enum
        {

            //集合がなければ空集合を返す
            if (subsets == null || subsets.Count == 0) return new FiniteSet<TEnum>();
                //まず空集合を設置
                var retSet = new FiniteSet<TEnum>();
            foreach(FiniteSet<TEnum> subset in subsets)
            {
                retSet = retSet.Or(subset);
            }

            return retSet;
        }


        /// <summary>
        /// 部分集合族の総乗を取る
        /// </summary>
        /// <param name="subsets">部分集合族</param>
        /// <returns>総乗</returns>
        public static FiniteSet<TEnum> ProductOfSeq<TEnum>(this FamilyOfSubsets<TEnum> subsets)
            where TEnum : Enum
        {
            //集合がなければ空集合を返す
            if (subsets == null || subsets.Count == 0) return new FiniteSet<TEnum>();
            //まず最大値を設置
            var retSet = subsets.Max;
            foreach (FiniteSet<TEnum> subset in subsets)
            {
                retSet = retSet.And(subset);
            }
            return retSet;
        }

        #endregion


        #region 判定（return bool）
        /// <summary>
        /// 集合族に空集合があるかどうかを判定（集合族自体が空なら偽）
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <returns>空集合があるかどうか</returns>
        public static bool isContainsEmpty<TEnum>(this FamilyOfSubsets<TEnum> subsets)
            where TEnum : Enum
        {
            //集合がなければ偽
            if (subsets == null || subsets.Count == 0) return false;
            return subsets.Contains(new FiniteSet<TEnum>());
        }

        /// <summary>
        ///  ∩-完備,σ乗法性があるかどうか（共通部分∩について閉じているかどうか）
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <returns>∩-完備</returns>
        public static bool isProductComplete<TEnum>(this FamilyOfSubsets<TEnum> subsets)
            where TEnum : Enum
        {
            foreach (FiniteSet<TEnum> subset1 in subsets)
            {
                foreach (FiniteSet<TEnum> subset2 in subsets)
                {
                    FiniteSet<TEnum> andSet = subset1.And(subset2);
                    if (!subsets.Contains(andSet)) return false;
                }
            }

            return true;

        }

        /// <summary>
        ///  ∪-完備,σ加法性があるかどうか（和集合∪について閉じているかどうか）
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <returns>∪-完備</returns>
        public static bool isSumComplete<TEnum>(this FamilyOfSubsets<TEnum> subsets)
            where TEnum : Enum
        {
            foreach (FiniteSet<TEnum> subset1 in subsets)
            {
                foreach (FiniteSet<TEnum> subset2 in subsets)
                {
                    FiniteSet<TEnum> orSet = subset1.Or(subset2);
                    if (!subsets.Contains(orSet)) return false;
                }
            }

            return true;
        }

        /// <summary>
        ///  ＼-完備かどうか（差集合＼について閉じているかどうか）
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <returns>＼-完備</returns>
        public static bool isDiffComplete<TEnum>(this FamilyOfSubsets<TEnum> subsets)
            where TEnum : Enum
        {
            foreach (FiniteSet<TEnum> subset1 in subsets)
            {
                foreach (FiniteSet<TEnum> subset2 in subsets)
                {
                    FiniteSet<TEnum> diffSet = subset1.Diff(subset2);
                    IEqualityComparer<FiniteSet<TEnum>> comparer = new FiniteSetComparer<TEnum>();
                    if (!subsets.Contains(diffSet, comparer)) return false;
                }
            }

            return true;
        }


        /// <summary>
        ///  与えた集合が、集合族の全体集合の必要条件を満たしているかどうか
        /// ∀subset∈Subsets ∀element∈subset (element ∈ universe)
        /// 
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <param name="universe">全体集合かどうか判定する集合</param>
        /// <returns>与えた集合は全体集合の必要条件を満たす</returns>
        public static bool isUniverseOK<TEnum>(this FamilyOfSubsets<TEnum> subsets, FiniteSet<TEnum> universe)
            where TEnum : Enum
        {

            foreach (FiniteSet<TEnum> subset in subsets)
            {
                foreach (TEnum element in subset)
                {
                    if (!universe.Contains(element)) return false;
                }
            }
            return true;
        }


        /// <summary>
        ///  補-完備かどうか（補集合cについて閉じているかどうか）
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <param name="universe">全体集合</param>
        /// <returns>補-完備</returns>
        public static bool isComplementComplete<TEnum>(this FamilyOfSubsets<TEnum> subsets,FiniteSet<TEnum> universe)
            where TEnum : Enum
        {
            if (!subsets.isUniverseOK(universe)) throw new ArgumentException("全体集合が、部分集合族の要素である、集合の要素を網羅していません");

            foreach (FiniteSet<TEnum> subset in subsets)
            {
                FiniteSet<TEnum> diffSet = universe.Diff(subset);
                if (!subsets.Contains(diffSet)) return false;
            }
            return true;
        }


        /// <summary>
        ///  △-完備かどうか（対称差△について閉じているかどうか）
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <returns>△-完備</returns>
        public static bool isSymDiffComplete<TEnum>(this FamilyOfSubsets<TEnum> subsets)
            where TEnum : Enum
        {
            foreach (FiniteSet<TEnum> subset1 in subsets)
            {
                foreach (FiniteSet<TEnum> subset2 in subsets)
                {
                    FiniteSet<TEnum> xorSet = subset1.Xor(subset2);
                    if (!subsets.Contains(xorSet)) return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 有限加法族かどうか。これは有限個の台集合であれば「完全加法族」と同値である
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <param name="universe">全体集合・台集合</param>
        /// <returns>有限加法族・完全加法族</returns>
        public static bool isFinitelyAdditive<TEnum>(this FamilyOfSubsets<TEnum> subsets, FiniteSet<TEnum> universe)
            where TEnum : Enum
        {
            //集合がなければ偽
            if (subsets == null || subsets.Count == 0) return false;
            //空集合がなければ偽
            else if (!subsets.isContainsEmpty()) return false;
            //台集合がなければ偽
            else if (!subsets.Contains(universe)) return false;
            //和集合について閉じてなければ偽
            else if (!subsets.isSumComplete()) return false;
            //補集合について閉じてなければ偽
            else if (!subsets.isComplementComplete(universe)) return false;

            //条件を満たしているので真
            return true;
        }

        /// <summary>
        /// （有限）位相空間かどうか。集合族が開集合族の定義を満たすかどうか。
        /// 
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <param name="universe">全体集合・台集合</param>
        /// <returns>（有限）位相空間</returns>
        public static bool isTopological<TEnum>(this FamilyOfSubsets<TEnum> subsets, FiniteSet<TEnum> universe)
            where TEnum : Enum
        {
            //集合がなければ偽
            if (subsets == null || subsets.Count == 0) return false;
            //空集合がなければ偽
            else if (!subsets.isContainsEmpty()) return false;
            //台集合がなければ偽
            else if (!subsets.Contains(universe)) return false;
            //和集合について閉じてなければ偽
            else if (!subsets.isSumComplete()) return false;
            //交叉について閉じてなければ偽
            else if (!subsets.isProductComplete()) return false;

            //条件を満たしているので真
            return true;
        }

        /// <summary>
        /// 密着位相かどうか。
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <param name="universe">全体集合・台集合</param>
        /// <returns>密着位相である</returns>
        public static bool isIniscrete<TEnum>(this FamilyOfSubsets<TEnum> subsets, FiniteSet<TEnum> universe)
            where TEnum : Enum
        {
            //まず位相空間でなければ偽
            if (!subsets.isTopological(universe)) return false;
            //濃度が2でなければ偽
            else if (subsets.Count != 2) return false;

            //条件を満たしているので真
            return true;
        }

        /// <summary>
        /// 離散位相かどうか。
        /// </summary>
        /// <param name="subsets">集合族</param>
        /// <param name="universe">全体集合・台集合</param>
        /// <returns>離散位相である</returns>
        public static bool isDiscrete<TEnum>(this FamilyOfSubsets<TEnum> subsets, FiniteSet<TEnum> universe)
            where TEnum : Enum
        {
            //まず位相空間でなければ偽
            if (!subsets.isTopological(universe)) return false;
            //濃度が2^universe.Countでなければ偽
            else if (subsets.Count != ((BigInteger)1 << universe.Count)) return false;

            //条件を満たしているので真
            return true;
        }


        #endregion

        #region 発展的な演算
        /// <summary>
        /// 部分集合族の包含関係 ⊆ から 自己関係 R を生成するメソッド
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="subsets">部分集合族</param>
        /// <param name="universe">全体集合</param>
        /// <returns></returns>
        public static EndoRelation<FiniteSet<TEnum>> CreateSubsetRelation<TEnum>(this FamilyOfSubsets<TEnum> subsets, FiniteSet<TEnum> universe)
            where TEnum : Enum
        {
            if (!subsets.isUniverseOK(universe)) throw new ArgumentException("全体集合が、部分集合族の要素である、集合の要素を網羅していません");

            var retRel = new EndoRelation<FiniteSet<TEnum>>();

            //自己関係の全体集合は冪集合（もとの台集合の部分集合族なので）
            retRel.SetUniverse(universe.PowerSet());

            //要素となる集合を格納
            foreach (FiniteSet<TEnum> subsetX in subsets)
            {
                foreach (FiniteSet<TEnum> subsetY in subsets)
                {
                    //X⊆Y ⇔ X R Y という定義で自己関係Rを定義
                    if (subsetX.IsSubsetOf(subsetY))
                    {
                        retRel.Add((subsetX,subsetY));
                    }

                }

            }

            return retRel;

        }


        #endregion

    }
}
