using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        // 싱글톤 처리
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static string FilePath => Application.persistentDataPath;
    private void Start()
    {
        Debug.Log("파일 저장 위치는 " + FilePath + "입니다.");
    }

    //파일 존재 여부 반환
    private bool isFileExisting(string path)
    {
        return File.Exists(path);
    }

    //디렉터리 존재 여부 반환
    private bool isDirectoryExisting(string path)
    {
        return Directory.Exists(path);
    }

    //디렉터리 생성
    private void createDirectory(string path)
    {
        if (!isDirectoryExisting(path))
            Directory.CreateDirectory(path);
    }

    //Text 파일 저장
    public void saveText(string relativePath, string data)
    {
        string fullPath = Path.Combine(FilePath, relativePath);
        createDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllText(fullPath, data, Encoding.UTF8);
    }

    //Text 파일 로드
    public string loadText(string relativePath)
    {
        string fullPath = Path.Combine(FilePath, relativePath);
        if (!isFileExisting(fullPath))
            return null;

        return File.ReadAllText(fullPath, Encoding.UTF8);
    }

    //파일 삭제
    public void deleteFile(string relativePath)
    {
        string fullPath = Path.Combine(FilePath, relativePath);
        if (isFileExisting(fullPath))
            File.Delete(fullPath);
    }

    //디렉터리 삭제
    public void deleteAllData()
    {
        if (isDirectoryExisting(FilePath))
            Directory.Delete(FilePath, true);
    }

    public static void ClearAllFiles()
    {
        string directoryPath = Application.persistentDataPath;

        if (Directory.Exists(directoryPath))
        {
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
    }

    //파일 용량 반환
    public long getFileSize(string relativePath)
    {
        string fullPath = Path.Combine(FilePath, relativePath);
        if (!isFileExisting(fullPath))
            return 0;

        return new FileInfo(fullPath).Length;
    }

    //JSON을 파일로 저장
    public void saveJson<T>(string relativePath, T data)
    {
        string fullPath = Path.Combine(FilePath, relativePath);
        string directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directory))
            createDirectory(directory);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(fullPath, json, Encoding.UTF8);
    }

    //파일의 JSON 반환
    public T loadJson<T>(string relativePath) where T : new()
    {
        string fullPath = Path.Combine(FilePath, relativePath);

        if (!isFileExisting(fullPath))
            return new T();

        string json = File.ReadAllText(fullPath, Encoding.UTF8);
        return JsonUtility.FromJson<T>(json);
    }

    //파일의 JSON을 불러올 수 있는지 체크
    public bool tryLoadJson<T>(string relativePath, out T data)
    {
        string fullPath = Path.Combine(FilePath, relativePath);

        if (!isFileExisting(fullPath))
        {
            data = default;
            return false;
        }

        string json = File.ReadAllText(fullPath, Encoding.UTF8);
        data = JsonUtility.FromJson<T>(json);
        return true;
    }

    //JSON 파일 삭제
    public void deleteJson(string relativePath)
    {
        string fullPath = Path.Combine(FilePath, relativePath);

        if (isFileExisting(fullPath))
            File.Delete(fullPath);
    }

    public List<List<string>> LoadCSV(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(path);

        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + path);
            return null;
        }

        string[] lines = csvFile.text.Trim().Split('\n');

        if (lines.Length == 0)
            return null;

        string[] firstRow = lines[0].Split(',');
        int rowCount = lines.Length;
        int colCount = firstRow.Length;

        string[,] result = new string[rowCount,colCount];
        string loger = "";

        for (int i = 0; i < rowCount; i++)
        {
            string[] columns = lines[i].Trim().Split(',');
            for (int j = 0; j < colCount; j++)
            {
                if (j < columns.Length)
                    result[i, j] = columns[j];
                else
                    result[i, j] = "null";
                loger += result[i, j] + ",";
            }
            loger += "\n";
        }
        Debug.Log(loger);

        List<List<string>> result2 = Utility.ConvertToList(result);

        return result2;
    }

    // ** 게임 데이터 초기화
    public void ResetGameData()
    {
        ClearAllFiles();
    }
}
