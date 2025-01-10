using System;
using System.ComponentModel;

namespace AvaloniaUI.Controls.Banking;

public class SecureCreditCard : IDisposable, INotifyPropertyChanged
{
    // Timeout logic variables
    private readonly DateTime _creationTime;

    // Flag to track if the object has been disposed
    private bool _disposed;

    private string _encryptedCardholderName;

    // Encrypted Fields (stored in Base64)
    private string _encryptedCardNumber;
    private string _encryptedCvv;
    private string _encryptedExpirationDate;
    private readonly TimeSpan _timeoutDuration = TimeSpan.FromMinutes(5);

    public SecureCreditCard()
    {
        // Initialize the timeout
        _creationTime = DateTime.UtcNow;
    }

    // Constructor
    public SecureCreditCard(string cardNumber, string expirationDate, string cvv, string cardholderName) : this()
    {
        // Store encrypted values
        CardNumber = cardNumber;
        ExpirationDate = expirationDate;
        CVV = cvv;
        CardholderName = cardholderName;
    }

    // Properties with property change notification
    public string CardNumber
    {
        get => Decrypt(_encryptedCardNumber);
        set
        {
            if (Decrypt(_encryptedCardNumber).Replace(" ", "") != value.Replace(" ", ""))
            {
                _encryptedCardNumber = Encrypt(value.Replace(" ", ""));
                OnPropertyChanged(nameof(CardNumber));
            }
        }
    }

    public string ExpirationDate
    {
        get => Decrypt(_encryptedExpirationDate);
        set
        {
            if (Decrypt(_encryptedExpirationDate) != value)
            {
                _encryptedExpirationDate = Encrypt(value);
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }
    }

    public string CVV
    {
        get => Decrypt(_encryptedCvv);
        set
        {
            if (Decrypt(_encryptedCvv) != value)
            {
                _encryptedCvv = Encrypt(value);
                OnPropertyChanged(nameof(CVV));
            }
        }
    }

    public string CardholderName
    {
        get => Decrypt(_encryptedCardholderName);
        set
        {
            if (Decrypt(_encryptedCardholderName) != value)
            {
                _encryptedCardholderName = Encrypt(value);
                OnPropertyChanged(nameof(CardholderName));
            }
        }
    }

    // Dispose method
    public void Dispose()
    {
        if (_disposed)
            return;

        // Clear sensitive data
        ResetData();

        // Zero out the fields to avoid memory leakage
        ZeroOutMemory(_encryptedCardNumber);
        ZeroOutMemory(_encryptedExpirationDate);
        ZeroOutMemory(_encryptedCvv);
        ZeroOutMemory(_encryptedCardholderName);

        _disposed = true;

        GC.SuppressFinalize(this);
    }

    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;

    // Method to raise PropertyChanged event
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Method to encrypt the data
    private string Encrypt(string plainText)
    {
        return SecureDataHelper.Encrypt(plainText);
    }

    // Method to decrypt the data
    private string Decrypt(string encryptedText)
    {
        // Check for timeout condition
        if (DateTime.UtcNow - _creationTime > _timeoutDuration)
            throw new InvalidOperationException("The credit card data has expired due to timeout.");

        return SecureDataHelper.Decrypt(encryptedText);
    }

    // Method to reset and clear sensitive data securely
    public void ResetData()
    {
        _encryptedCardNumber = null;
        _encryptedExpirationDate = null;
        _encryptedCvv = null;
        _encryptedCardholderName = null;

        OnPropertyChanged(nameof(CardNumber));
        OnPropertyChanged(nameof(ExpirationDate));
        OnPropertyChanged(nameof(CVV));
        OnPropertyChanged(nameof(CardholderName));
    }

    // Helper method to zero out memory
    private static void ZeroOutMemory(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            var charArray = data.ToCharArray();
            Array.Clear(charArray, 0, charArray.Length);
        }
    }

    // Finalizer
    ~SecureCreditCard()
    {
        Dispose();
    }
}