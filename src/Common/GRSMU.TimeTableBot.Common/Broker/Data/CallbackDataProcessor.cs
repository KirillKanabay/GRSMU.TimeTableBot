﻿using Newtonsoft.Json;

namespace GRSMU.TimeTableBot.Common.Common.Handlers.Data
{
    public static class CallbackDataProcessor
    {
        public static string CreateCallbackData(CallbackData callbackData)
        {
            var value = JsonConvert.SerializeObject(callbackData);

            return value;
        }

        public static CallbackData ReadCallbackData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            var callbackData = JsonConvert.DeserializeObject<CallbackData>(data);

            return callbackData;
        }
    }
}
