using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Binding_HotKey_Exemple
{
    public partial class BindingHotkeyForm : Form
    {
        public Action action;
        HotKeysArrange TempHotKeysArrange = new HotKeysArrange(); //Временная модель для установки горячих клавиш
        private TextBox textBox; //Передаём  textBox из главной формы чтобы отобразить в нем комбинацию клавиш.

        public BindingHotkeyForm(Action action, TextBox textBox)
        {
            InitializeComponent();
            KeyPreview = true;
            this.action = action;
            TempHotKeysArrange.Action = action; //Устанавливаем метод который будет работать с заданной комбинацией клавиш
            this.textBox = textBox;
        }
        private void BindingHotkeyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (TempHotKeysArrange.OneKey == null)
            {
                TempHotKeysArrange.OneKey = e.KeyCode;
                textBox1.Text += e.KeyCode.ToString();
                textBox1.Select(textBox1.Text.Length, 0);
                return;
            }
            if (TempHotKeysArrange.TwoKey == null)
            {
                TempHotKeysArrange.TwoKey = e.KeyCode;
                textBox1.Text += $" + {e.KeyCode}";
                textBox1.Select(textBox1.Text.Length, 0);
                return;
            }
            if (TempHotKeysArrange.FreeKey == null)
            {
                TempHotKeysArrange.FreeKey = e.KeyCode;
                textBox1.Text += $" + {e.KeyCode}";
                textBox1.Select(textBox1.Text.Length, 0); //Устанавливаем курсор в конец строки, не красиво выглядит когда курсов мигает при каждом изменение в начале....
                return;
            }
        }
        private void button2_Click(object sender, EventArgs e) => Close(); // В случае отмены закрываем форму

        private void button1_Click(object sender, EventArgs e)
        {
            textBox.Text = $"{(TempHotKeysArrange.OneKey != null ? TempHotKeysArrange.OneKey : "")}" +
                           $"{(TempHotKeysArrange.TwoKey != null ? " + " + TempHotKeysArrange.TwoKey : "")}" +
                           $"{(TempHotKeysArrange.FreeKey != null ? " + " + TempHotKeysArrange.FreeKey : "")}";
            textBox.Select(textBox.Text.Length, 0);
            Form1.HotKeysLis.RemoveAll(x => x.Action == action); //Удаляем старые комбинации клавиш для данной привязки
            Form1.HotKeysLis.Add(TempHotKeysArrange); // Добавляем новое сочетание клавиш
            Close();
        }
    }
}
