using Microsoft.AspNetCore.Components;
using BlazorPurchaseOrders.Data;

using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using Microsoft.Identity.Client;
using BlazorPurchaseOrders.Shared;

namespace BlazorPurchaseOrders.Pages
{
    public partial class ProductPage:ComponentBase
    {

        [Inject] IProductService ProductService { get; set; }
        [Inject] ISupplierService SupplierService { get; set; }

        [Inject] NavigationManager NavigationManager { get; set; }



        /* --------------Variables--------------------- */
        IEnumerable<Product> product;
        IEnumerable<Supplier> supplier;

        // toolbar list items
        private List<ItemModel> Toolbaritems = new List<ItemModel>();
        // save id for selected product
        public int SelectedProductId { get; set; } = 0;
        //string for change dialog title
        string HeaderText = "";




        //object to manipulate
        Product addeditProduct = new Product();
        


        //Dialog for add/edit product
        SfDialog DialogAddEditProduct; 
        //Dialog for delete product
        SfDialog DialogDeleteProduct;

        //warning component
        WarningPage Warning;
        string WarningHeaderMessage = "";
        string WarningContentMessage = "";


        /*-----------------------------------------------Initialized ----------------------------------*/
        // show data
        // add items to toolbar
        protected override async Task OnInitializedAsync()
        {
            product= await ProductService.ProductList();
            supplier = await SupplierService.SupplierList();


            //first item
            Toolbaritems.Add(new ItemModel
            {
                Text = "Add",
                TooltipText = "Add a new Product",
                PrefixIcon = "e-add"
            });
            Toolbaritems.Add(new ItemModel
            {
                Text = "Edit",
                TooltipText = "Edit selected Product",
                PrefixIcon = "e-edit"
            });

            Toolbaritems.Add(new ItemModel
            {
                Text = "Delete",
                TooltipText = "Delete selected Product",
                PrefixIcon = "e-delete"
            });

        }



        /*---------------------------------------------- Handlers ----------------------------------- */
        
        //SelectedRow handler (is async)
        public void RowSelectHandler(RowSelectEventArgs<Product> args)
        {
            //{args.Data} returns the current selected records.
            SelectedProductId = args.Data.ProductID;
        }

        public async Task  ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
            
        {
            if (args.Item.Text == "Add")
            {
                // Open dialog (addedit) 

                addeditProduct = new Product(); //refresh
                HeaderText = "Add Product";
                await this.DialogAddEditProduct.ShowAsync();


            }
            else if (args.Item.Text == "Edit")
            {
                //check if selectedindex !=0
                if (SelectedProductId == 0)
                {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select a Product from the grid.";
                    Warning.OpenDialog();
                }
                else
                {
                    HeaderText = "Edit Product";
                    addeditProduct = await ProductService.Product_GetOne(SelectedProductId);
                    await this.DialogAddEditProduct.ShowAsync();
                }

            }
            else if (args.Item.Text == "Delete")
            {
                if (SelectedProductId == 0)
                {
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "Please select a Product from the grid.";
                    Warning.OpenDialog();
                }
                else
                {
                    HeaderText = "Delete Product";
                    addeditProduct = await ProductService.Product_GetOne(SelectedProductId);
                    await this.DialogDeleteProduct.ShowAsync();
                }
            }

        }

        /*---------------------------------------------- Dialog methods ----------------------------------- */

        // Close dialog for Add/Edit Dialog
        public async Task  CloseDialog()
        {
            await this.DialogAddEditProduct.ShowAsync();   
        }
        

        // Save button dialog for Add/Edit Dialog

        protected async Task ProductSave()
        {
            if (addeditProduct.ProductID == 0)
            {
                int success = await ProductService.ProductInsert(addeditProduct.ProductCode, addeditProduct.ProductDescription
                , addeditProduct.ProductUnitPrice, addeditProduct.ProductSupplierID);

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
                    addeditProduct = new Product();
                  
                }


            }
            else
            {
                int Success = await ProductService.ProductUpdate(
                SelectedProductId,
                addeditProduct.ProductCode,
                addeditProduct.ProductDescription,
                addeditProduct.ProductUnitPrice,
                addeditProduct.ProductSupplierID,
                addeditProduct.ProductIsArchived
                );

                if (Success != 0)
                {
                    //show dialog
                    WarningHeaderMessage = "Warning!";
                    WarningContentMessage = "This Tax Description already exists; it cannot be added again.";
                    Warning.OpenDialog();
                }
                else
                {
                    await this.DialogAddEditProduct.HideAsync();
                    this.StateHasChanged();
                    addeditProduct = new Product();
                    SelectedProductId = 0;
                }

            }

            //End process
            //Refresh datagrid
            product = await ProductService.ProductList();
            StateHasChanged();
        }

        public async void ConfirmDeleteNo()
        {
            await DialogDeleteProduct.HideAsync();
            SelectedProductId = 0;
        }
        public async void ConfirmDeleteYes()
        {
            int Success = await ProductService.ProductUpdate(
                SelectedProductId,
                addeditProduct.ProductCode,
                addeditProduct.ProductDescription,
                addeditProduct.ProductUnitPrice,
                addeditProduct.ProductSupplierID,
                addeditProduct.ProductIsArchived = true);
            if (Success != 0)
            {
                //Duplicate Error
                WarningHeaderMessage = "Warning!";
                WarningContentMessage = "Unknown error has occurred - the record has not been deleted!";
                Warning.OpenDialog();
            }
            else
            {
                await this.DialogDeleteProduct.HideAsync();
                product = await ProductService.ProductList();
                this.StateHasChanged();
                addeditProduct = new Product();
                SelectedProductId = 0;
            }
        }
    }
}
