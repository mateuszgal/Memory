using System;

namespace Memory
{
    internal class Program
    {
        public static string[] words = new string[100];

        static void Main(string[] args)
        {

            StreamReader plik;
            //string[] words = new string[100];
            Random random = new Random();
            plik = File.OpenText("C:\\Users\\Admin\\source\\repos\\Memory3.0\\Data\\Words.txt");

           // while (plik.ReadLine) {
            //}

            for (int i = 0; i < 100; i++) 
            {
                words[i] = plik.ReadLine();
            }
            Play();


        }
        public static void Play()
        {

            int chances;
            int num_of_pairs;
            Random random = new Random();
            // wybieranie poziomu trudności
            Console.WriteLine("wybierz poziom trudnosci, 1-easy 2-hard");
            while (true)
            {
                string a = Console.ReadLine();
                if (a.Length > 0)
                {
                    if (a[0] == '1')
                    {
                        chances = 10;
                        num_of_pairs = 4;
                        break;
                    }
                    else if (a[0] == '2')
                    {
                        chances = 15;
                        num_of_pairs = 8;
                        break;
                    }
                    else
                    {

                        Console.WriteLine("podaj poprawną wartośc!");
                    }
                }
                else
                {
                    Console.WriteLine("podaj poprawną wartośc!");
                }
            }

            List<Word> firstRow = new List<Word>();
            List<Word> secondRow = new List<Word>();
            List<Word> temporary = new List<Word>();
            // tworzenie listy numerów 

            var numbers = new List<int>();
            for (int i = 0; i < 100; i++)
            {
                numbers.Add(i);

            }// WYJEBAĆ I LOSOWAC SŁOWA Z LISTY


            // Losowanie słów
            for (int i = 0; i < num_of_pairs; i++)
            {
                int k = random.Next(words.Length - 1 - i);

                Word randomWord = new Word(words[numbers[k]]);
                firstRow.Add(randomWord);
                temporary.Add(randomWord);
                numbers.RemoveAt(k);

            }

            // Losowanie kolejności drugiej listy słów

            // na listach  i obiektach mieszanie i szukanie najdłuższego

            int maxLengthofWord = 0;

            while (firstRow.Count() > secondRow.Count())
            {
                int k;
                k = random.Next(temporary.Count());
                if (temporary[k].content.Length > maxLengthofWord)
                {

                    maxLengthofWord = temporary[k].content.Length;
                }
                if (!secondRow.Contains(temporary[k]))
                {
                    secondRow.Add(new Word(temporary[k].content));
                    temporary.RemoveAt(k);
                }

            }

            // 8 słow

            //wydłuzanie i maskowanie
            for (int i = 0; i < firstRow.Count(); i++)
            {
                firstRow[i].ExtendTo(maxLengthofWord);
                secondRow[i].ExtendTo(maxLengthofWord);

            }

            // Stowrzenie górnego wiersza z numerami
            string[] frame = new string[num_of_pairs];
            for (int i = 1; i <= num_of_pairs; i++)
            {

                frame[i - 1] = i + "";
                for (int j = 0; j < maxLengthofWord - 1; j++)
                {

                    frame[i - 1] = frame[i - 1] + " ";
                }
            }
            //LOGIC
            int punkty = 0;
            while (chances > 0)
            {
                Console.Clear();
                Console.WriteLine("Liczba pozostałych szans= " + chances);
                Display(frame, "  ");
                Display(firstRow, "A ");
                Display(secondRow, "B ");
                Console.WriteLine("");
                Console.WriteLine(" Wpisz wiersz (A lub B) oraz kolumnę którą chcesz odsłonić");
                int w1 = new int();
                int w2 = new int();
                int chosenRow = 0;
                bool isr1 = false;
                bool isr2 = false;
                bool iscorrect;
                while (!(isr2 && isr1))
                {
                    int x;
                    iscorrect = false;
                    Console.Clear();
                    Console.WriteLine("Liczba pozostałych szans= " + chances);
                    Display(frame, "  ");
                    Display(firstRow, "A ");
                    Display(secondRow, "B ");
                    Console.WriteLine("");
                    Console.WriteLine(" Wpisz wiersz (A lub B) oraz kolumnę którą chcesz odsłonić");
                    while (true)
                    {
                        string odp = Console.ReadLine();
                        if (!String.IsNullOrEmpty(odp))
                        {
                            odp = odp.ToUpper();

                            if (odp[0] == 'A')
                            {
                                if (!isr1)
                                {
                                    iscorrect = true;
                                    chosenRow = 1;
                                }
                                else
                                {
                                    Console.WriteLine("Wybierz drugi wiersz");
                                    //Thread.Sleep(1000);
                                }

                            }
                            else if (odp[0] == 'B')
                            {

                                if (!isr2)
                                {
                                    iscorrect = true;
                                    chosenRow = 2;
                                }
                                else
                                {
                                    Console.WriteLine("Wybierz drugi wiersz");
                                }

                            }
                            else
                            {
                                Console.WriteLine("Nie rozpoznałem wiersza, spróbuj ponownie");
                            }

                            if (iscorrect)
                            {

                                try
                                {
                                    if (odp.Length >= 2)
                                    {
                                        x = Int32.Parse(odp[1].ToString());
                                    }
                                    else
                                    {
                                        x = 99;
                                    }
                                    if (x <= num_of_pairs)
                                    {
                                        if (chosenRow == 1)
                                        {

                                            if (firstRow[x - 1].IsShhown())
                                            {
                                                Console.WriteLine("Słowo jest już odsłonione, podaj poprawną wartość");
                                            }
                                            else
                                            {
                                                firstRow[x - 1].Show();
                                                w1 = x;
                                                isr1 = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (secondRow[x - 1].IsShhown())
                                            {
                                                Console.WriteLine("Słowo jest już odsłonione, podaj poprawną wartość");
                                            }
                                            else
                                            {
                                                secondRow[x - 1].Show();
                                                w2 = x;
                                                isr2 = true;

                                                break;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("Wartośc z poza zakresu");
                                    }
                                }

                                catch (FormatException e)
                                {
                                    Console.WriteLine("Podaj poprawną wartość");
                                }
                            }
                        }
                        else
                        {

                            Console.WriteLine("Podaj poprawną wartośc");
                        }
                    }
                }


                if (firstRow[w1 - 1] == secondRow[w2 - 1])
                {
                    punkty++;
                    if (punkty == num_of_pairs)
                    {
                        break;
                    }
                }
                else
                {
                    chances--;
                    Console.Clear();
                    Console.WriteLine("Liczba pozostałych szans= " + chances);
                    Display(frame, "  ");
                    Display(firstRow, "A ");
                    Display(secondRow, "B ");
                    Thread.Sleep(1000);
                    firstRow[w1 - 1].Hide();
                    secondRow[w2 - 1].Hide();

                }
            }

            if (punkty == num_of_pairs)
            {
                Console.Clear();
                Console.WriteLine("Brawo, wygrałeś, pozostało Ci " + chances + " szans!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Ale frajer!");
            }
            Console.WriteLine("Chcesz zgarać ponownie? wpisz Y(yes) lub N(no)");
            string b;
            while (true)
            {
                b = Console.ReadLine();
                if (b.Length > 0)
                {
                    b = b.ToUpper();
                    if (b == "Y")
                    {
                        Play();
                        break;
                    }
                    else if (b == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Podaj odpowiedź");
                    }
                }
                else
                {
                    Console.WriteLine("podaj odpowiedź");
                }
            }


        }
        /* public static void Display(string[] table)
         {
             Console.WriteLine("");
             for (int i = 0; i < table.Length; i++)
             {
                 Console.Write(table[i] + " ");
             }

         }*/
        public static void Display(List<Word> table, string before)
        {
            Console.WriteLine("");
            Console.Write(before);
            int i = 0;
            while (i < table.Count())
            {
                table[i].Display();
                i++;
            }
        }
        public static void Display(string[] table, string before)
        {
            Console.WriteLine("");
            Console.Write(before);
            for (int i = 0; i < table.Length; i++)
            {
                Console.Write(table[i] + " ");
            }

        }
        /* public static void Cover(ref String[] pairsToModify, int pos, string mask)
         {
             pairsToModify[pos - 1] = mask;
         } */
        /* public static bool TrytoUncover(ref String[] pairsToModify, int pos, string mask, string[] pairs)
         {
             if (pairsToModify[pos - 1] == mask)
             {
                 pairsToModify[pos - 1] = pairs[pos - 1];
                 while (pairsToModify[pos - 1].Length < mask.Length)
                 {
                     pairsToModify[pos - 1] += " ";
                 }
                 return true;
             }
             return false;
         }*/
    }
    class Word
    {
        bool visible;
        public string content;
        string mask;
        public Word()
        {
            visible = false;
            mask = "";
        }
        public Word(string text)
        {
            content = text;
            visible = false;
            mask = "";
        }
        public bool IsShhown()
        {
            return visible;
        }
        public void Hide()
        {
            visible = false;
        }
        public void Show()
        {
            visible = true;
        }
        public void ExtendTo(int x)
        {
            while (content.Length < x)
            {
                content += ' ';
            }
            while (mask.Length < x)
            {
                mask += "X";
            }

        }
        public void Display()
        {
            if (visible) { Console.Write(content + " "); }
            else
            {
                Console.Write(mask + " ");
            }

        }
        public static bool operator ==(Word a, Word b)
        {
            return (a.content == b.content);
        }
        public static bool operator !=(Word a, Word b)
        {
            return (a.content != b.content);
        }
    }

}