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


        /* ------- Variables ----------*/

        //POHeader List
        IEnumerable<POHeader> poHeaderList;

        //Toolbaritems List
        private List<ItemModel> Toolbaritems = new List<ItemModel>();
       
       /*--------- Initialized -------------*/
       protected override async Task OnInitializedAsync()
        {
            // get list POHeader from db
            poHeaderList = await POHeaderService.POHeaderList();

            //add toolbar items

            // Add item
            Toolbaritems.Add(new ItemModel()
            {
                Text="Add",
                TooltipText="Add a new order",
                PrefixIcon="e-add"
            });

            // Edit item
            Toolbaritems.Add(new ItemModel()
            {
                Text = "Edit",
                TooltipText = "Edit selected order",
                PrefixIcon = "e-edit"
            });

            // Delete item
            Toolbaritems.Add(new ItemModel() { 
                Text="Delete",
                TooltipText="Delete selected order",
                PrefixIcon="e-delete"
            });


        }



        /*--------------handlers ------------*/


        // ToolbarClickHandler 
        public void ToolbarClickHandler(ClickEventArgs args)
        {
            if (args.Item.Text == "Add")
            {
             
            }else if(args.Item.Text == "Edit") { 
            
            }else if(args.Item.Text=="Delete") { 
            
            }
        }

    }
}
