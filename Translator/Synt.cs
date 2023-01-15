using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class Synt
    {
        public class Warning
        {
            //id нужного токена, и обнаруженный нетерминал
            public Configuration.elem nTerm;
            public Configuration.elem term;
            public int id;
            public Warning(int i = -1, Configuration.elem s = new Configuration.elem(), Configuration.elem s1 = new Configuration.elem())
            {
                nTerm = s;
                term = s1;
                id = i;
            }
        }
        public static Warning warn = new Warning();
        //Формируем ошибку
        void ErrorForm(Warning w)
        {
            Code.SyntError = "";
            Code.SyntError += "В строке " + Code.Tokens[warn.id].str_num + " столбце " + Code.Tokens[warn.id].pos_num + " встречено \"" + Code.Tokens[warn.id].value + "\" ";
            if (!w.term.term)
            {
                Pravilo current = Gramma.p.Find(x => x.leftSide == warn.term.value);
                if ((current.leftSide != "letter") && (current.leftSide != "digit"))
                    while (current.rightSide[0][0].terminal != "")
                    {
                        current = Gramma.p.Find(x => x.leftSide == current.rightSide[0][0].notTerminal);
                    }
                else
                {
                    if (current.leftSide == "letter")
                        Code.SyntError += "ожидался идентификатор.";
                    if (current.leftSide == "digit")
                        Code.SyntError += "ожидалась цифра.";
                    return;
                }
                Code.SyntError += "ожидалось \"" + current.rightSide[0][0].terminal + "\".";
            }
            {
                if (!w.nTerm.term)
                {
                    if (w.nTerm.value == "letter")
                        Code.SyntError += "ожидался идентификатор.";
                    else
                    {
                        if (w.nTerm.value == "digit")
                            Code.SyntError += "ожидалась цифра.";
                        else
                            Code.SyntError += "ожидалось \"" + w.term.value + "\" или возможная альтернатива.";
                    }
                }
                else
                    Code.SyntError += "ожидалось \"" + w.term.value + "\" или возможная альтернатива.";
            }


        }
        //Обработка текста для нужд синтаксического анализатора
        public Synt()
        {
            Code.SyntError = "none";
            code = Code.AllCode;
            int line = 1, column = -1;
            for (int i = 0; i < code.Length; i++, column++)
            {
                if (code[i] == '\n')
                {
                    line++;
                    column = -1;
                }

                if (code[i] == '\'')
                {
                    int j = 1;
                    try
                    {
                        while (code[i + (j++)] != '\'') ;
                        code = code.Remove(++i, j - 2);
                    }
                    catch (Exception)
                    {
                        Code.SyntError = "В строке " + line + " столбце " + column + " ожидалось \' (апсостроф, символ конца строки)";
                        return;
                    }

                }
            }

            code = code.Replace("\r\n", " ");
            while (code.IndexOf("  ") != -1)
                code = code.Replace("  ", " ");

        }

        //Синтаксический анализ кода
        public bool GoAnalyze()
        {
            if (Code.SyntError != "none") return false;




            Gramma.Init();
#if DEBUG
            Gramma.Check();
#endif
            Configuration config = new Configuration();

            //История конфигураций
            Stack<Configuration> history = new Stack<Configuration>();
            history.Push(config);

            while (true)
            {
                //Отношение перехода делаем на токенах

                if (config.s == 'q')
                {
                    if (config.L2.Count != 0)
                    {
                        //1 пункт отношения, "На верху L2 не терминал, растим дерево"
                        if (!config.L2.Peek().term)
                        {

                            Configuration.elem item = new Configuration.elem(config.L2.Pop());
                            Pravilo current = Gramma.p.Find(x => x.leftSide == item.value); //текущее правило
                            if (item.value != "string")    //проверка не ожидалась ли строка
                            {
                                if (item.value != "chislo" && item.value != "integer" && item.value != "real")
                                {
                                    if (item.value != "id")
                                    {

                                        config.L1.Push(new Configuration.elem(current.leftSide, false, 1));//в магазин L1 кладем левую часть правила
                                        for (int altIndex = current.rightSide[0].Count - 1; altIndex >= 0; altIndex--)//кладем первую альтернативу в магазин L2 поэлементно
                                        {
                                            if (current.rightSide[0][altIndex].notTerminal != "")
                                            {
                                                config.L2.Push(new Configuration.elem(current.rightSide[0][altIndex].notTerminal, false, 1));
                                            }
                                            else
                                            {
                                                config.L2.Push(new Configuration.elem(current.rightSide[0][altIndex].terminal, true, 1));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Успешное прохождение идентификатора
                                        if (Code.Tokens[config.i].klass == "идентификатор")
                                        {
                                            config.L1.Push(new Configuration.elem(current.leftSide, false, 1));
                                            config.L2.Push(new Configuration.elem(Code.Tokens[config.i].value, true, 1));

                                        }
                                        else
                                        {
                                            config.L1.Push(new Configuration.elem(current.leftSide, false, 1));//в магазин L1 кладем левую часть правила
                                            for (int altIndex = current.rightSide[0].Count - 1; altIndex >= 0; altIndex--)//кладем первую альтернативу в магазин L2 поэлементно
                                            {
                                                if (current.rightSide[0][altIndex].notTerminal != "")
                                                {
                                                    config.L2.Push(new Configuration.elem(current.rightSide[0][altIndex].notTerminal, false, 1));
                                                }
                                                else
                                                {
                                                    config.L2.Push(new Configuration.elem(current.rightSide[0][altIndex].terminal, true, 1));
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Успешное прохождение числа
                                    if (Code.Tokens[config.i].klass == "число   ")
                                    {
                                        if (Code.Tokens[config.i].type == "real")
                                        {
                                            config.L1.Push(new Configuration.elem(current.leftSide, false, 2));
                                            config.L2.Push(new Configuration.elem(Code.Tokens[config.i].value, true, 2));
                                        }
                                        else
                                        {
                                            config.L1.Push(new Configuration.elem(current.leftSide, false, 1));
                                            config.L2.Push(new Configuration.elem(Code.Tokens[config.i].value, true, 1));
                                        }
                                    }
                                    else
                                    {
                                        config.L1.Push(new Configuration.elem(current.leftSide, false, 1));//в магазин L1 кладем левую часть правила
                                        for (int altIndex = current.rightSide[0].Count - 1; altIndex >= 0; altIndex--)//кладем первую альтернативу в магазин L2 поэлементно
                                        {
                                            if (current.rightSide[0][altIndex].notTerminal != "")
                                            {
                                                config.L2.Push(new Configuration.elem(current.rightSide[0][altIndex].notTerminal, false, 1));
                                            }
                                            else
                                            {
                                                config.L2.Push(new Configuration.elem(current.rightSide[0][altIndex].terminal, true, 1));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Успешное прохождение строки
                                if (Code.Tokens[config.i].klass == "строка")
                                {
                                    config.L1.Push(new Configuration.elem(current.leftSide, false, 1));
                                    config.L2.Push(new Configuration.elem(Code.Tokens[config.i].value, true, 1));
                                }
                                else
                                {
                                    config.L1.Push(new Configuration.elem(current.leftSide, false, 1));//в магазин L1 кладем левую часть правила
                                    for (int altIndex = current.rightSide[0].Count - 1; altIndex >= 0; altIndex--)//кладем первую альтернативу в магазин L2 поэлементно
                                    {
                                        if (current.rightSide[0][altIndex].notTerminal != "")
                                        {
                                            config.L2.Push(new Configuration.elem(current.rightSide[0][altIndex].notTerminal, false, 1));
                                        }
                                        else
                                        {
                                            config.L2.Push(new Configuration.elem(current.rightSide[0][altIndex].terminal, true, 1));
                                        }
                                    }
                                }
                            }
                            history.Push(config);
                        }

                        //2 пункт отношения, "На верху L2 терминал сравниваем входной токен"
                        else
                        {
                            if (config.i < Code.Tokens.Count)
                            {
                                Configuration.elem item = new Configuration.elem(config.L2.Peek());
                                //"Успешное сравнение"
                                if ((Code.Tokens[config.i].value == item.value) || (item.value == "lambda"))
                                {
                                    if ((warn != null) && (warn.id < config.i))
                                    {
                                        //Убираем подозрительный токен
                                        warn = new Warning();
                                    }
                                    config.L2.Pop();
                                    config.L1.Push(item);
                                    if (item.value != "lambda") config.i++;
                                }
                                //4 пункт отношения. Не успешное сравнение
                                else
                                {
                                    //Сохраняем подозрительный токен
                                    if (warn.id < config.i)
                                    {
                                        warn = new Warning();
                                        warn.nTerm = config.L1.Peek();
                                        warn.term = item;
                                        warn.id = config.i;
                                    }
                                    config.s = 'b';
                                }
                            }
                            else
                            {
                                Code.SyntError = "Неожиданный конец файла";
                                return false;
                            }
                            history.Push(config);
                        }
                    }
                    else
                    {
                        //3 пункт отншения. Успешное завершение
                        if (config.i == Code.Tokens.Count)
                        {
                            config.s = 't';
                            config.L2.Push(new Configuration.elem("lambda", true, 0));
                            history.Push(config);
                            Code.conclusion = config.Seq();
                            return true;
                        }
                        else
                        {
                            ErrorForm(warn);
                            return false;
                        }
                    }

                }
                else
                {
                    if (!config.L1.Peek().term)
                    //6. Испытание очередной альтернативы
                    {
                        if (!config.L2.Peek().term || !(config.L1.Peek().value == "id"))
                        {

                            Configuration.elem item = new Configuration.elem(config.L1.Pop());
                            Pravilo current = Gramma.p.Find(x => x.leftSide == item.value); //текущее правило    
                            if (current.rightSide.Count > item.altIndex)
                            //a. замена альтернативы если существует иная
                            {
                                for (int elemIndex = 0; elemIndex < current.rightSide[item.altIndex - 1].Count; elemIndex++)
                                {
                                    if (config.L2.Count != 0)
                                    {
                                        config.L2.Pop();
                                    }
                                    else
                                    {
                                        ErrorForm(warn);
                                        return false;
                                    }
                                }
                                for (int elemIndex = current.rightSide[item.altIndex].Count - 1; elemIndex >= 0; elemIndex--)//суём j+1 альтернативу в магазин L2 поэлементно
                                {
                                    if (current.rightSide[item.altIndex][elemIndex].notTerminal != "")
                                    {
                                        config.L2.Push(new Configuration.elem(current.rightSide[item.altIndex][elemIndex].notTerminal, false, item.altIndex + 1));
                                    }
                                    else
                                    {
                                        config.L2.Push(new Configuration.elem(current.rightSide[item.altIndex][elemIndex].terminal, true, item.altIndex + 1));
                                    }
                                }
                                item.altIndex++;
                                config.L1.Push(item);
                                config.s = 'q';
                            }
                            else
                            {
                                if (config.i == 0 && item.value == "main")
                                //б. прекращение разбора
                                {
                                    ErrorForm(warn);
                                    return false;
                                }
                                else
                                //в. Отмена результата
                                {
                                    for (int elemIndex = 0; elemIndex < current.rightSide[item.altIndex - 1].Count; elemIndex++)
                                    {
                                        config.L2.Pop();
                                    }
                                    config.L2.Push(new Configuration.elem(item.value, false, 1));
                                }
                            }

                        }
                        else
                        {
                            config.L2.Pop();
                            config.L2.Push(new Configuration.elem(config.L1.Pop().value, false, 1));
                        }
                    }

                    else
                    //5 Возврат по ходу
                    {
                        Configuration.elem item = new Configuration.elem(config.L1.Pop());
                        config.L2.Push(item);  
                        if (item.value != "lambda") config.i--;
                    }
                    history.Push(config);
                }
            }


        }
        private String code;
    }
}
