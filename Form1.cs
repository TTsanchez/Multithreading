using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form : System.Windows.Forms.Form
    {
        //Ядра
        Thread ThrA;
        Thread ThrB;
        Thread ThrC;
        Thread ThrD;
        Thread ThrE;
        Thread ThrF;
        Thread ThrG;
        Thread ThrH;
        Thread ThrK;
        //объект локер
        static object locker = new object();

        //Время начала работы
        long ExecStartTime;

        //Количество завершенных заданий
        int FinishedTasks;

        //Результаты выполенения функций
        int F1_res;
        int F2_res;
        int F3_res;
        int F4_res;
        int F5_res;
        int F6_res;
        int F7_res;
        int F8_res;

        int[] M1 = new int[5];
        int[] M2 = new int[5];
        int[] M3 = new int[5];

        //Метод для графического интерфейса
        public Form()
        {
            InitializeComponent();
        }

        //Нажатие на кнопку "Запуск", запуск треда А
        private void ButtonStart(object sender, EventArgs e)
        {
            FinishedTasks = 0;
            progressBar1.Value = 0;

            //Задаём время начала выполения
            ExecStartTime = Timestamp();

            LogText("---------------------------------------------------------------------");

            //Создаём тред А
            ThrA = new Thread(new ThreadStart(TaskA));
            ThrA.Start();
        }

        //Нажатие на кнопку "Очистить лог"
        private void ButtonClear(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void ButtonSave(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Today;
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            string formattedDateTime = DateTime.Now.ToString("ddMMyyyy_HHmmss");
            string FileName = "log" + formattedDateTime + ".txt";
            int n = 1;
            while (File.Exists(FileName))
            {
                FileName = "log" + formattedDateTime + "(" + n + ")" + ".txt";
                n++;
            }
            using (StreamWriter writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), FileName)))
            {
                writer.Write(textBox1.Text);
            }
        }

        //Инициализация генератора булевых значений
        Random rng = new Random();
        public void RandNums(int[] M)
        {
            for (int i = 0; i < M.Length; i++)
            {
                M[i] = rng.Next(1, 101);
            }
        }

        //Функция для вывода данных на экран
        public void LogText(String what)
        {
            //Если вызов из треда, инвокаем в гуй
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(LogText), new object[] { what });
                return;
            }
            String pre = "\r\n";
            if (textBox1.Text == "")
            {
                pre = "";
            }
            textBox1.Text += pre + what.ToString();
        }

        //Выводит информацию при запуске треда, так же возвращает время в миллисекундах
        public long ThreadInfoHandler(String ActiveThread, String ParentThread)
        {
            LogText("[" + ActiveThread + "] Тред " + ActiveThread + " запущен тредом " + ParentThread + " в " + (Timestamp() - ExecStartTime).ToString() + " мс");
            return Timestamp();
        }

        //Выводит информацию при окончании треда, выводит время выполения треда в миллисекундах, увеличивает значение прогресс-бара
        public void ThreadExitHandler(String ActiveThread, long StartTime)
        {
            //Если вызов из треда, инвокаем в гуй
            if (InvokeRequired)
            {
                this.Invoke(new Action<string, long>(ThreadExitHandler), new object[] { ActiveThread, StartTime });
                return;
            }

            ProgressUp(true);

            //Подсчитываем время исполения
            long NowTime = Timestamp();
            String ExecTime = (NowTime - StartTime).ToString();

            LogText("[" + ActiveThread + "] Тред " + ActiveThread + " завершился, время выполения: " + ExecTime + "ms, время окончания: " + (NowTime - ExecStartTime).ToString() + " ms\r\n");
        }

        //Обновляет прогресс-бар и процентное отношение завершения
        public void ProgressUp(bool UpdateText = true)
        {
            //Если вызов из треда, инвокаем в гуй
            if (InvokeRequired)
            {
                this.Invoke(new Action<bool>(ProgressUp), new object[] { UpdateText });
                return;
            }

            FinishedTasks += 1;
            progressBar1.Value = FinishedTasks;

            if (UpdateText)
            {

                label1.Text = ("Прогресс выполнения: " + Math.Round((float)FinishedTasks /9 * 100)).ToString() + " %";
            }
        }

        //Время в миллисекундах unix
        public long Timestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        //Функция F1
        public int F1(int[] M1, int[] M2, int[] M3)
        {
            int SumMin = M1.Min() + M2.Min() + M3.Min();
            
            return SumMin;
        }

        //Функция F2
        public int F2(int[] M1, int[] M2, int[] M3)
        {
            int SumMax = M1.Max() + M2.Max() + M3.Max();
            return SumMax;
        }

        //Функция F3
        public int F3(int[] M1, int[] M2, int[] M3)
        {
            int Sum = M1.Length + M2.Length  + M3.Length;
            return Sum;
        }

        //Функция f4
        public int F4(int f3)
        {
            int n1 = f3 * 100;
            return n1;
        }

        //Функция f5
        public int F5(int f3)
        {
            int n2 = f3 + 100500;
            return n2;
        }

        //Функция f6
        public int F6(int f3)
        {
            int n3 = (int)Math.Sqrt(f3);
            return n3;
        }

        //Функция f7
        public int F7(int f4, int f5, int f6)
        {
            int n4 = f5 / (f4 + f6);
            return n4;
        }

        //Функция f8
        public int F8(int f1, int f2, int f7)
        {
            int n5 = (f2 - f1) * f7;
            return n5;
        }

        //Задача A
        public void TaskA()
        {
            long startTime = ThreadInfoHandler("A", "UI-тред");

            //Ждём секунду, как требуется в варианте
            System.Threading.Thread.Sleep(1000);


            LogText("Генерация массива М1");
            RandNums(M1);
            LogText("М1 сгенерирован: " + string.Join(", ", M1));

            LogText("Генерация массива M2");
            RandNums(M2);
            LogText("M2 сгенерирован: " + string.Join(", ", M2));

            LogText("Генерация массива M3");
            RandNums(M3);
            LogText("M3 сгенерирован: " + string.Join(", ", M3));

            ThreadExitHandler("A", startTime);

            ThrB = new Thread(() => TaskB("А"));
            ThrB.Start();
            ThrC = new Thread(() => TaskC("А"));
            ThrC.Start();
            ThrD = new Thread(() => TaskD("А"));
            ThrD.Start();

        }

        //Задача B
        public void TaskB(String ParentThread)
        {
            long startTime = ThreadInfoHandler("B", ParentThread);
            //Ждём секунду, как требуется в варианте
            System.Threading.Thread.Sleep(3000);

            LogText("[B] Выполняется функция F1");
            lock (M1)
            {
                lock (M2)
                {
                    lock (M3)
                    {
                        F1_res = F1(M1, M2, M3);
                    }
                }
            }
            LogText("[B] Значение F1: " + F1_res.ToString());
            ThreadExitHandler("B", startTime);

            lock (ThrC)
            {
                lock (ThrH)
                {
                    if ((!ThrC.IsAlive) & (!ThrH.IsAlive))
                    {
                        ThrK = new Thread(() => TaskK("B"));
                        ThrK.Start();
                    }
                }
            }

        }

        //Задача C
        public void TaskC(String ParentThread)
        {
            long startTime = ThreadInfoHandler("C", ParentThread);
            //Ждём секунду, как требуется в варианте
            System.Threading.Thread.Sleep(3000);

            LogText("[C] Выполняется функция F2");
            lock (M1)
            {
                lock (M2)
                {
                    lock (M3)
                    {
                        F2_res = F2(M1, M2, M3);
                    }
                }
            }
            LogText("[C] Значение F2: " + F2_res.ToString());
            ThreadExitHandler("C", startTime);

            lock (ThrB)
            {
                lock (ThrH)
                {
                    if ((!ThrB.IsAlive) & (!ThrH.IsAlive))
                    {
                        ThrK = new Thread(() => TaskK("C"));
                        ThrK.Start();
                    }
                }
            }

        }

        //Задача D
        public void TaskD(String ParentThread)
        {
            long startTime = ThreadInfoHandler("D", ParentThread);
            //Ждём секунду, как требуется в варианте
            System.Threading.Thread.Sleep(1000);

            LogText("[D] Выполняется функция F3");
            lock (M1)
            {
                lock (M2)
                {
                    lock (M3)
                    {
                        F3_res = F3(M1, M2, M3);
                    }
                }
            }
            LogText("[D] Значение F3: " + F3_res.ToString());
            ThreadExitHandler("D", startTime);
            ThrE = new Thread(() => TaskE("D"));
            ThrE.Start();
            ThrF = new Thread(() => TaskF("D"));
            ThrF.Start();
            ThrG = new Thread(() => TaskG("D"));
            ThrG.Start();
        }

        //Задача E
        public void TaskE(String ParentThread)
        {
            long startTime = ThreadInfoHandler("E", ParentThread);
            //Ждём 2 секунды, как требуется в варианте
            System.Threading.Thread.Sleep(1000);

            LogText("[E] Выполняется функция F4");
            F4_res = F4(F3_res);
            LogText("[E] Значение F4: " + F4_res.ToString());
            ThreadExitHandler("E", startTime);
            lock (ThrF)
            {
                lock (ThrG)
                {
                    if ((!ThrF.IsAlive) & (!ThrG.IsAlive))
                    {
                        ThrH = new Thread(() => TaskH("E"));
                        ThrH.Start();
                    }
                }
            }
        }

        //Задача F
        public void TaskF(String ParentThread)
        {
            long startTime = ThreadInfoHandler("F", ParentThread);
            //Ждём 2 секунды, как требуется в варианте
            System.Threading.Thread.Sleep(1000);

            LogText("[F] Выполняется функция F5");
            F5_res = F5(F3_res);
            LogText("[F] Значение F5: " + F5_res.ToString());
            ThreadExitHandler("F", startTime);
            lock (ThrE)
            {
                lock (ThrG)
                {
                    if ((!ThrE.IsAlive) & (!ThrG.IsAlive))
                    {
                        ThrH = new Thread(() => TaskH("F"));
                        ThrH.Start();
                    }
                }
            }

        }
        //Задача G
        public void TaskG(String ParentThread)
        {
            long startTime = ThreadInfoHandler("G", ParentThread);
            //Ждём секунду, как требуется в варианте
            System.Threading.Thread.Sleep(1000);

            LogText("[G] Выполняется функция F6");
            F6_res = F6(F3_res);
            LogText("[G] Значение F6: " + F6_res.ToString());
            ThreadExitHandler("G", startTime);
            lock (ThrE)
            {
                lock (ThrF)
                {
                    if ((!ThrE.IsAlive) & (!ThrF.IsAlive))
                    {
                        ThrH = new Thread(() => TaskH("G"));
                        ThrH.Start();
                    }
                }
            }
        }
        //Задача H
        public void TaskH(String ParentThread)
        {
            long startTime = ThreadInfoHandler("H", ParentThread);
            //Ждём 1 секунду, как требуется в варианте
            System.Threading.Thread.Sleep(1000);

            LogText("[H] Выполняется функция F7");
            F7_res = F7(F4_res, F5_res, F6_res);
            LogText("[H] Значение F7: " + F7_res.ToString());
            ThreadExitHandler("H", startTime);
            lock (ThrC)
            {
                lock (ThrB)
                {
                    if ((!ThrC.IsAlive) & (!ThrB.IsAlive))
                    {
                        ThrK = new Thread(() => TaskK("H"));
                        ThrK.Start();
                    }
                }
            }

        }
        //Задача K
        public void TaskK(String ParentThread)
        {
            long startTime = ThreadInfoHandler("K", ParentThread);

            //Ждём 1 секундy, как требуется в варианте
            System.Threading.Thread.Sleep(1000);

            LogText("[K] Выполняется функция F8");
            F8_res = F8(F1_res, F2_res, F7_res);
            LogText("[K] Значение F8: " + F8_res.ToString());
            ThreadExitHandler("K", startTime);
        }
    }
}