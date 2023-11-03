using Microsoft.AspNetCore.Components;
using BlazorPurchaseOrders.Data;
using Syncfusion.Blazor.Navigations;
using System.Diagnostics;
using Syncfusion.Blazor.Grids;
using BlazorPurchaseOrders.Shared;

namespace BlazorPurchaseOrders.Pages
{
    public partial   class Index:ComponentBase
    {
        [Inject] IPOHeaderService POHeaderService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        /* Warning Vars*/

        WarningPage Warning;
        string WarningHeaderMessage = "";
        string WarningContentMessage = "";


        // Create an empty list, named poheader, of empty POHeader objects.
        IEnumerable<POHeader> poheader;

        private List<ItemModel> Toolbaritems = new List<ItemModel>();

        int POHeaderID = 0;
        //from datagrid
        private int selectedPOHeaderID { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            //Populate the list of countries objects from the Countries table.
            poheader = await POHeaderService.POHeaderList();

            Toolbaritems.Add(new ItemModel() { Text = "Add", TooltipText = "Add a new order", PrefixIcon = "e-add" });
            Toolbaritems.Add(new ItemModel() { Text = "Edit", TooltipText = "Edit selected order", PrefixIcon = "e-edit" });
            Toolbaritems.Add(new ItemModel() { Text = "Delete", TooltipText = "Delete selected order", PrefixIcon = "e-delete" });
        }

        ///Handlers-----------------------------------
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

                if(selectedPOHeaderID== 0)
                {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select an Order from the grid";
                    Warning.OpenDialog();
                }
                else
                {
                    NavigationManager.NavigateTo($"/purchaseorder/{selectedPOHeaderID}");
                }
            }

            if (args.Item.Text == "Edit")
            {
                //Code for deleting
            }
        }

        public void RowSelectHandler(RowSelectEventArgs<POHeader> args) {

            selectedPOHeaderID = args.Data.POHeaderID;
        }


    }
}
