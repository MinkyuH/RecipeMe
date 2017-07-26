using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using RecipeMe.Model;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeMe
{
    public partial class CustomVision : ContentPage
    {

		
		public CustomVision()
        {
            InitializeComponent();
        }

        private async void LoadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;
			Globals.checker = true;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

			await MakePredictionRequest(file);
        }

		static byte[] GetImageAsByteArray(MediaFile file)
		{
			var stream = file.GetStream();
			BinaryReader binaryReader = new BinaryReader(stream);
			return binaryReader.ReadBytes((int)stream.Length);
		}

		async Task MakePredictionRequest(MediaFile file)
		{
			var client = new HttpClient();

			client.DefaultRequestHeaders.Add("Prediction-Key", "6b188bd282ac425a8e3755635c55f37e");

			string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/34669e80-95d0-4bc7-8062-1f1d94f76082/image?iterationId=a7ea51b3-732b-4588-b424-3a2b7af7c800";

			HttpResponseMessage response;

			byte[] byteData = GetImageAsByteArray(file);

			using (var content = new ByteArrayContent(byteData))
			{

				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				response = await client.PostAsync(url, content);


				if (response.IsSuccessStatusCode)
				{



					

					var responseString = await response.Content.ReadAsStringAsync();
					EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);
					var probability = responseModel.Predictions.OrderByDescending(probabilityH => probabilityH.Probability);
					var output_result = probability.Take(1).Single();

					BarIndicator.IsVisible = true;

					if (output_result.Probability > 0.5) {
						TagLabel.Text = output_result.Tag;
						AzureManager.AzureManagerInstance.SetIngredient(output_result.Tag);
						await BarIndicator.ProgressTo(1, 80, Easing.Linear);
						BarIndicator.IsVisible = false;
						BarIndicator.ProgressTo(0, 80, Easing.Linear);

					}
					else {
						await BarIndicator.ProgressTo(1, 80, Easing.Linear);

						TagLabel.Text = "Doesn't exist in the database";
						BarIndicator.IsVisible = false;
						BarIndicator.ProgressTo(0, 80, Easing.Linear);
					}

				}
				file.Dispose();
			}
		}

	}
}