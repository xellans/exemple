using System.Diagnostics;

namespace Binding_HotKey_Exemple
{
    public partial class Form1 : Form
    {
        public static List<HotKeysArrange> HotKeysLis = new(); //Список со всевозможными комбинациями, сделан статическим чтобы можно было задать его из других форм
        public List<Keys?> KeysListTemp = new(); //Временный список нажатых клавиш, нужен для проверки что было нажато.
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.KeyCode);
            if (!KeysListTemp.Contains(e.KeyCode) && HotKeysLis.Any(hotKey => hotKey.OneKey == e.KeyCode)) //Если первая нажатая клавиша равна первой клавиши любой комбинации, добавляем её
                KeysListTemp.Add(e.KeyCode);

            if (!KeysListTemp.Contains(e.KeyCode) && HotKeysLis.Any(hotKey => hotKey.TwoKey == e.KeyCode)) //Если вторая нажатая клавиша равна второй клавиши любой комбинации, добавляем её
                KeysListTemp.Add(e.KeyCode);

            if (!KeysListTemp.Contains(e.KeyCode) && HotKeysLis.Any(hotKey => hotKey.FreeKey == e.KeyCode)) //Если третья нажатая клавиша равна третьей клавиши любой комбинации, добавляем её
                KeysListTemp.Add(e.KeyCode);

            #region Проверка нажатых клавиш.
            if (KeysListTemp.Count == 1) // Проверяем что в нажатых клавишах для отлова находится одна клавиша
            {
                var temp = HotKeysLis.Where(hotKey => KeysListTemp.Contains(hotKey.OneKey) && hotKey.TwoKey == null && hotKey.FreeKey == null).FirstOrDefault();
                if (temp != null)
                {
                    temp.Action(); //Вызываем наш привязанный метод
                    KeysListTemp.Clear();
                }
            }

            if (KeysListTemp.Count == 2) // Проверяем что в нажатых клавишах для отлова находятся две клавиши
            {
                var temp = HotKeysLis.Where(hotKey => KeysListTemp.Contains(hotKey.OneKey) && KeysListTemp.Contains(hotKey.TwoKey) && hotKey.FreeKey == null).FirstOrDefault();
                if (temp != null)
                {
                    temp.Action();  //Вызываем наш привязанный метод
                    KeysListTemp.Clear();
                }
            }

            if (KeysListTemp.Count == 3) // Проверяем что в нажатых клавишах для отлова находятся три клавиши
            {
                var temp = HotKeysLis.Where(hotKey => KeysListTemp.Contains(hotKey.OneKey) && KeysListTemp.Contains(hotKey.TwoKey) && KeysListTemp.Contains(hotKey.FreeKey)).FirstOrDefault();
                if (temp != null)
                {
                    temp.Action(); //Вызываем наш привязанный метод
                    KeysListTemp.Clear();
                }
            }
            #endregion
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) => KeysListTemp.Clear(); //Если пользователь отпустил клавишу, удаляем все данные из списка.

        #region Методы для которых будут задаваться комбинации клавиш
        public void OneMessage() => MessageBox.Show(label1.Text);
        public void TwoMessage() => MessageBox.Show(label2.Text);
        public void FreeMessage() => MessageBox.Show(label3.Text);
        #endregion

        #region Передача методов для привязки их к горячим клавишам
        private void textBox1_Click(object sender, EventArgs e)
        {
            BindingHotkeyForm form = new(OneMessage, textBox1);
            form.Show();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            BindingHotkeyForm form = new(TwoMessage, textBox2);
            form.Show();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            BindingHotkeyForm form = new(FreeMessage, textBox3);
            form.Show();
        }
        #endregion
    }
    /// <summary>
    /// Модель для горячих клавиш, я ограничился тремя сочетаниями комбинаций
    /// </summary>
    public class HotKeysArrange
    {
        public Action Action { get; set; }
        public Keys? OneKey { get; set; }

        public Keys? TwoKey { get; set; }

        public Keys? FreeKey { get; set; }
    }
}
