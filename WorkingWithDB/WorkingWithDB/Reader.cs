using DnsClient.Protocol;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace WorkingWithDB
{
    enum ReadResult
    {
        Succesfull,
        Menu,
        End,
        Error
    }
    class Reader
    {
        public delegate bool Question<T>(T? readline);
        public static bool Try<T>(out T result)
            {
            ReadResult readLineResult;
            do
            {
                readLineResult = Reader.ReadWhile(out result);
                if (readLineResult == ReadResult.Succesfull)
                {
                    return true;
                }
                Console.WriteLine($"Invalid Value {typeof(T)}");
            }
            while (readLineResult == ReadResult.Error);
            return false; //ReadResult.End
        }
        public static void TryWhile<T>(out T result)
        {
            var readLineResult = Reader.ReadWhile(out result);
            while (readLineResult != ReadResult.Succesfull)
            {
                Console.WriteLine($"Invalid Value {typeof(T)}");
                readLineResult = Reader.ReadWhile(out result);
            }
        }



        public static ReadResult ReadWhile<T>(out T result)
        {
            string? s = Console.ReadLine();
            switch (s)
            {
                case null:
                    result = default;
                    return ReadResult.Error;
                case "end":
                    result = default;
                    return ReadResult.End;
                case "menu":
                    result = default;
                    return ReadResult.Menu;
                default:
                    try
                    {
                        result = (T)Convert.ChangeType(s, typeof(T));
                        return ReadResult.Succesfull;
                    }
                    catch
                    {
                        result = default;
                        return ReadResult.Error;
                    }
            }
        }

        public static bool Try<T>(out T result, Question<T> question)
        {
            ReadResult readLineResult;
            do
            {
                readLineResult = Reader.ReadWhile(out result);
                if (readLineResult == ReadResult.Succesfull && question(result))
                {
                    return true;
                }
                Console.WriteLine("Invalid Value");
            }
            while (readLineResult == ReadResult.Error || !question(result));
            return false; //ReadResult.End
        }
    }
}