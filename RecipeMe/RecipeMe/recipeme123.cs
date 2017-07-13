using Newtonsoft.Json;

namespace RecipeMe
{
	public class recipeme123
	{
		[JsonProperty(PropertyName = "id")]
		public string ID { get; set; }

		[JsonProperty(PropertyName = "MainIngredient")]
		public string MainIngredient { get; set; }

		[JsonProperty(PropertyName = "RecipeType")]
		public string RecipeType { get; set; }

		[JsonProperty(PropertyName = "RecipeDescription")]
		public string RecipeDescription { get; set; }

		[JsonProperty(PropertyName = "RecipeLink")]
		public string RecipeLink { get; set; }

	}
}
