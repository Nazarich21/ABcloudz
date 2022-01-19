using System;
using System.Collections.Generic;
using System.Numerics;


namespace Sum_of_sequence
{
    public class Exponent
    {
        private int _exponent { get; set; }
        private int _afterDot { get; set; }//length of mantisa after dot
        private string _mantisa { get; set; }//without dot
        private string _sign { get; set; }

        public static Exponent Zero
        {
            get => new Exponent("0.0e0");
        }
        public Exponent(Exponent ex)
        {
            _mantisa = ex._mantisa;
            _exponent = ex._exponent;
            _afterDot = ex._afterDot;
            _sign = ex._sign;

        }
        public Exponent()
        {
            _mantisa = null;
            _exponent = 0;
            _afterDot = 0;
            _sign = null;
        }


        public Exponent(string input)
        {
            string input2 = input.Remove(1, 1);
            char[] charArr = input.ToCharArray();
            int expPlace = 0;
            int dotPlace = 0;
            int sign1 = 0;
            int plus = 0;
            int minus = 0;
            for (int i = 0; i < charArr.Length; i++)
            {
                if (charArr[i].ToString() == "e")
                {
                    expPlace = i;
                }
                else if (charArr[i].ToString() == "." || charArr[i].ToString() == ",")
                {
                    dotPlace = i;
                }
                else if (charArr[i].ToString() == "+")
                {
                    plus = i;
                    sign1 = i;
                }
                else if (charArr[i].ToString() == "-")
                {
                    minus = i;
                    sign1 = i;
                }
                else if (Char.IsLetter(charArr[i]))
                {
                    throw new InvalidOperationException();
                }

            }

            if (dotPlace != 1)
            {
                throw new InvalidOperationException();
            }

            if (sign1 == 0)
            {
                _sign = "+";
            }
            else
            {
                _sign = input.Substring(sign1, 1);
            }

            _afterDot = input.Substring(dotPlace, expPlace - dotPlace - 1).Length;
            _mantisa = input.Substring(0, expPlace);

            if (plus > 0)
            {
                _exponent = Convert.ToInt32(input2.Substring(expPlace + 1));
            }
            else if (minus > 0)
            {
                _exponent = Convert.ToInt32(input2.Substring(expPlace + 1));
            }
            else
            {
                _exponent = Convert.ToInt32(input2.Substring(expPlace));
            }

        }


        public static Exponent Sum(Exponent  ex1, Exponent ex2)
        {
            Exponent ex1m = (Exponent)ex1.MemberwiseClone();
            Exponent ex2m = (Exponent)ex2.MemberwiseClone();

            string num1 = Exponent.ToZeroExponent(ex1m);
            string num2 = Exponent.ToZeroExponent(ex2m);

            List<char[]> resList1 = Exponent.BeforeAfterDotSearch(num1);
            List<char[]> resList2 = Exponent.BeforeAfterDotSearch(num2);

            int ab = 0;
            string mantisAfterDotSum = Exponent.SumCharArrayAfter(resList1[1], resList2[1], ref ab);
            string mantisBedoreDotSum = Exponent.SumCharArrayBefore(resList1[0], resList2[0], ref ab);
            string res = mantisBedoreDotSum + mantisAfterDotSum;

            var a = new Exponent();
            if (mantisBedoreDotSum == "0")
            {
                int exponent = 1;
                while (mantisAfterDotSum.StartsWith("0"))
                {
                    mantisAfterDotSum = mantisAfterDotSum.Substring(1);
                    exponent++;
                }
                a._sign = "-";
                a._exponent = exponent;
                a._mantisa = mantisAfterDotSum.Insert(1, ".");
                a._afterDot = (a._mantisa.Length) - 2;

            }
            else if (mantisAfterDotSum == "")
            {
                a._sign = "+";
                a._mantisa = mantisBedoreDotSum.Insert(1, ".");
                a._exponent = (mantisBedoreDotSum.Length) - 1;
                while (a._mantisa.EndsWith("0") && a._mantisa.Length > 3)
                {
                    a._mantisa = a._mantisa.Substring(0, (a._mantisa.Length) - 1);
                }
                a._afterDot = (a._mantisa.Length) - 2;
            }
            else
            {
                a._exponent = (mantisBedoreDotSum.Length) - 1;
                a._sign = "+";
                while (res.EndsWith("0") && res.Length > 2)
                {
                    res = res.Substring(0, (res.Length) - 1);
                }

                a._mantisa = res.Insert(1, ".");
                a._afterDot = (a._mantisa.Length) - 2;
            }

            return a;
        }
        public override string ToString()
        {
            if (_sign != null)
            {
                if (_mantisa.Length > 41)
                    _mantisa = _mantisa.Substring(0,41);

                return _mantisa + "e" + _sign + _exponent;
            }
            else
            {
                if (_mantisa.Length > 41)
                    _mantisa = _mantisa.Substring(0,41);

                return _mantisa + _sign + _exponent;
            }

        }
        public static List<char[]> BeforeAfterDotSearch(string s1)
        {
            var arr1 = s1.ToCharArray();
            List<char[]> resList = new List<char[]>();

            char[] arr1BeforeDot;
            char[] arr1AfterDot;

         
            if (Array.Exists(arr1, element => element == (char)46))
            {
                int dotPlace = 0;
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (arr1[i].ToString() == ".")
                    {
                        dotPlace = i;
                    }

                }
                arr1BeforeDot = new char[dotPlace];
                Array.Copy(arr1, 0, arr1BeforeDot, 0, dotPlace);
                arr1AfterDot = new char[arr1.Length - dotPlace - 1];
                Array.Copy(arr1, dotPlace + 1, arr1AfterDot, 0, arr1.Length - dotPlace - 1);
                resList.Add(arr1BeforeDot);
                resList.Add(arr1AfterDot);
            }
            else
            {
                arr1BeforeDot = arr1;
                arr1AfterDot = null;
                resList.Add(arr1BeforeDot);
                resList.Add(null);
            }
            return resList;




        }



