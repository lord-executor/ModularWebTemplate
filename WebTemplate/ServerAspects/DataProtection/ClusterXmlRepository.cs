using System.Xml.Linq;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Options;

namespace WebTemplate.ServerAspects.DataProtection;

public class ClusterXmlRepository : IXmlRepository
{
    private readonly ILogger _logger;
    private readonly DataProtectionOptions _options;
    private readonly IList<KeyItem> _items = new List<KeyItem>();

    public ClusterXmlRepository(IOptions<DataProtectionOptions> options, ILogger<ClusterXmlRepository> logger)
    {
        _logger = logger;
        _options = options.Value;

        _items.Add(new KeyItem
        {
            FriendlyName = "key-de9e2934-76db-422d-a335-0147d554743e",
            ApplicationContext = "D:\\storage\\development\\projects\\ModularWebTemplate\\WebTemplate\\",
            Xml = """
                  <?xml version="1.0" encoding="UTF-8" standalone="no"?>
                  <key id="de9e2934-76db-422d-a335-0147d554743e" version="1">
                      <creationDate>2024-06-25T19:23:20.5700934Z</creationDate>
                      <activationDate>2024-06-25T19:20:02.4724178Z</activationDate>
                      <expirationDate>2024-09-23T19:20:02.4724178Z</expirationDate>
                      <descriptor deserializerType="Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel.AuthenticatedEncryptorDescriptorDeserializer, Microsoft.AspNetCore.DataProtection, Version=8.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60">
                          <descriptor>
                              <encryption algorithm="AES_256_CBC"/>
                              <validation algorithm="HMACSHA256"/>
                              <masterKey xmlns:p4="http://schemas.asp.net/2015/03/dataProtection" p4:requiresEncryption="true">
                                  <!-- Warning: the key below is in an unencrypted form. -->
                                  <value>/3hfCXwejFWQtJVHgxXjQ8+Ak+Z+yhIvHZlKaYpe75AAkpDM33Y6hkQ8XeteiwAZMQuhNEzLA0+p9CsiEJbg4w==</value>
                              </masterKey>
                          </descriptor>
                      </descriptor>
                  </key>
                  """
        });
    }

    public IReadOnlyCollection<XElement> GetAllElements()
    {
        _logger.LogInformation("Getting ALL elements");
        return _items.Select(k => XElement.Parse(k.Xml)).ToList();
    }

    public void StoreElement(XElement element, string friendlyName)
    {
        _logger.LogInformation("Storing key {keyName}", friendlyName);

        _items.Add(new KeyItem
        {
            FriendlyName = friendlyName,
            ApplicationContext = _options.ApplicationDiscriminator!,
            Xml = element.ToString(SaveOptions.None)
        });
    }
}