using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace Project_T
{
    public partial class Form1 : Form
    {
        BigInteger num;
        int N = 1, K = 1;
        public Form1()
        {
            InitializeComponent();
        }
        int unhappy_rec(int X, List<int> Number) // рекурсивная функция поиска несчастливых номеров
        {
            if (X <= K)
            {
                if (Number.Contains(X)) // если номер содержит цифру, равную искомому числу X, номер счастливый
                { return 0; }
            }
            for (int i = 0; i < Number.Count; i++)
            {
                int max = Number[i]; // берем самую большую цифру в номере
                if (max > X) // если она больше искомого числа X, продолжить цикл
                { continue; }
                Number.Remove(max); // удаляем эту цифру из номера
                if (unhappy_rec(X - max, Number) == 0) // рекурс. вызов функции, где искомое число X уменьшено на выбранную цифру
                    return 0;
                else while (i < Number.Count && Number[i] == max) // проскакиваем повторения выбранной цифры в этом номере
                    { i++; }
                return 1;
            }
            return 1;
        }
        void add(List<int> Number, int index, int k) // рекурсиваная функция создания следующего номера в указанной системе счисления (K)
        {
            if (index < 0)
                return;
            if (Number[index] + k <= K) // если изменяется только текущий разряд
            { Number[index] = Number[index] + k; }
            else if (Number[index] + k == K + 1) // нужно добавить 1 к разряду более высокого порядка
            {
                add(Number, index - 1, 1);
                for (int i = index; i < Number.Count; i++)
                    Number[i] = 0;
            }
            else if (Number[index] + k > K + 1) // нужно добавить 1 к разряду более высокого порядка и оставшуюся часть к текущему
            {
                add(Number, index - 1, 1);
                for (int i = index; i < Number.Count; i++)
                    Number[i] = 0;
                add(Number, index, k - K - 1);
            }
        }
        BigInteger generate() // генерация массива номеров и их обработка
        {
            List<int> Number = new List<int> { }; // Номер
            List<int> SortNumber = new List<int> { }; // Номер, в котором отсортированы цифры
            for (int j = 0; j < N; j++)
            { Number.Add(0); } // первый номер состоит из нулей
            BigInteger n = 0, unh = 0, temp = 0; // n - счетчик обработанных номеров, unh - несчастливых, temp - вспомогательный, для цикла
            int half; // значение половины суммы цифр номера
            while (n < num - 1) // пока не обработали все номера
            {
                int unh1 = 0; //этот блок if-else заставляет обращаться к переменным типа BigInteger реже
                if (n + 1000000000 < num - 1) //1 раз в 1 миллиард номеров, так как обращение к ним дольше
                { // обрабатывается (так себе оптимизация, конечно)
                    temp = 1000000000;
                    n += 1000000000;
                }
                else
                {
                    temp = num - 1 - n;
                    n = num - 1;
                }
                for (int k = 0; k < temp; k++) // обработка номеров
                {
                    add(Number, N - 1, 1); // генерируем следующий по порядку номер
                    if (Number.Sum() % 2 == 1) // если сумма цифр нечетная, он несчастливый
                    { unh1++; }
                    else
                    {
                        SortNumber = Number.ToList();
                        SortNumber.Sort();
                        SortNumber.Reverse(); // на основе номера создали массив отсортированных цифр
                        half = Convert.ToInt32(SortNumber.Sum() / 2); // половина суммы цифр
                        unh1 += unhappy_rec(half, SortNumber); // определение, несчастливый номер или нет
                    }
                }
                unh += unh1;
            }            
            return unh; // возвращаем кол-во несчастливых номеров
        }
        private void button1_Click(object sender, EventArgs e) // расчет задачи
        {
            DateTime time1 = DateTime.Now, time2; // таймеры для вывода времени решения задачи
            richTextBox1.Clear();
            num = BigInteger.Pow(K + 1, N); // общее число номеров
            BigInteger k = generate(); // вызов функции создания и обработки массива номеров; k - число несчастливых
            time2 = DateTime.Now;
            richTextBox1.Text += "Из общего числа номеров " + (num) + " несчастливыми являются " + k + '\n';
            richTextBox1.Text += "Расчеты заняли " + (time2 - time1).Minutes.ToString() + "мин " + (time2 - time1).Seconds.ToString() + "c " + (time2 - time1).Milliseconds.ToString() + "мс";            
        }
        private void trackBar1_Scroll(object sender, EventArgs e) // задание параметра N
        {
            N = trackBar1.Value;
            if(N*K>300)
            {
                K = 300 / N;
                trackBar2.Value = K;
            }
            textBox1.Text = N.ToString();
            textBox2.Text = K.ToString();
        }

        private void trackBar2_Scroll(object sender, EventArgs e) // задание параметра K
        {
            K = trackBar2.Value;
            if (N * K > 300)
            {
                N = 300 / K;
                trackBar1.Value = N;
            }
            textBox1.Text = N.ToString();
            textBox2.Text = K.ToString();
        }
    }
}
