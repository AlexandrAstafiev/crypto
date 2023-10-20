using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSATest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Создаём экземпляр класса для работы с криптографией
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
            
            // Выгуржаем ключи шифрования
            string publickey = RSA.ToXmlString(false); //получим открытый ключ
            string privatekey = RSA.ToXmlString(true); //получим закрытый ключ

            // выводим ключи на форму
            richTextBox1.Text = publickey;
            richTextBox2.Text = privatekey;

            RSACryptoServiceProvider Rsa = new RSACryptoServiceProvider(1024);
            Rsa.FromXmlString(publickey);
            byte[] EncryptedData;
            byte[] data = new byte[1024];
            data = Encoding.Unicode.GetBytes(textBox1.Text);
            EncryptedData = Rsa.Encrypt(data, false);

            textBox2.Text = Encoding.Unicode.GetString(EncryptedData);

            byte[] DecryptedData;


            DecryptedData = RSA.Decrypt(EncryptedData, false);

            textBox3.Text = Encoding.Unicode.GetString(DecryptedData);
        }
    }
}
