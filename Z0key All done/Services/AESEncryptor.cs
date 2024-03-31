using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;

namespace Z0key.Services
{
    public class AESEncryptor
    {


        public byte[] Key { get; set; }

        public void EncryptFile(string inputFile)
        {

            string directory = Path.GetDirectoryName(inputFile);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile);
            string fileExtension = Path.GetExtension(inputFile);
            string encryptFileName = $"{fileNameWithoutExtension}_encrypt{fileExtension}";

            if (fileNameWithoutExtension.Contains("_decrypt"))
            { 
                encryptFileName = $"{fileNameWithoutExtension.Replace("_decrypt","_encrypt")}{fileExtension}";
            }

            
            string newFilePath = Path.Combine(directory, encryptFileName);

            try
            {
                using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open))
                using (FileStream encryptedFileStream = new FileStream($"{newFilePath}", FileMode.Create))
                {
                    Console.WriteLine($"input file length {inputFileStream.Length}");
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = Key;

                        byte[] iv = aes.IV;
                        encryptedFileStream.Write(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(
                                   encryptedFileStream,
                                   aes.CreateEncryptor(),
                                   CryptoStreamMode.Write))
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;
                            while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cryptoStream.Write(buffer, 0, bytesRead);
                            }

                            Console.WriteLine($"encrypt file length {encryptedFileStream.Length}");
                        }
                    }
                }
                File.Delete(inputFile);

                Console.WriteLine("File encrypted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Encryption failed. {ex}");
            }
        }

        public void DecryptFile(string encryptedFile)
        {

            string directory = Path.GetDirectoryName(encryptedFile);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(encryptedFile);
            string fileExtension = Path.GetExtension(encryptedFile);

            string decryptFileName = $"{fileNameWithoutExtension.Replace("_encrypt", "_decrypt")}{fileExtension}";
            string newFilePath = Path.Combine(directory, decryptFileName);

            try
            {
                using (FileStream encryptedFileStream = new FileStream(encryptedFile, FileMode.Open))
                {
                    Console.WriteLine($"encrypt file length {encryptedFileStream.Length}");
                    using (Aes aes = Aes.Create())
                    {
                        byte[] iv = new byte[aes.IV.Length];
                        encryptedFileStream.Read(iv, 0, iv.Length);
                        aes.Key = Key;
                        aes.IV = iv;

                        Console.WriteLine($"IV length {aes.IV.Length}");

                        using (CryptoStream cryptoStream = new CryptoStream(
                                   encryptedFileStream,
                                   aes.CreateDecryptor(),
                                   CryptoStreamMode.Read))
                        {
                            using (FileStream decryptedFileStream = new FileStream(newFilePath, FileMode.Create))
                            {

                                byte[] buffer = new byte[1024];
                                int bytesRead;
                                while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    decryptedFileStream.Write(buffer, 0, bytesRead);
                                }



                                Console.WriteLine($"decrypt file length {decryptedFileStream.Length}");
                                Console.WriteLine(decryptFileName);
                            }
                        }
                    }
                }
                File.Delete(encryptedFile);

                Console.WriteLine("File decrypted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Decryption failed. {ex}");
            }
        }
    }
}
