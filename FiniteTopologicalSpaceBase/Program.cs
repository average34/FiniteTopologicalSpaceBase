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

            var SetZ = new FiniteSet<element>() { (element)1, (element)2, (element)3, (element)4, (element)5 };


            Console.WriteLine("Hello World!");
            //Console.WriteLine(SetY.PowerSet().Contains(SetY));
            //Console.WriteLine(SetY.PowerSet().Contains(SetX));

            //5.OpenSetsListOutMethod();
            6.OpenSetsListOutMethod();
            //8.OpenSetsListOutMethod();

            Console.ReadLine();

        }
    }
}
