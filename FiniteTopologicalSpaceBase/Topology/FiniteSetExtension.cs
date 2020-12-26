using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace FiniteTopologicalSpaceBase.Topology
{
    /// <summary>
    /// 有限集合FiniteSetの拡張メソッド
    /// </summary>
    public static class FiniteSetExtension
    {

        #region 基礎的な演算
        /// <summary>
        /// 集合の濃度を返す
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="set">集合</param>
        /// <returns>濃度</returns>
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

        #endregion

        #region 判定（return bool）

        #endregion

        #region 発展的な演算

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


        /// <summary>
        /// 台集合からなあらゆる開集合系を網羅したリストを生成するメソッド。
        /// 2^(2^n - 2)という計算をしているため実行がとてつもなく重く、時間もかかる。
        /// そのため、このメソッドで生成したリストを別途jsonやxmlに保存したほうがよい。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="originalSet">元の集合（台集合）</param>
        /// <returns>開集合系リスト</returns>
        public static SortedSet<FamilyOfSubsets<TEnum>> OpenSetsList<TEnum>(this FiniteSet<TEnum> originalSet)
            where TEnum : Enum
        {
            //まず冪集合をつくる　すでに重いと思う
            FamilyOfSubsets<TEnum> PowerSet = originalSet.PowerSet();

            //返すリスト
            var retList = new SortedSet<FamilyOfSubsets<TEnum>>();
            //まず密着位相をつくる
            //var family = originalSet.IndiscreteFamily();

            //クソでかくなるのでビット演算を使わざるを得なかった

            BigInteger indiscreteIndex = 4;

            //扱ってる対象が密着空間かどうか
            bool isDiscrete = false;
            // 1 << n は 2^n (2のn乗)
            for (BigInteger index = 0; index < ((BigInteger)1 << (1 << originalSet.Count) - 2); index++)
            {
                //離散空間の次の位相空間のナンバーを記録する
                //この位相空間は密着位相である
                if (isDiscrete)
                {
                    indiscreteIndex = index;
                    isDiscrete = false;
                }

                //開集合系を初期化
                var openSets = new FamilyOfSubsets<TEnum>();

                //空集合を入れる。これは必ず入れる
                openSets.Add(new FiniteSet<TEnum>());
                //台集合を入れる。これも必ず入れる
                openSets.Add(originalSet);


                //要素の有無を判定し、追加。
                for (int j = 0; j < (1 << originalSet.Count); j++)
                {
                    if ((index & ((BigInteger)1 << j - 1)) != 0)
                    {
                        //ElementAtがクソ重いのでどうにかすべき
                        openSets.Add(PowerSet.ElementAt(j));
                    }
                }

                //位相空間かどうか判定を行う。位相空間でなければ何もしない
                if (openSets.isTopological(originalSet))
                {
                    //開集合系の条件を満たす
                    retList.Add(openSets);

                    bool Dis = openSets.isDiscrete(originalSet);


                    System.IO.StreamWriter sw = new System.IO.StreamWriter(
                      "test.txt", // 出力先ファイル名
                      true, // 追加書き込み
                      Encoding.UTF8);

                    //Console.SetOut(sw); // 出力先（Outプロパティ）を設定

                    Console.WriteLine(index.ToString("x4") + ":" + index.ToString() + ":" +
                        retList.Count + ":" + Dis + ":" +
                        openSets.ToString());

                    //離散空間の場合、しばらくの間は位相空間となる数字が出てこないので、
                    //bit31の値を倍にする
                    //if (Dis && bit31 >= 4)
                    //{
                    //    isDiscrete = true;
                    //    bit31 = indiscreteIndex * 2 - 1;
                    //}


                    sw.Dispose(); // ファイルを閉じてオブジェクトを破棄

                }
                openSets = null;
            }


            Console.WriteLine(retList.Count + "個");
            return retList;
        }


        /// <summary>
        /// 台集合からなあらゆる開集合系を網羅したリストを生成するメソッド。の改良版。
        /// https://twitter.com/noshi91/status/1342137767615066113?s=20
        /// というアドバイスをもらったのでそれを転写。
        /// ビット演算を用いてなんとかしたが、それでもやはり実行がとてつもなく重く、時間もかかる。
        /// そのため、このメソッドで生成したリストを別途jsonやxmlに保存したほうがよい。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="originalSet">元の集合（台集合）</param>
        /// <returns>開集合系リスト</returns>
        public static SortedSet<FamilyOfSubsets<TEnum>> OpenSetsListVer2<TEnum>(this FiniteSet<TEnum> originalSet)
            where TEnum : Enum
        {
            //Ver1では openSetsを作る→openSetsが開集合系の可能性を満たすかどうか判定していたが…

            //まず冪集合をつくる　すでに重いと思う
            FamilyOfSubsets<TEnum> PowerSet = originalSet.PowerSet();
            //返すリスト
            var retList = new SortedSet<FamilyOfSubsets<TEnum>>();


            BigInteger ans = 0;

            void dfs(BigInteger s, int i)
            {
                if (i == 32)
                {
                    string printFamilySets = "";

                    for (int t = 0; t != 32; ++t)
                    {
                        if (((s >> t) & 1) == 0)
                        {
                            continue;
                        }
                        printFamilySets += "{";
                        for (int k = 0; k != 5; ++k)
                        {
                            if (((t >> k) & 1) == 1)
                            {
                                printFamilySets += ",";
                            }
                        }
                        printFamilySets += "},";
                    }
                    Console.WriteLine(printFamilySets);

                    ans += 1;
                    return;
                }
                dfs(s, i + 1);
                for (int t = 0; t != i; ++t)
                {
                    if (((s >> t) & 1) == 1 && ((s >> (t & i)) & 1) == 0)
                    {
                        return;
                    }
                }
                dfs(s | (1 << i), i + 1);
            };

            dfs(0, 0);


            Console.WriteLine(retList.Count + "個");
            return retList;
        }



        #endregion

    }
}
