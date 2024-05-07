
using System.Net;
using Unsplasharp;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace UnsplashTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddScoped<IImageDownloader, ImageDownloader>();
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.MapGet("/img", async (string imgQuery, ImageDownloader downloader) =>
			{
				UnsplasharpClient client = new UnsplasharpClient("03jvn6lavoGJKCLyVN_Tw1GR654rFZbZbfVqRb6qiCE");
				var photo = await client.SearchPhotos(imgQuery);
				
				
				var folder = "images";
				var fileName = "test";
				var url = photo.First().Urls.Regular;

				await downloader.DownloadImageAsync(folder, fileName, new Uri(url));

				return url;
			});


			app.Run();
		}

		//private async Task DownloadImageAsync(string directoryPath, string fileName, Uri uri)
		//{
		//	using var httpClient = new HttpClient();

		//	// Get the file extension
		//	var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
		//	var fileExtension = Path.GetExtension(uriWithoutQuery);

		//	// Create file path and ensure directory exists
		//	var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
		//	Directory.CreateDirectory(directoryPath);

		//	// Download the image and write to the file
		//	var imageBytes = await httpClient.GetByteArrayAsync(uri);
		//	await File.WriteAllBytesAsync(path, imageBytes);
		//}

	}
}
