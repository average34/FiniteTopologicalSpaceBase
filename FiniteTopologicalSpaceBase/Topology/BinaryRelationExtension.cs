using System;
using System.Collections.Generic;
using System.Text;

namespace FiniteTopologicalSpaceBase.Topology
{
    /// <summary>
    /// クラスBinaryRelation<T1,T2> の拡張メソッド
    /// </summary>
    public static class BinaryRelationExtension
    {
        /// <summary>
        /// 二項関係Rについて、 x R yを満たすかどうか
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">左辺</param>
        /// <param name="rel">関係</param>
        /// <param name="item2">右辺</param>
        /// <returns>x R yを満たす</returns>
        public static bool hasRelation<T1, T2>(this T1 item1, BinaryRelation<T1, T2> rel, T2 item2)
        {
            return rel.Contains((item1, item2));
        }

        #region 一般の二項関係について考える性質

        /// <summary>
        /// 二項関係Rが左一意的であるかどうか
        /// x R y かつ z R y なるときは必ず x = z となる
        /// 単射であるとも言う
        /// </summary>
        /// <param name="rel">二項関係</param>
        /// <returns>左一意的・単射</returns>
        public static bool isLeftUnique<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            foreach (var itemX in rel.Domain)
            {
                foreach (var itemZ in rel.Domain)
                {

                    foreach (var itemY in rel.Codomain)
                    {
                        //x R y かつ z R y なるときは必ず x = z となる
                        if (hasRelation(itemX, rel, itemY) && hasRelation(itemZ, rel, itemY))
                        {
                            if (!(itemX.Equals(itemZ))) return false;
                        }
                    }
                }
            }

            return true;

        }


        /// <summary>
        /// 二項関係Rが右一意的であるかどうか
        /// x R y かつ x R z なるときは必ず y = z となる
        /// 函数的・部分写像ともいう
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">二項関係</param>
        /// <returns>右一意的</returns>
        public static bool isRightUnique<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            foreach (var itemX in rel.Domain)
            {
                foreach (var itemY in rel.Codomain)
                {
                    foreach (var itemZ in rel.Codomain)
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
        /// 二項関係Rが一対一であるかどうか
        /// 左一意的かつ右一意的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">二項関係R</param>
        /// <returns>一対一</returns>
        public static bool isOneToOne<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            return rel.isLeftUnique() && rel.isRightUnique();

        }

        /// <summary>
        /// 二項関係Rが左全域的かどうか
        /// U の各元 x に対して、それぞれ x R y となるような y ∈ U がとれる（存在する）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">二項関係R</param>
        /// <returns>左全域的</returns>
        public static bool isLeftTotal<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            foreach (var itemX in rel.Domain)
            {
                bool ExistY = false;
                foreach (var itemY in rel.Codomain)
                {
                    if (hasRelation<T1, T2>(itemX, rel, itemY))
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
        /// 二項関係Rが右全域的かどうか
        /// Cod の各元 y に対して、それぞれ x R y となるような x ∈ Dom がとれる（存在する）
        /// 全射とも言う
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">二項関係R</param>
        /// <returns>右全域的・全射</returns>
        public static bool isRightTotal<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            foreach (var itemY in rel.Codomain)
            {
                bool ExistX = false;
                foreach (var itemX in rel.Domain)
                {
                    if (hasRelation<T1, T2>(itemX, rel, itemY))
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
        /// 二項関係Rが対応かどうか
        /// 左全域的かつ右全域的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">二項関係R</param>
        /// <returns>対応</returns>
        public static bool isCorrespondence<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            return rel.isLeftTotal() && rel.isRightTotal();

        }

        /// <summary>
        /// 二項関係Rが関数かどうか
        /// 右一意的かつ左全域的
        /// 函数関係・一意対応・写像とも言う
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">二項関係R</param>
        /// <returns>関数</returns>
        public static bool isFunction<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            return rel.isLeftTotal() && rel.isRightUnique();

        }

        /// <summary>
        /// 二項関係Rが全単射かどうか
        /// 一対一かつ対応
        /// 一対一対応・双射とも言う もちろん関数の条件を満たす
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rel">二項関係R</param>
        /// <returns>関数</returns>
        public static bool isBijection<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            return rel.isOneToOne() && rel.isCorrespondence();

        }



        #endregion

        #region 一般の二項関係について考える操作

        /// <summary>
        /// 逆関係
        /// 要するに逆像の関係バージョン。R^−1 := {(y, x) | (x, y) ∈ R}.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="rel">二項関係</param>
        /// <returns>逆関係</returns>
        public static BinaryRelation<T2, T1> Converse<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            var retRel = new BinaryRelation<T2, T1>();
            foreach ((T1, T2) tuple in rel)
            {
                retRel.Add((tuple.Item2, tuple.Item1));
            }

            return retRel;
        }


        /// <summary>
        /// 補関係
        /// 補集合。 R^c ＝(X×Y)＼R
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="rel">二項関係</param>
        /// <returns>逆関係</returns>
        public static BinaryRelation<T1, T2> Complement<T1, T2>(this BinaryRelation<T1, T2> rel)
        {
            var retRel = new BinaryRelation<T1, T2>();
            foreach (var itemX in rel.Domain)
            {
                foreach (var itemY in rel.Codomain)
                {
                    if(!rel.Contains((itemX, itemY))) retRel.Add((itemX, itemY));
                }
            }

            return retRel;
        }



        #endregion
    }
}
