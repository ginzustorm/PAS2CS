using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Translator
{
    public partial class FirstForm : Form
    {
        public FirstForm()
        {
            InitializeComponent();
            ChosePath.Focus();
        }

        private void FirstForm_Load(object sender, EventArgs e)
        {

            RUN.Enabled = false;


#if DEBUG
            //***debug only***
            Code.PasPath = "text11.pas";
            StreamReader reader = new StreamReader(Code.PasPath);
            Code.AllCode = reader.ReadToEnd() + "\r\n";
            reader.Close();
            RUN.Enabled = true;
            //****************
#endif   

        }

        private void FirstForm_Shown(object sender, EventArgs e)
        {
            ChosePath.Focus();
        }

        //Считываем, в случае ошибки - выводим информацию
        private void ChosePath_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "*.pas";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName == null) return;
            try
            {
                RUN.Enabled = true;
                StreamReader reader = new StreamReader(openFileDialog.FileName);
                Code.AllCode = reader.ReadToEnd() + "\r\n";
                reader.Close();
                Code.PasPath = FileLocation.Text = openFileDialog.FileName;
            }
            catch (FileNotFoundException no_file)
            {
                MessageBox.Show(no_file.Message, "Error, file not found", MessageBoxButtons.OK);
            }
            catch (Exception Unknown_e)
            {
                MessageBox.Show(Unknown_e.Message, "Unknown error", MessageBoxButtons.OK);
                //throw;
            }
        }

        //Начало анализа
        private void RUN_Click(object sender, EventArgs e)
        {

            //Лексический анализ
            Lexer lexer=new Lexer();
            if (lexer.GoAnalyze())
            {
                MessageBox.Show("Ваш код прошёл проверку лексического анализатора", "Лексический анализатор: Успех =)", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Ошибка: " + Code.LexError + "\nПроцесс трансляции будет прерван", "Название ошибки", MessageBoxButtons.OK); //ошибки нужно рассказывать
                return;
            }

            //Синтаксический анализ
            Synt syntax = new Synt();
            Semantics sem = new Semantics();
            if (syntax.GoAnalyze())
            {
                if (sem.GoAnalazy())
                    MessageBox.Show("Ваш код прошёл проверку синтаксического анализатора", "Синтаксический анализатор: Успех =)", MessageBoxButtons.OK);
                else
                {
                    MessageBox.Show("Ошибка: " + Code.SyntError + "\nПроцесс трансляции будет прерван", "Название ошибки", MessageBoxButtons.OK); //ошибки нужно рассказывать
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ошибка: " + Code.SyntError + "\nПроцесс трансляции будет прерван", "Название ошибки", MessageBoxButtons.OK); //ошибки нужно рассказывать
                return;
            }

            //Генерация кода
            Generation gener=new Generation();
            if (gener.GoGenerate())
            {
                MessageBox.Show("Ваш код на языке Pascal успешно транслирован на язык C# \nФайл расположен в директории \"" + Path.GetDirectoryName(Path.GetFullPath(Code.PasPath)) + "\"\nИмя файла: \""+Path.GetFileName(Path.GetFullPath(Code.PasPath)).Replace(".pas",".cs")+"\"", "Генератор кода: Успех =)", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show(Code.GenerationError + "\nПроцесс трансляции будет прерван", "Название ошибки", MessageBoxButtons.OK); //ошибки нужно рассказывать
                return;
            }
            Code.Idents = new List<String>();
            Code.Tokens = new List<Token>();
        }
    }
}
