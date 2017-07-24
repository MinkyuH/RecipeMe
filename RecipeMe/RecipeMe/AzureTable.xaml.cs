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
			if (Globals.checker == false)
			{
				await DisplayAlert("Error", "Please Take a picture first", "dismiss");
			}
			else
			{
				var ingredient = AzureManager.AzureManagerInstance.GetIngredient();
				List<recipeme123> ingredientInfo = await AzureManager.AzureManagerInstance.GetIngredientDetail();
				IngredientList.ItemsSource = ingredientInfo.Where(p => String.Equals(p.MainIngredient.ToLower(), ingredient.ToLower())).OrderBy(p => p.RecipeType);
			}
		}

		private void Button_Clicked(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return;
			}
			Device.OpenUri(new Uri(((recipeme123)e.SelectedItem).RecipeLink.ToString()));
		}
	}
}
