using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services;

public class ContactService(ContactRepository repo, ContactFactory factory)
{
    private readonly ContactRepository _repo = repo;
    private readonly ContactFactory _factory = factory;

    public async Task<ResponseResult> CreateAsync(ContactFormModel model)
    {
        try
        {
            var entity = _factory.PopulateContactEntity(model);

            if(entity != null)
            {
                var result = await _repo.CreateAsync(entity);
                if (result.StatusCode == StatusCode.OK)
                {
                    return ResponseFactory.Ok("Message recieved");
                }
            }
            return ResponseFactory.Error("Something went wrong");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }

    public async Task<ResponseResult> ApiCallCreateSendRequestAsync(ContactFormModel model) 
    {
        try
        {
            using var http = new HttpClient();
            
            var json = JsonConvert.SerializeObject(model);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync("https://localhost:7126/api/contact/?key=e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b", content);

            if(response.IsSuccessStatusCode)
            {
                return ResponseFactory.Ok("Your message have been recieved");
            }
            return ResponseFactory.Error("Something went wrong");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
}
