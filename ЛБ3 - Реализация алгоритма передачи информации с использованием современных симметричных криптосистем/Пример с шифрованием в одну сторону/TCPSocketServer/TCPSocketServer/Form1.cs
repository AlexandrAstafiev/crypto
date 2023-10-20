using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Добавляем пространства имен для работы сокетов
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Security.Cryptography;
using System.IO;

namespace TCPSocketServer
{
    public partial class Form1 : Form
    {
        // Раздел глобальных переменных
        private static Socket Server; // Создаем объект сокета-сервера
        private static Socket Handler; // Создаем объект вспомогательного сокета

        private static IPHostEntry ipHost; // Класс для сведений об адресе веб-узла
        private static IPAddress ipAddr; // Предоставляет IP-адрес
        private static IPEndPoint ipEndPoint; // Локальная конечная точка

        private static Thread socketThread; // Создаем поток для поддержки потока
        private static Thread WaitingForMessage; // Создаем поток для приёма сообщений

        // Создаем объект класса RSACryptoServiceProvider для работы с библиотекой шифрования
        // Инициализирует новый экземпляр класса RSACryptoServiceProvider с созданной случайным образом парой ключей указанного размера.
        // 1024 - размер ключей
        public static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        public static Aes myAes = Aes.Create();  // Экземпляр класса Aes
                                          // отвечает за шифрование и ключи
                                          // Здесь также генерируется ключ: сам ключ и вектор инициаизации (IV)
                                          // Объявляем и инициализируем в глобальных, чтобы иметь постоянный ключ
        private static byte[] AESKey = new byte[32];
        private static byte[] AESIV = new byte[16];

        // Переменная отслеживания существования ключа
        private static bool keyExist = false;


        private static byte[] encryptedData; // Переменная для хранения зашифрованных сообщений для вывода на экран

        public Form1()
        {
            InitializeComponent();
        }

        // Функция запуска сокета
        private void startSocket()
        {
            // IP-адрес сервера, для подключения
            string HostName = "0.0.0.0"; 
            // Порт подключения
            string Port = tbPort.Text;

            // Разрешает DNS-имя узла или IP-адрес в экземпляр IPHostEntry.
            ipHost = Dns.Resolve(HostName);
            // Получаем из списка адресов первый (адресов может быть много)
            ipAddr = ipHost.AddressList[0];
            // Создаем конечную локальную точку подключения на каком-то порту
            ipEndPoint = new IPEndPoint(ipAddr, int.Parse(Port));

            try
            {
                // Создаем сокет сервера на текущей машине
                Server = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            }
            catch (SocketException error)
            {
                // 10061 — порт подключения занят/закрыт
                if (error.ErrorCode == 10061)
                {
                    MessageBox.Show("Порт подключения закрыт!");
                    Application.Exit();
                }
            }

            // Ждем подключений
            try
            {
                // Связываем удаленную точку с сокетом
                Server.Bind(ipEndPoint);
                // Не более 10 подключения на сокет
                Server.Listen(10);

                // Начинаем "прослушивать" подключения
                while (true)
                {
                    Handler = Server.Accept();
                    if (Handler.Connected)
                    {
                        // Позеленим кнопочку для красоты, чтобы пользователь знал, что соединение установлено
                        bConnect.Invoke(new Action(() => bConnect.Text = "Клиент подключен"));
                        bConnect.Invoke(new Action(() => bConnect.BackColor = Color.Green));
                        // Создаем новый поток, указываем на ф-цию получения сообщений в классе Worker
                        WaitingForMessage = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(GetMessages));
                        // Запускаем, в параметрах передаем листбокс (история чата)
                        WaitingForMessage.Start(new Object[] { lbHistory });
                    }
                    break;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Проблемы с подключением");
            }

        }

