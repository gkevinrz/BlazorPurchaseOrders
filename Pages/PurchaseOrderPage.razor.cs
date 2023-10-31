using Microsoft.AspNetCore.Components;
using BlazorPurchaseOrders.Data;

namespace BlazorPurchaseOrders.Pages
{
    public  partial class PurchaseOrderPage: ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }



        /*------- Variables ------*/
        string pageTitle;

    }
}
