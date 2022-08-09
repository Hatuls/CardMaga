using UnityEngine;


namespace Rei.Utilities
{
    public static class VectorHelper
    {
        public static float GetRandomValue(this Vector2 vector2)
            => Random.Range(vector2.x, vector2.y);
    }

    public static class JsonUtilityConverter
    {


        public static string ConvertToJson(object data, bool print = true)
        {
            if (data == null)
                throw new System.Exception($"JsonConverter : object parameter is null!");

            var jsonString = JsonUtility.ToJson(data);
            if (print)
                Debug.Log(jsonString);
            return jsonString;
        }

        public static T ConvertFromJson<T>(string jsonData) where T: class
        {
            if (jsonData == null || jsonData.Length == 0)
                throw new System.Exception($"The String JsonData was not assigned or the length was 0!");

            T dataFromJson = JsonUtility.FromJson<T>(jsonData);

            if (dataFromJson == null)
                throw new System.Exception($"Json Was Not able to convert to {typeof(T)}!");

            return dataFromJson as T;
        }
    }
}