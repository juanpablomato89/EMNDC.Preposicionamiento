using System.DirectoryServices.Protocols;
using System.Net;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models.Responses;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.Extensions.Options;

namespace EMNDC.Preposicionamiento.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly LdapSettings _ldapSettings;
        private readonly NetworkCredential _adminCredentials;


        public ActiveDirectoryService(
            IOptions<LdapSettings> ladapSettings)
        {
            _ldapSettings = ladapSettings.Value ?? throw new ArgumentNullException(nameof(ladapSettings.Value));
            _adminCredentials = new NetworkCredential(_ldapSettings.AdminUser, _ldapSettings.AdminPassword, _ldapSettings.Domain);
        }

        public async Task<ActiveDirectoryUserResponse> AuthenticateAsync(string username, string password)
        {
            var cleanUsername = username.Contains('\\')
                ? username.Split('\\')[1]
                : username.Contains('@')
                    ? username.Split('@')[0]
                    : username;

            var domain = _ldapSettings.Domain;

            using (var connection = new LdapConnection(new LdapDirectoryIdentifier(
                _ldapSettings.LdapServer,
                _ldapSettings.LdapPort)))
            {
                connection.Credential = new NetworkCredential($"{username}", password);
                connection.AuthType = AuthType.Negotiate;
                connection.SessionOptions.Sealing = true;
                connection.SessionOptions.Signing = true;

                await Task.Run(() => connection.Bind());

                var userDetails = await GetUserDetailsAsync(cleanUsername, connection);

                return userDetails;
            }
        }

        public async Task<ActiveDirectoryUserResponse> GetUserDetailsAsync(string username)
        {
            using (var connection = new LdapConnection(new LdapDirectoryIdentifier(
                _ldapSettings.LdapServer,
                _ldapSettings.LdapPort)))
            {
                if (_adminCredentials != null)
                {
                    connection.Credential = _adminCredentials;
                }
                connection.AuthType = AuthType.Negotiate;
                await Task.Run(() => connection.Bind());

                return await GetUserDetailsAsync(username, connection);
            }
        }

        private async Task<ActiveDirectoryUserResponse> GetUserDetailsAsync(
            string username,
            LdapConnection connection)
        {
            var user = new ActiveDirectoryUserResponse
            {
                UserName = username,
                Groups = new List<string>()
            };

            var searchRequest = new SearchRequest(
                _ldapSettings.UserSearchBase,
                $"(&(objectClass=user)(sAMAccountName={username}))",
                SearchScope.Subtree,
                new string[] {
                    "displayName", "mail", "givenName", "sn",
                    "userAccountControl", "lastLogon", "memberOf"
                });

            var searchResponse = (SearchResponse)await Task.Run(() =>
                connection.SendRequest(searchRequest));

            if (searchResponse.Entries.Count > 0)
            {
                var entry = searchResponse.Entries[0];

                user.DisplayName = GetAttributeValue(entry, "displayName");
                user.Email = GetAttributeValue(entry, "mail");
                user.FirstName = GetAttributeValue(entry, "givenName");
                user.LastName = GetAttributeValue(entry, "sn");

                var uacValue = entry.Attributes["userAccountControl"][0].ToString();
                if (int.TryParse(uacValue, out int uac))
                {
                    user.IsEnabled = (uac & 0x0002) == 0;
                }

                if (entry.Attributes["memberOf"] != null)
                {
                    foreach (var group in entry.Attributes["memberOf"])
                    {
                        var groupDn = group.ToString();
                        if (groupDn.Contains("CN="))
                        {
                            var groupName = groupDn.Split(',')[0].Replace("CN=", "");
                            user.Groups.Add(groupName);
                        }
                    }
                }
            }
            return user;
        }

        public async Task<bool> ValidateUserInGroupAsync(string username, string groupName)
        {
            var user = await GetUserDetailsAsync(username);
            return user.Groups.Any(g =>
                g.Equals(groupName, StringComparison.OrdinalIgnoreCase));
        }

        private string GetAttributeValue(SearchResultEntry entry, string attributeName)
        {
            if (entry.Attributes.Contains(attributeName))
            {
                var attribute = entry.Attributes[attributeName];
                if (attribute.Count > 0)
                {
                    return attribute[0].ToString();
                }
            }
            return string.Empty;
        }
    }
}
