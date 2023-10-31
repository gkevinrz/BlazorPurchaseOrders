using BlazorPurchaseOrders.Data;
using BlazorPurchaseOrders.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;

namespace BlazorPurchaseOrders.Pages
{
    public partial class SupplierPage:ComponentBase

    {
        [Inject] ISupplierService SupplierService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        //Dialog
        SfDialog DialogAddEditSupplier;
        //Dialog for delete
        SfDialog DialogDeleteSupplier;

        //warning component
        WarningPage Warning;
        string WarningHeaderMessage = "";
        string WarningContentMessage = "";

        // tax list
        IEnumerable<Supplier> supplier;


        // list of tools
        private List<ItemModel> Toolbaritems = new List<ItemModel>();

        //object 
        Supplier addeditSupplier = new Supplier();

        //text for header
        string HeaderText = "";

        //tax id for edit or delete
        public int SelectedSupplierId { get; set; } = 0;

        // get tax list and add toolbar
        protected override async Task OnInitializedAsync()
        {
             supplier = await SupplierService.SupplierList();
            // Add Tax
            Toolbaritems.Add(new ItemModel()
            {
                Text = "Add",
                TooltipText = "Add a new Supplier",
                PrefixIcon = "e-add"
            });

            Toolbaritems.Add(new ItemModel()
            {
                Text = "Edit",
                TooltipText = "Edit selected Supplier",
                PrefixIcon = "e-edit"
            });

            Toolbaritems.Add(new ItemModel()
            {
                Text = "Delete",
                TooltipText = "Delete selected Supplier",
                PrefixIcon = "e-delete"
            });
        }


        // handler for toolbar
        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Add")
            {
                // Open dialog (addedit)

                addeditSupplier = new Supplier(); //refresh
                HeaderText = "Add Supplier";
                await this.DialogAddEditSupplier.ShowAsync();


            }
            else if (args.Item.Text == "Edit")
            {
                //check if selectedindex !=0
                if (SelectedSupplierId == 0)
                {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select a Supplier from the grid.";
                    Warning.OpenDialog();
                }
                else
                {
                    HeaderText = "Edit Supplier";
                    addeditSupplier = await SupplierService.Supplier_GetOne(SelectedSupplierId);
                    await this.DialogAddEditSupplier.ShowAsync();
                }

            }
            else if (args.Item.Text == "Delete")
            {
                if (SelectedSupplierId == 0)
                {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select a Supplier from the grid.";
                    Warning.OpenDialog();
                }
                else
                {
                    HeaderText = "Delete Supplier";
                    addeditSupplier = await SupplierService.Supplier_GetOne(SelectedSupplierId);
                    await this.DialogDeleteSupplier.ShowAsync();
                }
            }
        }


        // Save new Tax
        protected async Task SupplierSave()
        {
            if (addeditSupplier.SupplierID == 0)
            {
                int success = await SupplierService.SupplierInsert(addeditSupplier.SupplierName, addeditSupplier.SupplierAddress1
                ,addeditSupplier.SupplierAddress2,addeditSupplier.SupplierAddress3,addeditSupplier.SupplierPostCode,addeditSupplier.SupplierEmail
                );
                
                if (success != 0)
                {
                    // Change dialog message (warning component)
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "This Supplier Description already exists; it cannot be added again.";
                    Warning.OpenDialog();

                }
                else
                {
                    // Clears the dialog and is ready for another entry
                    // User must specifically close or cancel the dialog 
                    addeditSupplier = new Supplier();
                    await CloseDialog();
                }


            }
            else
            {
                int Success = await SupplierService.SupplierUpdate(addeditSupplier.SupplierID, addeditSupplier.SupplierName, addeditSupplier.SupplierAddress1
                , addeditSupplier.SupplierAddress2, addeditSupplier.SupplierAddress3, addeditSupplier.SupplierPostCode, addeditSupplier.SupplierEmail,addeditSupplier.SupplierIsArchived);

                if (Success != 0)
                {
                    //show dialog
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "This Tax Description already exists; it cannot be added again.";
                    Warning.OpenDialog();
                }
                else
                {
                    await this.DialogAddEditSupplier.HideAsync();
                    this.StateHasChanged();
                    addeditSupplier = new Supplier();
                    SelectedSupplierId = 0;
                }

            }

            //End process
            //Refresh datagrid
            supplier = await SupplierService.SupplierList();
            StateHasChanged();

        }



        // Close dialog (addedit)
        private async Task CloseDialog()
        {
            await this.DialogAddEditSupplier.HideAsync();
        }

        // handler for selected row
        public void RowSelectHandler(RowSelectEventArgs<Supplier> args)
        {
            //{args.Data} returns the current selected records.
            SelectedSupplierId = args.Data.SupplierID;
        }


        /* Methods for Delete Dialog */

        public async void ConfirmDeleteNo()
        {
            await DialogDeleteSupplier.HideAsync();
            SelectedSupplierId = 0;
        }

        public async void ConfirmDeleteYes()
        {

            int Success = await SupplierService.SupplierUpdate(addeditSupplier.SupplierID, addeditSupplier.SupplierName, addeditSupplier.SupplierAddress1
            , addeditSupplier.SupplierAddress2, addeditSupplier.SupplierAddress3, addeditSupplier.SupplierPostCode, addeditSupplier.SupplierEmail, addeditSupplier.SupplierIsArchived=true);


            if (Success != 0)
            {
                //duplicate check
                WarningHeaderMessage = "Warning!";
                WarningContentMessage = "Unknown error has occurred - the record has not been deleted!";
                Warning.OpenDialog();
            }
            else
            {
                await this.DialogDeleteSupplier.HideAsync();
                supplier = await SupplierService.SupplierList();
                this.StateHasChanged();
                addeditSupplier = new Supplier();
                SelectedSupplierId = 0;
            }
        }


    }
}
