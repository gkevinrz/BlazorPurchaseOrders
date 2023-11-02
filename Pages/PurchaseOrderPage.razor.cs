using Microsoft.AspNetCore.Components;
using BlazorPurchaseOrders.Data;

using System;
using System.Collections.Generic;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorPurchaseOrders.Pages
{
    public  partial class PurchaseOrderPage: ComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject] ISupplierService SupplierService { get; set; }
        [Inject] IPOHeaderService POHeaderService { get; set; }
        [Inject] IProductService ProductService { get; set; }
        [Inject] ITaxService TaxService { get; set; }

        POHeader orderaddedit = new POHeader();
        IEnumerable<Supplier> supplier;
        IEnumerable<Product> product;
        IEnumerable<Tax> tax;

        string pagetitle = "";

        private string UserName;

        SfGrid<POLine> OrderLinesGrid;
        public List<POLine> orderLines = new List<POLine>();
        private List<ItemModel> Toolbaritems = new List<ItemModel>();

        SfDialog DialogAddEditOrderLine;
        public POLine addeditOrderLine = new POLine();

        [Parameter]
        public int POHeaderID { get; set; }

        //Executes on page open, sets headings and gets data in the case of edit
        protected override async Task OnInitializedAsync()
        {
            supplier = await SupplierService.SupplierList();
            orderaddedit.POHeaderOrderDate = DateTime.Now;
            product = await ProductService.ProductList();
            tax = await TaxService.TaxList();

            if (POHeaderID == 0)
            {
                pagetitle = "Add an Order";
            }
            else
            {
                pagetitle = "Edit an Order";
            }

            //Get user if logged in and populate the 'Requested by' column
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                UserName = user.Identity.Name;
            }
            else
            {
                UserName = "The user is NOT authenticated.";
            }

            orderaddedit.POHeaderRequestedBy = UserName;

            Toolbaritems.Add(new ItemModel() { Text = "Add", TooltipText = "Add a new order line", PrefixIcon = "e-add" });
            Toolbaritems.Add(new ItemModel() { Text = "Edit", TooltipText = "Edit selected order line", PrefixIcon = "e-edit" });
            Toolbaritems.Add(new ItemModel() { Text = "Delete", TooltipText = "Delete selected order line", PrefixIcon = "e-delete" });

        }

        private void OnChangeSupplier(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int, Supplier> args)
        {
            this.orderaddedit.POHeaderSupplierAddress1 = args.ItemData.SupplierAddress1;
            this.orderaddedit.POHeaderSupplierAddress2 = args.ItemData.SupplierAddress2;
            this.orderaddedit.POHeaderSupplierAddress3 = args.ItemData.SupplierAddress3;
            this.orderaddedit.POHeaderSupplierPostCode = args.ItemData.SupplierPostCode;
            this.orderaddedit.POHeaderSupplierEmail = args.ItemData.SupplierEmail;
        }

        // Executes OnValidSubmit of EditForm above
        protected async Task OrderSave()
        {
            if (POHeaderID == 0)
            {
                //Save the record
                bool Successs = await POHeaderService.POHeaderInsert(orderaddedit);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }

        //Executes if user clicks the Cancel button.
        void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }

        public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            if (args.Item.Text == "Add")
            {
                //Code for adding goes here
                addeditOrderLine = new POLine();          // Ensures a blank form when adding
                addeditOrderLine.POLineNetPrice = 0;
                addeditOrderLine.POLineTaxID = 0;
                addeditOrderLine.POLineProductID = 0;
                await this.DialogAddEditOrderLine.ShowAsync();
            }

            if (args.Item.Text == "Edit")
            {
                //Code for adding goes here
            }

            if (args.Item.Text == "Delete")
            {
                //Code for adding goes here
            }
        }

        private void OrderLineSave()
        {
            if (addeditOrderLine.POLineID == 0)
            {
                //Code to save order line goes here
                orderLines.Add(new POLine
                {
                    POLineHeaderID = 0,
                    POLineProductID = addeditOrderLine.POLineProductID,
                    POLineProductCode = addeditOrderLine.POLineProductCode,
                    POLineProductDescription = addeditOrderLine.POLineProductDescription,
                    POLineProductQuantity = addeditOrderLine.POLineProductQuantity,
                    POLineProductUnitPrice = addeditOrderLine.POLineProductUnitPrice,
                    POLineNetPrice = addeditOrderLine.POLineNetPrice,
                    POLineTaxRate = addeditOrderLine.POLineTaxRate,
                    POLineTaxAmount = addeditOrderLine.POLineTaxAmount,
                    POLineGrossPrice = addeditOrderLine.POLineGrossPrice
                });
                addeditOrderLine.POLineProductID = 0;
                addeditOrderLine.POLineProductCode = "";
                addeditOrderLine.POLineProductDescription = "";
                addeditOrderLine.POLineProductQuantity = 0;
                addeditOrderLine.POLineProductUnitPrice = 0;
                addeditOrderLine.POLineNetPrice = 0;
                addeditOrderLine.POLineTaxID = 0;
                addeditOrderLine.POLineTaxRate = 0;
                addeditOrderLine.POLineTaxAmount = 0;
                addeditOrderLine.POLineGrossPrice = 0;
                OrderLinesGrid.Refresh();
                StateHasChanged();

                // addeditOrderLine = new POLine(); 

             
            }
        }

        private async Task CloseDialog()
        {
            await this.DialogAddEditOrderLine.HideAsync();
        }

        private void OnChangeProduct(Syncfusion.Blazor.DropDowns.SelectEventArgs<Product> args)
        {
            this.addeditOrderLine.POLineProductCode = args.ItemData.ProductCode;
            this.addeditOrderLine.POLineProductDescription = args.ItemData.ProductDescription;
            this.addeditOrderLine.POLineProductUnitPrice = args.ItemData.ProductUnitPrice;
            POLineCalc();
        }

        private void OnChangeTax(Syncfusion.Blazor.DropDowns.SelectEventArgs<Tax> args)
        {
            // int testTaxId = args.ItemData.TaxID;
            this.addeditOrderLine.POLineTaxRate = args.ItemData.TaxRate;
            POLineCalc();
        }

        private void POLineCalc()
        {
            addeditOrderLine.POLineNetPrice = addeditOrderLine.POLineProductUnitPrice * addeditOrderLine.POLineProductQuantity;
            addeditOrderLine.POLineTaxAmount = addeditOrderLine.POLineNetPrice.Value * addeditOrderLine.POLineTaxRate;
            addeditOrderLine.POLineGrossPrice = addeditOrderLine.POLineNetPrice.Value * (1 + addeditOrderLine.POLineTaxRate);
        }
    }
}
