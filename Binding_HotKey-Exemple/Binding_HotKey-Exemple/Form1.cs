using System.Diagnostics;

namespace Binding_HotKey_Exemple
{
    public partial class Form1 : Form
    {
        public static List<HotKeysArrange> HotKeysLis = new(); //������ �� ������������� ������������, ������ ����������� ����� ����� ���� ������ ��� �� ������ ����
        public List<Keys?> KeysListTemp = new(); //��������� ������ ������� ������, ����� ��� �������� ��� ���� ������.
        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.KeyCode);
            if (!KeysListTemp.Contains(e.KeyCode) && HotKeysLis.Any(hotKey => hotKey.OneKey == e.KeyCode)) //���� ������ ������� ������� ����� ������ ������� ����� ����������, ��������� �
                KeysListTemp.Add(e.KeyCode);

            if (!KeysListTemp.Contains(e.KeyCode) && HotKeysLis.Any(hotKey => hotKey.TwoKey == e.KeyCode)) //���� ������ ������� ������� ����� ������ ������� ����� ����������, ��������� �
                KeysListTemp.Add(e.KeyCode);

            if (!KeysListTemp.Contains(e.KeyCode) && HotKeysLis.Any(hotKey => hotKey.FreeKey == e.KeyCode)) //���� ������ ������� ������� ����� ������� ������� ����� ����������, ��������� �
                KeysListTemp.Add(e.KeyCode);

            #region �������� ������� ������.
            if (KeysListTemp.Count == 1) // ��������� ��� � ������� �������� ��� ������ ��������� ���� �������
            {
                var temp = HotKeysLis.Where(hotKey => KeysListTemp.Contains(hotKey.OneKey) && hotKey.TwoKey == null && hotKey.FreeKey == null).FirstOrDefault();
                if (temp != null)
                {
                    temp.Action(); //�������� ��� ����������� �����
                    KeysListTemp.Clear();
                }
            }

            if (KeysListTemp.Count == 2) // ��������� ��� � ������� �������� ��� ������ ��������� ��� �������
            {
                var temp = HotKeysLis.Where(hotKey => KeysListTemp.Contains(hotKey.OneKey) && KeysListTemp.Contains(hotKey.TwoKey) && hotKey.FreeKey == null).FirstOrDefault();
                if (temp != null)
                {
                    temp.Action();  //�������� ��� ����������� �����
                    KeysListTemp.Clear();
                }
            }

            if (KeysListTemp.Count == 3) // ��������� ��� � ������� �������� ��� ������ ��������� ��� �������
            {
                var temp = HotKeysLis.Where(hotKey => KeysListTemp.Contains(hotKey.OneKey) && KeysListTemp.Contains(hotKey.TwoKey) && KeysListTemp.Contains(hotKey.FreeKey)).FirstOrDefault();
                if (temp != null)
                {
                    temp.Action(); //�������� ��� ����������� �����
                    KeysListTemp.Clear();
                }
            }
            #endregion
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) => KeysListTemp.Clear(); //���� ������������ �������� �������, ������� ��� ������ �� ������.

        #region ������ ��� ������� ����� ���������� ���������� ������
        public void OneMessage() => MessageBox.Show(label1.Text);
        public void TwoMessage() => MessageBox.Show(label2.Text);
        public void FreeMessage() => MessageBox.Show(label3.Text);
        #endregion

        #region �������� ������� ��� �������� �� � ������� ��������
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
    /// ������ ��� ������� ������, � ����������� ����� ����������� ����������
    /// </summary>
    public class HotKeysArrange
    {
        public Action Action { get; set; }
        public Keys? OneKey { get; set; }

        public Keys? TwoKey { get; set; }

        public Keys? FreeKey { get; set; }
    }
}
