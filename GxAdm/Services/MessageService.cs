using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlazorBootstrap;

using GxAdm.Components;

namespace GxAdm.Services
{
    public interface IMessageService
    {
        void Show(string text, ToastType type = ToastType.Info, int? durationSeconds = 5);
        //void Show(string text, ToastType type = ToastType.Info);
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
        //public void Show(string text, ToastType type = ToastType.Info)
        //{
        //    // ✅ CORRECT: Matches GlobalMessageHost.TriggerMessage(string, ToastType)
        //    GlobalMessageHost.TriggerMessage(text, type);
        //}
        //public void Show(string text, ToastType type = ToastType.Info)
        //{
        //    var msg = new GlobalMessageHost.PageMessage { Text = text, Type = type };
        //    GlobalMessageHost.Trigger(msg);
        //}
        //public void Show(string text, ToastType type)
        //{
        //    var msg = new GlobalMessageHost.PageMessage { Text = text, Type = type };
        //    using var scope = _services.CreateScope();
        //    var toaster = scope.ServiceProvider.GetRequiredService<IMessageHostToaster>();  // See below
        //    toaster?.Show(msg);  // Or use events/CascadingValue notifier
        //}
    }
}
