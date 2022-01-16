using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AESHelper
{
    private string _key = "xxxxxxxxxxxxxxxxxxxxxxxx";
    private string _iv = "xxxxxxxxxxxxxxxxxxxxxxxx";
    private string _salt = "xxxxxxxxxxxxxxxxxxxxxxxx";

    public string AESEncrypt(string encryptString)
    {
        return Convert.ToBase64String(AESEncrypt(Encoding.Default.GetBytes(encryptString)));
    }

    public byte[] AESEncrypt(byte[] encryptByte)
    {
        if (encryptByte.Length == 0)
        {
            throw new Exception("明文不得为空");
        }

        if (string.IsNullOrEmpty(_key))
        {
            throw new Exception("密钥不得为空");
        }

        byte[] mStrEncrypt;
        byte[] mBtIv = Convert.FromBase64String(_iv);
        byte[] mSalt = Convert.FromBase64String(_salt);
        Rijndael mAesProvider = Rijndael.Create();
        try
        {
            MemoryStream mStream = new MemoryStream();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(_key, mSalt);
            ICryptoTransform transform = mAesProvider.CreateEncryptor(pdb.GetBytes(32), mBtIv);
            CryptoStream mCsstream = new CryptoStream(mStream, transform, CryptoStreamMode.Write);
            mCsstream.Write(encryptByte, 0, encryptByte.Length);
            mCsstream.FlushFinalBlock();
            mStrEncrypt = mStream.ToArray();
            mStream.Close();
            mStream.Dispose();
            mCsstream.Close();
            mCsstream.Dispose();
        }
        catch (IOException ex)
        {
            throw ex;
        }
        catch (CryptographicException ex)
        {
            throw ex;
        }
        catch (ArgumentException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            mAesProvider.Clear();
        }

        return mStrEncrypt;
    }

    public string AESDecrypt(string decryptString)
    {
        return Convert.ToBase64String(AESDecrypt(Encoding.Default.GetBytes(decryptString)));
    }

    public byte[] AESDecrypt(byte[] decryptByte)
    {
        if (decryptByte.Length == 0)
        {
            throw new Exception("密文不得为空");
        }

        if (string.IsNullOrEmpty(_key))
        {
            throw new Exception("密钥不得为空");
        }

        byte[] mStrDecrypt;
        byte[] mBtIv = Convert.FromBase64String(_iv);
        byte[] mSalt = Convert.FromBase64String(_salt);
        Rijndael mAesProvider = Rijndael.Create();
        try
        {
            MemoryStream mStream = new MemoryStream();
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(_key, mSalt);
            ICryptoTransform transform = mAesProvider.CreateDecryptor(pdb.GetBytes(32), mBtIv);
            CryptoStream mCsstream = new CryptoStream(mStream, transform, CryptoStreamMode.Write);
            mCsstream.Write(decryptByte, 0, decryptByte.Length);
            mCsstream.FlushFinalBlock();
            mStrDecrypt = mStream.ToArray();
            mStream.Close();
            mStream.Dispose();
            mCsstream.Close();
            mCsstream.Dispose();
        }
        catch (IOException ex)
        {
            throw ex;
        }
        catch (CryptographicException ex)
        {
            throw ex;
        }
        catch (ArgumentException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            mAesProvider.Clear();
        }

        return mStrDecrypt;
    }
}