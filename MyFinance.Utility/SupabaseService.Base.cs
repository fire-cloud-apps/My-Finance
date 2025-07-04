using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MyFinance.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Utility
{
    public partial class SupabaseService : IAsyncDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ISnackbar _snackbar;
        private readonly IDialogService _dialogService;
        private readonly NavigationManager _navigationManager;
        private DotNetObjectReference<SupabaseService>? _dotNetRef;

        public event Action<JsSession?, string?>? OnAuthStateChanged;
        public JsSession? CurrentSession { get; private set; }

        public SupabaseService(IJSRuntime jsRuntime, ISnackbar snackbar, IDialogService dialogService, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _snackbar = snackbar;
            _dialogService = dialogService;
            _navigationManager = navigationManager;
        }

        public async Task InitializeAsync()
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("supabaseInterop.setServiceReference", _dotNetRef);
        }

    }
}
