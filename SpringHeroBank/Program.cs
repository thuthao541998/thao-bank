using System;
using System.Collections.Generic;
using SpringHeroBank.entity;
using SpringHeroBank.model;
using SpringHeroBank.utility;
using SpringHeroBank.view;

namespace SpringHeroBank {
    class Program {
        public static Account currentLoggedIn;
        delegate int DemoDelegate (int x, int y);

        static int Add (int x, int y) {
            return x + y;

        }
        static int Minus (int x, int y) {
            return x - y;
        }
        static void Main (string[] args) {
            // DemoDelegate myDele;
            // System.Console.WriteLine ("Enter x");
            // var x1 = Console.ReadLine ();
            // var x = Int32.Parse (x1);
            // System.Console.WriteLine ("Enter y");
            // var y1 = Console.ReadLine ();
            // var y = Int32.Parse (y1);
            // System.Console.WriteLine ("1.Add");
            // System.Console.WriteLine ("2.Minus");
            // System.Console.WriteLine ("Please enter choice");
            // var choice1 = Console.ReadLine ();     
            // var choice = Int32.Parse(choice1);
            // switch (choice) {
            //     case 1:
            //         myDele = Add;                                 
            //         System.Console.WriteLine(myDele (x, y));
            //         break;
            //     case 2:
            //         myDele = Minus;
            //         System.Console.WriteLine(myDele (x, y));
            //         break;
            // }

            MainView.GenerateMenu(); 
            //FileHandle.ReadTransaction("NeverEverGetBackTogether.txt");    
        }
    }
}