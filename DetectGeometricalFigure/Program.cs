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
            List<float> farg = new List<float>();
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
                    Console.Write(checkQuadrangle.Result);
                if (!String.IsNullOrEmpty(checkTriangle.Result))
                    Console.Write(checkTriangle.Result);
            }
        }

       static string[] ValidateInput(string[] arg)
        {
            string[] result = arg.Length < 3 || arg.Length > 4 ? null :arg;
            if (result == null)
            {
                Console.WriteLine("nierozpoznano");
                Environment.Exit(0);
            }

            if (result.Any(x => String.IsNullOrEmpty(x)))
            {
                Console.WriteLine("nierozpoznano");
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
                    Console.WriteLine("nierozpoznano");
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
                            return "trojkat_rownoboczny";
                        else if ((a == b) || (b == c) || (c == a))
                            return "trojkat_rownoramienny";

                        return "trojkat_roznoramienny";

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
                    float a = args[0], b = args[1], c = args[2], d = args[3];
                    if (a == b && b == c && c == d)
                        return "kwadrat";
                    else if ((a == b && c == d) || (a == c && b == d) || (d == a && b == c))
                        return "prostokat";
                    return "czworobok";

                }
                else
                    return null;//Nie można zbudować czworoboku z trzech boków
            });
        }
    }
}
