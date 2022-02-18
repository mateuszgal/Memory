namespace Memory
{
    internal class Program
    {
        public static List<string> words = new();

        static void Main(string[] args)
        {
            StreamReader plik;
            Random random = new Random();
            plik = File.OpenText("C:\\Users\\Admin\\source\\repos\\Memory\\Data\\Words.txt");
            while (!plik.EndOfStream)
            {
                words.Add(plik.ReadLine());
            }
            Play();


        }
        public static void Play()
        {
            List<Word> firstRow = new List<Word>();
            Random random = new Random();
            List<string> wordsCopy = words;
            int chances;
            int num_of_pairs;
            int level = ChooseLevel();
            if (level == 1)
            {
                chances = 8;
                num_of_pairs = 4;
            }
            else
            {
                chances = 15;
                num_of_pairs = 8;
            }
            // level chosen
            // 
            int maxLengthofWord = 0;
            for (int i = 0; i < num_of_pairs; i++)
            {
                int k = random.Next(wordsCopy.Count());
                if (wordsCopy[k].Length > maxLengthofWord)
                {
                    maxLengthofWord = wordsCopy[k].Length;
                }
                Word randomWord = new Word(wordsCopy[k]);
                firstRow.Add(randomWord);
                firstRow.Add(new Word(wordsCopy[k]));
                wordsCopy.RemoveAt(k);
            }
            RandomizeList(ref firstRow);
            foreach (Word word in firstRow)
            {
                word.ExtendTo(maxLengthofWord);
            }
            string[] frame = MakeAFrame(num_of_pairs,maxLengthofWord);
            //LOGIC
            int punkty = 0;
            while (chances > 0)
            {
                int w1;
                int w2;
                Display(frame, firstRow, chances, maxLengthofWord);
                Console.WriteLine("");
                Console.WriteLine(" Enter the row (A or B) and the column you want to reveal (Example A1)");
                w1 = AskForCell(0, num_of_pairs, firstRow);
                firstRow[w1 - 1].Visible = true;
                Display(frame, firstRow, chances, maxLengthofWord);
                Console.WriteLine("Enter the second word to reveal");
                w2 = AskForCell(w1, num_of_pairs, firstRow);
                firstRow[w2 - 1].Visible = true;
                Display(frame, firstRow, chances, maxLengthofWord);

                if (firstRow[w1 - 1].Content == firstRow[w2 - 1].Content)
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
                    Display(frame, firstRow, chances, maxLengthofWord);
                    Thread.Sleep(1000);
                    firstRow[w1 - 1].Visible = false;
                    firstRow[w2 - 1].Visible = false;

                }
            }

            if (punkty == num_of_pairs)
            {
                Console.Clear();
                Console.WriteLine("Well done, you won, you have " + chances + " chances left!");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You lose");
            }
            if (AskForPlayingAgain())
            {
                Play();
            }
        }


        public static int ChooseLevel()
        {
            Random random = new Random();
            Console.WriteLine("select the difficulty level, type 1 for easy or  2 for hard");
            while (true)
            {
                string a = Console.ReadLine();
                if (a.Length > 0)
                { //plawecki to chuj
                    if (a[0] == '1')
                    {
                        return 1;
                    }
                    else if (a[0] == '2')
                    {
                        return 2;
                    }
                    else
                    {

                        Console.WriteLine("Enter the correct value!");
                    }
                }
                else
                {
                    Console.WriteLine("Enter the correct value!");
                }
            }
        }
        public static void RandomizeList(ref List<Word> list)
        {
            Random random = new Random();
            List<Word> listCopy = new List<Word>();
            foreach (Word word in list)
            {
                listCopy.Add(word);
            }
            while (list.Any())
            {
                list.RemoveAt(0);
            }
            while (listCopy.Count > 0)
            {
                int k = random.Next(listCopy.Count);
                list.Add(listCopy[k]);
                listCopy.RemoveAt(k);
            }
        }
        public static int AskForCell(int previousCell, int numOfPairs, List<Word> list)
        {
            int x = 0;
            while (true)
            {
                string answear = Console.ReadLine();
                answear.ToUpper();
                if (String.IsNullOrEmpty(answear))
                {
                    Console.WriteLine("Try again");
                }
                else if (answear.Length < 2)
                {
                    Console.WriteLine("You didn't enter a number");
                }
                else if (answear[0] == 'A')
                {
                    try
                    {
                        x = int.Parse(answear.Substring(1, 1));
                        if (x != previousCell)
                        {
                            if (!list[x - 1].Visible)
                            {
                                return x;
                            }
                        }
                        else
                        {
                            Console.WriteLine("You cannot choose the same word twice");
                        }

                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Try again");
                    }

                }
                else if (answear[0] == 'B')
                {
                    try
                    {
                        x = int.Parse(answear.Substring(1, 1));
                        if (x + numOfPairs != previousCell)
                        {
                            if (!list[x + numOfPairs - 1].Visible)
                            {
                                return x + numOfPairs;
                            }
                        }
                        else
                        {
                            Console.WriteLine("You cannot choose the same word twice");
                        }
                    }
                    catch (FormatException)
                    {

                    }
                }
                else
                {
                    Console.WriteLine("Try again");
                }

            }


            return 4;
        }
        public static void Display(string[] frame, List<Word> list, int chances, int maxLengthofWord)
        {
            string mask = "";
            int i = 0;
            while (i < maxLengthofWord)
            {
                mask += "X";
                i++;
            }
            Console.Clear();
            Console.WriteLine("You have " + chances + " chances left!");
            Console.WriteLine("");
            Console.Write("  ");
            foreach (String piece in frame)
            {
                Console.Write(piece + " ");
            }
            Console.WriteLine("");
            Console.Write("A ");
            i = 0;
            while (i < (list.Count() / 2))
            {

                if (list[i].Visible)
                {
                    Console.Write(list[i].Content + " ");
                }
                else
                {
                    Console.Write(mask + " ");
                }
                i++;
            }
            Console.WriteLine("");
            Console.Write("B ");
            while (i < list.Count())
            {

                if (list[i].Visible)
                {
                    Console.Write(list[i].Content + " ");
                }
                else
                {
                    Console.Write(mask + " ");
                }
                i++;
            }

        }
        public static string[] MakeAFrame(int num_of_pairs, int maxLengthofWord)
        {   string[] frame = new string[num_of_pairs];
            for (int i = 1; i <= num_of_pairs; i++)
            {

                frame[i - 1] = i + "";
                for (int j = 0; j < maxLengthofWord - 1; j++)
                {

                    frame[i - 1] = frame[i - 1] + " ";
                }
            }
            return frame;
        }
        public static bool  AskForPlayingAgain()
        {
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
                        return true;
                    }
                    else if (b == "N")
                    {
                        return false;
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

    }

}