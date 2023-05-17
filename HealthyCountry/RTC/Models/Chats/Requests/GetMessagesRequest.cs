using System;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Localization;

namespace HealthyCountry.RTC.Models.Chats.Requests;

public class GetMessagesRequest
{
    [JsonIgnore]
    public ParticipantModel Requester { get; set; }
    public Guid ChatId { get; set; }
    public int Limit { get; set; } = 50;
    public int Skip { get; set; } = 0;
}
// internal class GetMessagesRequestValidator : BasicAbstractValidator<GetMessagesRequest>
// {
//     public GetMessagesRequestValidator() : base(null)
//     {
//         
//     }
//     public GetMessagesRequestValidator(IStringLocalizer localizer) : base(localizer)
//     {
//         RuleFor(x => x.ChatId).
//             Required(localizer);
//         
//         RuleFor(x => x.Requester)
//             .SetValidator(new ParticipantModelValidator(localizer));
//     }
//     
// }