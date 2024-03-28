
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.HomeModels;
using Infrastructure.Repositories;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Infrastructure.Services;

public class SubscriberService(SubscriberRepository repo, SubscriberFactory factory)
{
    private readonly SubscriberRepository _repo = repo;
    private readonly SubscriberFactory _factory = factory;

    public async Task<ResponseResult> CreateSubscriberAsync(NewsletterModel model)
    {
        try
        {
            var existsResult = await _repo.ExistsAsync(x => x.Email == model.Email);
            if (existsResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var entity = _factory.PopulateSubscriberEntity(model);
                var createResult = await _repo.CreateAsync(entity);
                if (createResult.StatusCode == StatusCode.OK)
                {
                    return ResponseFactory.Ok(createResult.Message);

                }

            }
            else if (existsResult.StatusCode == StatusCode.EXISTS)
            {
                return ResponseFactory.Exists(existsResult.Message);
            }
            return ResponseFactory.Error();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }

    public async Task<ResponseResult> ApiCallCreateSubscriberAsync(NewsletterModel model)
    {
        try
        {
            using var http = new HttpClient();

            var json = JsonConvert.SerializeObject(model);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync("https://localhost:7126/api/subscribers/?key=e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b", content);
            if (response.IsSuccessStatusCode)
            {
                return ResponseFactory.Ok("You have been registered");
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                return ResponseFactory.Exists("This email is already subscriberd");
            }
            return ResponseFactory.Error("Something went wrong");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
}
