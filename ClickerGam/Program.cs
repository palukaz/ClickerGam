using System;
using System.IO;

namespace ClickerGam
{
    public class Clicker
    {
        public static string Name;
        public static int Points;
        static string dirPath = @"A:\Games\Clicker";
        public static int SaveAmount = Directory.GetFiles(dirPath).Length;
        public static void ShowFunc()
        {
            Console.WriteLine("0: Выйти");
            Console.WriteLine("111: Создать новое сохранение");
            Console.WriteLine("222: Удалить сохранение");
            Console.WriteLine("333: Поиск сохранения");
        }
        public static int Search()
        {
            Console.Clear();
            Console.WriteLine("Введите имя сохранения: ");
            ShowSaves();
            Console.WriteLine();
            string userInput = Console.ReadLine();
            int fileNum = 1;
            foreach (var file in Directory.EnumerateFiles(dirPath, "*.txt"))
            {
                int counter = 0;

                foreach (string line in File.ReadLines(dirPath+@"\saveSlot_" + (fileNum) + ".txt"))
                {
                    if (counter == 0)
                    {
                        if (line.Contains(userInput))
                        {
                            Console.WriteLine($"\n{fileNum}. Имя: {line}"); 
                        }
                    }
                }
                fileNum++;
            }
            Console.WriteLine("\n0: Вернуться\nВыберите сохранение... (если ничего не вышло, значит нет совпадений!)\n");
            int pointer;
            while (true)
            {
                try
                {
                    pointer = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неверный формат... (Выберите номер сохранения)");
                    continue;
                }
                break;
            }
            return pointer;
        }
        public static void SaveGame(int saveNum, int points, string saveName)
        {
            StreamWriter sw = new StreamWriter(dirPath+@"\saveSlot_" + saveNum + ".txt",false);
            sw.WriteLine(saveName); sw.WriteLine(points);
            sw.Close();
        }
        public static void StartGame(int saveNum)
        {
            Console.Clear();
            int counter = 0;
            foreach (string line in File.ReadLines(dirPath + @"\saveSlot_" + (saveNum) + ".txt"))
            {
                if (counter == 0)
                {
                    Name = line;
                    counter++;
                }
                else Points = Convert.ToInt32(line);
            }
            Console.WriteLine("Имя сохранения: " + Name + "\n");
            Console.WriteLine("Очки: " + Points + "\n");
            Console.WriteLine("Нажмите на любую клавишу, чтобы получить очки...");
            while (true)
            {
                var key = Console.ReadKey(false);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    SaveGame(saveNum,Points,Name);
                    ChooseSaves();
                }
                Console.Clear();
                Console.WriteLine("Имя сохранения: " + Name + "\n");
                Points++;
                Console.WriteLine("Очки: " + Points);
                Console.WriteLine("\nПродолжай... (Esc, чтобы сохранить и выйти)");
            }
        }
        public static void DeleteSave()
        {
            Console.Clear();
            Console.WriteLine("Введите номер сохранения, которое хотите удалить: ");
            ShowSaves();
            int saveNum;
            while (true)
            {
                try { saveNum = Convert.ToInt32(Console.ReadLine()); break; }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Введите число!");
                    ShowSaves();
                }
            }
            FileInfo file = new FileInfo(dirPath + @"\saveSlot_" +saveNum+".txt");
            if (file.Exists)
            {
                file.Delete();
                SortSaves();
                SaveAmount--;
                Console.Clear();
                Console.WriteLine("Файл успешно удалён! Нажмите любую клавишу, чтобы вернуться...");
                Console.ReadKey();
                Console.Clear();
                ChooseSaves();
            }
        }
        public static void CreateSave()
        {
            Console.Clear();
            Console.WriteLine("Введите название сохранения: ");
            string saveName = Console.ReadLine();
            FileStream fstream = new FileStream(dirPath + @"\saveSlot_" + (SaveAmount + 1) + ".txt", FileMode.Create);
            fstream.Close();
            StreamWriter streamWriter = new StreamWriter(dirPath + @"\saveSlot_" + (SaveAmount + 1) + ".txt");
            streamWriter.WriteLine(saveName);
            streamWriter.WriteLine("0");
            streamWriter.Close();
            Console.WriteLine("Сохранение успешно создано! \nНажмите любую клавишу, чтобы вернуться...");
            Console.ReadKey();
            SaveAmount++;
            Console.Clear();
            ChooseSaves();
        }
        public static void SortSaves()
        {
            int counter = 1;
            foreach (var file in Directory.EnumerateFiles(dirPath, "*.txt"))
            {
                
                string oldf = file;
                string newf = dirPath + @"\saveSlot_" +counter+".txt";
                if (oldf != newf)
                {
                    File.Move(oldf, newf);
                    File.Delete(oldf);
                    counter++;
                }
                else  counter++; 
            }
        }
        public static void ShowSaves()
        {
            int fileNum = 1;
            foreach (var file in Directory.EnumerateFiles(dirPath, "*.txt"))
            {
                int counter = 0;
                
                    foreach (string line in File.ReadLines(dirPath + @"\saveSlot_" + (fileNum) + ".txt"))
                    {
                        if (counter == 0)
                        {
                            Name = line;
                            counter++;
                        }
                        else Points = Convert.ToInt32(line);
                    }
                Console.WriteLine($"\n{fileNum}. Имя: {Name} || Очки: {Points} ");
                fileNum++;
            }   
        }
        public static void ChooseSaves()
        {
            int pointer;
            while (true)
            {
                Console.Clear();
                ShowFunc();
                Console.WriteLine("Выберите файл сохранения: ");
                ShowSaves();
                try
                {
                    Console.WriteLine();
                    pointer = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.Clear();
                    Console.WriteLine("Введенная строка имела неверный формат.");
                    ShowFunc();
                    continue;
                }
                if (pointer == 0) Environment.Exit(-1);
                else if (pointer > 0 && pointer <= SaveAmount) StartGame(pointer);
                else if (pointer == 111) CreateSave();
                else if (pointer == 222) DeleteSave();
                else if (pointer == 333)
                {
                    pointer = Search();
                    if (pointer != 0) StartGame(pointer);
                    else
                    {
                        Console.Clear();
                        continue;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Введите значение от 0 до " + SaveAmount + "...");
                    ShowFunc();
                }
            }
        }
        public static void StartMenu()
        {
            Console.WriteLine("Добро пожаловать в кликер! Нажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey(false);
            Console.Clear();
            SortSaves();
            ChooseSaves();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Clicker.StartMenu();
        }
    }
}
