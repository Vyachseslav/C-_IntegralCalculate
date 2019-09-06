using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace Method_trapecii
{
    class Program
    {
        /// <summary>
        /// Структура с функциями обработки данных
        /// </summary>
        struct functions
        {
            /// <summary>
            /// Вычисляет интеграл методом трапеций по составной формуле
            /// </summary>
            /// <param name="a">Начало интегрирования</param>
            /// <param name="b">Конец интегрирования</param>
            /// <param name="norm">Нормировочный множитель</param>
            /// <param name="press">Массив со значениями давления</param>
            /// <param name="masIn">Массив с оставшимися данными</param>
            /// <returns>Среднемассовую температуру в Кельвинах</returns>
            public static double trap_Sost(int a, int b, double norm, int[] press, double[,] masIn)
            {
                int count = press.Length;
                double result = 0, sum = 0;
                for (int i = 0; i < count - 1; i++)
                {
                    if (press[i] >= b)
                        break;
                    sum += (masIn[i, 0] + masIn[i + 1, 0]) * (press[i + 1] - press[i]) / 2;
                    //Console.WriteLine(i.ToString() + "\t" + press[i].ToString() + "\t" + sum.ToString());
                }
                result = sum / norm;
                return result;
            }
        }
        static void Main(string[] args)
        {
            CultureInfo inf = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            System.Threading.Thread.CurrentThread.CurrentCulture = inf;
            inf.NumberFormat.NumberDecimalSeparator = ".";

            int count = 0;
            int[] presLev = new int[24];
            double[,] matr = new double[24, 5];
            string[] words = new string[6];

            /*StreamReader fread = new StreamReader(@"D:\V_120101_0000_GDAS.dat");
            fread.ReadLine();
            do
            {
                words = fread.ReadLine().Replace('.', ',').Split('\t');
                presLev[count] = int.Parse(words[0]);
                for (int i = 1; i < 6; i++)
                    matr[count, i - 1] = double.Parse(words[i]);
                //Console.WriteLine(presLev[count].ToString() + "\t" + matr[count, 0].ToString() + "\t" + matr[count, 2].ToString());
                count++;
            } while (!fread.EndOfStream);
            fread.Close();
            double res = integral(50, 1050, 10, 1000.0, presLev, matr);
            
            Console.WriteLine("Result = " + res.ToString());*/
            
            try
            {
                StreamReader fread = new StreamReader(@"D:\Evgeniya\2015\01\V_150101_0000_GDAS.dat");
                fread.ReadLine();
                do
                {
                    words = fread.ReadLine().Split('\t');//Replace('.', ',').
                    presLev[count] = int.Parse(words[0]);
                    for (int i = 1; i < 6; i++)
                        matr[count, i - 1] = double.Parse(words[i]);
                    //Console.WriteLine(presLev[count].ToString() + "\t" + matr[count, 0].ToString() + "\t" + matr[count, 2].ToString());
                    count++;
                } while (!fread.EndOfStream);
                fread.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка открытия файла:\n" + e);
            }

            Console.WriteLine(functions.trap_Sost(20, 1000, 980, presLev, matr).ToString());

            Console.ReadKey(true);
        }
        ///<summary>
        ///Возвращает значение определенного интеграла
        ///по формуле Котеса
        ///</summary>
        ///<param name="a">Конец отрезка интегрирования</param>
        ///<param name="b">Начало отрезка интегрирования</param>
        ///<param name="delitel">Среднее значение атмосферного давления</param>
        ///<param name="iterCount">Число итераций</param>
        ///<param name="massIn">Массив данных: давление</param>
        ///<param name="pressLev">Остальные данные в файле: Температура в K и цельсиях, высота...</param>
        public static double integral(int a, int b, int iterCount, double delitel, int[] pressLev, double[,] massIn)
        {
            
            int value = 0, step = 0, num = 0;
            double result = 0, sum_in = 0, sum_out = 0;

            step = (b - a) / iterCount;
            num = pressLev.Length;
            value = a;

            for (int i = 0; i < num; i++)
            {
                if (pressLev[i] == a || pressLev[i] == b)
                {
                    sum_in += massIn[i, 0];
                    continue;
                }
            }
            sum_in /= 2;
            for (int i = 0; i < num; i++)
            {
                if (i % 2 == 0 || pressLev[i] == a || pressLev[i] == b)
                    continue;
                value += 100;
                if (pressLev[i] == value)
                    sum_out += massIn[i, 0];
            }
            result = (step * (sum_in + sum_out)) / delitel;
            return result;
        }
    }
}
