using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlazorBootstrap;

using GxPilo.Components;

namespace GxPilo.Services
{
    public interface IMessageService
    {
        void Show(string text, ToastType type = ToastType.Info, int? durationSeconds = 5);
    }

    // MessageService.cs (inject IJSRuntime if adding dismiss)
    public class MessageService : IMessageService
    {
        private readonly IServiceProvider _services;  // For scoped container
        public MessageService(IServiceProvider services) => _services = services;

        public void Show(string text, ToastType type = ToastType.Info, int? durationSeconds = 5)
        {
            GlobalMessageHost.TriggerMessage(text, type, durationSeconds);
        } 
    }
}
