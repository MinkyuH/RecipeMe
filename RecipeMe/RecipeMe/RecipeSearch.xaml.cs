using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeMe
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecipeSearch : ContentPage
	{
		public RecipeSearch()
		{
			InitializeComponent();
		}

		private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
		{
			var ingredient = AzureManager.AzureManagerInstance.GetIngredient();
			List<recipeme123> ingredientInfo = await AzureManager.AzureManagerInstance.GetIngredientDetail();
			IngredientList.ItemsSource = ingredientInfo.Where(p => String.Equals(p.MainIngredient.ToLower(), Search.Text.ToLower())).OrderBy(p => p.RecipeType);
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			Device.OpenUri(new Uri("www.google.co.nz"));
		}
	}
}