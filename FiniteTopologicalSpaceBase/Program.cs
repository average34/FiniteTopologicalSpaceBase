using System;
using System.Collections.Generic;
using FiniteTopologicalSpaceBase.Topology;

namespace FiniteTopologicalSpaceBase
{
    class Program
    {
        static void Main(string[] args)
        {

            var SetX = new FiniteSet<element>();
            SetX.Add(element.One);
            SetX.Add(element.Five);

            var SetY = new FiniteSet<element>();
            SetY.Add(element.Two);
            SetY.Add(element.Four);
            SetY.Add(element.Five);

            Console.WriteLine("Hello World!");
            Console.WriteLine(SetX.ToString("G"));
            Console.WriteLine(SetY.ToString("D"));
            Console.WriteLine(SetX.And(SetY as FiniteSet<element>));
            Console.WriteLine(SetX.Or(SetY as FiniteSet<element>));
            Console.WriteLine(SetX.Diff(SetY as FiniteSet<element>));
            Console.WriteLine(SetX.Xor(SetY as FiniteSet<element>));
            Console.WriteLine(new FiniteSet<element>());
            Console.WriteLine(SetX.PowerSet());
            Console.WriteLine(SetY.PowerSet());
            Console.WriteLine(SetX.PowerSet().ToString("G"));
            Console.WriteLine(SetY.PowerSet().ToString("G"));

            Console.WriteLine("二項関係のテスト1");
            var rel = new EndoRelation<element> ();
            rel.Add((element.One, element.Two));
            rel.Add((element.Two, element.Three));
            rel.Add((element.Three, element.Four));
            rel.Add((element.Four, element.Five));
            rel.Add((element.Five, element.Five));
            Console.WriteLine(rel.ToString());
            Console.WriteLine(rel.ToString("G"));
            Console.WriteLine(rel.ToString("F"));
            Console.WriteLine(rel.ToString("D"));
            Console.WriteLine(rel.ToString("X"));


            Console.WriteLine("二項関係のテスト2");
            var rel2 = new EndoRelation<FiniteSet<element>>();
            rel2.Add((SetX, SetY));
            rel2.Add((SetX, SetX));
            Console.WriteLine(rel2);
            Console.WriteLine(rel2.ToString());
            Console.WriteLine(rel2.ToString(null));
            Console.WriteLine(rel2.ToString("D"));

            Console.WriteLine("二項関係のテスト3:部分集合族のなす関係");
            EndoRelation<FiniteSet<element>> rel3 = (SetY.PowerSet()).CreateSubsetRelation(SetY);
            Console.WriteLine(rel3);
            Console.WriteLine(rel3.ToString());
            Console.WriteLine(rel3.ToString(null));
            Console.WriteLine(rel3.ToString("D"));
            Console.ReadLine();

        }
    }
}
