using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetectGeometricalFigure
{
    class Program
    {
        static void Main(string[] args)
        {
            string arg;
            List<float> farg = new List<float>();
            Console.WriteLine("Wprowadz boki figury (min 3 - max 4) odzielone jedna spacja:");
            arg = Console.ReadLine();
            string[] sarg = ValidateInput(args);
            farg = GetFloats(sarg).ToList();
            var checksList = new List<Task>();
            var checkTriangle = CheckTriangle(farg);
            var checkQuadrangle = CheckQuadrangle(farg);

            checksList.Add(checkTriangle);
            checksList.Add(checkQuadrangle);
            Task.WaitAll(checksList.ToArray());

            if (String.IsNullOrEmpty(checkQuadrangle.Result) && String.IsNullOrEmpty(checkTriangle.Result))
                Console.WriteLine("nierozpoznano");
            else
            {
                if (!String.IsNullOrEmpty(checkQuadrangle.Result))
                    Console.Write(checkQuadrangle.Result+" ");
                if (!String.IsNullOrEmpty(checkTriangle.Result))
                    Console.Write(checkTriangle.Result + " ");
            }
        }

       static string[] ValidateInput(string[] arg)
        {
            string[] result = arg.Length < 3 || arg.Length > 4 ? null :arg;
            if (result == null)
            {
                Console.WriteLine("Zła liczba boków - min 3 , max 4");
                Environment.Exit(0);
            }

            if (result.Any(x => String.IsNullOrEmpty(x)))
            {
                Console.WriteLine("Zły format danych");
                Environment.Exit(0);
            }
            return result;
        }

       static IEnumerable<float> GetFloats(string[] sarg)
       {
            foreach(var s in sarg)
            {
                float f;
                if (!float.TryParse(s, out f))
                {
                    Console.WriteLine("Argumenty muszą być liczbami");
                    Environment.Exit(0);
                }
                else
                    yield return f;
            }
       }

        static public  Task<string> CheckTriangle(List<float> args)
        {
            return Task.Run(() =>
            {
                if (args.Count == 3)
                {
                    float a = args[0], b = args[1], c = args[2];

                    if ((a + b > c) )
                    {
                        if ((a == b) && (a == c) && (b == c))
                            return "Można zbudować trójkąt równoramienny i równoboczny";
                        else if ((a == b) || (b == c) || (c == a))
                            return "Można zbudować trójkąt równoramienny";

                        return "Można zbudować trójkąt różnoramienny";

                    }
                    else
                        return null;//Nie można zbudować trójkąta z podanych boków
                }
                else
                    return null;//Nie można zbudować trójkąta z czterech boków
            });
        }

        static public Task<string> CheckQuadrangle(List<float> args)
        {
            return Task.Run(() =>
            {
                if (args.Count == 4 && args.Max()<=args.Sum()-args.Max())
                {
                    string result = "";
                    float a = args[0], b = args[1], c = args[2], d = args[3];
                    if (a == b && b == c && c == d)
                        result += "Można zbudować kwadrat i prostokąt";
                    else if ((a == b && c == d) || (a == c && b == d) || (d == a && b == c))
                        result += "Można zbudować prostokąt";
                    return result + " Można zbudować czworobok";

                }
                else
                    return null;//Nie można zbudować czworoboku z trzech boków
            });
        }
    }
}