        // Ф-ция, работающая в новом потоке: получение новых сообщений ————
        public static void GetMessages(Object obj)
        {
            // Получаем объект истории чата (лист бокс)
            Object[] Temp = (Object[])obj;
            System.Windows.Forms.ListBox ChatListBox = (System.Windows.Forms.ListBox)Temp[0];

            // В бесконечном цикле получаем сообщения
            while (true)
            {
                // Ставим паузу, чтобы на время освобождать порт для отправки сообщений
                Thread.Sleep(50);

                    try
                    {
                        // Получаем сообщение от клиента
                        string Message = GetDataFromClient();
                        // Добавляем в историю + текущее время если это не запрос ключа
                        if (Message!= "OpenKeyRequest")
                            ChatListBox.Invoke(new Action(() => ChatListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Client:  " + Message)));
                    }
                    catch { }
                
            }
        }
        /*
         Функционал криптографии AES
         
         */
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

        /*
         Конец функционала криптографии AES
         */
        // Отправка информации на клиент
        public static void SendDataToClient(string Data)
        {
            byte[] SendMsg=null;
            if (keyExist)
            {
                SendMsg = EncryptStringToBytes_Aes(Data, AESKey, AESIV);
                encryptedData = SendMsg;
            }
            else 
            {
                SendMsg = Encoding.Unicode.GetBytes(Data);
            }
            
            Handler.Send(SendMsg);
        }

        // Получение информации от клиента
        public static string GetDataFromClient()
        {
            string GetInformation = "";

            byte[] GetBytes = new byte[128]; // 128 т.к. RSA по умолчанию работает с блоками такой длинны
            // При 128 байтах только 58 символов допустимо
            int BytesRec = Handler.Receive(GetBytes);
            GetInformation += Encoding.Unicode.GetString(GetBytes, 0, BytesRec);
            
            // -----------Обработка запроса ключа--------------------------
            if (GetInformation == "OpenKeyRequest")  // Получили запрос на открытый ключ
            {
                //MessageBox.Show("Получен запрос на получение открытого ключа");
                // Выгружаем ключ 
                // Функция ToXmlString выгружает ключ текущего объекта
                // false, чтобы экспортировать информацию об открытом ключе или передать
                // true для экспорта информации об открытом и закрытом ключах.
                string publicKeyXml = RSA.ToXmlString(false);
                // Посылаем открытый ключ
                byte[] SendMsg = Encoding.Unicode.GetBytes(publicKeyXml);
                Handler.Send(SendMsg);
            }
            else // Если это не ключ, значит это сообщение
            {
                // Расшифровываем полученное сообщение
                byte[] dataToDecrypt = Encoding.Unicode.GetBytes(GetInformation);
                try
                {
                    byte[] decryptedData = RSA.Decrypt(GetBytes, false);
                    // Если длина сообщения равна 48 байтам, то возможно это ключ
                    if (decryptedData.Length == 48)
                    {
                        // Пробуем разделить сообщение на ключ и вектор инициализации
                        for (int i = 0; i < 48; i++)
                            if (i < 32)
                                AESKey[i] = decryptedData[i];
                            else
                                AESIV[i - 32] = decryptedData[i];
                        GetInformation = "Получен ключ симметричного шифрования";
                        
                        keyExist = true;
                    }
                }
                catch {
                }
                





            }
            // -----------Обработка запроса ключа--------------------------


            return GetInformation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            socketThread = new Thread(new ThreadStart(startSocket));
            socketThread.IsBackground = true;
            socketThread.Start();
            bConnect.Enabled = false;
            bConnect.Text = "Ожидание подключения";
            bConnect.BackColor = Color.Yellow;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (AESKey.ToString() != "") // Проверяем получен ли ключ симметричного шифрования
            {
                // Посылаем клиенту новое сообщение
                SendDataToClient(tbMessage.Text);
                // Добавляем в историю свое же сообщение + ник + время написания
                lbHistory.Items.Add(DateTime.Now.ToShortTimeString() + " Server (AES): " + tbMessage.Text.ToString());
                // Автопрокрутка истории чата
                lbHistory.TopIndex = lbHistory.Items.Count - 1;
                //lbHistory.Items.Add(DateTime.Now.ToShortTimeString() + " Server (Crypto): " + Encoding.Unicode.GetString(encryptedData));
                // Убираем текст из поля ввода
                tbMessage.Text = "";
            }
            else
            {
                MessageBox.Show("Ключ симметричного шифрования не доступен.");
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // При закрытии приложения принудительно отключаем сокеты и потоки

            if (WaitingForMessage != null)
                if (WaitingForMessage.IsAlive)
                    WaitingForMessage.Abort();
            if (socketThread != null)
                if (socketThread.IsAlive)
                    socketThread.Abort();
            if (Server != null)
                if (Server.Connected)
                    Server.Close();
            if (Handler != null)
                if (Handler.Connected)
                    Handler.Close();

            Application.ExitThread();
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            myAes.Key = AESKey;
            myAes.IV = AESIV;

            string keyInInt = "";
            for (int i = 0; i < myAes.Key.Length; i++)
            {
                keyInInt += myAes.Key[i].ToString() + ", ";
            }
            textBox1.Text = keyInInt.Substring(0, keyInInt.Length - 2); // Убираем лишнюю запятую, а то меня раздражает

            // Выводим вектор инициализации
            string IVInInt = "";
            for (int i = 0; i < myAes.IV.Length; i++)
            {
                IVInInt += myAes.IV[i].ToString() + ", ";
            }
            textBox2.Text = IVInInt.Substring(0, IVInInt.Length - 2); // Убираем лишнюю запятую, а то меня раздражает



            string textToEncrypt = "Привет от Астафьева Александра! Это текст для симметричного шифрования";
            // Шифрование данных в массив байт.
            // ВНИМАНИЕ! Используем полученные ключи, а не сгенерированные автоматически!!!
            byte[] encrypted = EncryptStringToBytes_Aes(textToEncrypt, AESKey, AESIV);

            // Расшифрование массива байт в строку.
            // ВНИМАНИЕ! Используем полученные ключи, а не сгенерированные автоматически!!!
            string roundtrip = DecryptStringFromBytes_Aes(encrypted, AESKey, AESIV);

            // Вывод данных.
            lbHistory.Items.Add("Исходный текст:       " + textToEncrypt);
            lbHistory.Items.Add("Зашифрованный текст:  " + Encoding.Unicode.GetString(encrypted, 0, encrypted.Length));
            lbHistory.Items.Add("Расшифрованный текст: " + roundtrip);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //myAes.Key = Encoding.Unicode.GetBytes("Password");
        }
    }
}
