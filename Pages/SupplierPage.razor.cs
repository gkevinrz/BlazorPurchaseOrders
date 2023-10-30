using BlazorPurchaseOrders.Data;
using BlazorPurchaseOrders.Shared;
using Microsoft.AspNetCore.Components;
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
        SfDialog DialogDeleteSuppler;

        //warning component
        WarningPage Warning;

        string WarningHeaderMessage = "";
        string WarningContentMessage = "";

        // tax list
        IEnumerable<Supplier> supplier;


        // list of tools
        private List<ItemModel> Toolbaritems = new List<ItemModel>();

        //object 
        Tax addeditTax = new Tax();

        //text for header
        string HeaderText = "";

        //tax id for edit or delete
        public int SelectedTaxId { get; set; } = 0;

        // get tax list and add toolbar
        protected override async Task OnInitializedAsync()
        {
            tax = await TaxService.TaxList();
            // Add Tax
            Toolbaritems.Add(new ItemModel()
            {
                Text = "Add",
                TooltipText = "Add a new Tax Rate",
                PrefixIcon = "e-add"
            });

            Toolbaritems.Add(new ItemModel()
            {
                Text = "Edit",
                TooltipText = "Edit selected Tax Rate",
                PrefixIcon = "e-edit"
            });

            Toolbaritems.Add(new ItemModel()
            {
                Text = "Delete",
                TooltipText = "Delete selected Tax Rate",
                PrefixIcon = "e-delete"
            });
        }


        // handler for toolbar
        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Add")
            {
                // Open dialog (addedit)

                addeditTax = new Tax(); //refresh
                HeaderText = "Add Tax Rate";
                await this.DialogAddEditTax.ShowAsync();


            }
            else if (args.Item.Text == "Edit")
            {
                //check if selectedindex !=0
                if (SelectedTaxId == 0)
                {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select a Tax Rate from the grid.";
                    Warning.OpenDialog();
                }
                else
                {
                    HeaderText = "Edit Tax Rate";
                    addeditTax = await TaxService.Tax_GetOne(SelectedTaxId);
                    await this.DialogAddEditTax.ShowAsync();
                }

            }
            else if (args.Item.Text == "Delete")
            {
                if (SelectedTaxId == 0)
                {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select a Tax Rate from the grid.";
                    Warning.OpenDialog();
                }
                else
                {
                    HeaderText = "Delete Tax Rate";
                    addeditTax = await TaxService.Tax_GetOne(SelectedTaxId);
                    await this.DialogDeleteTax.ShowAsync();
                }
            }
        }


        // Save new Tax
        protected async Task TaxSave()
        {
            if (addeditTax.TaxID == 0)
            {
                int success = await TaxService.TaxInsert(addeditTax.TaxDescription, addeditTax.TaxRate);
                if (success != 0)
                {
                    // Change dialog message (warning component)
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "This Tax Description already exists; it cannot be added again.";
                    Warning.OpenDialog();

                }
                else
                {
                    // Clears the dialog and is ready for another entry
                    // User must specifically close or cancel the dialog 
                    addeditTax = new Tax();
                    await CloseDialog();
                }


            }
            else
            {
                int Success = await TaxService.TaxUpdate(addeditTax.TaxDescription, addeditTax.TaxRate, SelectedTaxId, addeditTax.TaxIsArchived);
                if (Success != 0)
                {
                    //show dialog
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "This Tax Description already exists; it cannot be added again.";
                    Warning.OpenDialog();
                }
                else
                {
                    await this.DialogAddEditTax.HideAsync();
                    this.StateHasChanged();
                    addeditTax = new Tax();
                    SelectedTaxId = 0;
                }

            }

            //End process
            //Refresh datagrid
            tax = await TaxService.TaxList();
            StateHasChanged();

        }



        // Close dialog (addedit)
        private async Task CloseDialog()
        {
            await this.DialogAddEditTax.HideAsync();
        }

        // handler for selected row
        public void RowSelectHandler(RowSelectEventArgs<Tax> args)
        {
            //{args.Data} returns the current selected records.
            SelectedTaxId = args.Data.TaxID;
        }


        /* Methods for Delete Dialog */

        public async void ConfirmDeleteNo()
        {
            await DialogDeleteTax.HideAsync();
            SelectedTaxId = 0;
        }

        public async void ConfirmDeleteYes()
        {
            int Success = await TaxService.TaxUpdate(addeditTax.TaxDescription, addeditTax.TaxRate, SelectedTaxId, addeditTax.TaxIsArchived = true);
            if (Success != 0)
            {
                //duplicate check
                WarningHeaderMessage = "Warning!";
                WarningContentMessage = "Unknown error has occurred - the record has not been deleted!";
                Warning.OpenDialog();
            }
            else
            {
                await this.DialogDeleteTax.HideAsync();
                tax = await TaxService.TaxList();
                this.StateHasChanged();
                addeditTax = new Tax();
                SelectedTaxId = 0;
            }
        }


    }
}
