using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;

public class EncryptionHelper
{
    private static string key;

    private static string GetEncryptionKeyFromDatabase()
    {
        string key = "";

        using (MySqlConnection connection = DatabaseHelper.GetConnection())
        {
            connection.Open();
            string query = "SELECT value FROM configurations WHERE name = @name";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@name", "google-app-pwd");
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    key = result.ToString();
                }
            }
        }

        return key;
    }


    static EncryptionHelper()
    {
        key = GetEncryptionKeyFromDatabase();
    }

    public static string Encrypt(string plainText)
    {
        byte[] iv = new byte[16];
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = iv;
            plainText = $"{plainText}|{Encoding.UTF8.GetString(iv)}";
            aesAlg.Padding = PaddingMode.Zeros;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }
                return BitConverter.ToString(msEncrypt.ToArray()).Replace("-", "");
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        using (AesManaged aesAlg = new AesManaged())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            string[] queryString = Encoding.UTF8.GetString(hexStringToByteArray(cipherText)).Split('|');
            byte[] iv = new byte[16];
            if (queryString.Length > 3)
            {
                iv = Encoding.UTF8.GetBytes(queryString[3]);
            }
            aesAlg.IV = iv;
            aesAlg.Padding = PaddingMode.Zeros;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(hexStringToByteArray(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    public static byte[] hexStringToByteArray(string hexString)
    {
        if (hexString == null)
        {
            return new byte[0];
        }

        int numberChars = hexString.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }
        return bytes;
    }
}
