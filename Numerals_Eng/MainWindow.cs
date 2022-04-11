﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Numerals_Eng
{
    enum RomanNumerals
    {
        I=1,
        V=5,
        X=10,
        L=50,
        C=100,
        D=500,
        M=1000
    }

    public partial class MainWindow : Form
    {
        string[] words;
        string[] ones = new string[]{
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine"
            };
        string[] second_ten=new string[]
            {
                "eleven",
                "twelve",
                "thirteen",
                "fourteen",
                "fifteen",
                "sixteen",
                "seventeen",
                "eighteen",
                "nineteen"
            };
        string[] tens=new string[]
            {
                "ten",
                "twenty",
                "thirty",
                "forty",
                "fifty",
                "sixty",
                "seventy",
                "eighty",
                "ninety"
            };
        List<NumeralEng> numeralList=new List<NumeralEng>();
        public MainWindow()
        {

            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            label1.Text = "=";
            label2.Visible=false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "=";

            Regex regex = new Regex(@"\s+");
            string text = regex.Replace(textBox1.Text, " ");
            Regex regex_begin = new Regex(@"^\s+");
            text = regex_begin.Replace(text, "");
            Regex regex_end = new Regex(@"\s+$");
            text = regex_end.Replace(text,"");

            words = text.Split(' ');
            if (check_syntax())
            {
                if(check_logic())
                {
                    int number = conversion();
                    if(number!=0)
                    {
                        label1.Text += number.ToString();
                        label2.Text = toRomanNum(number);
                        label2.Visible = true;
                    }
                }
            }
            numeralList.Clear();
        }

        public bool ones_check (string word)
        {
            bool ones_bool = false;
            for (int i = 0; i < ones.Length; i++)
            {
                if (word == ones[i])
                {
                    ones_bool = true;
                    break;
                }
            }
            return ones_bool;
        }
        public bool tens_check(string word)
        {
            bool tens_bool = false;
            for (int i = 0; i < tens.Length; i++)
            {
                if (word == tens[i])
                {
                    tens_bool = true;
                    break;
                }
            }
            return tens_bool;
        }
        public bool check_syntax()
        {
            bool word_bool = true;
            foreach (string word in words)
            {
                NumeralEng numeral = new NumeralEng();
                bool ones_bool = false;
                //string error = "Синтансическая ошибка";

                //единица
                if (ones_check(word))
                {
                    ones_bool = true;
                    numeral.SetTypeNum("единица");  
                }

                //сотня
                if (word == "hundred")
                {
                    ones_bool = true;
                    numeral.SetTypeNum("сотня");
                }

                //союз
                if (word == "and")
                {
                    ones_bool = true;
                    numeral.SetTypeNum("союз");
                }

                //десяток
                if(tens_check(word))
                {
                    ones_bool = true;
                    numeral.SetTypeNum("десяток");
                }

                //второй десяток
                for (int i = 0; i < second_ten.Length; i++)
                {
                    if (word == second_ten[i])
                    {
                        ones_bool = true;
                        numeral.SetTypeNum("второй десяток");
                    }
                }


                //coставной десяток
                if(word.Contains("-"))
                {
                    string[] tens_ones = word.Split('-');
                    if(tens_check(tens_ones[0]) && ones_check(tens_ones[1]))
                    {
                        ones_bool = true;
                        numeral.SetTypeNum("составной десяток");
                    }
                }

                if(ones_bool)
                {
                    numeral.SetValue(word);
                    numeralList.Add(numeral);
                    continue;
                }
                else
                {
                    MessageBox.Show($"Синтаксическая ошибка ({word})");
                    return false;
                }
                
            }
            return true;
        }
        public bool check_logic()
        {
            for (int i=0;i<numeralList.Count;i++)
            {
                if (numeralList.Count<2)
                {
                    MessageBox.Show("Минимальное количество числительных - 2");
                    return false;
                }

                if(numeralList[0].GetTypeNum()!="единица")
                {
                    MessageBox.Show("Перед сотней должна идти единица");
                    return false;   
                }

                if(numeralList.Count>=2 && numeralList[1].GetTypeNum()!="сотня")
                {
                    MessageBox.Show($"После единицы не может идти {numeralList[1].GetTypeNum()}");
                    return false;
                }


                if (numeralList.Count >= 3 && numeralList[2].GetTypeNum() != "союз")
                {
                    //MessageBox.Show($"После сотни не может идти {numeralList[2].GetTypeNum()}");
                    MessageBox.Show("После сотни должен быть союз and");
                    return false;
                }

                if (numeralList.Count == 3)
                {
                    MessageBox.Show("После союза должно быть числительное");
                    return false;    
                }

                if (numeralList.Count == 4 && numeralList[3].GetTypeNum() == "сотня")
                {
                    MessageBox.Show($"После союза не может идти {numeralList[3].GetTypeNum()}");
                    return false;
                }

                if (numeralList.Count == 4 && numeralList[3].GetTypeNum() == "союз")
                {
                    MessageBox.Show($"После союза не может идти союз");
                    return false;
                }

                if (numeralList.Count >= 5)
                {
                    MessageBox.Show("Максимальное кол-во числительных - 4");
                    return false;
                }
            }
            return true;
        }

        public int conversion()
        {
            string number = String.Empty;
            int noStringNum = 0;
            for (int i = 0; i < ones.Length; i++)
            {
                if (numeralList[0].GetValue() == ones[i])
                {
                    i += 1;
                    number =  i.ToString()+ "00";
                    noStringNum = Int32.Parse(number);
                    if(numeralList.Count != 4)
                    {
                        return noStringNum;
                    }
                    break;
                }
            }
            if(numeralList.Count == 4)
            {
                if (numeralList[3].GetTypeNum() == "единица")
                {
                    for (int i = 0; i < ones.Length; i++)
                    {
                        if (numeralList[3].GetValue() == ones[i])
                        {
                            i += 1;
                            noStringNum = noStringNum + i;
                            return noStringNum;
                        }
                    }
                }

                if (numeralList[3].GetTypeNum()=="второй десяток")
                {
                    for (int i = 0; i < second_ten.Length; i++)
                    {
                        if (numeralList[3].GetValue() == second_ten[i])
                        {
                            noStringNum = noStringNum + i+11;
                            return noStringNum;
                        }
                    }
                }

                if (numeralList[3].GetTypeNum() == "десяток")
                {
                    for (int i = 0; i < tens.Length; i++)
                    {
                        if (numeralList[3].GetValue() == tens[i])
                        {
                            i+=1;
                            noStringNum = noStringNum + i*10;
                            return noStringNum;
                        }
                    }
                }

                if (numeralList[3].GetTypeNum() == "составной десяток")
                {
                    for (int i = 0; i < tens.Length; i++)
                    {
                        for (int j=0;j<ones.Length;j++)
                        {
                            string tens_ones = tens[i] +"-"+ones[j];
                            if (numeralList[3].GetValue()==tens_ones)
                            {
                                i += 1;
                                j += 1;
                                noStringNum = noStringNum + i*10+j;
                                return noStringNum;
                            }
                        }
                    }
                }
            }
            return noStringNum;
        }

        public string toRomanNum(int number)
        {
            string resultRomanNum = String.Empty;
            string numberForCheck = number.ToString();

            if(numberForCheck[0]=='9')
            {
                resultRomanNum = RomanNumerals.C.ToString() + RomanNumerals.M.ToString();
            }
            if (numberForCheck[0] == '4')
            {
                resultRomanNum = RomanNumerals.C.ToString() + RomanNumerals.D.ToString();
            }
            if (numberForCheck[0] == '5')
            {
                resultRomanNum = RomanNumerals.D.ToString();
            }
            if (int.Parse(numberForCheck[0].ToString())<4)
            {
                for (int i = 0; i < int.Parse(numberForCheck[0].ToString()); i++)
                {
                    resultRomanNum += RomanNumerals.C.ToString();
                }
            }
            if (int.Parse(numberForCheck[0].ToString())>5& int.Parse(numberForCheck[0].ToString()) <9)
            {
                resultRomanNum = RomanNumerals.D.ToString();
                for (int i=5;i< int.Parse(numberForCheck[0].ToString()); i++)
                {
                    resultRomanNum += RomanNumerals.C.ToString();
                }
            }


            if (numberForCheck[1] == '9')
            {
                resultRomanNum += RomanNumerals.X.ToString() + RomanNumerals.C.ToString();
            }
            if (numberForCheck[1] == '4')
            {
                resultRomanNum += RomanNumerals.X.ToString() + RomanNumerals.L.ToString();
            }
            if (numberForCheck[1] == '5')
            {
                resultRomanNum += RomanNumerals.L.ToString();
            }
            if (int.Parse(numberForCheck[1].ToString()) < 4)
            {
                for (int i = 0; i < int.Parse(numberForCheck[1].ToString()); i++)
                {
                    resultRomanNum += RomanNumerals.X.ToString();
                }
            }
            if (int.Parse(numberForCheck[1].ToString()) > 5 & int.Parse(numberForCheck[1].ToString()) < 9)
            {
                resultRomanNum += RomanNumerals.L.ToString();
                for (int i = 5; i < int.Parse(numberForCheck[1].ToString()); i++)
                {
                    resultRomanNum += RomanNumerals.X.ToString();
                }
            }

            if (numberForCheck[2] == '9')
            {
                resultRomanNum += RomanNumerals.I.ToString() + RomanNumerals.X.ToString();
            }
            if (numberForCheck[2] == '4')
            {
                resultRomanNum += RomanNumerals.I.ToString() + RomanNumerals.V.ToString();
            }
            if (numberForCheck[2] == '5')
            {
                resultRomanNum += RomanNumerals.V.ToString();
            }
            if (int.Parse(numberForCheck[2].ToString()) < 4)
            {
                for (int i = 0; i < int.Parse(numberForCheck[2].ToString()); i++)
                {
                    resultRomanNum += RomanNumerals.I.ToString();
                }
            }
            if (int.Parse(numberForCheck[2].ToString()) > 5 & int.Parse(numberForCheck[2].ToString()) < 9)
            {
                resultRomanNum += RomanNumerals.V.ToString();
                for (int i = 5; i < int.Parse(numberForCheck[2].ToString()); i++)
                {
                    resultRomanNum += RomanNumerals.I.ToString();
                }
            }

            return resultRomanNum;
        }
    }
}