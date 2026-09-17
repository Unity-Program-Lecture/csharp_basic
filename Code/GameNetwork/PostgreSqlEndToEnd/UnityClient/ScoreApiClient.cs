using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreApiClient : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;

    // 서버와 같은 PC에서 시험할 때만 사용하는 로컬 주소입니다.
    private const string ServerUrl = "http://127.0.0.1:5080";

    public void SendDemoScore()
    {
        StartCoroutine(SendScoreCoroutine());
    }

    private IEnumerator SendScoreCoroutine()
    {
        ScoreRequest requestBody = new ScoreRequest();
        requestBody.playerId = 101;
        requestBody.score = 250;

        string json = JsonUtility.ToJson(requestBody);
        byte[] body = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request =
               new UnityWebRequest(ServerUrl + "/scores", UnityWebRequest.kHttpVerbPOST))
        {
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isHttpError || request.isNetworkError)
#endif
            {
                resultText.text = "요청 실패: " + request.error;
                yield break;
            }

            ScoreResponse response =
                JsonUtility.FromJson<ScoreResponse>(request.downloadHandler.text);

            resultText.text = "저장 완료\nPlayerId: " + response.playerId +
                              "\nScore: " + response.score +
                              "\nUpdatedAt: " + response.updatedAt;
        }
    }

    [System.Serializable]
    private class ScoreRequest
    {
        public int playerId;
        public int score;
    }

    [System.Serializable]
    private class ScoreResponse
    {
        public int playerId;
        public int score;
        public string updatedAt;
    }
}
