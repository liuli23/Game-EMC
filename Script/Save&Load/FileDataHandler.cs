using UnityEngine;
using System;
using System.IO;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    //���ܲ���
    private bool encryptData = false;
    private string codeWord = "limo";

    public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)//�����˼���ѡ��
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.encryptData = encryptData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToSave = JsonUtility.ToJson(data,true);

            if(encryptData) dataToSave = EncryptDecrypt(dataToSave);


            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("����ʧ�ܣ�" + fullPath + "\n" + e);
        }

    }


    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath,dataFileName);
        GameData loadData = null;

        if (File.Exists(fullPath)) {
            try
            {
                string dataToload = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToload = reader.ReadToEnd();
                    }
                }

                if(encryptData)dataToload = EncryptDecrypt(dataToload);

                loadData = JsonUtility.FromJson<GameData>(dataToload);
            }
            catch (Exception e)
            {
                Debug.LogError("��ȡ����" + fullPath + "\n" + e);
            }
        }
        return loadData;
    }


    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if(File.Exists(fullPath)) 
            File.Delete(fullPath);

    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifiedData;
    }

}
