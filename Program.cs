using System;

namespace Pallada
{
    class Key
    {
        public long AKey;
        public long BKey;

        private long MemoryA1;
        private long MemoryB1;
        private long MemoryA2;
        private long MemoryB2;
        private long Module;
        private int iterations;

        public Key(long A1, long B1, long A2, long B2, long Modul)
        {
            MemoryA1 = A1;
            MemoryB1 = B1;
            MemoryA2 = A2;
            MemoryB2 = B2;
            Module = Modul;
            AKey = A1;
            BKey = B1;
            iterations = 0;
        }

        public void RecalculateKeys()
        {
            if (iterations == 0)
            {
                AKey = MemoryA2;
                BKey = MemoryB2;
            }
            else
            {
                long tmp = AKey;
                MemoryA2 = MemoryA1;
                MemoryA1 = tmp;
                AKey = (MemoryA2 * MemoryA1) % Module;
                tmp = BKey;
                MemoryB2 = MemoryB1;
                MemoryB1 = tmp;
                BKey = (MemoryB2 + MemoryB1) % Module;
            }
            iterations++;
        }
    }
    class Program
    {
        static bool IsSimple(int number)
        {
            for (int i=2; i<number; i++){
                if ((number % i) == 0){
                    return false;
                }
            }
            return true;
        }

        static int FindSimpleMax(int StartNumber)
        {
            while (!IsSimple(StartNumber))
            {
                StartNumber--;
            }
            return StartNumber;
        }
        static long FindOpposition(long Number, long Module)
        {
            long x2 = 1;
            long x1 = 0;
            long y2 = 0;
            long y1 = 1;
            long q, r, x, y;
            long a = Module; long b = Number;
            while (b>0)
            {
                q = a / b;
                r = a - q * b;
                x = x2 - q * x1;
                y = y2 - q * y1;
                a = b; b = r;
                x2 = x1; x1 = x;
                y2 = y1; y1 = y;
            }
            if (a != 1)
            {
                return -1;
            }
            else
            {
                y = y2;
                if (y > 0)
                {
                    return y;
                }
                else return y + Module;
            }
        }

        static long EncryptionNumber(long X, long KeyA, long KeyB, long Module)
        {
            return (KeyA * X + KeyB) % Module;
        }

        static long DecriptionNumber(long X, long OposA, long KeyB, long Module)
        {
            long diff = (X - KeyB);
            while (diff<0)
            {
                diff += Module;
            }
            return (OposA * diff) % Module;
        }

        static void Main(string[] args)
        {
            int AlphabetPower = FindSimpleMax(Convert.ToInt32(Math.Pow(2, 16)));
            int KeyA, KeyB;
            string OpenText, ClosedText, InputText;

            Console.WriteLine("Affine code:");

            Console.WriteLine("Please, inter keys (First - key A, second - key B)...");
            InputText = Console.ReadLine();
            KeyA = Convert.ToInt32(InputText);
            InputText = Console.ReadLine();
            KeyB = Convert.ToInt32(InputText);

            Console.WriteLine("Inter text...");
            OpenText = Console.ReadLine();

            char[] Symbols = new char[OpenText.Length];
            for (int i=0; i<OpenText.Length; i++)
            {
                char Symbol = OpenText[i];
                Symbols[i] = (char)EncryptionNumber((long)Symbol, KeyA, KeyB, AlphabetPower);
            }
            ClosedText = new string(Symbols);

            Console.WriteLine("\nA = " + KeyA.ToString() + "\nB = " + KeyB.ToString());
            Console.WriteLine("\nEncrypted Text:\n" + ClosedText);

            long OppositionA = FindOpposition(KeyA, AlphabetPower);
            Console.WriteLine("\nA^(-1) = " + OppositionA.ToString());

            for (int i = 0; i < ClosedText.Length; i++)
            {
                char Symbol = ClosedText[i];
                Symbols[i] = (char)DecriptionNumber((long)Symbol, OppositionA, KeyB, AlphabetPower);
            }
            OpenText = new string(Symbols);

            Console.WriteLine("\nDecrypted Text:\n" + OpenText);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            Console.Clear();
            Console.WriteLine("Affine recurrent code:");

            int KeyA2, KeyB2;

            Console.WriteLine("Please, inter first part of keys (First - key A1, second - key B1)...");
            InputText = Console.ReadLine();
            KeyA = Convert.ToInt32(InputText);
            InputText = Console.ReadLine();
            KeyB = Convert.ToInt32(InputText);

            Console.WriteLine("Please, inter second part of keys (First - key A2, second - key B2)...");
            InputText = Console.ReadLine();
            KeyA2 = Convert.ToInt32(InputText);
            InputText = Console.ReadLine();
            KeyB2 = Convert.ToInt32(InputText);

            Key MyKeys = new Key(KeyA, KeyB, KeyA2, KeyB2, AlphabetPower);

            Console.WriteLine("Inter text...");
            OpenText = Console.ReadLine();

            Symbols = new char[OpenText.Length];
            for (int i = 0; i < OpenText.Length; i++)
            {
                char Symbol = OpenText[i];
                Console.WriteLine("NumberIteration = " + i.ToString() + "\tA = " + MyKeys.AKey.ToString() + ";\tB = " + MyKeys.BKey.ToString());
                Symbols[i] = (char)EncryptionNumber((long)Symbol, MyKeys.AKey, MyKeys.BKey, AlphabetPower);
                MyKeys.RecalculateKeys();
            }
            ClosedText = new string(Symbols);
            Console.WriteLine("\nEncrypted Text:\n" + ClosedText);

            MyKeys = new Key(KeyA, KeyB, KeyA2, KeyB2, AlphabetPower);

            Symbols = new char[ClosedText.Length];
            for (int i = 0; i < ClosedText.Length; i++)
            {
                char Symbol = ClosedText[i];
                OppositionA = FindOpposition(MyKeys.AKey, AlphabetPower);
                Console.WriteLine("NumberIteration = " + i.ToString() + "\tA^(-1) = " + OppositionA.ToString() + ";\tB = " + MyKeys.BKey.ToString());
                Symbols[i] = (char)DecriptionNumber((long)Symbol, OppositionA, MyKeys.BKey, AlphabetPower);
                MyKeys.RecalculateKeys();
            }
            OpenText = new string(Symbols);

            Console.WriteLine("\nDecrypted Text:\n" + OpenText);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
