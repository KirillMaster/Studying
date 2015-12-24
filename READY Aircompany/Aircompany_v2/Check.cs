using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Airport
{
    static public class Check
    {
        static public void OnlyDigits(object textComposition)
        {
            TextCompositionEventArgs textCompoisitonEventArgs = textComposition as TextCompositionEventArgs;
            if (!Char.IsDigit(textCompoisitonEventArgs.Text, 0))
            {
                textCompoisitonEventArgs.Handled = true;
            }
        }
        static public void OnlyText(object textComposition)
        {
            TextCompositionEventArgs textCompoisitonEventArgs = textComposition as TextCompositionEventArgs;
            if (!Char.IsLetter(textCompoisitonEventArgs.Text, 0)) 
            {
                textCompoisitonEventArgs.Handled = true;
            }
        }
        static public void OnlyTextAndDigits(object textComposition)
        {
            TextCompositionEventArgs textCompoisitonEventArgs = textComposition as TextCompositionEventArgs;
            if ((!Char.IsLetter(textCompoisitonEventArgs.Text, 0)) && (!Char.IsDigit(textCompoisitonEventArgs.Text, 0)) && (textCompoisitonEventArgs.Text != "-"))
            {
                textCompoisitonEventArgs.Handled = true;
            }
        }
        static public void PutTimeHanlder(object sender)
        {
            string inputText = (sender as TextBox).Text;

            if (inputText.Length == 2)
            {
                (sender as TextBox).Text = inputText + ":";
                (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
            }
        }

        //static public void OnlyText(string alreadyInputText, int maxCountSymbols, TextCompositionEventArgs e)
        //{
        //    if ((!Char.IsLetter(e.Text, 0)) || (alreadyInputText.Length >= maxCountSymbols))
        //        e.Handled = true;
        //}

        //static public void OnlyDigits(string alreadyInputText, int maxCountSymbols, TextCompositionEventArgs e)
        //{
        //    if ((!Char.IsDigit(e.Text, 0)) || (alreadyInputText.Length >= maxCountSymbols))
        //        e.Handled = true;
        //}

        //static public void TextAndDigits(string alreadyInputText, int maxCountSymbols, TextCompositionEventArgs e)
        //{
        //    if (((!Char.IsDigit(e.Text, 0)) && (!Char.IsLetter(e.Text, 0))) || (alreadyInputText.Length >= maxCountSymbols))
        //        e.Handled = true;
        //}
        static public TimeSpan GetTime(string stringTime)
        {
            if (stringTime.Length == 0) throw new Exception("Введены не все значения!");
            else
            {
                string[] time = stringTime.Split(':');
                if (Int32.Parse(time[0]) > 24 || Int32.Parse(time[1]) > 59) throw new Exception("Некорректная форма записи времени!");
                TimeSpan timeSpan = new TimeSpan(Int32.Parse(time[0]), Int32.Parse(time[1]), 0);
                return timeSpan;
            }
        }

        static public bool InputTime(object sender, TextCompositionEventArgs e)
        {
            Regex inputRegex = new Regex(@"^[0-9]$");
            Match match = inputRegex.Match(e.Text);
            string inputText = (sender as TextBox).Text;
            if (match.Success && inputText.Length <= 4) return true;
            else return false;
        }

        static public void AllStringInput(params string[] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if ((text[i] == null) || (text[i].Length == 0)) throw new Exception("Введены не все значения!");
            }
        }
        
        static public void CorrectDate(DateTime? earlyDate, DateTime? lastDate)
        {
            if (earlyDate.Value.Date > lastDate.Value.Date) throw new Exception("Неверно введены даты! Дата отправления должна быть раньше даты прибытия.");
        }

        static public void IfOneStringInput(params string[] text)
        {
            int countNullStrings = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if ((text[i] == null) || (text[i].Length == 0)) countNullStrings++;
            }
            if (countNullStrings == text.Length) throw new Exception("Не было введено ни одного значения!");
        }

        static public bool IsTimeNull(string stringTime)
        {
            if ((stringTime == null) || (stringTime.Length == 0)) return true;
            else return false;
        }

        static public void ItemSelected(params ComboBox[] boxes)
        {
            foreach (ComboBox box in boxes)
            {
                if (box.SelectedItem == null) throw new Exception("Введены не все значения!");
            }
        }

    }
}
