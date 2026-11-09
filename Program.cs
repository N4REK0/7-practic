using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _7
{
    internal class Program
    {
        static void ProcessPlantInspection(ref int batteryCharge, ref double tankNutrientMl, int leafMaturityIndex, out bool needsSpraying, out string warningCode)
        {
            needsSpraying = false;
            warningCode = "OK";

            // Проверка корректности данных

            if (batteryCharge < 0 || batteryCharge > 100 || tankNutrientMl < 0 || leafMaturityIndex < 0 || leafMaturityIndex > 100)
            {
                warningCode = "INVALID_DATA";
                return;
            }
            Console.WriteLine("");
            Console.WriteLine("====================");
            Console.WriteLine("Все данные корректны");
            Console.WriteLine("====================");
            Console.WriteLine("");
            // Проверка заряда батареи

            if (batteryCharge < 10)
            {
                warningCode = "LOW_CHARGE";
                return;
            }

            // Проверка запаса питательного раствора

            if (tankNutrientMl < 10)
            {
                warningCode = "LOW_NUTRIENT";
                return;
            }

            // Проверка зрелости листьев

            if (leafMaturityIndex < 60)
            {
                needsSpraying = true;
                warningCode = "NADO_OPRISKAT";
            }

            Console.WriteLine("Проверка успешно завершена");
        }



        // Задача 2. Распределение раствора
        // Перегрузка 1 Точечный впрыск

        static bool DispenseSolution(ref double tankMl, double doseMl)
        {
            if (tankMl < 0 || doseMl <= 0)
            {
                return false;
            }

            if (tankMl < doseMl)
            {
                return false;
            }

            tankMl -= doseMl;


            Console.WriteLine($"Выполнен впрыск {doseMl} Мл. ");
            Console.WriteLine($"Остаток раствора в баке {tankMl} Мл.");

            return true;
        }



        // Задача 2. Распределение раствора
        // Перегрузка 2 Обработка

        static bool DispenseSolution(ref double tankMl, double areaSqM, int pestLevel, out double finalConcentration)
        {
            finalConcentration = 0;

            if (tankMl < 0 || areaSqM <= 0 || pestLevel < 1 || pestLevel > 5)
            {
                return false;
            }

            //скольно раствора потратим
            double doseMl = areaSqM * 10 * pestLevel;

            if (tankMl < doseMl)
            {
                return false;
            }

            tankMl -= doseMl;

            //сколько концентрата получим в итоге
            finalConcentration = pestLevel * 2;

            Console.WriteLine($"Оставшееся количество раствора: {tankMl} мл.");
            Console.WriteLine($"Площадь обработки: {areaSqM} м2");
            Console.WriteLine($"Уровень опасности: {pestLevel} ");
            Console.WriteLine($"Израсходовано раствора: {doseMl} мл.");
            Console.WriteLine($"Финальная концентрация: {finalConcentration}");

            return true;
        }



        // Задача 3.
        // Рекурсия

        static int PredictInfectionSpread(int initialPlants, double humidity, int hoursRemaining) // initialPlants - исходные растения, // humidity - влажность, // hoursRemaining - осталось часов, // predictedPlants - прогнозируемые растения
        {
            if (initialPlants < 0 || humidity < 0 || humidity > 100 || hoursRemaining < 0 || hoursRemaining > 24)
            {
                Console.WriteLine("Ошибка: неправильные данные");
                return 0;
            }

            if (hoursRemaining == 0)
            {
                return initialPlants;
            }

            double growthRate = 1 + humidity / 100.0;
            int nextPlants = (int)(initialPlants * growthRate);

            return PredictInfectionSpread(nextPlants, humidity, hoursRemaining - 1);
        }



        // Задача 4.
        // EvaluateMicroclimateIndex (params)

        static double CalculateZoneQuality(int optimalTemp, params int[] zoneTemps) // optimalTemp - оптимальная температура, zoneTemps - произвольное количество показаний термодатчиков яруса
        {
            int sum = 0;

            foreach (int temp in zoneTemps)
            {
                sum += (optimalTemp - temp) * (optimalTemp - temp);
            }

            return Math.Sqrt(sum / zoneTemps.Length);
        }



        // Задача 5.
        // AnalyzeCanopyMatrix (матрица):

        static void AnalyzeCanopyMatrix(double[,] ndviMatrix)
        {
            int rows = ndviMatrix.GetLength(0);
            int cols = ndviMatrix.GetLength(1);

            double maxIndex = 0;

            int healthRow = 0;
            int healthCol = 0;


            for (int i = 0; i < ndviMatrix.GetLength(0); i++ )
            {
                for (int j = 0; j < ndviMatrix.GetLength(1); j++ )
                {
                    if (ndviMatrix[i , j] < 0.3)
                    {
                        ndviMatrix[i, j] = 0;
                    }

                    if (ndviMatrix[i , j] > maxIndex)
                    {
                        maxIndex = ndviMatrix[i, j];
                        healthRow = i;
                        healthCol = j;
                    }
                }
            }

            Console.WriteLine("Матрица: ");

            // Вывод матрицы с подцветкой

            for (int i = 0; i < ndviMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < ndviMatrix.GetLength(1); j++)
                {
                    if (ndviMatrix[i, j] == maxIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.Write($"{ndviMatrix[i, j]}\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Самый здоровый сектор: строка: {healthRow + 1} столбец {healthCol + 1}");
            Console.WriteLine($"Самый большой показатель: {maxIndex}");

        }


        static void Main(string[] args)
        {
            // Проверка и вывод всех задач

            // Задача 1.
            // ProcessPlantInspection

            Console.WriteLine("");
            Console.WriteLine("===== Задача 1 =====");
            Console.WriteLine("");

            Console.Write("Введите заряд батареи дрона: ");
            int batteryCharge = int.Parse(Console.ReadLine());

            Console.Write("Введите количество питательного раствора в баке: ");
            double tankNutrientMl = double.Parse(Console.ReadLine());

            int leafMaturityIndex = 60;
            Console.WriteLine($"Индекс зрелости всех листьев {leafMaturityIndex}");

            ProcessPlantInspection(ref batteryCharge, ref tankNutrientMl, leafMaturityIndex, out bool needsSpraying, out string warningCode);

            Console.WriteLine($"Заряд батареи: {batteryCharge}");
            Console.WriteLine($"Остаток раствора: {tankNutrientMl}");
            Console.WriteLine($"Нужно опрыскивание: {needsSpraying}");
            Console.WriteLine($"Код предупреждения: {warningCode}");



            // Задача 2.1
            // DispenseSolution (перегрузка)

            Console.WriteLine("");
            Console.WriteLine("===== Задача 2.1 =====");
            Console.WriteLine("");

            double tankMl = 500;
            double doseMl = 50;

            bool result = DispenseSolution(ref tankMl, doseMl);

            Console.WriteLine($"Результат впрыска {result}");



            // Задача 2.2
            // DispenseSolution (перегрузка)

            Console.WriteLine("");
            Console.WriteLine("===== Задача 2.2 =====");
            Console.WriteLine("");

            double tankMl2 = 1000;
            double areaSqM = 5;
            int pestLevel = 3;
            double finalConcentration = 0;

            bool result2 = DispenseSolution(ref tankMl2, areaSqM, pestLevel, out finalConcentration);

            Console.WriteLine($"Результат обработки: {result2}");



            // Задача 3
            // CalculateSporesSpread (рекурсия)

            Console.WriteLine("");
            Console.WriteLine("===== Задача 3 =====");
            Console.WriteLine("");

            int initialPlants = 10;
            double humidity = 50;
            int hoursRemaining = 3;

            int predictinfectionspread = PredictInfectionSpread(initialPlants, humidity, hoursRemaining);

            Console.WriteLine($"Прогнозируемые растения: {predictinfectionspread}");



            // Задача 4
            // EvaluateMicroclimateIndex (params):

            Console.WriteLine("");
            Console.WriteLine("===== Задача 4 =====");
            Console.WriteLine("");

            int optimalTemp = 25;

            double calculatezonequality = CalculateZoneQuality(optimalTemp, 23, 24, 27, 26, 25);

            Console.WriteLine($"Микроклимат: {calculatezonequality}");

            // Задача 5
            // AnalyzeCanopyMatrix (матрица)

            Console.WriteLine("");
            Console.WriteLine("===== Задача 5 =====");
            Console.WriteLine("");

            double[,] ndviMatrix =
            {
                { 0.8, 0.7, 0.2 },
                { 0.5, 0.9, 0.6 },
                { 0.1, 0.8, 0.4 }
            };

            AnalyzeCanopyMatrix(ndviMatrix);
        }
    }
}
