using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AESTestForLab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Раздел глоабльных переменных
        Aes myAes = Aes.Create();  // Экземпляр класса Aes
                                   // отвечает за шифрование и ключи
                                   // Здесь также генерируется ключ: сам ключ и вектор инициализации  (IV)
                                   // Объявляем и инициализируем в глобальных, чтобы иметь постоянный ключ

        // Алгоритм шифрования от Microsoft
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Пооверка входящих переменных на валидность
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Создается экземпляр класса Aes для использования ключа и IV
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key; // 
                aesAlg.IV = IV;

                // Работа производится с использованием потоков
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Возвращаем зашифрованные данные.
            return encrypted;
        }

        // Алгоритм расшифровки от Microsoft
        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Пооверка входящих переменных на валидность
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Строка для формирования расшифрованного текста
            string plaintext = null;

            // Создается экземпляр класса Aes для использования ключа и IV
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Работа производится с использованием потоков
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            // Возвращаем расшифрованные данные.
            return plaintext;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Строка для шифрования
            string original = "Привет от Астафьева Александра!";

            // Создаём новый экземпляк класса Aes
            
            //myAes = Aes.Create();
            
            // Выводим ключ в компоненты textBox
            textBox1.Text = Encoding.Unicode.GetString(myAes.Key);
            string keyInInt = "";
            for (int i = 0; i < myAes.Key.Length; i++)
            {
                keyInInt += myAes.Key[i].ToString()+", ";
            }
            textBox2.Text = keyInInt.Substring(0, keyInInt.Length - 2); // Убираем лишнюю запятую, а то меня раздражает

            // Выводим вектор инициализации в компоненты textBox
            textBox4.Text = Encoding.Unicode.GetString(myAes.IV);
            string IVInInt = "";
            for (int i = 0; i < myAes.IV.Length; i++)
            {
                IVInInt += myAes.IV[i].ToString() + ", ";
            }
            textBox3.Text = IVInInt.Substring(0, IVInInt.Length - 2); // Убираем лишнюю запятую, а то меня раздражает


            // Шифрование данных в массив байт.
            byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

            // Расшифрование массива байт в строку.
            string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

            // Вывод данных.
            listBox1.Items.Add("Исходный текст:       " + original);
            listBox1.Items.Add("Зашифрованный текст:  " + Encoding.Unicode.GetString(encrypted, 0, encrypted.Length));
            listBox1.Items.Add("Расшифрованный текст: " + roundtrip);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            myAes.GenerateIV();
            myAes.GenerateKey();
            // Выводим ключ в компоненты textBox
            textBox1.Text = Encoding.Unicode.GetString(myAes.Key);
            string keyInInt = "";
            for (int i = 0; i < myAes.Key.Length; i++)
            {
                keyInInt += myAes.Key[i].ToString() + ", ";
            }
            textBox2.Text = keyInInt.Substring(0, keyInInt.Length - 2); // Убираем лишнюю запятую, а то меня раздражает

            // Выводим вектор инициализации в компоненты textBox
            textBox4.Text = Encoding.Unicode.GetString(myAes.IV);
            string IVInInt = "";
            for (int i = 0; i < myAes.IV.Length; i++)
            {
                IVInInt += myAes.IV[i].ToString() + ", ";
            }
            textBox3.Text = IVInInt.Substring(0, IVInInt.Length - 2); // Убираем лишнюю запятую, а то меня раздражает
        }
    }
}
