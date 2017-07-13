using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace RecipeMe
{
    public partial class AzureTable : ContentPage
    {
        public AzureTable()
        {
            InitializeComponent();
        }

		private async void IngredientHandler(object sender, EventArgs e)
		{
			var ingredient = AzureManager.AzureManagerInstance.GetIngredient();
			List<recipeme123> ingredientInfo = await AzureManager.AzureManagerInstance.GetIngredientDetail();
			IngredientList.ItemsSource = ingredientInfo.Where(p => String.Equals(p.MainIngredient, ingredient)).OrderBy(p => p.RecipeType);
		}
	}
}