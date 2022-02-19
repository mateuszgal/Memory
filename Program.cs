namespace Memory
{
    internal class Program
    {
        public static List<string> words = new();

        static void Main(string[] args)
        {
            StreamReader plik;
            Random random = new Random();
            plik = File.OpenText("Data\\Words.txt");
            while (!plik.EndOfStream)
            {
                words.Add(plik.ReadLine());
            }
            plik.Close();
            Play();


        }
        public static void Play()
        {               
            List<Result> highscoresEasy = new List<Result>();
            List<Result> highscoresHard = new List<Result>();
            StreamReader file;
            file = File.OpenText("Data\\Highscores.txt");
            while (!file.EndOfStream)
            {
               string[] table= file.ReadLine().Split('|');
                Result a = new Result(table);
                if (a.Level == "easy")
                {
                    highscoresEasy.Add(a);
                }
                else
                {
                    highscoresHard.Add(a);
                }
            }
            file.Close();

            DateTime start = DateTime.Now;
            int chances;
            int num_of_pairs;
            int level = ChooseLevel();
            string levelName;
            if (level == 1)
            {
                chances = 8;
                num_of_pairs = 4;
                levelName = "easy";
            }
            else
            {
                chances = 15;
                num_of_pairs = 8;
                levelName = "hard";
            }
            // level is chosen

            List<Word> pairsOfwords = GenerateRandomizedList(num_of_pairs);
            int maxLengthofWord = LengthOfLongestWord(pairsOfwords);
            string frame = MakeAFrame(num_of_pairs, maxLengthofWord);
            //LOGIC
            int points = 0;
            int tries = 0;
            while (chances > 0)
            {
                int w1;
                int w2;
                Display(frame, pairsOfwords, chances, maxLengthofWord);
                Console.WriteLine("\nEnter the row (A or B) and the column you want to reveal (Example A1)");
                w1 = AskForCell(0, num_of_pairs, pairsOfwords);
                Display(frame, pairsOfwords, chances, maxLengthofWord);
                Console.WriteLine("\nEnter the second word to reveal");
                w2 = AskForCell(w1, num_of_pairs, pairsOfwords);
                Display(frame, pairsOfwords, chances, maxLengthofWord);
                tries++;
                if (pairsOfwords[w1 - 1].Content == pairsOfwords[w2 - 1].Content)
                {
                    points++;
                    if (points == num_of_pairs)
                    {
                        break;
                    }
                }
                else
                {
                    chances--;
                    Display(frame, pairsOfwords, chances, maxLengthofWord);
                    Thread.Sleep(1000);
                    pairsOfwords[w1 - 1].Visible = false;
                    pairsOfwords[w2 - 1].Visible = false;

                }
            }

            if (points == num_of_pairs)
            {
                Console.Clear();
                Console.WriteLine("Well done, you won, you have " + chances + " chances left!");
                DateTime end = DateTime.Now;
                TimeSpan time = end - start;
                Console.WriteLine("It took you: " + (double)(time.TotalMilliseconds) / 1000 + " seconds");
                DateOnly date = DateOnly.FromDateTime(DateTime.Now);
                string name = AskForName();
                Result result = new Result(name, date, time.TotalMilliseconds, tries, levelName);
                if (level == 1)
                {
                    CheckIfHighscore(result, ref highscoresEasy);
                    DisplayHighscores(highscoresEasy);
                }
                else
                {
                    CheckIfHighscore(result, ref highscoresHard);
                    DisplayHighscores(highscoresHard);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You lose");
                if (level == 1)
                {
                    DisplayHighscores(highscoresEasy);
                }
                else
                {
                    DisplayHighscores(highscoresHard);
                }
            }
            SaveHighscores(highscoresEasy,highscoresHard);
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
                { 
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
        public static int AskForCell(int previousCell, int numOfPairs, List<Word> list)
        {
            int x = 0;
            while (true)
            {
                string answear = Console.ReadLine();
                answear = answear.ToUpper();
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
                                list[x - 1].Visible = true;
                                return x;
                            }
                            else
                            {
                                Console.WriteLine("This field has already been exposed");
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
                                list[x + numOfPairs - 1].Visible=true;
                                return x + numOfPairs;
                            }
                            else
                            {
                                Console.WriteLine("This field has already been exposed");
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
        }
        public static void Display(string frame, List<Word> list, int chances, int maxLengthofWord)
        {
            int i = 0;

            Console.Clear();
            Console.WriteLine("You have " + chances + " chances left!"+"\n");
            Console.Write(frame+"\n");
            Console.Write("A ");
            while (i < (list.Count() / 2))
            {

                if (list[i].Visible)
                {
                    Console.Write(list[i].Content.PadRight(maxLengthofWord,' ') + " ");
                }
                else
                {
                    Console.Write("X".PadRight(maxLengthofWord, 'X') + " ");
                }
                i++;
            }

            Console.Write("\nB ");
            while (i < list.Count())
            {

                if (list[i].Visible)
                {
                    Console.Write(list[i].Content.PadRight(maxLengthofWord, ' ') + " ");
                }
                else
                {
                    Console.Write("X".PadRight(maxLengthofWord, 'X') + " ");
                }
                i++;
            }

        }
        public static string MakeAFrame(int num_of_pairs, int maxLengthofWord)
        {   string frame = "  ";
            for (int i = 1; i <= num_of_pairs; i++)
            {

                frame  += i + " ";
                for (int j = 0; j < maxLengthofWord - 1; j++)
                {

                    frame += " ";
                }
            }
            return frame;
        }
        public static bool  AskForPlayingAgain()
        {
            Console.WriteLine("Do you want to play again? Type Y for Yes or N for No");
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
                        Console.WriteLine("Type answear");
                    }
                }
                else
                {
                    Console.WriteLine("Type answear");
                }
            }
        }
        public static List<Word> GenerateRandomizedList(int pairs)
        {
            Random random = new Random();
            List<Word> listOfwords = new List<Word>();
            List<string> wordsCopy = words;
            for (int i = 0; i < pairs; i++)
            {
                int k = random.Next(wordsCopy.Count());
                Word randomWord = new Word(wordsCopy[k]);
                listOfwords.Add(randomWord);
                listOfwords.Add(new Word(wordsCopy[k]));
                wordsCopy.RemoveAt(k);
            }
            List<Word> listCopy = new List<Word>();
            foreach (Word word in listOfwords)
            {
                listCopy.Add(word);
            }
            while (listOfwords.Any())
            {
                listOfwords.RemoveAt(0);
            }
            while (listCopy.Count > 0)
            {
                int k = random.Next(listCopy.Count);
                listOfwords.Add(listCopy[k]);
                listCopy.RemoveAt(k);
            }
            return listOfwords;
        }
        public static int LengthOfLongestWord(List<Word> list)
        {
            int max=0;
            foreach(Word word in list)
            {
                if(word.Content.Length > max)
                {
                    max = word.Content.Length;
                }
            }
            return max;

        }
        public static string AskForName()
        {
            string name = "";
            Console.WriteLine("What's your name?");
            while (true)
            {
                name = Console.ReadLine();
                if (!String.IsNullOrEmpty(name))
                {
                    return name;
                }
                Console.WriteLine("Try again");


            }
        }
        public static void CheckIfHighscore(Result result, ref List<Result> highscore) 
        {
            int i = 0;
            while(i < highscore.Count())
            {
                if (result.Time<highscore[i].Time)
                {
                    break;
                }
                i++;
            }
            if (highscore.Count >= 10) {
                highscore.Insert(i, result);
                highscore.RemoveAt(highscore.Count() - 1);
            }
            else
            {
                highscore.Insert(i, result);
            }
            
        }
        public static void DisplayHighscores(List<Result> highscores)
        {
            int maxName=4;
            int maxDate=4;
            int maxTime=4;
            int maxTries=5;
            foreach (Result result in highscores)
            {
                if (result.NameOfplayer.Length > maxName)
                {
                    maxName = result.NameOfplayer.Length;
                }
                if (result.DateOfresult.ToString().Length > maxDate)
                {
                    maxDate=result.DateOfresult.ToString().Length;
                }
                if ((result.Time/1000).ToString().Length > maxTime) 
                { 
                    maxTime = (result.Time/1000).ToString().Length;
                }
            }
            Console.WriteLine("Name".PadRight(maxName, ' ') + "|"+"date".PadRight(maxDate, ' ') + "|"+"time".PadRight(maxTime, ' ') + "|"+"tries");
            foreach(Result result in highscores)
            {
                Console.WriteLine(result.NameOfplayer.PadRight(maxName, ' ') + "|" + result.DateOfresult.ToString().PadRight(maxDate, ' ') + "|" + ((result.Time)/1000).ToString().PadRight(maxTime, ' ') + "|" + result.Tries.ToString().PadRight(maxTries, ' ') + '|' + result.Level);
            }
            Console.Write("\n");

        }
        public static void SaveHighscores(List<Result> highscores1,List<Result> highscores2 )
        {
            using StreamWriter file = new("Data\\Highscores.txt");
            foreach(Result result in highscores1)
            {
                file.WriteLine(result.NameOfplayer + "|" + result.DateOfresult.ToString() + "|" + result.Time + "|" + result.Tries + '|' + result.Level);
            }
            foreach (Result result in highscores2)
            {
                file.WriteLine(result.NameOfplayer + "|" + result.DateOfresult.ToString() + "|" + result.Time + "|" + result.Tries + '|' + result.Level);
            }
            file.Close();
        }

    }

}