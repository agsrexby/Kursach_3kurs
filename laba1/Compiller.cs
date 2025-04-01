using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private void лексическийАнализToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
    
            var lexer = new LexicalAnalyzer();
            var tokens = lexer.Analyze(richTextBox1.Text);
    
            // Форматированный вывод результатов
            richTextBox2.SelectionFont = new Font("Consolas", 10, FontStyle.Bold);
            richTextBox2.AppendText("РЕЗУЛЬТАТЫ ЛЕКСИЧЕСКОГО АНАЛИЗА:\n\n");
    
            foreach (var token in tokens)
            {
                // Выделение不同类型 разными цветами
                if (token.Type == LexicalAnalyzer.TokenType.Invalid)
                    richTextBox2.SelectionColor = Color.Red;
                else
                    richTextBox2.SelectionColor = Color.Black;
            
                richTextBox2.AppendText($"[{token.Type}] {token.TypeName}: '{token.Value}' (поз. {token.StartPos}-{token.EndPos})\n");
            }
    
            // Подсветка ошибок в исходном коде
            HighlightLexicalErrors(tokens);
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        public static class RichTextBoxExtensions
        {
            public static void AppendText(RichTextBox box, string text, Color color, bool bold = false)
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;
                box.SelectionColor = color;
                box.SelectionFont = new Font(box.Font, bold ? FontStyle.Bold : FontStyle.Regular);
                box.AppendText(text);
                box.SelectionColor = box.ForeColor;
                box.SelectionFont = box.Font;
            }
        }
        
        private void синтаксическийАнализToolStripMenuItem_Click(object sender, EventArgs e)
{
    try
    {
        richTextBox2.Clear();
        var lexer = new LexicalAnalyzer();
        var tokens = lexer.Analyze(richTextBox1.Text);
        var parser = new SyntaxAnalyzer();
        var result = parser.Parse(tokens);

        // Устанавливаем стиль для заголовка
        richTextBox2.SelectionFont = new Font(richTextBox2.Font, FontStyle.Bold);
        richTextBox2.SelectionColor = Color.Blue;
        richTextBox2.AppendText("СИНТАКСИЧЕСКИЙ АНАЛИЗ С ВОССТАНОВЛЕНИЕМ ОШИБОК:\n\n");

        if (result.IsValid)
        {
            richTextBox2.SelectionColor = Color.Green;
            richTextBox2.AppendText("✓ Выражение корректно\n");
        }
        else
        {
            // Вывод информации об ошибках
            richTextBox2.SelectionColor = Color.Red;
            richTextBox2.AppendText($"ОБНАРУЖЕНО ОШИБОК: {result.Errors.Count}\n");
            
            // Вывод действий по восстановлению
            if (result.RecoveryMessages.Any())
            {
                richTextBox2.SelectionColor = Color.DarkOrange;
                richTextBox2.AppendText("\nВОССТАНОВЛЕНИЕ ОШИБОК:\n");
                foreach (var recovery in result.RecoveryMessages)
                {
                    richTextBox2.AppendText($"• {recovery}\n");
                }
            }

            // Вывод самих ошибок
            richTextBox2.SelectionColor = Color.DarkRed;
            richTextBox2.AppendText("\nДЕТАЛИ ОШИБОК:\n");
            richTextBox2.SelectionFont = new Font(richTextBox2.Font, FontStyle.Regular);
            
            foreach (var error in result.Errors)
            {
                richTextBox2.AppendText($"• {error}\n");
            }

            HighlightErrorsInCode(result.Errors);
        }
    }
    catch (Exception ex)
    {
        richTextBox2.SelectionColor = Color.Red;
        richTextBox2.SelectionFont = new Font(richTextBox2.Font, FontStyle.Bold);
        richTextBox2.AppendText($"КРИТИЧЕСКАЯ ОШИБКА АНАЛИЗА: {ex.Message}\n");
    }
    finally
    {
        // Восстанавливаем стандартные настройки
        richTextBox2.SelectionColor = richTextBox2.ForeColor;
        richTextBox2.SelectionFont = richTextBox2.Font;
        richTextBox2.ScrollToCaret();
    }
}

private void HighlightErrorsInCode(List<string> errors)
{
    // Сначала сбросим всю подсветку
    richTextBox1.SelectAll();
    richTextBox1.SelectionBackColor = Color.White;
    
    // Регулярное выражение для поиска позиций в сообщениях об ошибках
    var errorPositions = new List<int>();
    var regex = new Regex(@"Позиция (\d+)");
    
    foreach (var error in errors)
    {
        var match = regex.Match(error);
        if (match.Success && int.TryParse(match.Groups[1].Value, out int pos))
        {
            errorPositions.Add(pos - 1); // Convert to 0-based index
        }
    }
    
    // Подсвечиваем все ошибочные позиции
    foreach (var pos in errorPositions.Distinct().OrderBy(p => p))
    {
        if (pos >= 0 && pos < richTextBox1.TextLength)
        {
            richTextBox1.Select(pos, 1);
            richTextBox1.SelectionBackColor = Color.LightPink;
        }
    }
    
    richTextBox1.Select(0, 0); // Снимаем выделение
}
        
        
        private void HighlightLexicalErrors(List<LexicalAnalyzer.Token> tokens)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
    
            foreach (var token in tokens.Where(t => t.Type == LexicalAnalyzer.TokenType.Invalid))
            {
                richTextBox1.Select(token.StartPos - 1, token.EndPos - token.StartPos + 1);
                richTextBox1.SelectionBackColor = Color.Pink;
            }
    
            richTextBox1.Select(0, 0);
        }

        private void HighlightSyntaxErrors(List<string> errors)
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
    
            foreach (var error in errors)
            {
                int posStart = error.IndexOf("Позиция") + 8;
                int posEnd = error.IndexOf(":");
                if (posStart > 7 && posEnd > posStart)
                {
                    if (int.TryParse(error.Substring(posStart, posEnd - posStart), out int pos))
                    {
                        richTextBox1.Select(pos - 1, 1);
                        richTextBox1.SelectionBackColor = Color.LightCoral;
                    }
                }
            }
    
            richTextBox1.Select(0, 0);
        }
        
    }
}