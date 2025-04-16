using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba1
{
    public partial class Compiller : Form
    {
        private string currentFilePath = string.Empty;
        private bool isTextChanged = false;
        public Compiller()
        {
            InitializeComponent();
        }

        // Обработчик для пункта "Создать"
        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            currentFilePath = string.Empty;
            isTextChanged = false;
        }

        // Обработчик для пункта "Открыть"
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog.FileName;
                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                isTextChanged = false;
            }
        }

        // Обработчик для пункта "Сохранить"
        private void сохранитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                // Если файл уже открыт, сохраняем изменения в него
                richTextBox1.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                isTextChanged = false;
            }
            else
            {
                // Если файл не открыт, вызываем диалог сохранения
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName; // Сохраняем путь к файлу
                    richTextBox1.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                    isTextChanged = false;
                }
            }
        }

        // Обработчик для пункта "Сохранить как"
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = saveFileDialog.FileName;
                richTextBox1.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                isTextChanged = false;
            }
        }
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            isTextChanged = true;
        }

        // Обработчик для пункта "Выход"
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isTextChanged)
            {
                DialogResult result = MessageBox.Show("Вы хотите сохранить изменения перед выходом?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    сохранитьToolStripMenuItem_Click_1(sender, e); // Сохраняем изменения
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Отменяем выход
                }
            }

            Application.Exit(); // Выход из программы
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }

        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void вызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Путь к PDF-файлу (относительно исполняемого файла программы)
            string pdfPath = "Resources\\Manual.pdf";

            try
            {
                // Открываем PDF-файл с помощью программы по умолчанию
                Process.Start(pdfPath);
            }
            catch (Exception ex)
            {
                // Если что-то пошло не так, показываем сообщение об ошибке
                MessageBox.Show($"Не удалось открыть файл справки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Это приложение является курсовой работой Попова Алексея Романовича, студента группы АВТ-214.",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            currentFilePath = string.Empty;
            isTextChanged = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog.FileName;
                richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
                isTextChanged = false;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                // Если файл уже открыт, сохраняем изменения в него
                richTextBox1.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                isTextChanged = false;
            }
            else
            {
                // Если файл не открыт, вызываем диалог сохранения
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName; // Сохраняем путь к файлу
                    richTextBox1.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                    isTextChanged = false;
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            string pdfPath = "Resources\\Manual.pdf";

            try
            {
                Process.Start(pdfPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл справки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Это приложение является курсовой работой Попова Алексея Романовича, студента группы АВТ-214.",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = richTextBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Введите текст для анализа.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ЛЕКСИЧЕСКИЙ АНАЛИЗ
            Lexer lexer = new Lexer(input);
            List<Token> tokens = lexer.Tokenize();

            //dataGridViewTokens.Rows.Clear();
            //foreach (var token in tokens)
            //{
            //    dataGridViewTokens.Rows.Add(token.Code, token.Type, token.Lexeme, token.Position);
            //}

            // СИНТАКСИЧЕСКИЙ АНАЛИЗ
            Parser parser = new Parser(tokens);
            string result = parser.Analyze();

            if (result.Contains("Ошибка"))
            {
                richTextBox2.ForeColor = Color.Red;
                richTextBox2.Text = result;
            }
            else
            {
                richTextBox2.ForeColor = Color.Green;
                richTextBox2.Text = "Ошибок не обнаружено";
            }
        }
    }
}