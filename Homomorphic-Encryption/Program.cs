using System;
using System.Numerics;

namespace Homomorphic_Encryption
{
    internal static class Program
    {
        private static BigInteger _privateKey;
        private static BigInteger[] _publicKey;
        
        private const string Value1 = "5";
        private const string Value2 = "3";

        public static void Main(string[] args)
        {
            Console.WriteLine("Initial data: " + Value1 + ", " + Value2);
            Keygen();

            var first = Encrypt(Value1);
            var second = Encrypt(Value2);
            
            Console.WriteLine("Encrypted data: " + first + ", " + second);

            var multiplication = Multiplication(first, second);
            Console.WriteLine("Multiplication: " + multiplication);
            Console.WriteLine("Decrypted multiplication: " + Decrypt(multiplication));
            
            var summation = Summation(first, second);
            Console.WriteLine("Summation: " + summation);
            Console.WriteLine("Decrypted summation: " + Decrypt(summation));
            
            Console.WriteLine("Decrypted data: " + Decrypt(first) + ", " + Decrypt(second));
        }

        private static void Keygen()
        {
            var rnd = new Random();
            var rand = new byte[16];
            _publicKey = new BigInteger[100];

            do
            {
                rnd.NextBytes(rand);
                _privateKey = new BigInteger(rand);
                _privateKey = BigInteger.Abs(_privateKey);
            } while (BigInteger.GreatestCommonDivisor(_privateKey, 1000000) != 1);

            for (var i = 0; i < 100; i++)
            {
                rnd.NextBytes(rand);
                _publicKey[i] = new BigInteger(rand);
                _publicKey[i] = BigInteger.Abs(_publicKey[i]) * _privateKey + 1000000 * rnd.Next(10, 100);
            }
        }
        
        private static string Encrypt(string value)
        {
            BigInteger.TryParse(value, out var e);
            var t = new BigInteger(0);
            var rand = new Random();

            for (var i = 0; i < 100; i++)
            {
                if (rand.Next(2) == 1)
                {
                    t += _publicKey[i];
                }
            }

            return (e + t).ToString();
        }
        
        private static string Decrypt(string value)
        {
            BigInteger.TryParse(value, out var e);
            var m = BigInteger.Remainder(e, _privateKey);

            return BigInteger.Remainder(m, 1000000).ToString();
        }
        
        private static string Summation(string first, string second)
        {
            BigInteger.TryParse(first, out var a);
            BigInteger.TryParse(second, out var b);

            return (a + b).ToString();
        }
        
        private static string Multiplication(string first, string second)
        {
            BigInteger.TryParse(first, out var a);
            BigInteger.TryParse(second, out var b);

            return (a * b).ToString();
        }
    }
}