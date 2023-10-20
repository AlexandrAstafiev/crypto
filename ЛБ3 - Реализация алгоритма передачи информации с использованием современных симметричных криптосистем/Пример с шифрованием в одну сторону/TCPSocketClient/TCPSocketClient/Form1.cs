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
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;

namespace TCPSocketClient
{
    public partial class Form1 : Form
    {
        private static Socket Client; // Создаем объект сокета-сервера

        private static IPHostEntry ipHost; // Класс для сведений об адресе веб-узла
        private static IPAddress ipAddr; // Предоставляет IP-адрес
        private static IPEndPoint ipEndPoint; // Локальная конечная точка

        private static Thread socketThread; // Создаем поток для поддержки потока
        private static Thread WaitingForMessage; // Создаем поток для приёма сообщений

        public static string gettedPublicKey = "";
        public static string PrivatecKey = "";

        // Создаем объект класса RSACryptoServiceProvider для работы с библиотекой шифрования
        // Инициализирует новый экземпляр класса RSACryptoServiceProvider с созданной случайным образом парой ключей указанного размера.
        // 1024 - размер ключей
        public static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        public static Aes myAes = Aes.Create();  // Экземпляр класса Aes
                                   // отвечает за шифрование и ключи
                                   // Здесь также генерируется ключ: сам ключ и вектор инициаизации (IV)
                                   // Объявляем и инициализируем в глобальных, чтобы иметь постоянный ключ
        public static bool keySended = false;
        // Переменная для хранения статуса отправки ключа
        // Если ключ отправлен, то будем считать весь дальнейший трафик шифрованным


