using Microsoft.AspNetCore.Components;

namespace GxStk.Components.Paccu.Valida
{
    public class EnrolFormGridBase<TItem> : ComponentBase
    {
        [Parameter] public EventCallback<TItem> RowSelected { get; set; }
        protected string TiwRenderKey => $"tiw-{Guid.NewGuid()}";
    }

    public class GridHeaderActionsContext
    {
        public EventCallback OnAdd { get; set; }
        public EventCallback OnSave { get; set; }
        public EventCallback OnCancel { get; set; }
        public bool IsAddMode { get; set; }
        public bool IsEditMode { get; set; }
    }
}
