using Microsoft.AspNetCore.Components;
using BlazorPurchaseOrders.Data;
using Syncfusion.Blazor.Navigations;
using System.Diagnostics;

namespace BlazorPurchaseOrders.Pages
{
    public partial   class Index:ComponentBase
    {
        [Inject] IPOHeaderService POHeaderService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }



        // Create an empty list, named poheader, of empty POHeader objects.
        IEnumerable<POHeader> poheader;

        private List<ItemModel> Toolbaritems = new List<ItemModel>();

        int POHeaderID = 0;

        protected override async Task OnInitializedAsync()
        {
            //Populate the list of countries objects from the Countries table.
            poheader = await POHeaderService.POHeaderList();

            Toolbaritems.Add(new ItemModel() { Text = "Add", TooltipText = "Add a new order", PrefixIcon = "e-add" });
            Toolbaritems.Add(new ItemModel() { Text = "Edit", TooltipText = "Edit selected order", PrefixIcon = "e-edit" });
            Toolbaritems.Add(new ItemModel() { Text = "Delete", TooltipText = "Delete selected order", PrefixIcon = "e-delete" });
        }


        public void ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Add")
            {
                //Code for adding goes here
                POHeaderID = 0;
                NavigationManager.NavigateTo($"/purchaseorder/{POHeaderID}");

            }

            if (args.Item.Text == "Edit")
            {
                //Code for editing
            }

            if (args.Item.Text == "Edit")
            {
                //Code for deleting
            }
        }


    }
}
