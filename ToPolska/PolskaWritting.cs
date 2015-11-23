using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToPolska
{
    public struct PWret
    {
        public int ind;
        public string str;
    }

    public struct BCret
    {
        public int ind;
        public bool correctly;
    }


    class PW
    {
        Dictionary<string, string> Errors = new Dictionary<string, string>();//список кодов ошибок и их описаний

        public PW()
        {
            Errors.Add("error0", "Неправильно расставлены скобки");
            Errors.Add("error1", "Пустое выражение для польской записи");
            Errors.Add("error3", "Применение символа \"минус\" к пустому выражению");
        }

        /// <summary>
        /// Посимвольное разбиение строки выражения
        /// </summary>
        char[] parts;


        /// <summary>
        /// Функция проверки выражения на правильную расстановку скобок
        /// </summary>
        bool BracketsCheck()
        {
            BCret subresult = new BCret();

            int ind = 0;//с какого символа начать проверку

            while (ind < parts.Length)
            {
                if (parts[ind] == '(')
                {
                    subresult = subBracketsCheck(ind + 1);

                    if (subresult.correctly == false)
                        return false;
                    else
                        ind = subresult.ind;
                }
                else if (parts[ind] == ')')
                {
                    return false;
                }

                ind++;
            }


            return true;//если функция дошла до конца, то всё нормально
        }

        BCret subBracketsCheck(int ind)
        {
            BCret subresult = new BCret();

            //int openBracketsCount = 0;
            //int clBracketsCount = 0;
            while (ind < parts.Length)
            {
                if (parts[ind] == '(')
                {
                    subresult = subBracketsCheck(ind + 1);

                    if (subresult.correctly == false)
                    {
                        return subresult;
                    }
                    else
                        ind = subresult.ind;
                }
                else if (parts[ind] == ')')
                {
                    subresult.correctly = true;
                    subresult.ind = ind;
                    return subresult;
                }

                ind++;
            }

            subresult.correctly = false;//если дошло до конца, то не удалось найти парную скобку
            return subresult;
        }



        /// <summary>
        /// Находит польскую запись выражения
        /// </summary>
        /// <param name="str">Текстовая запись выражения</param>
        /// <returns></returns>
        public string toPolska(string str)
        {
            //TODO - else заменить на case (где получится)

            str = str.Replace(" ", "")//удалени всех пробелов из строки
                   .Replace(")(", ")*(");//удалени всех пробелов из строки
            parts = new char[str.Length];
            parts = str.ToArray<char>();//посимвольное разбиение исходной строки

            if (!BracketsCheck())//если неправильно расставлены скобки
                return "error0";// Неправильно расставлены скобки

            int ind = str.Length - 1;//индекс проверяемого символа
            string substr = "";//строка, возвращаемая рекурсией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            string Polska = "";//польская запись выражения

            while (ind >= 0)
            {
                if (parts[ind] == '+')
                {
                    subresult = addRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '-')
                {
                    if (ind - 1 >= 0 && (parts[ind - 1] == '^' || parts[ind - 1] == '!' || parts[ind - 1] == '*' || parts[ind - 1] == '/' || parts[ind - 1] == '+' || parts[ind - 1] == '-'))//если перед знаком минуса стоит оператор
                    {
                        if (Polska == "")
                        {
                            //subresult.str = "error3";
                            //subresult.ind = ind;

                            return "error3";
                        }
                        else
                        {
                            Polska = "-ː0ː" + substr + 'ː' + Polska;

                            ind--;
                        }
                    }
                    else
                    {
                        subresult = subRec(ind - 1);

                        ind = subresult.ind;
                        substr = subresult.str;

                        if (Polska == "")
                            Polska = substr;
                        else
                            Polska = substr + 'ː' + Polska;
                    }
                }
                else if (parts[ind] == '*')
                {
                    subresult = mulRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '/')
                {
                    subresult = divRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '^')
                {
                    subresult = degRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '!')
                {
                    subresult = facRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    return Polska;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    string subPolska = "";
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }
                    if (ind >= 0 && Char.IsLetter(parts[ind]))//если перед цифрами буквы
                    {
                        while (ind >= 0 && Char.IsLetter(parts[ind]))
                        {
                            subPolska = parts[ind] + subPolska;
                            ind = ind - 1;
                        }
                    }

                    if (Polska == "")
                        Polska = subPolska + number.ToString();
                    else
                        Polska = subPolska + number.ToString() + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами снова цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
                else if (parts[ind] == '?')
                {
                    subresult = queryRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
            }


            return Polska;
        }


        PWret addRec(int ind)
        {
            string substr = "";//строка, возвращаемая внутренней рекурсией
            string Polska = "0";//строка, возвращаемая данной функцией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            while (ind >= 0)
            {
                if (parts[ind] == '+')
                {
                    subresult = addRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '-')
                {
                    subresult = subRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '*')
                {
                    subresult = mulRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '/')
                {
                    subresult = divRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '^')
                {
                    subresult = degRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '!')
                {
                    subresult = facRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    subresult.ind = ind;
                    subresult.str = "+ː" + Polska;

                    return subresult;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    string subPolska = "";
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }
                    if (ind >= 0 && Char.IsLetter(parts[ind]))//если перед цифрами буквы
                    {
                        while (ind >= 0 && Char.IsLetter(parts[ind]))
                        {
                            subPolska = parts[ind] + subPolska;
                            ind = ind - 1;
                        }
                    }

                    if (Polska == "0")
                        Polska = subPolska + number.ToString();
                    else
                        Polska = subPolska + number.ToString() + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами снова цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "0")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
            }

            subresult.str = "+ː" + Polska;
            subresult.ind = ind;

            return subresult;
        }


        PWret subRec(int ind)
        {
            string substr = "";//строка, возвращаемая внутренней рекурсией
            string Polska = "0";//строка, возвращаемая данной функцией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            while (ind >= 0)
            {
                if (parts[ind] == '+')
                {
                    subresult = addRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '-')
                {
                    subresult = subRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '*')
                {
                    subresult = mulRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '/')
                {
                    subresult = divRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '^')
                {
                    subresult = degRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '!')
                {
                    subresult = facRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "0")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    subresult.ind = ind;
                    subresult.str = "-ː" + Polska;

                    return subresult;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    string subPolska = "";
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }
                    if (ind >= 0 && Char.IsLetter(parts[ind]))//если перед цифрами буквы
                    {
                        while (ind >= 0 && Char.IsLetter(parts[ind]))
                        {
                            subPolska = parts[ind] + subPolska;
                            ind = ind - 1;
                        }
                    }

                    if (Polska == "0")
                        Polska = subPolska + number.ToString();
                    else
                        Polska = subPolska + number.ToString() + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами снова цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "0")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
            }

            subresult.str = "-ː" + Polska;
            subresult.ind = ind;

            return subresult;
        }


        PWret mulRec(int ind)
        {
            string substr = "";//строка, возвращаемая внутренней рекурсией
            string Polska = "1";//строка, возвращаемая данной функцией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            while (ind >= 0)
            {
                if (parts[ind] == '+' || parts[ind] == '-')
                {
                    subresult.ind = ind;
                    subresult.str = "*ː" + Polska;

                    return subresult;
                }
                else if (parts[ind] == '*')
                {
                    subresult = mulRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '/')
                {
                    subresult = divRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '^')
                {
                    subresult = degRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '!')
                {
                    subresult = facRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    subresult.ind = ind;
                    subresult.str = "*ː" + Polska;

                    return subresult;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    string subPolska = "";
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }
                    if (ind >= 0 && Char.IsLetter(parts[ind]))//если перед цифрами буквы
                    {
                        while (ind >= 0 && Char.IsLetter(parts[ind]))
                        {
                            subPolska = parts[ind] + subPolska;
                            ind = ind - 1;
                        }
                    }

                    if (Polska == "1")
                        Polska = subPolska + number.ToString();
                    else
                        Polska = subPolska + number.ToString() + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами снова цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "1")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
            }

            subresult.str = "*ː" + Polska;
            subresult.ind = ind;

            return subresult;
        }


        PWret divRec(int ind)
        {
            string substr = "";//строка, возвращаемая внутренней рекурсией
            string Polska = "1";//строка, возвращаемая данной функцией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            while (ind >= 0)
            {
                if (parts[ind] == '+' || parts[ind] == '-')
                {
                    subresult.ind = ind;
                    subresult.str = "/ː" + Polska;

                    return subresult;
                }
                else if (parts[ind] == '*')
                {
                    subresult = mulRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '/')
                {
                    subresult = divRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '^')
                {
                    subresult = degRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '!')
                {
                    subresult = facRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "1")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    subresult.ind = ind;
                    subresult.str = "/ " + Polska;

                    return subresult;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    string subPolska = "";
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }
                    if (ind >= 0 && Char.IsLetter(parts[ind]))//если перед цифрами буквы
                    {
                        while (ind >= 0 && Char.IsLetter(parts[ind]))
                        {
                            subPolska = parts[ind] + subPolska;
                            ind = ind - 1;
                        }
                    }

                    if (Polska == "1")
                        Polska = subPolska + number.ToString();
                    else
                        Polska = subPolska + number.ToString() + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами снова цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        Polska = substr + 'ː' + Polska;
                    }
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "1")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;

                    if (ind >= 0 && Char.IsNumber(parts[ind]))//если перед буквами цифры
                    {
                        subresult = mulRec(ind);

                        ind = subresult.ind;
                        substr = subresult.str;

                        if (Polska == "1")
                            Polska = substr;
                        else
                            Polska = substr + 'ː' + Polska;
                    }
                }
            }

            subresult.str = "/ː" + Polska;
            subresult.ind = ind;

            return subresult;
        }


        PWret degRec(int ind)
        {
            string substr = "";//строка, возвращаемая внутренней рекурсией
            string Polska = "";//строка, возвращаемая данной функцией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            while (ind >= 0)
            {
                if (parts[ind] == '+' || parts[ind] == '-' || parts[ind] == '*' || parts[ind] == '/')
                {
                    subresult.ind = ind;
                    subresult.str = "^ː" + Polska;

                    return subresult;
                }
                else if (parts[ind] == '^')
                {
                    subresult = degRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '!')
                {
                    subresult = facRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    subresult.ind = ind;
                    subresult.str = "^ː" + Polska;

                    return subresult;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = number.ToString();
                    else
                        Polska = number.ToString() + 'ː' + Polska;
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;
                }
            }

            subresult.str = "^ː" + Polska;
            subresult.ind = ind;

            return subresult;
        }


        PWret facRec(int ind)
        {
            string substr = "";//строка, возвращаемая внутренней рекурсией
            string Polska = "";//строка, возвращаемая данной функцией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            while (ind >= 0)
            {
                if (parts[ind] == '+' || parts[ind] == '-' || parts[ind] == '*' || parts[ind] == '/' || parts[ind] == '^')
                {
                    subresult.ind = ind;
                    subresult.str = "!ː" + Polska;

                    return subresult;
                }
                else if (parts[ind] == '!')
                {
                    Polska = "!ː" + Polska;

                    subresult = mulRec(ind);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;

                    subresult.str = Polska;

                    return subresult;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    subresult.ind = ind;
                    subresult.str = "!ː" + Polska;

                    return subresult;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = number.ToString();
                    else
                        Polska = number.ToString() + 'ː' + Polska;
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;
                }
            }

            subresult.str = "! " + Polska;
            subresult.ind = ind;

            return subresult;
        }


        PWret brackRec(int ind)
        {
            string substr = "";//строка, возвращаемая внутренней рекурсией
            string Polska = "";//строка, возвращаемая данной функцией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            while (ind >= 0)
            {
                if (parts[ind] == '+')
                {
                    subresult = addRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '-')
                {
                    if (ind - 1 >= 0 && (parts[ind - 1] == '^' || parts[ind - 1] == '!' || parts[ind - 1] == '*' || parts[ind - 1] == '/' || parts[ind - 1] == '+' || parts[ind - 1] == '-'))//если перед знаком минуса стоит оператор
                    {
                        if (Polska == "")
                        {
                            subresult.str = "error3";
                            subresult.ind = ind;

                            return subresult;
                        }
                        else
                        {
                            Polska = "-ː0ː" + substr + 'ː' + Polska;

                            ind--;
                        }
                    }
                    else
                    {
                        subresult = subRec(ind - 1);

                        ind = subresult.ind;
                        substr = subresult.str;

                        if (Polska == "")
                            Polska = substr;
                        else
                            Polska = substr + 'ː' + Polska;
                    }
                }
                else if (parts[ind] == '*')
                {
                    subresult = mulRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '/')
                {
                    subresult = divRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '^')
                {
                    subresult = degRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '!')
                {
                    subresult = facRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    subresult.ind = ind - 1;
                    subresult.str = Polska;

                    return subresult;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = number.ToString();
                    else
                        Polska = number.ToString() + 'ː' + Polska;
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;
                }
            }

            subresult.str = Polska;
            subresult.ind = ind - 1;

            return subresult;
        }


        PWret queryRec(int ind)
        {
            string substr = "";//строка, возвращаемая внутренней рекурсией
            string Polska = "";//строка, возвращаемая данной функцией

            PWret subresult = new PWret();//что возвращает рекурсия операции

            while (ind >= 0)
            {
                //if (parts[ind] == '+' || parts[ind] == '-' || parts[ind] == '*' || parts[ind] == '/' || parts[ind] == '^' || parts[ind] == '!')
                //{
                //    subresult.ind = ind;
                //    subresult.str = "?," + Polska;

                //    return subresult;
                //}
                if (parts[ind] == '+')
                {
                    subresult = addRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '-')
                {
                    subresult = subRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '*')
                {
                    subresult = mulRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '/')
                {
                    subresult = divRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '^')
                {
                    subresult = degRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '!')
                {
                    subresult = facRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == ')')
                {
                    subresult = brackRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
                else if (parts[ind] == '(')
                {
                    subresult.ind = ind;
                    subresult.str = "?ː" + Polska;

                    return subresult;
                }
                else if (Char.IsNumber(parts[ind]))//если это цифра
                {
                    int number = 0;
                    int poryadok = 1;

                    while (ind >= 0 && Char.IsNumber(parts[ind]))
                    {
                        number = number + (int)Char.GetNumericValue(parts[ind]) * poryadok;
                        poryadok = poryadok * 10;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = number.ToString();
                    else
                        Polska = number.ToString() + 'ː' + Polska;
                }
                else if (Char.IsLetter(parts[ind]))//если это буква
                {
                    string letter = "";

                    while (ind >= 0 && Char.IsLetter(parts[ind]))
                    {
                        letter = parts[ind] + letter;
                        ind = ind - 1;
                    }

                    if (Polska == "")
                        Polska = letter;
                    else
                        Polska = letter + 'ː' + Polska;
                }
                else if (parts[ind] == '?')
                {
                    subresult = queryRec(ind - 1);

                    ind = subresult.ind;
                    substr = subresult.str;

                    if (Polska == "")
                        Polska = substr;
                    else
                        Polska = substr + 'ː' + Polska;
                }
            }

            subresult.str = "?ː" + Polska;
            subresult.ind = ind;

            return subresult;
        }
    }
}
