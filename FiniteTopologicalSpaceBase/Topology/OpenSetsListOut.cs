using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace FiniteTopologicalSpaceBase.Topology
{
    public static class OpenSetsListOut
    {

        /// <summary>
        /// 「自然数Nを与えたとき、集合X={1,2,3,…,N}を考え、
        /// その冪集合P(X)の部分集合のうち、共通部分を取る演算について閉じている部分集合を全列挙せよ」
        /// https://twitter.com/noshi91/status/1342137767615066113
        /// </summary>
        /// <param name="cardinality">台集合の濃度</param>
        /// <returns>積集合-閉 を満たす集合族の個数</returns>
        public static BigInteger OpenSetsListOutMethod(this int cardinality)
        {
            // Stopwatchクラス生成
            var sw = new System.Diagnostics.Stopwatch();

            //-----------------
            // 計測開始
            sw.Start();
            Console.WriteLine($"列挙開始 集合濃度:{cardinality}");


            BigInteger ans = 0;
            int powerCardinality = (1 << cardinality);

            //bitSets: 考えている集合族
            //addSet: 追加を検討している集合
            void dfs(BigInteger bitSets, int addSet)
            {

                if (addSet == powerCardinality)
                {
                    //bitSetsに空集合0が含まれていなければ答えとはしない
                    if (((bitSets >> 0) & 1) == 0) return;

                    //bitSetsに台集合(powerCardinality - 1)が含まれていなければ答えとはしない
                    if (((bitSets >> ((powerCardinality - 1))) & 1) == 0) return;

                    for (int set1 = 0; set1 != powerCardinality; ++set1)
                    {
                        //int set1 = 0;
                        //bitSetsに集合set1が含まれていて
                        if (((bitSets >> set1) & 1) == 1)
                        {
                            for (int set2 = 0; set2 != (powerCardinality - 1); ++set2)
                            {
                                //bitSetsに集合set2が含まれていて
                                if (((bitSets >> set2) & 1) == 1)
                                {
                                    //積集合set1∩set2 がbitSetsにが含まれていなければ答えとはしない
                                    //if (((bitSets >> (set1 & set2)) & 1) == 0) return;
                                    //和集合set1∪set2 がbitSetsにが含まれていなければ答えとはしない
                                    if (((bitSets >> (set1 | set2)) & 1) == 0)
                                    {

                                        //Console.WriteLine($"エラー:和集合が含まれていない:{bitSets}");
                                        //break;
                                        return;
                                    }
                                }
                            }
                        }
                    }


                    ans += 1;

                    //集合族の出力処理
                    {
                        //集合系を印刷するかどうか判定。しなければreturn;
                        if (cardinality >= 7 && ans % (powerCardinality * 10000) != 0) return;
                        else if (cardinality == 6 && (ans % 1000 != 0)) return;
                        else if (cardinality == 5 && ans % 1000 != 0) return;
                        //ここから集合系の印刷

                        StringBuilder printFamilySets = new StringBuilder();
                        printFamilySets.Append("{");
                        for (int t = 0; t != powerCardinality; ++t)
                        {
                            if (((bitSets >> t) & 1) == 0)
                            {
                                continue;
                            }
                            printFamilySets.Append("{");
                            for (int k = 0; k != cardinality; ++k)
                            {
                                if (((t >> k) & 1) == 1)
                                {
                                    printFamilySets.Append(k + 1);
                                    printFamilySets.Append(",");
                                }
                            }
                            if (printFamilySets.Length != 0) printFamilySets.Length -= 1; // 末尾を1文字削除
                            printFamilySets.Append("},");
                        }

                        if (printFamilySets.Length != 0) printFamilySets.Length -= 1; // 末尾を1文字削除
                        Console.Write($"{ans}:{bitSets}:");
                        Console.WriteLine($"{printFamilySets}");


                        TimeSpan ts = sw.Elapsed;
                        Console.WriteLine($"経過時間　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
                        //ここまで集合系の印刷
                    }
                    return;
                }

                //「addSet は追加しないことにする。bitSets はそのまま、次は addSet+1 の追加の如何を検討する」という再帰
                dfs(bitSets, addSet + 1);


                for (int t = 0; t != addSet; ++t)
                {
                    //bitSetsに集合tが含まれていて
                    if (((bitSets >> t) & 1) == 1)
                    {
                        //積集合t∩addSet がbitSetsに含まれていない場合、addSetは追加しない
                        if (((bitSets >> (t & addSet)) & 1) == 0)
                            return;
                    }
                }




                //「addSet を追加することにする。bitSets に addSet を追加して、次は addSet+1 ...」という再帰
                dfs(bitSets | (1 << addSet), addSet + 1);
            };

            dfs(0, 0);


            Console.WriteLine($"{cardinality}の有限位相空間：{ans}個");

            bool isCorrenct = false;
            switch (cardinality)
            {
                case 1: if (ans == 1) isCorrenct = true;
                    break;
                case 2:
                    if (ans == 4)
                        isCorrenct = true;
                    break;
                case 3:
                    if (ans == 29)
                        isCorrenct = true;
                    break;
                case 4:
                    if (ans == 355)
                        isCorrenct = true;
                    break;
                case 5:
                    if (ans == 6942)
                        isCorrenct = true;
                    break;
                case 6:
                    if (ans == 209527)
                        isCorrenct = true;
                    break;
                default:
                    break;
            }
            if(isCorrenct)
                Console.WriteLine($"正解");
            else
                Console.WriteLine($"【不正解】");


            // 計測停止
            sw.Stop();

            // 結果表示
            Console.WriteLine("■処理にかかった時間");
            TimeSpan ts = sw.Elapsed;
            Console.WriteLine($"　{ts}");
            Console.WriteLine($"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
            Console.WriteLine($"　{sw.ElapsedMilliseconds}ミリ秒");

            return ans;
        }
    }
}
