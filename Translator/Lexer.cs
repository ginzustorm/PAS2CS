using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Translator
{
    class Lexer
    {
        private String code;
        public Lexer()
        {
            code = Code.AllCode;
        }
        public bool GoAnalyze()
        {   
            File.Delete("result.txt");
            if (File.Exists("keys.txt"))
            {
                FileStream file_key = new FileStream("keys.txt", FileMode.Open, FileAccess.Read);
                StreamReader reader_key = new StreamReader(file_key);
                string k = reader_key.ReadToEnd();
                if (k != "")
                {
                    Token.keys.Clear();
                    Token.iden.Clear();
                    while (!String.IsNullOrEmpty(k))
                    {
                        string str = "";
                        int i = 0;
                        while (k[i] != ',')
                        {
                            str = str + k[i];
                            i++;
                        }
                        if (!String.IsNullOrEmpty(k)) k = k.Substring(i + 1);
                        Token.keys.Add(str);
                    }
                    List<Token> ending = new List<Token>();
                    StreamWriter writer = new StreamWriter("keywords.txt", false);
                    StreamWriter writer1 = new StreamWriter("identify.txt", false);

                    string g = code;
                    string zap = g;
                    string b = "";
                    bool l = control(ref g, ref b);
                    zap = g;
                    if (l)
                    {
                        g = erase_space(g);

                        int i = 0;

                        while (!String.IsNullOrEmpty(g))
                        {
                            string f = get_str(ref g);
                            i++;
                            bool log = true;
                            while (!String.IsNullOrEmpty(f))
                            {
                                Token r = new Token();
                                int a = 0;
                                string q = get_lex(ref f, ref log, ref a);
                                r.define(q, a, ref zap);
                                ending.Add(r);
                                Code.Tokens.Add(r);

                            }

                        }
                        for (int j = 0; j < ending.Count; j++)
                        {
                            ending[j].conclude();
                        }
                        for (int j = 0; j < Token.keys.Count; j++)
                        {
                            writer.Write(j);
                            writer.Write(' ');
                            writer.WriteLine(Token.keys[j]);


                        }
                        for (int j = 0; j < Token.iden.Count; j++)
                        {
                            writer1.Write(j);
                            writer1.Write(' ');
                            writer1.WriteLine(Token.iden[j]);

                            Code.Idents.Add(Token.iden[j]);
                        }


                    }
                    else
                    {
                        Code.LexError = "Недопустимый(ые) символ(ы)" + b;
                        return false;

                    }

                    writer.Close();
                    writer1.Close();
                }
                else
                {
                    Code.LexError = "Файл с ключевыми словами пуст";
                    return false;
                }
            }
            else
            {
                Code.LexError = "Нет файла с ключевыми словами";
                return false;
            }
            return true;


        }
        public static bool is_str(char a)
        {
            if ((int)a < 91 && (int)a > 64) return true;
            if ((int)a > 96 && (int)a < 123) return true;
            if ((int)a == 95) return true;
            return false;
        }
        public static bool is_num(char a)
        {
            if ((int)a < 58 && (int)a > 47) return true;
            return false;
        }
        public static bool is_control(char a)
        {
            if ((int)a < 48 && (int)a > 39) return true;
            if ((int)a > 57 && (int)a < 63) return true;
            if ((int)a == 93) return true;
            if ((int)a == 91) return true;
            if (a == ' ') return true;
            return false;
        }
        public static string erase_space(string str)
        {
            int i = 0;
            while (str[i] == ' ')
            {
                str = str.Remove(i, 1);
            }
            while (str[str.Length - 1] == ' ')
            {
                str = str.Remove(str.Length - 1);
            }
            i = 0;
            while (i < str.Length - 2)
            {
                if (str[i] == '\'')
                {
                    i++;
                    while (i < str.Length - 1)
                    {
                        if (str[i] != '\'') i++;
                        else break;
                    }
                    i++;
                }
                if (i < str.Length - 2 && (str[i] == ' ' || char.IsControl(str[i])) && str[i + 1] == ' ')
                {
                    str = str.Substring(0, i + 1) + str.Substring(i + 2, str.Length - 2 - i);
                }
                else
                {
                    i++;
                }
            }
            return str;
        }
        public static string get_str(ref string str)
        {
            int i = 0;
            int quote = 0;
            string res = "";
            bool log = false;
            while (!log)
            {
                while (i < str.Length && str[i] != ';')
                {
                    if (str.Length > 1 && quote % 2 == 0 && str[i] == '/' && str[i + 1] == '/')
                    {
                        int o = i+1;
                        while (str[o] != '\r' && str[o] < str.Length)
                        {
                            o++;
                        }
                        str = str.Substring(0, i) + str.Substring(o);

                    }
                    if (quote % 2 == 0 && str[i] == '{')
                    {
                        int o = i + 1;
                        while (str[o] != '}' && str[o] < str.Length)
                        {
                            o++;
                        }
                        str = str.Substring(0, i) + str.Substring(o + 1);
                    }
                    if (str[i] == '\'') quote++;
                    if (char.IsControl(str[i]))
                    {
                        if (quote % 2 == 0)
                            res = res + ' ';
                        i++;
                        continue;
                    }
                    res = res + str[i];
                    i++;
                }
                if (i >= str.Length) break;
                res = res + str[i];
                i++;
                if (quote % 2 == 1)
                {
                    log = false;
                }
                else log = true;
                if (str[i - 1] == '.' && str[i - 2] == '.') log = false;
                if (i < str.Length && (is_num(str[i - 2]) && is_num(str[i]))) log = false;
            }
            if (i - 1 != str.Length) str = str.Substring(i);
            else str = "";
            res = erase_space(res);
            return res;
        }
        public static string get_lex(ref string str, ref bool log, ref int id)
        {
            string res = "";
            HashSet<char> sym_oper = new HashSet<char>();
            sym_oper.Add('+');
            sym_oper.Add('-');
            sym_oper.Add('*');
            sym_oper.Add('/');
            sym_oper.Add('>');
            sym_oper.Add('<');
            sym_oper.Add('=');
            HashSet<char> sym_limit = new HashSet<char>();
            sym_limit.Add(',');
            sym_limit.Add(':');
            sym_limit.Add(';');
            sym_limit.Add(')');
            sym_limit.Add('(');
            sym_limit.Add('[');
            sym_limit.Add(']');
            while (str[0] == ' ') str = str.Substring(1);
            if (str[0] == '\'') // символы
            {
                res = res + str[0];
                int i=1;
                while (str[i] != '\'')
                {
                    res = res + str[i];
                    i++;
                }
                res = res + str[i];
                str = str.Substring(i + 1);
                log = true;
                id = 1;
                return res;
            }
            while (is_num(str[0])) // число
            {
                id = 2;
                res = res + str[0];
                int i = 1;
                while (is_num(str[i]))
                {
                    res = res + str[i];
                    i++;
                }
                if (str[i] == '.' && i < str.Length)
                {
                    if (is_num(str[i + 1]))
                    {
                        res = res + str[i];
                        str = str.Substring(i + 1);
                        continue;
                    }
                    else
                    {
                        str = str.Substring(i);
                        log = false;
                        return res;
                    }
                }
                else
                {
                    str = str.Substring(i);
                    log = false;
                    return res;
                }
            }
            if (sym_oper.Contains(str[0])) // оператор
            {
                id = 3;
                res = res + str[0];
                int i = 1;
                if (str[0] == '>' && str[i] == '=')
                {
                    res = res + str[1];
                    i++;
                }
                if (str[0] == '<' && (str[i] == '=' || str[i] == '>'))
                {
                    res = res + str[1];
                    i++;
                }
                if (log && (str[0] == '+' || str[0] == '-') && (is_num(str[i]) || is_num(str[i + 1])))
                {

                    str = str.Substring(i);
                    string  p = get_lex(ref str,ref log,ref id);
                    res = res + p;
                }

                str = str.Substring(i);
                log = true;
                return res;
            }
            if (sym_limit.Contains(str[0]) || str[0] == '.') // ограничитель
            {

                res = res + str[0];
                int i = 1;
                if (str[0] == ':' && str[i] == '=')
                {
                    res = res + str[i];
                    i++;
                }
                if (str[0] == '.' && str.Length > 1 && str[i] == '.')
                {
                    res = res + str[i];
                    i++;
                }
                str = str.Substring(i);
                log = true;
                id = 4;
                return res;
            }
            if (is_str(str[0])) // идентификатор
            {
                res = res + str[0];
                int i = 1;
                while (i<str.Length && !sym_limit.Contains(str[i]) && !sym_oper.Contains(str[i]) && str[i] != '.' && str[i] != ' ' && str[i] != '\'')
                {
                    res = res + str[i];
                    i++;
                }
                str = str.Substring(i);
                log = false;
                id = 5;
                return res;
            }
            log = true;
            return str;
        }
        public static bool control(ref string text, ref string mistakes)
        {
            bool log = true;
            int ns = 1;
            int pos = 1;
            int quote=0;
            int bracket = 0;
            int comment = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (bracket > 0 && quote % 2 == 0 && text[i] == '}')
                {
                    bracket--;
                    continue;
                }
                if (comment > 0 && text[i] == '\r')
                {
                    comment--;
                    continue;
                }
                if (char.IsControl(text[i]))
                {
                    if (text[i] == '\r')
                    {
                        ns++;
                        pos = 1;
                    }
                    continue;

                }
                if (bracket == 0 && text[i] == '\'')
                {
                    quote++;
                    continue;
                }
                if (bracket == 0 && quote % 2 == 0 && text[i] == '{')
                {
                    bracket++;
                    continue;
                }

                if (bracket == 0 && quote % 2 == 0 && comment == 0 && text[i] == '/' && text[i + 1] == '/')
                {
                    comment++;
                    continue;
                }

                if (comment == 0 && bracket == 0 && quote % 2 == 0 && !is_str(text[i]) && !is_num(text[i]) && !is_control(text[i]))
                {
                    log = false;
                    string k = '[' + ns.ToString() + ',' + pos.ToString() + ']' + '\t';
                    mistakes = mistakes + k;

                }
                if ((comment > 0 || bracket > 0) && text[i] != '/' && text[i - 1] != '/')
                {
                    char[] array = text.ToCharArray();
                    array[i] = '$';
                    text = new string(array);
                }
                pos++;
            }
            if (quote % 2 == 1)
            {
                mistakes = mistakes + "\r\n ++ в тексте присутствует лишняя кавычка!";
                return false;
            }
            return log;
        }

    }
}