        public static string SumCharArrayBefore(char[] arr1, char[] arr2, ref int ab)
        {
            string s1 = new string(arr1);
            string s2 = new string(arr2);
            BigInteger number1 = BigInteger.Parse(s1);
            BigInteger number2 = BigInteger.Parse(s2);
            number1 += number2;
            number1 += ab;
            return number1.ToString();
        }


        public static string SumCharArrayAfter(char[] arr1, char[] arr2, ref int ab)
        {
            string s1 = new string(arr1);
            string s2 = new string(arr2);

            if (s1.Length == 0)
            {
                return s2;

            }
            else if (s2.Length == 0)
            {
                return s1;
            }
            else if (s1.Length == 0 && s2.Length == 0)
            {
                return null;
            }
            else
            {
                if (s2.Length > s1.Length)
                {
                    int b = s1.Length;
                    for (int a = 0; a < s2.Length - b; a++)
                    {
                        s1 = s1.Insert(s1.Length, "0");
                    }

                    s1 = Exponent.StringAdd(s1, s2);

                    if (s1.Length > s2.Length)
                    {
                        s1 = s1.Substring(1);
                        ab = 1;
                        return s1;
                    }
                    else
                    {
                        return s1;
                    }
                }
                else
                {
                    int b = s2.Length;
                    for (int a = 0; a < s1.Length - b; a++)
                    {
                        s2 = s2.Insert(s2.Length, "0");
                    }

                    s1 = Exponent.StringAdd(s1, s2);

                    if (s1.Length > s2.Length)
                    {
                        s1 = s1.Substring(1);
                        ab = 1;
                        return s1;
                    }
                    else
                    {
                        return s1;
                    }
                }
            }
        }


        public static string StringAdd(string s1, string s2)
        {

            BigInteger number1 = BigInteger.Parse(s1);
            BigInteger number2 = BigInteger.Parse(s2);
            number1 += number2;
            string res = number1.ToString();
            if (res.Length < s1.Length)
            {
                while (res.Length != s1.Length)
                {
                    res = res.Insert(0, "0");
                }
            }

            return res;
        }


        public static string ToZeroExponent(Exponent ex)
        {
            if (ex._sign == "+" || ex._sign == null)
            {
                if (ex._exponent == 0)
                {
                    return ex._mantisa;
                }
                else if (ex._exponent > ex._afterDot)
                {
                    ex._mantisa = ex._mantisa.Remove(1, 1);
                    for (int a = ex._exponent - ex._afterDot; a > 0; a--)
                    {
                        ex._mantisa += "0";
                    }
                    return ex._mantisa;
                }
                else if (ex._exponent == ex._afterDot)
                {
                    ex._mantisa = ex._mantisa.Remove(1, 1);
                    return ex._mantisa;
                }
                else
                {
                    var arr = ex._mantisa.ToCharArray();
                    int dot = arr.Length - ex._afterDot - 1;
                    for (int a = 0; a < ex._exponent; a++)
                    {
                        char dotInArr = arr[dot];
                        arr[dot] = arr[dot + 1];
                        arr[dot + 1] = dotInArr;
                        dot++;
                    }
                    return new string(arr);
                }
            }
            else
            {
                if (ex._exponent == 1 && ex._sign == "-")
                {
                    ex._mantisa = ex._mantisa.Remove(1, 1);
                    ex._mantisa = "0." + ex._mantisa;
                    while (int.Parse(ex._mantisa[(ex._mantisa.Length - 1)].ToString()) == 0)
                    {
                        ex._mantisa = ex._mantisa.Substring(0, ex._mantisa.Length - 1);
                    }
                    return ex._mantisa;
                }
                else if (ex._exponent == 0 && ex._sign == "-")
                {
                    while (int.Parse(ex._mantisa[(ex._mantisa.Length - 1)].ToString()) == 0)
                    {
                        ex._mantisa = ex._mantisa.Substring(0, ex._mantisa.Length - 1);
                    }
                    return ex._mantisa;
                }
                else
                {
                    ex._mantisa = ex._mantisa.Remove(1, 1);
                    string before = "0.";
                    for (int a = 0; a < ex._exponent - 1; a++)
                    {
                        before += "0";
                    }
                    while (int.Parse(ex._mantisa[(ex._mantisa.Length - 1)].ToString()) == 0)
                    {
                        ex._mantisa = ex._mantisa.Substring(0, ex._mantisa.Length - 1);
                    }
                    ex._mantisa = before + ex._mantisa;
                    return ex._mantisa;
                }
            }
        }


        public static Exponent operator +(Exponent ex1, Exponent ex2)
        {
            Exponent res = new Exponent();
            res = Exponent.Sum( ex1,  ex2);
            return res;
        }
    }
}
