using System;
using System.IO;
using System.Security.Cryptography;

namespace AvaloniaUI.Controls.Banking;

public static class SecureDataHelper
{
    private static byte[] Key;
    private static byte[] IV;

    /// <summary>
    ///     Initializes the encryption key and IV.
    /// </summary>
    /// <param name="key">32-byte encryption key.</param>
    /// <param name="iv">16-byte initialization vector.</param>
    /// <exception cref="ArgumentException">Thrown if key or IV are invalid.</exception>
    public static void Initialize(byte[] key, byte[] iv)
    {
        if (key == null || key.Length != 32)
            throw new ArgumentException("Key must be 32 bytes.");
        if (iv == null || iv.Length != 16)
            throw new ArgumentException("IV must be 16 bytes.");

        Key = key;
        IV = iv;
    }

    /// <summary>
    ///     Encrypts a plain text string.
    /// </summary>
    /// <param name="plainText">The text to encrypt.</param>
    /// <returns>The encrypted text as a Base64 string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the helper is not initialized.</exception>
    public static string Encrypt(string plainText)
    {
        EnsureInitialized();

        if (string.IsNullOrEmpty(plainText))
            return null;

        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    ///     Decrypts an encrypted text string.
    /// </summary>
    /// <param name="cipherText">The encrypted text as a Base64 string.</param>
    /// <returns>The decrypted plain text.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the helper is not initialized.</exception>
    public static string Decrypt(string cipherText)
    {
        EnsureInitialized();

        if (string.IsNullOrEmpty(cipherText))
            return null;

        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    /// <summary>
    ///     Ensures that the helper has been initialized with a key and IV.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the helper is not initialized.</exception>
    private static void EnsureInitialized()
    {
        if (Key == null || IV == null)
            throw new InvalidOperationException(
                "SecureDataHelper has not been initialized. Call Initialize() with a valid key and IV before using this method.");
    }
}