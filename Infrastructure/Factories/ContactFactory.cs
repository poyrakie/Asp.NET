using Infrastructure.Entities;
using Infrastructure.Models;

namespace Infrastructure.Factories;

public class ContactFactory
{
    public ContactEntity PopulateContactEntity(ContactFormModel model)
    {
        var entity = new ContactEntity();
        if (model != null)
        {
            entity.FullName = model.FullName;
            entity.Email = model.Email;
            entity.Service = model.Service;
            entity.Message = model.Message;
        }
        return entity;
    }
}
