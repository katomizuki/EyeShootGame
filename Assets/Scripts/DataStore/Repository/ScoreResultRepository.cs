using System;
using System.Collections;
using System.Linq;
using System.Text.Json;
using UniRx;
using Domain.DTO;
using Domain.IRepository;
using UnityEngine.Networking;

namespace DataStore.Repository
{
    public sealed class ScoreResultRepository: IScoreResultRepository
    {
        private readonly string _apiUrl = "https://techtest.fly.dev/scores";

        public IEnumerator PostResult(IObserver<Unit> observer,float score, string name)
        {
                var jsonString = "{\"value\":" + score + ",\"player_name\":\"" + name + "\"}";
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
                UnityWebRequest.Post(_apiUrl, jsonString)
                    .SetRequestHeader("Content-Type", "application/json");
                var request = new UnityWebRequest(_apiUrl, "POST");
                request.timeout = 10;
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();
                        break;
                    case UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError:
                        observer.OnError(new Exception(request.error));
                        break;
                    case UnityWebRequest.Result.InProgress:
                        break;
                }
        }

        public IEnumerator GetResult(IObserver<ScoreDto[]> observer)
        {
            var request = UnityWebRequest.Get(_apiUrl);
            request.timeout = 10;
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    var result = JsonSerializer.Deserialize<ScoreResponseData[]>(request.downloadHandler.text)
                        .Select(data => new ScoreDto(data.PlayerName, data.Value)).ToArray();
                    observer.OnNext(result);
                    observer.OnCompleted();
                    break;
                case UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError or UnityWebRequest.Result.DataProcessingError:
                    observer.OnError(new Exception(request.error));
                    break;
                case UnityWebRequest.Result.InProgress:
                    break;
            }
        }
    }
}

