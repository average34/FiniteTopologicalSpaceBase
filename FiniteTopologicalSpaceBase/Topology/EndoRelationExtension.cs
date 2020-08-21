using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections.Immutable;

namespace FiniteTopologicalSpaceBase.Topology
{
    /// <summary>
    /// クラスEndoRelation<T> の拡張メソッド
    /// </summary>
    public static class EndoRelationExtension
    {

        /// <summary>
        /// 自己関係Rについて、 x R yを満たすかどうか
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">左辺</param>
        /// <param name="rel">関係</param>
        /// <param name="item2">右辺</param>
        /// <returns>x R yを満たす</returns>
        public static bool hasRelation<T>(this T item1, EndoRelation<T> rel, T item2)
        {
            return rel.Contains((item1, item2));
        }

        #region 一般の二項関係についても考える性質

        /// <summary>
        /// 自己関係Rが左一意的であるかどうか
        /// x R y かつ z R y なるときは必ず x = z となる
        /// 単射であるとも言う
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>左一意的</returns>
        public static bool isLeftUnique<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemZ in rel.Universe)
                {

                    foreach (var itemY in rel.Universe)
                    {
                        //x R y かつ z R y なるときは必ず x = z となる
                        if (hasRelation(itemX, rel, itemY) && hasRelation(itemZ, rel, itemY))
                        {
                            if (!(itemX.Equals(itemZ) )) return false;
                        }
                    }
                }
            }

            return true;

        }


        /// <summary>
        /// 自己関係Rが右一意的であるかどうか
        /// x R y かつ x R z なるときは必ず y = z となる
        /// 函数的・部分写像ともいう
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>右一意的</returns>
        public static bool isRightUnique<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    foreach (var itemZ in rel.Universe)
                    {
                        //x R y かつ x R z なるときは必ず y = z となる
                        if (hasRelation(itemX, rel, itemY) && hasRelation(itemX, rel, itemZ))
                        {
                            if (!(itemY.Equals(itemZ))) return false;
                        }
                    }
                }
            }

            return true;

        }

        /// <summary>
        /// 自己関係Rが一対一であるかどうか
        /// 左一意的かつ右一意的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係R</param>
        /// <returns>一対一</returns>
        public static bool isOneToOne<T>(this EndoRelation<T> rel)
        {
            return rel.isLeftUnique() && rel.isRightUnique();

        }

        /// <summary>
        /// 自己関係Rが左全域的かどうか
        /// U の各元 x に対して、それぞれ x R y となるような y ∈ U がとれる（存在する）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係R</param>
        /// <returns>左全域的</returns>
        public static bool isLeftTotal<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                bool ExistY = false;
                foreach (var itemY in rel.Universe)
                {
                    if (hasRelation<T>(itemX, rel, itemY))
                    {
                        ExistY = true;
                        break;
                    }
                }

                if (ExistY == false) return false;
            }

            return true;

        }


        /// <summary>
        /// 自己関係Rが右全域的かどうか
        /// U の各元 y に対して、それぞれ x R y となるような x ∈ U がとれる（存在する）
        /// 全射とも言う
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係R</param>
        /// <returns>右全域的</returns>
        public static bool isRightTotal<T>(this EndoRelation<T> rel)
        {
            foreach (var itemY in rel.Universe)
            {
                bool ExistX = false;
                foreach (var itemX in rel.Universe)
                {
                    if (hasRelation<T>(itemX, rel, itemY))
                    {
                        ExistX = true;
                        break;
                    }
                }

                if (ExistX == false) return false;
            }

            return true;

        }

        /// <summary>
        /// 自己関係Rが対応かどうか
        /// 左全域的かつ右全域的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係R</param>
        /// <returns>対応</returns>
        public static bool isCorrespondence<T>(this EndoRelation<T> rel)
        {
            return rel.isLeftTotal() && rel.isRightTotal();

        }

        /// <summary>
        /// 自己関係Rが関数かどうか
        /// 右一意的かつ左全域的
        /// 函数関係・一意対応・写像とも言う
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係R</param>
        /// <returns>関数</returns>
        public static bool isFunction<T>(this EndoRelation<T> rel)
        {
            return rel.isLeftTotal() && rel.isRightUnique();

        }

        /// <summary>
        /// 自己関係Rが全単射かどうか
        /// 一対一かつ対応
        /// 一対一対応・双射とも言う もちろん関数の条件を満たす
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係R</param>
        /// <returns>関数</returns>
        public static bool isBijection<T>(this EndoRelation<T> rel)
        {
            return rel.isOneToOne() && rel.isCorrespondence();

        }







        #endregion
        #region 自己関係の場合に考える性質

        /// <summary>
        /// 自己関係Rが反射的かどうか。
        /// すべての x∈U について x R x を満たすかどうか
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>反射的</returns>
        public static bool isReflexive<T>(this EndoRelation<T> rel)
        {
            foreach (var item in rel.Universe)
            {
                //item R itemを満たさない場合は偽
                if (!hasRelation(item, rel, item)) return false;
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが非反射的かどうか。
        /// すべての x∈U について x R x を満たさない
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>非反射的</returns>
        public static bool isIreflexive<T>(this EndoRelation<T> rel)
        {
            foreach (var item in rel.Universe)
            {
                //item R itemを満たす場合は偽
                if (hasRelation(item, rel, item)) return false;
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが余反射的かどうか。
        /// X の各元 x, y について、x R y ならば x = y が成り立つ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>余反射的</returns>
        public static bool isCoreflexive<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    //itemX R itemY だが itemX != itemY の場合は偽
                    if (hasRelation(itemX, rel, itemY))
                    {
                        if (!itemX.Equals(itemY))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが対称的かどうか。
        /// X の各元 x, y について、x R y ならば y R x が成り立つ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>対称的</returns>
        public static bool isSymmetric<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    //itemX R itemY だが itemY R itemX でない場合は偽
                    if (hasRelation(itemX, rel, itemY))
                    {
                        if (!hasRelation(itemY, rel, itemX))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが反対称的かどうか。
        /// X の各元 x, y について、x R y かつ y R x ならば x = yが成り立つ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>反対称的</returns>
        public static bool isAntiSymmetric<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    //itemX R itemY かつ itemY R itemX だが、
                    if (hasRelation(itemX, rel, itemY) && hasRelation(itemY, rel, itemX))
                    {
                        //itemXとitemYの値が同じでない場合は偽
                        if (!itemX.Equals(itemY))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが非対称かどうか。
        /// X の各元 x, y について、x R y ならば 常に y R x が偽
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>非対称</returns>
        public static bool isAsymmetric<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    //itemX R itemY だが、
                    if (hasRelation(itemX, rel, itemY))
                    {
                        //itemY R itemX の場合があるなら偽
                        if (hasRelation(itemY, rel, itemX))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが推移的かどうか。
        /// X の各元 x, y について、x R y かつ y R z ならば x R z 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>推移的</returns>
        public static bool isTransitive<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    foreach (var itemZ in rel.Universe)
                    {
                        //itemX R itemY かつ itemY R itemZ だが、
                        if (hasRelation(itemX, rel, itemY) && hasRelation(itemY, rel, itemZ))
                        {
                            //itemX R itemZ でない場合があるなら偽
                            if (!hasRelation(itemX, rel, itemZ))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 自己関係Rが完全性をもつかどうか。
        /// X の各元 x, y について、x R y または y R x の一方あるいは両方が必ず満足される        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>完全性</returns>
        public static bool isTotal<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    // ItemX R ItemY でも ItemY R ItemXでもない場合があるなら偽
                    if (!hasRelation(itemX, rel, itemY) && !hasRelation(itemX, rel, itemY))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが三分的かどうか。
        /// X の各元 x, y について、x R y, y R x, x = y のうちの何れか一つのみが成り立つ       /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>三分的</returns>
        public static bool isTrichotomous<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    bool isXRY = hasRelation(itemX, rel, itemY);
                    bool isYRX = hasRelation(itemX, rel, itemY);
                    bool isXequalY = itemX.Equals(itemY);

                    // どれか2条件を満たす要素があるなら偽
                    if (isXRY && (isYRX || isXequalY))
                        return false;
                    else if (isYRX && isXequalY)
                        return false;
                    // どれにも当てはまらないならば偽
                    if (!isXRY && !isYRX && !isXequalY)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが右ユークリッド的かどうか。
        /// X の任意の元 x, y, z について、x R y かつ x R z が成り立てば、
        /// 必ず y R z かつ z R y が成り立つ
        /// /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>右ユークリッド的</returns>
        public static bool isRightEuclidean<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    foreach (var itemZ in rel.Universe)
                    {
                        //itemX R itemY かつ itemX R itemZ だが、
                        if (hasRelation(itemX, rel, itemY) && hasRelation(itemX, rel, itemZ))
                        {
                            //itemY R itemZ でない、または itemX R itemZ でない場合があるなら偽
                            if (!hasRelation(itemY, rel, itemZ))
                            {
                                return false;
                            }
                            else if (!hasRelation(itemX, rel, itemZ))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが左ユークリッド的かどうか。
        /// X の任意の元 x, y, z について、x R z かつ y R z が成り立てば、
        /// 必ず x R y かつ y R x が成り立つ
        /// /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>左ユークリッド的</returns>
        public static bool isLeftEuclidean<T>(this EndoRelation<T> rel)
        {
            foreach (var itemX in rel.Universe)
            {
                foreach (var itemY in rel.Universe)
                {
                    foreach (var itemZ in rel.Universe)
                    {
                        //itemX R itemZ かつ itemY R itemZ だが、
                        if (hasRelation(itemX, rel, itemZ) && hasRelation(itemY, rel, itemZ))
                        {
                            //itemX R itemY でない、または itemY R itemX でない場合があるなら偽
                            if (!hasRelation(itemX, rel, itemY))
                            {
                                return false;
                            }
                            else if (!hasRelation(itemY, rel, itemX))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが連続的かどうか。
        /// X の各元 x に対して、x R y となるような y ∈ X がそれぞれとれる
        /// 必ず x R y かつ y R x が成り立つ
        /// /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>連続的</returns>
        public static bool isSerial<T>(this EndoRelation<T> rel)
        {
            //左全域的と全く同じ定義
            foreach (var itemX in rel.Universe)
            {
                bool ExistY = false;
                foreach (var itemY in rel.Universe)
                {
                    if (hasRelation(itemX, rel, itemY))
                    {
                        ExistY = true;
                        break;
                    }
                }

                if (ExistY == false) return false;
            }
            return true;
        }

        /// <summary>
        /// 自己関係Rが整礎的かどうか。
        /// X の任意の空でない部分集合Sが、極小元s(Sのどの元xもxRsとならない)を持つ
        /// ∀S ⊆ X (S ≠ X → ∃s∈S ∀x∈X ¬(x R s) )
        /// /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係</param>
        /// <returns>整礎的</returns>
        public static bool isWellFounded<T>(this EndoRelation<T> rel)
        {
            //全部分集合を取ることは冪集合をとることに等しく、こいつは場合によっては生成が5000兆時間かかるため、
            //部分集合を生成しながら正誤の判定をする
            SortedSet<T> universe = rel.Universe;


            /// ∀S ⊆ X (S ≠ X → ∃s∈S ∀x∈X ¬(x R s) )
            /// ¬(∃S ⊆ X ¬(S ≠ X → ∃s∈S ∀x∈X ¬(x R s) ))
            /// ¬(∃S ⊆ X (S = X ← ¬(∃s∈S ∀x∈X ¬(x R s)) ))

            // 1 << n は 2^n (2のn乗)
            for (int index = 0; index < (1 << universe.Count); index++)
            {
                var subset = new SortedSet<T>();
                int bit = 0;
                foreach (var element in universe)
                {
                    if ((index & (1 << bit)) != 0)
                    {
                        subset.Add(element);
                    }
                    bit++;
                }

                //判定を行う

                //空集合の場合は除外
                if (subset.Count == 0) continue;

                bool existS = false;
                foreach (var itemS in subset)
                {
                    //極小元だとわかった場合
                    if (rel.isMinimalElement(itemS))
                    {
                        //だいたいの場合当てはまると思うが、一応
                        if (!(subset is null))
                        {
                            existS = true;
                        }
                    }
                }

                //subsetにSがないとわかったら偽
                if (existS) return false;
            }
            return true;
        }

        /// <summary>
        /// 自己関係relがあるとき、全体集合 rel.Universe の 元element が極小元かどうか
        /// a∈A ∀x∈A:¬(xRa)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係R</param>
        /// <param name="element">元</param>
        /// <returns>極小元である</returns>
        public static bool isMinimalElement<T>(this EndoRelation<T> rel, T element)
        {
            //実はelementがなくても真ではあるのだが…
            if (element == null) {
                var myEx = new System.ComponentModel.WarningException("極小元かどうか判定する際、空のelementが指定されました");
                Console.WriteLine(myEx.Message);
                Console.WriteLine(myEx.ToString());
                return true;
            }
            else if (!rel.Universe.Contains(element))
            {
                throw new ArgumentException("elementが全体集合rel.Universeの要素ではありません");
            }

            //Xのどの元xも x R element とならない ような元elementかどうかを判定
            foreach (T itemX in rel.Universe)
            {
                if (hasRelation(itemX, rel, element)) return false;
            }
            return true;
        }

        /// <summary>
        /// 自己関係relがあるとき、全体集合 rel.Universe の 元element が極大元かどうか
        /// a∈A ∀x∈A:¬(aRx)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">自己関係R</param>
        /// <param name="element">元</param>
        /// <returns>極大元である</returns>
        public static bool isMaximulElement<T>(this EndoRelation<T> rel, T element)
        {
            //実はelementがなくても真ではあるのだが…
            if (element == null)
            {
                var myEx = new System.ComponentModel.WarningException("極小元かどうか判定する際、空のelementが指定されました");
                Console.WriteLine(myEx.Message);
                Console.WriteLine(myEx.ToString());
                return true;
            }
            else if (!rel.Universe.Contains(element))
            {
                throw new ArgumentException("elementが全体集合rel.Universeの要素ではありません");
            }

            //Xのどの元xも x R element とならない ような元elementかどうかを判定
            foreach (T itemX in rel.Universe)
            {
                if (hasRelation(element, rel, itemX)) return false;
            }
            return true;
        }

        #endregion

    }



}
