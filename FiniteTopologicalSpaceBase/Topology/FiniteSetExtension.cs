using System;
using System.Collections.Generic;
using System.Text;

namespace FiniteTopologicalSpaceBase.Topology
{
    /// <summary>
    /// 有限集合FiniteSetの拡張メソッド
    /// </summary>
    public static class FiniteSetExtension
    {
        public static int Cardinality<TEnum>(this FiniteSet<TEnum> set)
            where TEnum : Enum
        {
            return set.Count;
        }

        /// <summary>
        /// 2つの集合の交叉を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1⋂集合2</returns>
        public static FiniteSet<TEnum> And<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            return Intersection(set1, set2);
        }

        /// <summary>
        /// 2つの集合の交叉を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1⋂集合2</returns>
        public static FiniteSet<TEnum> Product<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            return Intersection(set1, set2);
        }

        /// <summary>
        /// 2つの集合の交叉を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1⋂集合2</returns>
        public static FiniteSet<TEnum> Intersection<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            var retSet = new FiniteSet<TEnum>(set1);
            retSet.IntersectWith(set2);
            retSet.Update();
            return retSet;
        }



        /// <summary>
        /// 2つの集合の和集合を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1∪集合2</returns>
        public static FiniteSet<TEnum> Or<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            return Union(set1, set2);
        }



        /// <summary>
        /// 2つの集合の和集合を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1∪集合2</returns>
        public static FiniteSet<TEnum> Sum<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            return Union(set1, set2);
        }


        /// <summary>
        /// 2つの集合の和集合を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1∪集合2</returns>
        public static FiniteSet<TEnum> Union<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            var retSet = new FiniteSet<TEnum>(set1);
            retSet.UnionWith(set2);
            retSet.Update();
            return retSet;
        }


        /// <summary>
        /// 2つの集合の差集合を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1＼集合2</returns>
        public static FiniteSet<TEnum> Diff<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            return Except(set1, set2);
        }

        /// <summary>
        /// 2つの集合の差集合を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1＼集合2</returns>
        public static FiniteSet<TEnum> Except<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            var retSet = new FiniteSet<TEnum>(set1);
            retSet.ExceptWith(set2);
            retSet.Update();
            return retSet;
        }


        /// <summary>
        /// 2つの集合の対称差（排他的論理和）を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1△集合2</returns>
        public static FiniteSet<TEnum> Xor<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            return SymmetricExcept(set1, set2);
        }

        /// <summary>
        /// 2つの集合の対称差（排他的論理和）を取る
        /// </summary>
        /// <param name="set1">集合1</param>
        /// <param name="set2">集合2</param>
        /// <returns>集合1△集合2</returns>
        public static FiniteSet<TEnum> SymmetricExcept<TEnum>(this FiniteSet<TEnum> set1, FiniteSet<TEnum> set2)
            where TEnum : Enum
        {
            var retSet = new FiniteSet<TEnum>(set1);
            retSet.SymmetricExceptWith(set2);
            retSet.Update();
            return retSet;
        }

        /// <summary>
        /// 与えた有限集合の冪集合を返す
        /// 冪集合は、すべての部分集合を要素にもつ
        /// </summary>
        /// <param name="originalSet">元の集合（台集合）</param>
        /// <returns>冪集合 P(originalSet)</returns>
        public static FamilyOfSubsets<TEnum> PowerSet<TEnum>(this FiniteSet<TEnum> originalSet)
            where TEnum : Enum
        {
            //まず密着位相をつくる
            var retFamily = originalSet.IndiscreteFamily();

            //クソでかくなるのでビット演算を使わざるを得なかった

            // 1 << n は 2^n (2のn乗)
            for (int index = 0; index < (1 << originalSet.Count); index++)
            {
                var subset = new FiniteSet<TEnum>();
                int bit = 0;
                foreach (var element in originalSet)
                {
                    if ((index & (1 << bit)) != 0)
                    { // if the j-th bit of i is set...
                        subset.Add(element); // add the item to the current sublist
                    }
                    bit++;
                }
                retFamily.Add(subset); // add the current sublist to the final result
            }



            return retFamily;
        }

        /// <summary>
        /// 与えた有限集合の密着位相となる部分集合族を返す
        /// </summary>
        /// <param name="originalSet">元の集合（台集合）</param>
        /// <returns>密着位相となる部分集合族 {originalSet,Empty}</returns>
        public static FamilyOfSubsets<TEnum> IndiscreteFamily<TEnum>(this FiniteSet<TEnum> originalSet)
            where TEnum : Enum
        {

            //空集合なら空集合の集合だけ返す　つまり{{}}
            if (originalSet == null || originalSet.Count == 0)
            {
                return new FamilyOfSubsets<TEnum>() { new FiniteSet<TEnum>() };
            }


            var retFamily = new FamilyOfSubsets<TEnum>();
            retFamily.Add(originalSet);
            retFamily.Add(new FiniteSet<TEnum>());

            return retFamily;
        }


    }
}
