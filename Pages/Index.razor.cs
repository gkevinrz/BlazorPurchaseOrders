using Microsoft.AspNetCore.Components;
using BlazorPurchaseOrders.Data;
using Syncfusion.Blazor.Navigations;
using System.Diagnostics;
using Syncfusion.Blazor.Grids;
using BlazorPurchaseOrders.Shared;
using Microsoft.JSInterop;

namespace BlazorPurchaseOrders.Pages
{
    public partial   class Index:ComponentBase
    {
        [Inject] IPOHeaderService POHeaderService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject]        IJSRuntime IJS {  get; set; }
        /* Warning Vars*/

        WarningPage Warning;
        string WarningHeaderMessage = "";
        string WarningContentMessage = "";
        /*Confirmation Dialog vars*/
        ConfirmPage ConfirmOrderDelete;
        string ConfirmHeaderMessage = "";
        string ConfirmContentMessage = "";
        public bool ConfirmationChanged { get; set; } = false;
        /*Get one purchase order*/
        POHeader orderHeader = new POHeader();



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
            Toolbaritems.Add(new ItemModel() { Text="Preview", TooltipText="Preview selected order",PrefixIcon="e-print"});
        }

        ///Handlers-----------------------------------
        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
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
            //part of delete purchase order
            if (args.Item.Text == "Delete")
            {   
                if(selectedPOHeaderID == 0) {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select an Order from the grid.";

                    Warning.OpenDialog();

                }
                else
                {
                    //get purchase order
                    orderHeader= await POHeaderService.POHeader_GetOne(selectedPOHeaderID);

                    //confirmation
                    ConfirmHeaderMessage = "Confirm Deletion";
                    ConfirmContentMessage = "Please confirm that this order should be deleted.";

                    ConfirmOrderDelete.OpenDialog();
                }
            }


            if (args.Item.Text == "Preview")
            {
                if (selectedPOHeaderID == 0)
                {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select an Order from the grid.";
                    Warning.OpenDialog();

                }
                else
                {
                    await IJS.InvokeAsync<object>("open", new object[] { "/previeworder/" + selectedPOHeaderID + "", "_blank" });
                    //NavigationManager.NavigateTo($"/previeworder/{selectedPOHeaderID}");
                }
            }
        }
        /*Handler*/
        public void RowSelectHandler(RowSelectEventArgs<POHeader> args) {

            selectedPOHeaderID = args.Data.POHeaderID;
        }

        protected async Task ConfirmOrderArchive(bool archiveConfirmed)
        {
            if (archiveConfirmed)
            {
                //change attribute
                orderHeader.POHeaderIsArchived= true;
                
                bool success= await POHeaderService.POHeaderUpdate(orderHeader);
             
                poheader = await POHeaderService.POHeaderList();
                StateHasChanged();
            }
        }
    }
}