        public Form1()
        {
            InitializeComponent();
        }
        private void startSocket()
        {
            // IP-адрес сервера, для подключения
            string HostName = tbAddress.Text;
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
                // Создаем сокет на текущей машине
                Client = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
                while (true)
                {
                    // Пытаемся подключиться к удаленной точке
                    Client.Connect(ipEndPoint);
                    if (Client.Connected) // Если клиент подключился
                    {
                        // Позеленим кнопочку для красоты, чтобы пользователь знал, что соединение установлено
                        bConnect.Invoke(new Action(() => bConnect.Text = "Подключение установлено"));
                        bConnect.Invoke(new Action(() => bConnect.BackColor = Color.Green));
                        // Создаем новый поток, указываем на ф-цию получения сообщений в классе Worker
                        WaitingForMessage = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(GetMessages));
                        // Запускаем, в параметрах передаем листбокс (история чата)
                        WaitingForMessage.Start(new Object[] { lbHistory, keyLabel, rtbOpenKey });
                    }
                    break;
                }
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

        }
        // Ф-ция, работающая в новом потоке: получение новых сообщенй ————
        public static void GetMessages(Object obj)
        {
            // Получаем объект истории чата (лист бокс)
            Object[] Temp = (Object[])obj;
            System.Windows.Forms.ListBox ChatListBox = (System.Windows.Forms.ListBox)Temp[0];
            
            
            // В бесконечном цикле получаем сообщения
            while (true)
            {
                // Ставим паузу, чтобы на время освобождать порт для отправки сообщений
                string Message="";
                byte[] byteMessage = null;
                Thread.Sleep(50);
                    try
                    {
                    //Message = GetDataFromServer();
                    byteMessage = GetByteDataFromServer();
                    Message = Encoding.Unicode.GetString(byteMessage);
                    if (keySended)
                        {
                            string specialText = "Пришло зашифрованное сообщение: " + Message;
                            Message = "(AES) ";
                            Message += DecryptStringFromBytes_Aes(byteMessage, myAes.Key, myAes.IV);
                        }
                        // Анализируем принимаемый пакет 
                        // Проверяем является ли сообщение ключём
                        
                    if   (Message.Length>14)  
                    if (Message.Substring(0, 13) == "<RSAKeyValue>")
                        {
                            // Проверим есть ли концовка ключа </RSAKeyValue>
                            Regex regex = new Regex("</RSAKeyValue>");
                            MatchCollection matches = regex.Matches(Message);
                            if (matches.Count == 1)
                            {
                                gettedPublicKey = Message;
                                RSA.FromXmlString(Message);
                                //SocketWorker.RSA.FromXmlString(SocketWorker.publicKeyXml);

                                // Заполняем label сообщением о получении ключа
                                Label keyLabel = (System.Windows.Forms.Label)Temp[1];
                                keyLabel.Invoke(new Action(() => keyLabel.Text = "Ключ получен"));
                                keyLabel.Invoke(new Action(() => keyLabel.ForeColor = Color.Green));

                                // Выводим в RichTextBox значение ключа
                                RichTextBox rtbOpenKey = (System.Windows.Forms.RichTextBox)Temp[2];
                                rtbOpenKey.Invoke(new Action(() => rtbOpenKey.Text = Message));

                            }
                            else
                            {
                                MessageBox.Show("Формат ключа не верен!");
                            }
                        }
                        else {
                          
                        //ChatListBox.Invoke(new Action(() => ChatListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Server: " + Message)));

                    }
                    ChatListBox.Invoke(new Action(() => ChatListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Server: " + Message)));

                }
                    catch (Exception e)
                {
                    // Выводим сообщение об ошибке
                    ChatListBox.Invoke(new Action(() => ChatListBox.Items.Add(DateTime.Now.ToShortTimeString() + " Server(Error): " + e.ToString())));
                }
            }
        }
        // Получение данных от сервера в виде строки
        public static string GetDataFromServer()
        {
            string GetInformation = "";

            // Создаем пустое “хранилище” байтов, куда будем получать информацию
            byte[] GetBytes = new byte[1024];
            int BytesRec = Client.Receive(GetBytes);
            if (keySended) // Если ключ получен, то трафик шифрованный
            { 
                
                string encryptedText = Encoding.Unicode.GetString(GetBytes, 0, BytesRec);
                //string decryptedText = DecryptStringFromBytes_Aes(GetBytes, myAes.Key, myAes.IV);
                GetInformation += encryptedText;
            }
            else // Если ключ не получен, то трафик открытый
            {
                // Переводим из массива битов в строку
                GetInformation += Encoding.Unicode.GetString(GetBytes, 0, BytesRec);
            }
            

            return GetInformation;
        }

        // Получение данных от сервера в виде массива байт
        public static byte[] GetByteDataFromServer()
        {
            string GetInformation = "";

            // Создаем пустое “хранилище” байтов, куда будем получать информацию
            byte[] GetBytes = new byte[1024];
            int BytesRec = Client.Receive(GetBytes);

            byte[] finalGetBytes = new byte[BytesRec];
            for (int i = 0; i < BytesRec; i++)
                finalGetBytes[i] = GetBytes[i];

            return finalGetBytes;
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
                aesAlg.Key = Key; 
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

                myAes.Key = Key;
                myAes.IV = IV;

                // Работа производится с использованием потоков
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);

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




        // Отправка информации на сервер
        public static void SendDataToServer(string Data)
        {
            // Используем unicode, чтобы не было проблем с кодировкой, при приеме информации
            byte[] SendMsg = Encoding.Unicode.GetBytes(Data);
            // Шифруем отправляемые данные
            //byte[] EncryptSendMsg = RSA.Encrypt(SendMsg, false);

            Client.Send(SendMsg);
        }
        // Функция запроса открытого ключа от сервера
        public static void SendRequestPublicKeyToServer()
        {
            byte[] SendMsg = Encoding.Unicode.GetBytes("OpenKeyRequest");
            Client.Send(SendMsg);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            socketThread = new Thread(new ThreadStart(startSocket));
            socketThread.IsBackground = true;
            socketThread.Start();
            bConnect.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Посылаем клиенту новое сообщение
            SendDataToServer(tbMessage.Text);
            // Добавляем в историю свое же сообщение + ник + время написания
            lbHistory.Items.Add(DateTime.Now.ToShortTimeString() + " Client:  " + tbMessage.Text.ToString());
            // Автопрокрутка истории чата
            lbHistory.TopIndex = lbHistory.Items.Count - 1;
            // Убираем текст из поля ввода
            tbMessage.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SendRequestPublicKeyToServer();
            // Разблокируем кнопку отправки ключа на сервер в надежде, что открытый ключ мы получили
            // По хорошему тут необходима проверка факта получения
            button3.Enabled = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // При закрытии приложения принудительно отключаем сокеты и потоки

            if (socketThread!=null)
                if (socketThread.IsAlive) 
                    socketThread.Abort();
            if (WaitingForMessage != null)
                if (WaitingForMessage.IsAlive)
                    WaitingForMessage.Abort();
            if (Client != null)
                if (Client.Connected)
                    Client.Close();

            Application.ExitThread();
            Application.Exit();
        }

        private void tbMessage_TextChanged(object sender, EventArgs e)
        {
            if (tbMessage.Text.Length > 57)
            { 
                MessageBox.Show("Максимальная длина текста 58 символов");
                tbMessage.Text = tbMessage.Text.Substring(0, 57);
            }
                
        }

        // Функция объединения ключа и верктора инициализации
        // Фо факту соединение массивов байт
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // Используем unicode, чтобы не было проблем с кодировкой, при приеме информации
            //byte[] SendMsg = Encoding.Unicode.GetBytes("222");

            // Шифруем отправляемые данные
            // Соединяем ключ и вектор инициализации в обно сообщение
            byte[] SendMsg = Combine(myAes.Key, myAes.IV);
            // Шифруем алгоритмом RSA
            byte[] EncryptSendMsg = RSA.Encrypt(SendMsg, false);
            lbHistory.Items.Add(DateTime.Now.ToShortTimeString() + " Client:  " + Encoding.Unicode.GetString(SendMsg, 0, SendMsg.Length));
            // Отправляем клиенту зашифрованный ключ
            Client.Send(EncryptSendMsg);
            keySended = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
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
            byte[] encrypted = EncryptStringToBytes_Aes(textToEncrypt, myAes.Key, myAes.IV);

            // Расшифрование массива байт в строку.
            // ВНИМАНИЕ! Используем полученные ключи, а не сгенерированные автоматически!!!
            string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

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
