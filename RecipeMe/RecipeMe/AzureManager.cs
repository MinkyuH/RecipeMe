using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeMe
{
	public class AzureManager
	{
		private static AzureManager instance;
		private MobileServiceClient client;
		private IMobileServiceTable<recipeme123> ingredients;
		private string ingredient;

		private AzureManager()
		{
			client = new MobileServiceClient("http://recipeme123.azurewebsites.net");
			ingredients = client.GetTable<recipeme123>();
		}

		public MobileServiceClient AzureClient
		{
			get { return client; }
		}

		public static AzureManager AzureManagerInstance
		{
			get
			{
				if (instance == null)
				{
					instance = new AzureManager();
				}

				return instance;
			}
		}

		public Task<List<recipeme123>> GetIngredientDetail()
		{
			return ingredients.ToListAsync();
		}

		public void SetIngredient(string ingredient)
		{
			this.ingredient = ingredient;
		}

		public string GetIngredient()
		{
			return ingredient;
		}
	}
}
