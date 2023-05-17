using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.RTC.Entities;
using HealthyCountry.RTC.Interfaces.Repositories;
using HealthyCountry.RTC.Models;
using MongoDB.Driver;

namespace HealthyCountry.RTC.Repositories
{
    public class IceServerRepository : IIceServerRepository
    {
        private readonly WebsocketContext _context;
        private readonly IMapper _mapper;

        public IceServerRepository(WebsocketContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<IceServerModel>> GetServers(string userId)
        {
            var entities = await 
                _context.IceServers.Find(FilterDefinition<IceServerEntity>.Empty).ToListAsync();
            var results = new List<IceServerModel>();
            
            foreach (var entity in entities)
            {
                if (string.IsNullOrWhiteSpace(entity.Urls))
                {
                    continue;
                }
                
                var model = new IceServerModel
                {
                    Urls = entity.Urls
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToArray()
                };

                if (string.IsNullOrWhiteSpace(entity.ServerSecret))
                {
                    model.Username = entity.UserName;
                    model.Credential = entity.Credential;
                }
                else
                {
                    // this credential would be valid for the next 120 min
                    model.Credential = GenerateIceServerCredentials(DateTime.UtcNow, userId, entity.ServerSecret, 120,
                        out var userName);
                    model.Username = userName;
                }

                results.Add(model);
            }
            return results;
        }

        public async Task CreateOrUpdateIceServer(IceServerModel iceServer)
        {
            var entity = _mapper.Map<IceServerEntity>(iceServer);

            var filter = Builders<IceServerEntity>.Filter.Eq(s => s.Id, entity.Id);

            var updateDefinition = Builders<IceServerEntity>.Update
                                 .Set(x => x.Id, entity.Id)
                                 .Set(x => x.Urls, entity.Urls)
                                 .Set(x => x.Credential, entity.Credential)
                                 .Set(x => x.UserName, entity.UserName)
                                 .Set(x => x.ServerSecret, entity.ServerSecret);

            var updateResult =
                await _context.IceServers.UpdateOneAsync(filter, updateDefinition, new UpdateOptions {IsUpsert = true});
        }

        private static string GenerateIceServerCredentials(DateTime startDate, string userId, string secret,
            int validInMinutes, out string userName)
        {
            var unixSecondsSinceUtcNow = startDate.Subtract(DateTime.UnixEpoch).TotalSeconds;

            // this credential would be valid for the next 
            var unixTimeStamp = (int) unixSecondsSinceUtcNow + (validInMinutes * 60);

            userName = $"{unixTimeStamp}:{userId}";

            var keyByte = Encoding.UTF8.GetBytes(secret ?? "");
            var userNameBytes = Encoding.UTF8.GetBytes(userName);
            using var hmacsha1 = new HMACSHA1(keyByte);
            var hashmessage = hmacsha1.ComputeHash(userNameBytes);
            return Convert.ToBase64String(hashmessage);
        }
    }
}
