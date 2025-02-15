#if (!UNITY_WEBGL && !UNITY_IOS) || UNITY_EDITOR
using MasterServerToolkit.MasterServer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MasterServerToolkit.Bridges.MongoDB
{
    public class AccountInfoMongoDB : IAccountInfoData
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string Id { get => _id.ToString(); set => _id = new ObjectId(value); }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Facebook { get; set; }
        public string Google { get; set; }
        public string Apple { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsGuest { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public DateTime LastLogin { get; set; }

        public event Action<IAccountInfoData> OnChangedEvent;

        public AccountInfoMongoDB()
        {
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Facebook = string.Empty;
            Google = string.Empty;
            Apple = string.Empty;
            Token = string.Empty;
            IsAdmin = false;
            IsGuest = true;
            IsEmailConfirmed = false;
            Properties = new Dictionary<string, string>();
        }

        public void MarkAsDirty()
        {
            OnChangedEvent?.Invoke(this);
        }

        public bool HasToken()
        {
            return !string.IsNullOrEmpty(Token);
        }

        public bool IsTokenExpired()
        {
            Properties.TryGetValue(MstDictKeys.USER_AUTH_TOKEN_EXPIRES, out string expireTime);

            if (!string.IsNullOrEmpty(expireTime))
            {
                long filetime = Convert.ToInt64(expireTime);
                return DateTime.FromFileTime(filetime) <= DateTime.Now;
            }
            else
            {
                return false;
            }
        }

        public void SetToken(int tokenExpiresInDays)
        {
            Token = Mst.Helper.CreateRandomAlphanumericString(64);
            Properties[MstDictKeys.USER_AUTH_TOKEN_EXPIRES] = DateTime.Now.AddDays(tokenExpiresInDays).ToFileTime().ToString();
        }

        public override string ToString()
        {
            var properties = new MstProperties();
            properties.Set("Id", Id);
            properties.Set("Username", Username);
            properties.Set("Password", Password);
            properties.Set("Email", Email);
            properties.Set("PhoneNumber", PhoneNumber);
            properties.Set("Facebook", Facebook);
            properties.Set("Google", Google);
            properties.Set("Apple", Apple);
            properties.Set("IsAdmin", IsAdmin);
            properties.Set("IsGuest", IsGuest);
            properties.Set("IsEmailConfirmed", IsEmailConfirmed);
            properties.Append(Properties);

            return properties.ToString();
        }
    }
}
#endif