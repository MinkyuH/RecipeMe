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
    [XamlCompilation(XamlCompilationOptions.Compile)]
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

			string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/34669e80-95d0-4bc7-8062-1f1d94f76082/image?iterationId=70aa188b-12a5-428e-9f5c-7c5cd235dc08";

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


					if (output_result.Probability > 0.5) {
						TagLabel.Text = output_result.Tag;
					}
					else {
						TagLabel.Text = "Doesn't exist in the database";
					}

				}
				file.Dispose();
			}
		}

	}
}