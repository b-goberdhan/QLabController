﻿using DeviceInterface.Delegates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceInterface.Devices
{
    public abstract class Device<TData> : IDisposable
    {

        public event MessageRecievedHandler<TData> Recieved;
        protected abstract Func<string> BackgroundReciever { get; }
        protected bool IsConnected { get; private set; }
        public string Name { get; private set; }
        protected Device(string name)
        {
            Name = name;
        }

        protected virtual void Send(object message)
        {

        }
        protected virtual async Task SendAsync(object message)
        {
            
        }
        private void NotifyRecieved(TData data)
        {
            Recieved?.Invoke(this, data);
        }
        private TData ParseRecieved(string jsonString)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<TData>(jsonString);
                return response;
            }           
            catch
            {
                return default(TData);
            }
        }
        protected string ConvertToJson(TData data)
        {
            return JsonConvert.SerializeObject(data);
        }
        public virtual void Dispose()
        {
            IsConnected = false;
        }
        public virtual async void Connect()
        {
            IsConnected = true;
            await Task.Run(() =>
            {
                while(IsConnected)
                {
                    string json = BackgroundReciever();
                    TData response = ParseRecieved(json);
                    if (response != null)
                    {
                        NotifyRecieved(response);
                    }
                }
                
            });
        }
        public virtual void Disconnect()
        {
            IsConnected = false;
        }
        internal class BaseSensor
        {
            public string Name { get; set; }
            public object Data { get; set; }
        }
    }
}
